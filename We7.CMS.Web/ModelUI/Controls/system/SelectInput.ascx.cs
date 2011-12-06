using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Framework.Util;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using We7.Framework;
namespace We7.Model.UI.Controls.system
{
    public partial class SelectInput : FieldControl
    {
       /// <summary>
        /// JS用, 这个更多选项对应XML里的节点名
       /// </summary>
        public string MatchupNode { get; set; }
       

        /// <summary>
        /// JS用,弹出对话框的路径
        /// </summary>
        public string DialogPath { get; set; }

        public override void InitControl()
        {
            TxtInput.Text = Value == null ? Control.DefaultValue : Value.ToString();
            TxtInput.CssClass = Control.CssClass;

            if (!String.IsNullOrEmpty(Control.Width))
            {
                TxtInput.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                TxtInput.Height = Unit.Parse(Control.Height);
            }
            MatchupNode = Control.Params[Constants.SELECTDATA];
            DialogPath = ResolveUrl("~/ModelUI/Controls/system/page/MoreSelect.aspx");
            //找存储下拉列表XML
            string path = Server.MapPath("~/config/SelectContent.xml");
            DrPListSelect.Items.Clear();
            #region 读XML
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);
                //得到顶层节点列表 
                XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;

                foreach (XmlElement element in topM)
                {
                    //得到第二层对应节点列表
                    if (element.Attributes["name"].Value == MatchupNode)
                    {
                        //得到第三层节点列表
                        foreach (XmlElement Selectelement in element)
                        {
                            ListItem item = new ListItem();
                            item.Value = Selectelement.Attributes["kye"].Value;
                            item.Text = Selectelement.Attributes["value"].Value;
                            DrPListSelect.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            #endregion
        }

        public override object GetValue()
        {
            return TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(TxtInput.Text.Trim()), Column.DataType);
        }

    }
}