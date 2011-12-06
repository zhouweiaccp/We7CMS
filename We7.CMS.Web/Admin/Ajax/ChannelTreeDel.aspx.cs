using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ChannelTreeDel : BasePage
    {
        string ChannelID
        {
            get
            {
                if (Request["node"] != null && Request["node"].ToString() != "root")
                    return We7Helper.FormatToGUID(Request["node"]);
                else
                    return string.Empty;
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string ret=DelChannel();
            Response.Write(ret);
            Response.End();
        }

        string DelChannel()
        {
            if (!CheckChannelPermission())
            {
                return "无法删除此栏目，没有权限！";
            }

            if (!We7Helper.IsEmptyID(ChannelID))
            {
                List<Channel> listSon = ChannelHelper.GetChannels(ChannelID);
                if (listSon != null)
                {
                    if (listSon.Count > 0)
                    {
                        return "栏目下有子栏目不能删除，请您先删除子栏目后再试。";
                    }
                }
                //删除节点
                Channel ch= ChannelHelper.GetChannel(ChannelID, new string[] { "Name","FullUrl" });
                ChannelHelper.DeleteChannel(ChannelID);
                TemplateMap.DeleteChannelUrls(ch.FullUrl);
                TemplateMap.ResetInstance();

                //记录日志
                string content = string.Format("删除栏目:“{0}”", ch.Name);
                AddLog("栏目管理", content);
            }
            return "0";
        }

        /// 检查栏目权限
        /// </summary>
        /// <returns></returns>
        bool CheckChannelPermission()
        {
            bool canUpdate = AccountID == We7Helper.EmptyGUID;

            if (!canUpdate)
            {
                if (ChannelID != null)
                {
                    List<string> ps = AccountHelper.GetPermissionContents(AccountID, ChannelID);
                    if (ps.Contains("Channel.Admin"))
                    {
                        canUpdate = true;
                    }
                }
            }
            return canUpdate;
        }
    }
}
