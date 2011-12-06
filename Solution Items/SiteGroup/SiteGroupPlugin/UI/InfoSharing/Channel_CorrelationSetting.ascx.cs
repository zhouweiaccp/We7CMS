using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebEngine2007.WebServices.ID;
using System.IO;
using We7.Plugin.DataSharing;
using We7.CMS.Config;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS;

namespace We7.Plugin.SiteGroupPlugin.InfoSharing
{
    public partial class Channel_CorrelationSetting : BaseUserControl
    {
        public string ChannelID
        {
            get { return Request["id"]; }
        }

        protected RemoteChannelHelper RemoteChannelHelper
        {
            get { return HelperFactory.GetHelper<RemoteChannelHelper>(); }
        }

        protected IDHelper IDHelper
        {
            get { return HelperFactory.GetHelper<IDHelper>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initilize();
            }
        }

        private void Initilize()
        {
            try
            {
                //所有当前站点的共享站点
                SitePartnership[] sps = null;

                //查找当前站点、当前栏目所创建的共享站点
                string siteID = CDHelper.GetSiteID();
                object objEnum = (object)EnumLibrary.SitePartnership.Sharing;
                sps = IDHelper.GetSharingSites(siteID, objEnum);

                if (sps != null)
                {
                    SiteDropDownList.Items.Clear();
                    foreach (SitePartnership sp in sps)
                    {
                        ListItem item = new ListItem();
                        item.Text = sp.ToSiteName;
                        item.Value = sp.ToSiteID;
                        SiteDropDownList.Items.Add(item);
                    }
                }

                if (SiteDropDownList.Items.Count > 0)
                {
                    SiteDropDownList.SelectedIndex = 0;
                    BindSelectForm(SiteDropDownList.SelectedValue);
                }

                //对已建立的关联关系进行查找并给赋值
                //获取当前站点siteID与channelID
                string fromSiteID = siteID;
                string fromChannelID = ChannelID;

                ChannelPartnership[] result = IDHelper.GetChannelPartnerships(fromSiteID, fromChannelID, objEnum);

                ChannelSelected.Items.Clear();

                if (result != null && result.Length>0)
                {
                    EnumLibrary.SiteAutoUsering useringType = (EnumLibrary.SiteAutoUsering)
                        StateMgr.GetStateValueEnum(result[0].EnumState, EnumLibrary.Business.SiteAutoUsering);
                    switch (useringType)
                    {
                        case EnumLibrary.SiteAutoUsering.MatchingUser:
                            IfAutoUseringCHK.Checked = true;
                            break;
                        case EnumLibrary.SiteAutoUsering.UnMatchingUser:
                            IfAutoUseringCHK.Checked = false;
                            break;
                        default:
                            break;
                    }

                    EnumLibrary.SiteSyncType syncType = (EnumLibrary.SiteSyncType)
                        StateMgr.GetStateValueEnum(result[0].EnumState, EnumLibrary.Business.SiteSyncType);
                    switch (syncType)
                    {
                        case EnumLibrary.SiteSyncType.ManualSync:
                            IfAutoSharingCHK.Checked = false;
                            break;
                        case EnumLibrary.SiteSyncType.AutoSync:
                            IfAutoSharingCHK.Checked = true;
                            break;
                        default:
                            break;
                    }

                    foreach (ChannelPartnership sp in result)
                    {
                        string value = sp.ToSiteID + "→" + sp.ToChannelID;
                        string text = sp.ToSiteName + "→" + sp.ToChannelName;
                        ListItem item = new ListItem(text, value);
                        ChannelSelected.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("页面初始化出错！出错原因：" + ex.Message);
            }
        }

        protected void SaveSiteButton_Click(object sender, EventArgs e)
        {
            try
            {
                //获取当前站点siteID与channelID
               SiteConfigInfo si = SiteConfigs.GetConfig();

                string fromSiteID = si.SiteID;
                string fromChannelID = ChannelID;
                string fromChannelName = ChannelHelper.GetFullPath(fromChannelID);

                //从前台页面获取关联站点ID及栏目ID
                string[] IDs = ListValue.Text.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                string[] Names = ListText.Text.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                //构造所需传递的状态码字串
                string strEnum = StateMgr.StateInitialize();

                if (IfAutoSharingCHK.Checked)
                {
                    strEnum = StateMgr.StateProcess(strEnum,
                        EnumLibrary.Business.SiteSyncType, (int)EnumLibrary.SiteSyncType.AutoSync);
                }
                else
                {
                    strEnum = StateMgr.StateProcess(strEnum,
                        EnumLibrary.Business.SiteSyncType, (int)EnumLibrary.SiteSyncType.ManualSync);
                }

                if (IfAutoUseringCHK.Checked)
                {
                    strEnum = StateMgr.StateProcess(strEnum,
                        EnumLibrary.Business.SiteAutoUsering, (int)EnumLibrary.SiteAutoUsering.MatchingUser);
                }
                else
                {
                    strEnum = StateMgr.StateProcess(strEnum,
                        EnumLibrary.Business.SiteAutoUsering, (int)EnumLibrary.SiteAutoUsering.UnMatchingUser);
                }

                IDHelper.UpdateChannelPartnerships(fromSiteID, fromChannelID, fromChannelName, IDs, Names, strEnum);

                Initilize();

                Messages.ShowMessage("栏目关联关系保存成功！");
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("栏目关联更新失败！出错原因：" + ex.Message);
            }
        }

        protected void SiteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string siteID = SiteDropDownList.SelectedValue;
            BindSelectForm(siteID);
        }

        public void BindSelectForm(string siteID)
        {
            ChannelSelect.NodeLevelList = RemoteChannelHelper.GetRemoteChannelNodeLevel(siteID, We7Helper.EmptyGUID);
            ChannelSelect.ChannelList = RemoteChannelHelper.GetRemoteChannels(siteID, We7Helper.EmptyGUID);
            MemoryStream memoryStream = new MemoryStream();
            TextWriter textWriter = new StreamWriter(memoryStream, System.Text.Encoding.UTF8);
            HtmlTextWriter htm = new HtmlTextWriter(textWriter);
            ChannelSelect.RenderControl(htm);
        }

    }
}