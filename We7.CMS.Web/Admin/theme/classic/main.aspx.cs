using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Thinkment.Data;
using Newtonsoft.Json;

namespace We7.CMS.Web.Admin
{
    public partial class main : BasePage
    {
        #region 字段属性
        protected string RequestAction
        {
            get
            {
                return Request["action"];
            }
        }
        
        #endregion

        /// <summary>
        /// 是否判断用户权限
        /// </summary>
        protected override bool NeedAnPermission
        {
            get
            {
                if (AccountHelper.GetAccount(AccountID, new string[] { "UserType" }).UserType == 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 页面初始化
        ///     检查商城Service是否可用
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestAction)
            {
                case "shop":
                    GetShop();
                    break;
                case "product":
                    GetProducts();
                    break;
            }
        }

        protected void GetProducts()
        {
            //检查商城的ShopService是否可用
            bool isShopServiceAvailable = base.IsShopServicesCanWork();

            if (isShopServiceAvailable)
            {
                 List<We7.CMS.ShopService.ProductInfo> list =  base.GetRecommendProduct(5);
                 if (list != null && list.Count > 0)
                 {
                     List<JsonProductModel> listResult = new List<JsonProductModel>();
                     for (int i = 0; i < list.Count; i++)
                     {
                         JsonProductModel model = new JsonProductModel();
                         model.Name = list[i].Name;
                         model.NameHtml = GetChopString(model.Name, 8, "...");
                         model.LevelHtml = GetLevelString(list[i].Level.ToString());
                         model.PageUrl = list[i].PageUrl;
                         model.Thumbnail = list[i].Thumbnail;
                         model.Point = list[i].Point.ToString();
                         model.Price = list[i].Price.ToString();
                         model.Sales = list[i].Sales.ToString();
                         listResult.Add(model);
                     }
                     string result =  JavaScriptConvert.SerializeObject(listResult);
                     Response.Clear();
                     Response.Write(result);
                     Response.Flush();
                     Response.End();
                 }
            }
        }

        protected void GetShop()
        {
            //检查商城的ShopService是否可用
            bool isShopServiceAvailable = base.IsShopServicesCanWork();

            if (isShopServiceAvailable)
            {
                List<We7.CMS.ShopService.StoreModel> list = base.GetRecommendStore(5);
                if (list != null && list.Count > 0)
                {
                    List<JsonShopModel> listResult = new List<JsonShopModel>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        JsonShopModel model = new JsonShopModel();
                        model.Url = list[i].Url;
                        model.NameHtml = GetChopString(list[i].StoreName, 10, "...");
                        model.LevelHtml = GetLevelString(list[i].Level.ToString());
                        model.StoreIntro = GetClearHtml(list[i].StoreIntro);
                        model.Face = list[i].Face;
                        model.TechnicalLevel = list[i].TechnicalLevel;
                        listResult.Add(model);
                    }
                    string result = JavaScriptConvert.SerializeObject(listResult);
                    Response.Clear();
                    Response.Write(result);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }

    /// <summary>
    /// 推荐产品JSON模型
    /// </summary>
    public class JsonProductModel
    {
        public string PageUrl { get; set; }
        public string Thumbnail { get; set; }
        public string Name { get; set; }
        public string NameHtml { get; set; }
        public string Price { get; set; }
        public string Sales { get; set; }
        public string LevelHtml { get; set; }
        public string Point { get; set; }
    }

    /// <summary>
    /// 推荐商铺JSON模型
    /// </summary>
    public class JsonShopModel
    {
        public string Url { get; set; }
        public string StoreIntro { get; set; }
        public string Face { get; set; }
        public string NameHtml { get; set; }
        public string TechnicalLevel { get; set; }
        public string LevelHtml { get; set; }
    }
}
