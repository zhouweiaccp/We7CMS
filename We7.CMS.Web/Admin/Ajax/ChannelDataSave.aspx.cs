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
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ChannelDataSave : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

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

        string ParentlID
        {
            get
            {
                if (Request["newParent"] != null && Request["newParent"].ToString() != "root")
                    return We7Helper.FormatToGUID(Request["newParent"]);
                else
                    return string.Empty;
            }
        }

        int ChannelIndex
        {
            get
            {
                if (Request["index"] != null)
                    return int.Parse(Request["index"].ToString());
                else
                    return 0;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
             string id = ChannelID;
             string parentId = ParentlID;
             if (We7Helper.IsEmptyID(id))
                 id = We7Helper.EmptyGUID;
             if (We7Helper.IsEmptyID(parentId))
                 parentId = We7Helper.EmptyGUID;

             string ret=SaveChannel(id, parentId, ChannelIndex);

             Response.Write(ret);
             Response.End();
        }

        string SaveChannel(string id,string parentId,int index)
        {
            if (!CheckChannelPermission())
            {
                return "您没有操作权限。";
            }

            try
            {
                //更新子节点的父节点属性
                Channel c = new Channel();
                c = ChannelHelper.GetChannel(id, null);
                if (c.ParentID != parentId)
                {
                    if (CanMove(c, parentId))
                    {
                        string oldUrl = c.FullUrl;
                        string oldPath = c.FullPath;
                        c.ParentID = parentId;
                        ChannelHelper.UpdateChannel(c);
                        //ChannelHelper.UpdateChannelUrlBatch(oldUrl, newUrl);
                        ChannelHelper.UpdateChannelUrlBatch2(oldUrl, c.FullUrl);
                        ChannelHelper.UpdateChannelPathBatch(c,oldPath);
                        TemplateMap.ReplaceChannelUrls(oldUrl, c.FullUrl);
                        TemplateMap.ResetInstance();
                    }
                    else
                        return "无法移动栏目，目标栏目下有标识为 " + c.ChannelName + " 的子栏目！";
                }
                if (c.Index != index)
                    ResortChannelList(id, c.ParentID, index);
                return "0";
            }
            catch (Exception ex)
            {
                return "无法保存数据！"+ex.Message;
            }
        }

        bool CanMove(Channel c, string newParentID)
        {
            string url = "";
            if (We7Helper.IsEmptyID(newParentID))
                url = "/" + c.ChannelName + "/";
            else
            {
                Channel pc = ChannelHelper.GetChannel(newParentID,null);
                if (pc != null)
                    url = pc.FullUrl + c.ChannelName + "/";
            }
            if (url != "")
            {
                string id = ChannelHelper.GetChannelIDByFullUrl(url);
                return id == We7Helper.NotFoundID;
            }
            return false;
        }

        /// <summary>
        /// 重新排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentID"></param>
        /// <param name="currentIndex"></param>
        void ResortChannelList(string id, string parentID, int currentIndex)
        {
            List<Channel> list = ChannelHelper.GetChannels(parentID);
            
            int oldIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id) oldIndex = i;
                list[i].Index = i;
            }

            if (oldIndex > currentIndex)
            {
                for (int j = oldIndex; j >= currentIndex; j--)
                {
                    if (list[j].ID == id)
                        list[j].Index = currentIndex;
                    else
                        list[j].Index = j+1;
                }
            }
            else
            {
                for (int j = oldIndex; j < currentIndex; j++)
                {
                    if (list[j].ID == id)
                    {
                        if (currentIndex > list.Count - 1)
                            list[j].Index = currentIndex - 1;
                        else
                            list[j].Index = currentIndex;
                    }
                    else
                        list[j].Index = j - 1;
                }
            }

            foreach (Channel ch in list)
            {
                ChannelHelper.UpdateChannel(ch);
            }
        }

        /// <summary>
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
