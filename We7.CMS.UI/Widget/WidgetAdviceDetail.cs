using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls.Core;
using We7.CMS.Common;

namespace We7.CMS.UI.Widget
{
    [ControlGroupDescription(Label = "反馈模型部件", Description = "自动生成的反馈模型部件", Icon = "反馈模型部件", DefaultType = "")]
    [ControlDescription(Desc = "反馈模型详细信息部件(自动生成)")]
    public class WidgetAdviceDetail : AdviceProvider
    {
        public List<AdviceReplyInfo> replies;

        public List<AdviceReplyInfo> Replies
        {
            get
            {
                if (replies == null)
                {
                    replies = AdviceFactory.Create().QueryReplies(GetAdviceID());
                    if (replies == null) replies = new List<AdviceReplyInfo>();
                }
                return replies;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IncludeJavaScript("/Admin/Ajax/advicecheck.js");
        }
    }
}
