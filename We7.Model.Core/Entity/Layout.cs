using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 布局信息
    /// </summary>
    [Serializable]
    public class Layout
    {
        private PanelCollection panels = new PanelCollection();
        private UCControl ucControl = new UCControl();


        /// <summary>
        /// 面板集合
        /// </summary>
        [XmlElement("panel")]
        public PanelCollection Panels
        {
            get { return panels; }
            set { panels = value; }
        }

        /// <summary>
        /// 前台控件信息
        /// </summary>
        public UCControl UCContrl
        {
            get { return ucControl; }
            set { ucControl = value; }
        }
    }

    public class UCControl
    {
        /// <summary>
        /// 前台列表字段
        /// </summary>
        [XmlElement("listFields")]
        public string ListFields { get; set; }

        /// <summary>
        /// 前台详细信息字段
        /// </summary>
        [XmlElement("detailFields")]
        public string DetailFields { get; set; }

        /// <summary>
        /// 部件列表字段
        /// </summary>
        [XmlElement("widgetListFields")]
        public string WidgetListFields { get; set; }

        /// <summary>
        /// 部件详细信息字段
        /// </summary>
        [XmlElement("widgetDetailFields")]
        public string WidgetDetailFields { get; set; }

        /// <summary>
        /// 是否生成前台控件
        /// </summary>
        [XmlElement("createEditor")]
        public bool CreateEditor { get; set; }

        /// <summary>
        /// 后台录入布局
        /// </summary>
        [XmlElement("adminEditor")]
        public string AdminEidtor { get; set; }

        /// <summary>
        /// 后台录入样式
        /// </summary>
        [XmlElement("adminEditorCss")]
        public string AdminEditorCss { get; set; }

        /// <summary>
        /// 后台信息预览
        /// </summary>
        [XmlElement("adminViewer")]
        public string AdminViewer { get; set; }

        /// <summary>
        /// 后台信息预览样式
        /// </summary>
        [XmlElement("adminViewerCss")]
        public string AdminViewerCss { get; set; }


        public string[] ListFieldArray
        {
            get
            {
               return !String.IsNullOrEmpty(ListFields) ? ListFields.Split(',') : null;
            }
        }

        public string[] DetailFieldArray
        {
            get
            {
                return !String.IsNullOrEmpty(DetailFields) ? DetailFields.Split(',') : null;
            }
        }

        public string[] WidgetListFieldArray
        {
            get
            {
                return !String.IsNullOrEmpty(WidgetListFields) ? WidgetListFields.Split(',') : null;
            }
        }

        public string[] WidgetDetailFieldArray
        {
            get
            {
                return !String.IsNullOrEmpty(WidgetDetailFields) ? WidgetDetailFields.Split(',') : null;
            }
        }
    }
}
