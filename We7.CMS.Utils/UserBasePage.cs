using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS
{
    public class UserBasePage:BasePage
    {
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.User;
            }
        }
    }
}
