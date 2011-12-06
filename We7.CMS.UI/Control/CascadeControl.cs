using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace We7.CMS.Controls
{
	/// <summary>
	/// CascadeControl 的摘要说明。
	/// 省市县联动选择控件
	/// </summary>

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CascadeControl runat=server></{0}:CascadeControl>")]
	public class CascadeControl :  WebControl, IPostBackDataHandler
	{
		#region 属性

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取选择文本")]
        public string Text 
		{
			get
			{
                return this.TextProvince + this.TextCity + this.TextCounty;
			}
		}
        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置省")]
        public string TextProvince 
		{
			get
			{
                if (ViewState["TextProvince"] != null)
				{
                    return ViewState["TextProvince"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState["TextProvince"] = value;
			}
		}

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置市")]
        public string TextCity 
		{
			get
			{
                if (ViewState["TextCity"] != null)
				{
                    return ViewState["TextCity"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState["TextCity"] = value;
			}
		}

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置县")]
        public string TextCounty 
		{
			get
			{
                if (ViewState["TextCounty"] != null)
				{
                    return ViewState["TextCounty"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState["TextCounty"] = value;
			}
		}		
        #endregion

		//省份
		private ArrayList parr = new ArrayList();
		//城市
		private ArrayList carr = new ArrayList();
		//县
		private ArrayList darr = new ArrayList();
		
		/// <summary>
		/// 将此控件呈现给指定的输出参数
		/// </summary>
		/// <param name="writer"> 要写出到的 HTML 编写器</param>
		protected override void Render(HtmlTextWriter output)
		{
			GetCascadingInfo();

			#region	省市县连选所需的javascript函数

            /*****************省 市 县 代码*********/
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(string),"CascadeControl_clientScript"))
			{
				output.WriteLine("<script language=\"JavaScript\">");
				output.WriteLine("var groups="+(parr.Count+1)+";");
				output.WriteLine("var group=new Array(groups);");
				output.WriteLine("for (i=0; i<groups; i++)");
				output.WriteLine("group[i]=new Array();");
				output.WriteLine("group[0][0]=new Option(\"请选择\",\"\");");

                for (int i = 0; i < carr.Count; i++)
                {
                    output.WriteLine("group[" + (i + 1) + "][0]=new Option(\"请选择\",\"" + parr[i].ToString() + "\");");
                    ArrayList arr = (ArrayList)carr[i];
                    for (int j = 0; j < arr.Count; j++)
                    {
                        output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "]=new Array();");
                        output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "][0]=new Option(\"" + arr[j].ToString() + "\",\"" + arr[j].ToString() + "\");");
                        ArrayList drr = (ArrayList)((ArrayList)darr[i])[j];
                        for (int m = 0; m < drr.Count; m++)
                        {
                            output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "][" + (m + 1) + "]=new Option(\"" + drr[m].ToString() + "\",\"" + drr[m].ToString() + "\");");
                        }
                    }
                }
                /********************************************************************************/


				output.WriteLine("function redirectff(x,obj,objc){");						  
				output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
				output.WriteLine("obj.options[m]=null;");
				output.WriteLine("for (i=1;i<group[x].length;i++){");
				output.WriteLine("obj.options[i]=new Option(group[x][i][0].text,group[x][i][0].value);");
				output.WriteLine("obj.options[0].selected=true;");
				output.WriteLine("}}");
                output.WriteLine("function redirectffcounty(x,y,obj){");						  
				output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
				output.WriteLine("obj.options[m]=null;");
				output.WriteLine("for (i=1;i<group[x][y].length;i++){");
				output.WriteLine("obj.options[i]=new Option(group[x][y][i].text,group[x][y][i].value);");
				output.WriteLine("obj.options[0].selected=true;");
				output.WriteLine("}}");
				output.WriteLine("</script>");
				this.Page.ClientScript.RegisterClientScriptBlock(typeof(string),"CascadeControl_clientScript", "",true);
			}

			#endregion

			#region 写select部分

			//初始化
            output.Write("<table style=\"border: solid 0px #fff;width:100%\"><tr><td>");
            output.Write("<select id=\"" + this.UniqueID + "_p\" name=\"" + this.UniqueID + "_p\" onChange=\"redirectff(this.options.selectedIndex,document.all." + this.UniqueID + "_c,document.all." + this.UniqueID + ")\" ");
			//select的其他属性(如何将WebControl的属性很方便的全部应用到控件中呢)
			if (this.CssClass != "")
			{
				output.Write(" class=\""+this.CssClass+"\"");
			}
			output.Write(">");
			//option
			output.Write("<option>请选择</option>");
			//找出初始选定的省
			int pindex = 0;
			for (int i=0;i<parr.Count;i++)
			{
				if(this.TextProvince != null)
				{
					//根据选定值给出默认项
                    if (this.TextProvince.IndexOf(parr[i].ToString()) > -1)
					{
						output.Write("<option selected>"+parr[i].ToString()+"</option>");
						pindex = i+1;
					}
					else
					{
						output.Write("<option>"+parr[i].ToString()+"</option>");
					}
				}
				else
				{
					output.Write("<option>"+parr[i].ToString()+"</option>");
				}
				
			}
			output.Write("</select>");
			output.Write("省");
            output.Write("<select id=\"" + this.UniqueID + "_c\" name=\"" + this.UniqueID + "_c\" onChange=\"redirectffcounty(document.all." + this.UniqueID + "_p.options.selectedIndex,this.options.selectedIndex,document.all." + this.UniqueID + ")\" ");
			if (this.CssClass != "")
			{
				output.Write(" class=\""+this.CssClass+"\"");
			}
			output.Write(">");
			output.Write("<option value=\"\">请选择</option>");
            int cindex = 0;
			//根据初始化选定的省来选定市
			if (pindex > 0)
			{
				ArrayList arr = (ArrayList)carr[pindex-1];
				for(int i=0;i<arr.Count;i++)
				{
                    if (this.TextCity != null)
                    {
                        //根据选定值给出默认项
                        if (this.TextCity.IndexOf(arr[i].ToString()) > -1)
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
			output.Write("市");
            output.Write("<select id=\"" + this.UniqueID + "\" name=\"" + this.UniqueID + "\"");
            if (this.CssClass != "")
            {
                output.Write(" class=\"" + this.CssClass + "\"");
            }
            output.Write(">");
            output.Write("<option value=\"\">请选择</option>");
            //根据初始化选定的市来选定县
            if (cindex > 0)
            {
                ArrayList arr = (ArrayList)((ArrayList)darr[pindex-1])[cindex-1];
                for (int i = 0; i < arr.Count; i++)
                {
                    if (this.TextCounty != null)
                    {
                        //根据选定值给出默认项
                        if (this.TextCounty.IndexOf(arr[i].ToString()) > -1)
                        {
                            output.Write("<option selected>" + arr[i].ToString() + "</option>");
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
            output.Write("县");
            output.Write("</td></tr></table>");
            #endregion
		}

		/// <summary>
		/// 从XML文件中读取省市县信息
		/// </summary>
		/// <returns></returns>
		private void GetCascadingInfo()
		{
			ArrayList arr = new ArrayList();
			ArrayList drr = new ArrayList();
			ArrayList zrr = new ArrayList();

			//从资源中获取
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream file = thisExe.GetManifestResourceStream("We7.CMS.Controls.CascadeControl.xml");
			XmlTextReader xr = new XmlTextReader(file);

			while(xr.Read())
			{
				if(xr.NodeType == XmlNodeType.Element && xr.Name == "province")
				{
					parr.Add(xr.GetAttribute("name"));
					arr = new ArrayList();
                    drr = new ArrayList();
				}
				if(xr.NodeType == XmlNodeType.Element && xr.Name == "city")
				{
					arr.Add(xr.GetAttribute("name"));
                    zrr = new ArrayList();
				}
                if (xr.NodeType == XmlNodeType.Element && xr.Name == "county")
				{
                    zrr.Add(xr.GetAttribute("name"));
				}
                if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "city")
				{
                    drr.Add(zrr);
				}
				if(xr.NodeType == XmlNodeType.EndElement && xr.Name == "province")
				{
					carr.Add(arr);
					darr.Add(drr);
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
				TextChanged(this,e);
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
            //省
            if (this.TextProvince == null || !this.Text.Equals(postCollection[this.UniqueID + "_p"]))
            {
                ViewState["TextProvince"] = postCollection[this.UniqueID + "_p"];
                blReturn = true;
            }
            //市
            if (this.TextCity == null || !this.Text.Equals(postCollection[this.UniqueID + "_c"]))
            {
                ViewState["TextCity"] = postCollection[this.UniqueID + "_c"];
                blReturn = true;
            }
            //县
            if (this.TextCounty == null || !this.Text.Equals(postCollection[this.UniqueID]))
            {
                ViewState["TextCounty"] = postCollection[this.UniqueID];
                blReturn = true;
            }

            return blReturn;
		}

		#endregion
	}
}
