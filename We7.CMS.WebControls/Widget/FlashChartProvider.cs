using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// FlashChar数据提供者
    /// </summary>
    public class FlashChartProvider : BaseWebControl
    {
        private string dataFile;
        const string renderJs = @"<script>if([Switch]){setInterval(function(){var c=document.getElementById('[Container]'); if(c){var p=c.parentNode;c=null;delete c; CollectGarbage();p.innerHTML='<div id=[Container]></div>';};swfobject.embedSWF('[VirtulPath]/js/open-flash-chart.swf', '[Container]','[Width]', '[Height]', '9.0.0', '[VirtulPath]/js/expressInstall.swf',{'data-file':'[DataFile]t='+new Date().getTime()})},[Interval]);}swfobject.embedSWF('[VirtulPath]/js/open-flash-chart.swf', '[Container]','[Width]', '[Height]', '9.0.0', '[VirtulPath]/js/expressInstall.swf',{'data-file':'[DataFile]t='+new Date().getTime()});</script>";

        /// <summary>
        /// 数据源的路径
        /// </summary>
        public string DataFile
        {
            get
            {
                if (DesignHelper.IsDesigning)
                {
                    dataFile = this.TemplateSourceDirectory + "/doc/line-dot.txt";
                }
                return dataFile;
            }
        }

        /// <summary>
        /// 控件的宽度
        /// </summary>
        public Unit Width { get; set; }

        private int interval;
        /// <summary>
        /// 刷新的时间间隔
        /// </summary>
        public int Interval
        {
            get
            {
                return interval;
            }
            set 
            {
                interval = value;
            }
        }

        /// <summary>
        /// 控件的高度
        /// </summary>
        public Unit Height { get; set; }

        /// <summary>
        /// 展示控件
        /// </summary>
        /// <returns></returns>
        protected string RenderChart()
        {
            string s = renderJs.Replace("[VirtulPath]", TemplateSourceDirectory);
            s = s.Replace("[Container]", this.ClientID);
            s = s.Replace("[Width]", Width.Value.ToString());
            s = s.Replace("[Height]", Height.Value.ToString());
            s = s.Replace("[DataFile]",DataFile+(DataFile.Contains("?")?"&":"?"));
            s = s.Replace("[Interval]", (Interval * 1000).ToString());
            s = s.Replace("[Switch]", Interval != 0 ? "true" : "false");
            return s;
        }

        protected override void OnLoad(EventArgs e)
        {
            AddMeta();
            IncludeJavaScript("swfobject.js");
            base.OnLoad(e);
        }

        protected void AddMeta()
        {
            if (Page.Header != null)
            {
                HtmlGenericControl meta = new HtmlGenericControl("meta");
                meta.Attributes["http-equiv"] = "pragma";
                meta.Attributes["content"] = "no-cache";
                Page.Header.Controls.AddAt(0,meta);
            }
        }
    }
}
