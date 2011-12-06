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
	/// ProvinceCityCascade 的摘要说明。
	/// 省市联动选择控件
	/// </summary>

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ProvinceCityCascade runat=server></{0}:ProvinceCityCascade>")]
	public class ProvinceCityCascade :  WebControl, IPostBackDataHandler
	{
		#region 属性

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取选择文本")]
        public string Text 
		{
			get
			{
                string province = "";
                string city = "";

                if (ViewState[this.ID + "_TextProvince"] != null)
                {
                    province = ViewState[this.ID + "_TextProvince"].ToString();
                }
                else
                {
                    province = "";
                }

                if (ViewState[this.ID + "_TextCity"] != null)
                {
                    city = ViewState[this.ID + "_TextCity"].ToString();
                }
                else
                {
                    city = "";
                }

                return province + "-" + city;
			}
            set
            {
                string[] temp = value.Split(new char[] { '-' });
                if (temp.Length>1)
                {
                    ViewState[this.ID + "_TextProvince"] = temp[0];
                    ViewState[this.ID + "_TextCity"] = temp[1];
                }
            }
		}

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置省")]
        public string TextProvince 
		{
			get
			{
                if (ViewState[this.ID + "_TextProvince"] != null)
				{
                    return ViewState[this.ID + "_TextProvince"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState[this.ID + "_TextProvince"] = value;
			}
		}

        [Bindable(true), Category("常用"), DefaultValue(""), Description("获取或设置市")]
        public string TextCity 
		{
			get
			{
                if (ViewState[this.ID + "_TextCity"] != null)
				{
                    return ViewState[this.ID + "_TextCity"].ToString();
				}
				else
				{
					return "";
				}
			}
			set
			{
                ViewState[this.ID + "_TextCity"] = value;
			}
		}	
        #endregion

		//省份
		private ArrayList parr = new ArrayList();
		//城市
		private ArrayList carr = new ArrayList();
		
		/// <summary>
		/// 将此控件呈现给指定的输出参数
		/// </summary>
		/// <param name="writer"> 要写出到的 HTML 编写器</param>
		protected override void Render(HtmlTextWriter output)
		{
			GetCascadingInfo();

			#region	省市连选所需的javascript函数

            /*****************省 市 县 代码*********/
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(string),"ProvinceCityCascade_clientScript"))
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
                        output.WriteLine("group[" + (i + 1) + "][" + (j + 1) + "]=new Option(\"" + arr[j].ToString() + "\",\"" + arr[j].ToString() + "\");");
                    }
                }
                /********************************************************************************/

				output.WriteLine("function redirectff(x,obj,objc){");						  
				output.WriteLine("for (m=obj.options.length-1;m>0;m--)");
				output.WriteLine("obj.options[m]=null;");
				output.WriteLine("for (i=1;i<group[x].length;i++){");
				output.WriteLine("obj.options[i]=new Option(group[x][i].text,group[x][i].value);");
				output.WriteLine("obj.options[0].selected=true;");
				output.WriteLine("}}");
				output.WriteLine("</script>");
				this.Page.ClientScript.RegisterClientScriptBlock(typeof(string),"ProvinceCityCascade_clientScript", "",true);
			}

			#endregion

			#region 写select部分

			//初始化
            output.Write("<table style=\"border: solid 0px #fff;width:100%\"><tr><td>");
            output.Write("<select id=\"" + this.ID + "_p\" name=\"" + this.ID + "_p\" onChange=\"redirectff(this.options.selectedIndex,document.all." + this.ID + "_c,document.all." + this.ID + ")\" ");
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
            output.Write("<select id=\"" + this.ID + "_c\" name=\"" + this.ID+ "_c\" ");
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
				}
				if(xr.NodeType == XmlNodeType.Element && xr.Name == "city")
				{
					arr.Add(xr.GetAttribute("name"));
				}
				if(xr.NodeType == XmlNodeType.EndElement && xr.Name == "province")
				{
					carr.Add(arr);
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
            if (this.TextProvince == null || !this.Text.Equals(postCollection[this.ID+ "_p"]))
            {
                ViewState[this.ID + "_TextProvince"] = postCollection[this.ID + "_p"];
                blReturn = true;
            }
            //市
            if (this.TextCity == null || !this.Text.Equals(postCollection[this.ID + "_c"]))
            {
                ViewState[this.ID + "_TextCity"] = postCollection[this.ID + "_c"];
                blReturn = true;
            }
            return blReturn;
		}

		#endregion
	}
}
