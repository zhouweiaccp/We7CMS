using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Xml;
using System.Collections.Specialized;
using System.IO;

namespace We7.CMS.Controls
{
    /// <summary>
    /// CommonTwoCascade 的摘要说明。
    /// 一级节点二级节点联动选择控件
    /// </summary>

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CommonTwoCascade runat=server></{0}:CommonTwoCascade>")]
    public class CommonTwoCascade : WebControl, IPostBackDataHandler
    {
        #region 属性

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取选择文本")]
        public string Text
        {
            get
            {
                string first = "";
                string second = "";

                if (ViewState[this.ID + "_TextFirst"] != null)
                {
                    first = ViewState[this.ID + "_TextFirst"].ToString();
                }
                else
                {
                    first = "";
                }

                if (ViewState[this.ID + "_TextSecond"] != null)
                {
                    second = ViewState[this.ID + "_TextSecond"].ToString();
                }
                else
                {
                    second = "";
                }

                return first + "-" + second;
            }
            set
            {
                string[] temp = value.Split(new char[] { '-' });
                if (temp.Length > 1)
                {
                    ViewState[this.ID + "_TextFirst"] = temp[0];
                    ViewState[this.ID + "_TextSecond"] = temp[1];
                }
            }
        }

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置一级节点")]
        public string TextFirst
        {
            get
            {
                if (ViewState[this.ID + "_TextFirst"] != null)
                {
                    return ViewState[this.ID + "_TextFirst"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState[this.ID + "_TextFirst"] = value;
            }
        }

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置二级节点")]
        public string TextSecond
        {
            get
            {
                if (ViewState[this.ID + "_TextSecond"] != null)
                {
                    return ViewState[this.ID + "_TextSecond"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState[this.ID + "_TextSecond"] = value;
            }
        }

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置一级节点名称")]
        public string FirstNodeName
        {
            get
            {
                if (ViewState[this.ID + "_FirstNodeName"] != null)
                {
                    return ViewState[this.ID + "_FirstNodeName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState[this.ID + "_FirstNodeName"] = value;
            }
        }

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置二级节点名称")]
        public string SecondNodeName
        {
            get
            {
                if (ViewState[this.ID + "_SecondNodeName"] != null)
                {
                    return ViewState[this.ID + "_SecondNodeName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState[this.ID + "_SecondNodeName"] = value;
            }
        }

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置联动所依据的XML文件网络路径")]
        public string CascadeXmlFile
        {
            get
            {
                if (ViewState[this.ID + "_CascadeXmlFile"] != null)
                {
                    return ViewState[this.ID + "_CascadeXmlFile"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState[this.ID + "_CascadeXmlFile"] = value;
            }
        }
        #endregion

        //一级节点份
        private ArrayList firstArr = new ArrayList();
        //二级节点
        private ArrayList secondArr = new ArrayList();

        /// <summary>
        /// 将此控件呈现给指定的输出参数
        /// </summary>
        /// <param name="writer"> 要写出到的 HTML 编写器</param>
        protected override void Render(HtmlTextWriter output)
        {
            GetCascadingInfo();

            #region	一级节点二级节点连选所需的javascript函数

            /*****************一级节点 二级节点 代码*********/
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(string), this.ID + "CommonTwoCascade_clientScript"))
            {
                output.WriteLine("<script language=\"JavaScript\">");
                output.WriteLine("var " + this.ID + "groups=" + (firstArr.Count + 1) + ";");
                output.WriteLine("var " + this.ID + "group=new Array(" + this.ID + "groups);");
                output.WriteLine("for (i=0; i<" + this.ID + "groups; i++)");
                output.WriteLine("" + this.ID + "group[i]=new Array();");
                output.WriteLine("" + this.ID + "group[0][0]=new Option(\"请选择\",\"\");");

                for (int i = 0; i < secondArr.Count; i++)
                {
                    output.WriteLine("" + this.ID + "group[" + (i + 1) + "][0]=new Option(\"请选择\",\"" + firstArr[i].ToString() + "\");");
                    ArrayList arr = (ArrayList)secondArr[i];
                    for (int j = 0; j < arr.Count; j++)
                    {
                        output.WriteLine("" + this.ID + "group[" + (i + 1) + "][" + (j + 1) + "]=new Array();");
                        output.WriteLine("" + this.ID + "group[" + (i + 1) + "][" + (j + 1) + "]=new Option(\"" + arr[j].ToString() + "\",\"" + arr[j].ToString() + "\");");
                    }
                }
                /********************************************************************************/

                output.WriteLine("function " + this.ID + "redirectff(x,obj,objc){");
                output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
                output.WriteLine("obj.options[m]=null;");
                output.WriteLine("for (i=1;i<" + this.ID + "group[x].length;i++){");
                output.WriteLine("obj.options[i]=new Option(" + this.ID + "group[x][i].text," + this.ID + "group[x][i].value);");
                output.WriteLine("obj.options[0].selected=true;");
                output.WriteLine("}}");
                output.WriteLine("</script>");
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), this.ID + "CommonTwoCascade_clientScript", "", true);
            }

            #endregion

            #region 写select部分

            //初始化
            output.Write("<table style=\"border: solid 0px #fff;width:100%\"><tr><td>");
            output.Write("<select id=\"" + this.ID + "_first\" name=\"" + this.ID + "_first\" onChange=\"" + this.ID + "redirectff(this.options.selectedIndex,document.all." + this.ID + "_second,document.all." + this.ID + ")\" ");
            //select的其他属性(如何将WebControl的属性很方便的全部应用到控件中呢)
            if (this.CssClass != "")
            {
                output.Write(" class=\"" + this.CssClass + "\"");
            }
            output.Write(">");
            //option
            output.Write("<option>请选择</option>");
            //找出初始选定的一级节点
            int pindex = 0;
            for (int i = 0; i < firstArr.Count; i++)
            {
                if (this.TextFirst != null)
                {
                    //根据选定值给出默认项
                    if (this.TextFirst.IndexOf(firstArr[i].ToString()) > -1)
                    {
                        output.Write("<option selected>" + firstArr[i].ToString() + "</option>");
                        pindex = i + 1;
                    }
                    else
                    {
                        output.Write("<option>" + firstArr[i].ToString() + "</option>");
                    }
                }
                else
                {
                    output.Write("<option>" + firstArr[i].ToString() + "</option>");
                }

            }
            output.Write("</select>");
            output.Write(FirstNodeName);
            output.Write("<select id=\"" + this.ID + "_second\" name=\"" + this.ID + "_second\" ");
            if (this.CssClass != "")
            {
                output.Write(" class=\"" + this.CssClass + "\"");
            }
            output.Write(">");
            output.Write("<option value=\"\">请选择</option>");
            int cindex = 0;
            //根据初始化选定的一级节点来选定二级节点
            if (pindex > 0)
            {
                ArrayList arr = (ArrayList)secondArr[pindex - 1];
                for (int i = 0; i < arr.Count; i++)
                {
                    if (this.TextSecond != null)
                    {
                        //根据选定值给出默认项
                        if (this.TextSecond.IndexOf(arr[i].ToString()) > -1)
                        {
                            output.Write("<option selected>" + arr[i].ToString() + "</option>");
                            cindex = i + 1;
                        }
                        else
                        {
                            output.Write("<option>" + arr[i].ToString() + "</option>");
                        }
                    }
                    else
                    {
                        output.Write("<option>" + arr[i].ToString() + "</option>");
                    }
                }
            }
            output.Write("</select>");
            output.Write(SecondNodeName);
            output.Write("</td></tr></table>");
            #endregion
        }

        /// <summary>
        /// 从XML文件中读取一级节点二级节点信息
        /// </summary>
        /// <returns></returns>
        private void GetCascadingInfo()
        {
            ArrayList arr = new ArrayList();

            XmlTextReader xr;
            if (!CascadeXmlFile.StartsWith("http://") && !CascadeXmlFile.StartsWith("https://"))
            {
                //从资源中获取
                string path = AppDomain.CurrentDomain.BaseDirectory;
                if (path.ToLower().EndsWith("bin"))
                {
                    path = path.Substring(0, path.Length - 4);
                }
                string filename = Path.Combine(path, CascadeXmlFile);
                xr = new XmlTextReader(filename);
            }
            else
            {
                xr = new XmlTextReader(CascadeXmlFile);
            }

            while (xr.Read())
            {
                if (xr.NodeType == XmlNodeType.Element && xr.Name == "first")
                {
                    firstArr.Add(xr.GetAttribute("name"));
                    arr = new ArrayList();
                }
                if (xr.NodeType == XmlNodeType.Element && xr.Name == "second")
                {
                    arr.Add(xr.GetAttribute("name"));
                }
                if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "first")
                {
                    secondArr.Add(arr);
                }
            }
            xr.Close();
        }

        #region IPostBackDataHandler 成员

        public event EventHandler TextChanged;

        /// <summary>
        /// 当由类实现时，用信号要求服务器控件对象通知 ASP.NET 应用程序该控件的状态已更改。
        /// </summary>
        public virtual void RaisePostDataChangedEvent()
        {
            OnTextChanged(EventArgs.Empty);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        /// <summary>
        /// 当由类实现时，为 ASP.NET 服务器控件处理回发数据。
        /// </summary>
        /// <param name="postDataKey">控件的主要标识符</param>
        /// <param name="postCollection">所有传入名称值的集合</param>
        /// <returns>如果服务器控件的状态在回发发生后更改，则为 true；否则为 false。</returns>
        public virtual bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool blReturn = false;
            //一级节点
            if (this.TextFirst == null || !this.Text.Equals(postCollection[this.ID + "_first"]))
            {
                ViewState[this.ID + "_TextFirst"] = postCollection[this.ID + "_first"];
                blReturn = true;
            }
            //二级节点
            if (this.TextSecond == null || !this.Text.Equals(postCollection[this.ID + "_second"]))
            {
                ViewState[this.ID + "_TextSecond"] = postCollection[this.ID + "_second"];
                blReturn = true;
            }
            return blReturn;
        }

        #endregion
    }
}
