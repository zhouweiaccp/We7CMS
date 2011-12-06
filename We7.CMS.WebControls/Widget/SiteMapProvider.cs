using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web.UI.WebControls;
using System.Data;

namespace We7.CMS.WebControls
{
    public class SiteMapProvider : BaseWebControl
    {
        #region 属性面板参数
        public string CssClass { get; set; }
        #endregion
        protected void GetAllChildChannel(string parentID, string type)
        {
            List<Channel> childChannels = ChannelHelper.GetChannels(parentID,true);
            if (childChannels != null)
            {
                for (int j = 0; j < childChannels.Count; j++)
                {
                    if (type == "One")
                    {
                        Response.Write(String.Format("<a href=\"{0}\">{1}</a>", childChannels[j].FullUrl, childChannels[j].Name));
                    }
                    else
                    {
                        Response.Write(String.Format("<li><a href=\"{0}\">{1}</a></li>", childChannels[j].FullUrl, childChannels[j].Name));
                    }
                    //GetAllChildChannel(childChannels[j].ID, type);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

    }
}
