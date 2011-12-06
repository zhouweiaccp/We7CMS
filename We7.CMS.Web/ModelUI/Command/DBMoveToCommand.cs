using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using We7.Framework.Util;
using We7.Framework;
using We7.CMS;
using We7.CMS.Common;
using We7.Model.Core;
using We7.Framework.Helper;
using We7.Framework.Cache;

namespace We7.Model.UI.Command
{
    public class DBMoveToCommand : ICommand
    {
        #region ICommand 成员

        public object Do(PanelContext data)
        {
            string oid = data.Objects["oid"] as string;
            if (We7Helper.IsEmptyID(oid))
            {
                UIHelper.Message.AppendInfo(MessageType.ERROR, "不能移动到根栏目");
            }
            ChannelHelper chHelper = HelperFactory.Instance.GetHelper<ChannelHelper>();
            Channel channel = chHelper.GetChannel(oid, null);
            if (channel == null)
                throw new Exception("当前栏目不存在");
            if (channel.ModelName != data.ModelName)
                throw new Exception("移动到的栏目类型与当前栏目类型不一致");

            List<DataKey> dataKeys = data.State as List<DataKey>;
            foreach (DataKey key in dataKeys)
            {
                string id = key["ID"] as string;
                if (DbHelper.CheckTableExits(data.Table.Name))
                    DbHelper.ExecuteSql(String.Format("UPDATE [{0}] SET [OwnerID]='{2}' WHERE [ID]='{1}'", data.Table.Name, id, oid));
            }
            UIHelper.SendMessage("移动成功");
            CacheRecord.Create(data.ModelName).Release();
            return null;
        }

        #endregion
    }
}
