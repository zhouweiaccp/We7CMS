using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Framework.Util;

namespace We7.Model.Core.UI
{
    public class ModelHandlerPage:Page
    {
        //取得当前信息的ID
        public string RecordID
        {
            get 
            {
                if (ViewState["$RecordID"] == null)
                {
                    ViewState["$RecordID"] = Utils.CreateGUID();
                }
                return ViewState["$RecordID"] as string;
            }
            set
            {
                ViewState["$RecordID"] = value;
            }
        }
    }
}
