using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core;
using We7.Model.Core.UI;
using System.Text.RegularExpressions;

namespace We7.Model.Core.ListControl
{
    public class ActionField : ModelControlField
    {
        protected string ModelName
        {
            get { return HttpContext.Current.Request["model"]; }
        }

        protected internal DataControlField CloneField()
        {
            ActionField df = CreateField() as ActionField;
            CopyProperties(df);
            return df;
        }

        protected override DataControlField CreateField()
        {
            return new ActionField();
        }

        bool isField;
        string[] textfields;

        Regex regexcommand = new Regex(@"^\s*(?<cmd>\w+)\((?<arg>.*)\)\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        protected override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState)
        {
            if (cellType == DataControlCellType.DataCell)
            {
                string cmd = Column.Params["cmd"];
                string cmdField = Column.Params["cmdField"];
                string cmdText = Column.Params["cmdText"];

                string[] cmds = cmd.Split('|');
                isField = !String.IsNullOrEmpty(cmdField);
                textfields = isField ? cmdField.Split('|') : cmdText.Split('|');

                for (int i = 0; i < cmds.Length; i++)
                {
                    if (i > 0)
                    {
                        Literal space = new Literal();
                        space.Text = "&nbsp;";
                        cell.Controls.Add(space);
                    }
                    if (regexcommand.IsMatch(cmds[i]))
                    {
                        InitHyperLink(cell, cmds[i], textfields[i]);
                    }
                    else
                    {
                        InitCommandButton(cell, cmds[i], textfields[i]);
                    }
                }
            }
        }

        void InitHyperLink(DataControlFieldCell cell, string cmd, string text)
        {
            HyperLink lnk = new HyperLink();
            lnk.DataBinding += new EventHandler(lnk_DataBinding);
            lnk.Text = text;
            lnk.Attributes["cmd"] = cmd;
            cell.Controls.Add(lnk);
        }


        const string OPENWINDOWN = "window.showModelessDialog('{0}',this,'scroll:0;status:0;help:0;resizable:1;dialogWidth:{1}px;dialogHeight:{2}px');return false;";
        Regex regexField = new Regex(@"{(?<field>\w+)}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        void lnk_DataBinding(object sender, EventArgs e)
        {
            HyperLink lnk = sender as HyperLink;
            object dataitem = DataBinder.GetDataItem(lnk.NamingContainer);
            string cmdstr = lnk.Attributes["cmd"];

            Match mc = regexcommand.Match(cmdstr);
            string cmd = mc.Groups["cmd"].Value;
            string arg = mc.Groups["arg"].Value;
            string[] args = !String.IsNullOrEmpty(arg) ? arg.Split(',') : null;

            if (cmd.ToLower() == "dialog")
            {
                InitDialog(lnk, dataitem, args);
            }
            else if (cmd.ToLower() == "link")
            {
                InitLink(lnk, dataitem, args);
            }
            else if (cmd.ToLower() == "edit")
            {
                InitEidt(lnk, dataitem, args);
            }
            else if (cmd.ToLower() == "view")
            {
                InitView(lnk, dataitem, args);
            }
            else if (cmd.ToLower() == "viewAdvice")
            {
                InitAdviceView(lnk, dataitem, args);
            }

        }

        void InitEidt(HyperLink lnk, object dataitem, string[] args)
        {
            lnk.NavigateUrl = "~/Admin/Addins/ModelEditor.aspx?notiframe=" + HttpContext.Current.Request["notiframe"] + "&model=" + ModelName + "&ID=" + GetValue(dataitem, "ID")+"&groupIndex=0";
            //lnk.Target = "_top";
        }

        void InitView(HyperLink lnk, object dataitem, string[] args)
        {
            lnk.NavigateUrl = "~/Admin/Addins/ModelViewer.aspx?notiframe=1&model=" + ModelName + "&ID=" + GetValue(dataitem, "ID");
            lnk.Target = "_blank";
        }

        void InitAdviceView(HyperLink lnk, object dataitem, string[] args)
        {
            lnk.NavigateUrl = "~/Admin/Addins/ModelViewer.aspx?notiframe=1&model=" + ModelName + "&ID=" + GetValue(dataitem, "ID");
            lnk.Target = "_blank";
        }

        void InitLink(HyperLink lnk, object dataitem, string[] args)
        {
            if (args == null || args.Length < 1)
                throw new SystemException("ActionField:参数不能小于1");

            string url = args[0];
            MatchCollection mcc = regexField.Matches(url);
            foreach (Match mc in mcc)
            {
                string field = mc.Groups["field"].Value;
                url = url.Replace("{" + field + "}", GetValue(dataitem, field));
            }
            lnk.NavigateUrl = url;
            lnk.Target = args.Length > 1 ? args[1] : "_self";
        }

        void InitDialog(HyperLink lnk, object dataitem, string[] args)
        {
            if (args == null || args.Length < 1)
                throw new SystemException("ActionField:参数不能小于1");

            string url = args[0];
            string width = args.Length > 1 ? args[1] : "600";
            string height = args.Length > 2 ? args[2] : "450";
            MatchCollection mcc = regexField.Matches(url);
            foreach (Match m in mcc)
            {
                url = url.Replace(m.Value, GetValue(dataitem, m.Groups["field"].Value));
            }

            lnk.Attributes["onclick"] = string.Format(OPENWINDOWN, lnk.ResolveUrl(url), width, height);
            lnk.NavigateUrl = "#";
        }

        void InitCommandButton(DataControlFieldCell cell, string cmd, string text)
        {
            LinkButton lnkbttn = new LinkButton();
            lnkbttn.DataBinding += new EventHandler(lnkbttn_DataBinding);
            lnkbttn.Text = text;
            lnkbttn.CommandName = cmd;
            lnkbttn.CommandArgument = RowIndex.ToString();
            cell.Controls.Add(lnkbttn);
            if (cmd.ToLower() == "delete")
            {
                lnkbttn.OnClientClick = "return window.confirm('您确定删除本记录吗?')";
            }
        }

        void lnkbttn_DataBinding(object sender, EventArgs e)
        {
            LinkButton lnkbttn = sender as LinkButton;
            object dataItem = DataBinder.GetDataItem(lnkbttn.NamingContainer);
            lnkbttn.Text = isField ? GetValue(dataItem, lnkbttn.Text) : lnkbttn.Text;
        }
    }
}
