using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Framework;
using We7.CMS;
using We7.CMS.Common;

namespace We7.Model.UI.Controls.system
{
    public partial class CategoryDoubleCascade : FieldControl
    {
        public override void InitControl()
        {
            string keyword = Control.Params["keyword"];
            CategoryHelper helper = HelperFactory.Instance.GetHelper<CategoryHelper>();
            List<Category> col = helper.GetChildrenListByKeyword(keyword);

            Field1DropDownList.DataSource = col;
            Field1DropDownList.DataTextField = "Name";
            Field1DropDownList.DataValueField = "ID";
            Field1DropDownList.DataBind();
            Field1DropDownList.Items.Insert(0, new ListItem("请选择", ""));
            Field1DropDownList.SelectedValue = Value as string;

            Field1DropDownList.Attributes.Add("onchange", "getSubcate(this)");
            Field2DropDownList.Attributes.Add("onchange", "subcateChange('" + Field2DropDownList.ClientID + "')");

            //已有内容还原
            if(Value!=null)
            {
                string keywordSubcate =  Value.ToString();
                Field2Hidden.Value = keywordSubcate;                
                List<Category> colSubcates = helper.GetSiblingListByKeyword(keywordSubcate);
                if(colSubcates!=null)
                {
                    Field2DropDownList.DataSource = colSubcates;
                    Field2DropDownList.DataTextField = "Name";
                    Field2DropDownList.DataValueField = "KeyWord";               
                    Field2DropDownList.DataBind();
                    Field2DropDownList.Items.Insert(0, new ListItem("请选择", ""));

                    Category parent = helper.GetCategory(colSubcates[0].ParentID);                           
                    //默认选中索引
                    int parentSelectIndex = col.FindIndex(p => p.ID == parent.ID) + 1;
                    int subSelectedIndex = colSubcates.FindIndex(p => p.KeyWord == keywordSubcate) + 1;         

                    Field2DropDownList.SelectedIndex = subSelectedIndex;
                    Field1DropDownList.SelectedIndex = parentSelectIndex;
                }
            }
        }

        public override object GetValue()
        {
            return Request.Form[Field2Hidden.UniqueID];
        }
    }
}