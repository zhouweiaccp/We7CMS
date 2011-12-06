using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Web.UI.HtmlControls;
using We7.Model.Core.Config;

namespace We7.Model.Core.UI
{
    public class LayoutEditor : UserControl
    {
        private UIHelper uiHlper;
        private bool IsInitialized;

        public PanelContext PanelContext { get; set; }

        /// <summary>
        /// 是否是浏览控件
        /// </summary>
        public bool IsViewer { get; set; }

        /// <summary>
        /// 布局控件样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 当前面板信息
        /// </summary>
        public Panel Panel
        {
            get { return PanelContext.Panel; }
        }

        public UIHelper UIHelper
        {
            get
            {
                if (uiHlper == null)
                {
                    uiHlper = new UIHelper(Page, PanelContext);
                }
                return uiHlper;
            }
        }

        public void InitLayout(PanelContext ctx)
        {
            
            PanelContext = ctx;
            if (!IsInitialized)
            {
                if (IsViewer)
                {
                    if (!String.IsNullOrEmpty(PanelContext.Panel.EditInfo.ViewerCss))
                    {
                        HtmlLink link = new HtmlLink();
                        link.Href = PanelContext.Panel.EditInfo.ViewerCss;
                        link.Attributes["type"] = "text/css";
                        link.Attributes["rel"] = "stylesheet";
                        Page.Header.Controls.Add(link);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(PanelContext.Panel.EditInfo.EditCss))
                    {
                        HtmlLink link = new HtmlLink();
                        link.Href = PanelContext.Panel.EditInfo.EditCss; ;
                        link.Attributes["type"] = "text/css";
                        link.Attributes["rel"] = "stylesheet";
                        Page.Header.Controls.Add(link);
                    }
                }
                IsInitialized = true;
            }

            foreach (We7Control ctr in Panel.EditInfo.Controls)
            {
                if (IsViewer && String.Compare("ID", ctr.Name) == 0)
                    continue;
                We7Control control = new We7Control();
                foreach (PropertyInfo prop in ctr.GetType().GetProperties())
                {
                    prop.SetValue(control, prop.GetValue(ctr, null), null);
                }
                if (IsViewer && !EnableControls.Contains(control.Type))
                {
                    control.Type = "Text";
                }
                PlaceHolder c = UIHelper.GetControl<PlaceHolder>("_" + control.ID, this);
                if (c != null)
                {
                    c.Controls.Clear();
                    c.Controls.Add(UIHelper.GetControl(control));
                }
                else
                {
                    FieldControl fc = UIHelper.GetControl<FieldControl>(control.ID, this);
                    if (UIHelper.GetControl<FieldControl>(control.ID, this) != null)
                        Controls.Remove(fc);
                    Controls.Add(UIHelper.GetControl(control));
                }
            }     
        }

        protected override void OnLoad(EventArgs e)
        {
            
        }

        private List<string> enableControls;
        protected List<string> EnableControls
        {
            get
            {
                if (enableControls == null)
                {
                    enableControls = ModelConfig.GetConfig().ViewerControl ?? new List<string>();
                }
                return enableControls;
            }
        }
    }
}
