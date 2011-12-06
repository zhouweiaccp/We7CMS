using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.IO;
using We7.CMS.Helpers;
using System.Web.UI;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
    public class ShopWindowJson : BasePage, IHttpHandler
    {
        Article ThisArticle = new Article();
        
        /// <summary>
        /// 业务助手工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)(HttpContext.Current.Application[HelperFactory.ApplicationID]); }
        }

        protected ArticleHelper ArticleHelper
        {
            get
            {
                return HelperFactory.GetHelper<ArticleHelper>();
            }
        }

        string linkID = "0";
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {            
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            linkID = context.Request["linkid"];
            string aid = linkID.Split('_')[0];
            ThisArticle = ArticleHelper.GetArticle(aid);
            context.Response.Write(BuildJson());
        }

        void BuildStrKV(StringBuilder sb, string key, object value)
        {
            sb.AppendFormat("{0}:'{1}'", key, value);
        }

        void BuildObjKV(StringBuilder sb, string key, object value)
        {
            sb.AppendFormat("{0}:{1}", key, value);
        }

        public string BuildJson()
        {
            string gid = linkID.Split('_')[1];
            string aid = linkID.Split('_')[0];

            Article a = ThisArticle;
            ShopWindow win = new ShopWindow();
            win.GroupID = gid;
            win.LinkID = linkID;
            ShopWindowImage img = new ShopWindowImage();
            if (a != null)
            {
                img.StyleNumber = a.ID.Trim('{').Trim('}').Split('-')[4];
                img.Name = a.Title;
                img.Desc = BuildContent(a.Content);
                img.Thumbnail = GetThumbUrl(a.Thumbnail, "_pt");
                img.MinThumbnail = GetThumbUrl(a.Thumbnail, "_pm");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "f");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "b");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "l");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "r");
                win.Images.Add(img);
            }
            return win.ToJson(ThisArticle.Title, img.Thumbnail);
        }

        public void AddThumb(List<string> Full, List<string> Zoom, string path, string ext)
        {
            string f = GetThumbUrl(path, "_p" + ext + "f");
            string z = GetThumbUrl(path, "_p" + ext + "z");
            if (!String.IsNullOrEmpty(f) && !String.IsNullOrEmpty(z))
            {
                Full.Add(f);
                Zoom.Add(z);
            }
        }

        public string GetThumbUrl(string path, string add)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            int i = path.LastIndexOf('.');
            path = path.Substring(0, i) + add + path.Substring(i);
            string s = path.TrimStart('~').TrimStart('/');
            s = "~/" + s;
            if (File.Exists(HttpContext.Current.Server.MapPath(s)))
                return path;
            return "";
        }

        string BuildContent(string content)
        {
            if (String.IsNullOrEmpty(content))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            StringReader sr = new StringReader(content);
            string s;
            while (!String.IsNullOrEmpty(s = sr.ReadLine()))
            {
                sb.AppendFormat("<li>{0}</li>", s);
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }


    public class ShopWindow
    {
        public ShopWindow()
        {
            Images = new List<ShopWindowImage>();
        }

        public string GroupID { get; set; }

        public string LinkID { get; set; }

        public List<ShopWindowImage> Images { get; set; }

        public string ToJson(string title, string thumb)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{").Append(GroupID).Append(":").Append("{");
            sb.AppendFormat("{0}:'{1}'", "link_id", LinkID);
            sb.AppendFormat(",{0}:'{1}'", "path", "path");
            sb.AppendFormat(",{0}:{1}", "panel_id", 232504);
            BuildImageList(sb, Images);
            BuildTemplate(sb, title, thumb);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 业务助手工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)(HttpContext.Current.Application[HelperFactory.ApplicationID]); }
        }

        void BuildTemplate(StringBuilder sb, string title, string thumb)
        {
            string aid = HttpContext.Current.Request["linkid"].Split('_')[0];
            bool isAdd = false;
            if (Security.CurrentAccountID != null)
            {
                isAdd = HelperFactory.GetHelper<FavoriteHelper>().IsAddFavorite(Security.CurrentAccountID, aid);
            }
            string languageFlag = "chinese";
            string addFavorite = isAdd ? "已在您的收藏之内" : "我要收藏";
            string sendEmail = "发邮件给您的朋友";
            string print = "打印";
            string close = "关闭";
            string account = "我的账号";
            string favorite = "我的收藏夹";
            languageFlag = HttpContext.Current.Request["languageFlag"];
            if (languageFlag == "english")
            {
                addFavorite = isAdd ? "Already in my wishlist" : "Add to my wishlist";
                sendEmail = "Email to your friends";
                print = "Print";
                close = "Close";
                account = "My Account";
                favorite = "My WishList";
            }

            sb.Append(@",accessories :[],
		                systemtexts : {
			                print : ""\u6253\u5370"",
			                close : ""\u5173\u95ed"",
			                back : ""\u8fd4\u56de"",
			                select_a_style : ""\u9009\u62e9\u6b3e\u5f0f"",
			                size_select : ""\u9009\u62e9\u5c3a\u7801""					},
		                templates : {
			                displaygroup_template: {
				                templateString: '<div id=""details_\\#{lightbox}_#{style_number}"" style=""display:none"" class=""product_details"">  <ul class=""functions""><li><a href=""#"" onclick=""Shop.closeLightbox($(this).up(\'div.lightbox\')); return false"">" + close + @"</a></li><li><a href=""#"" onclick=""Shop.printStyle(\'#{displayGroup.displayGroup_id}\',this); return false"">" + print + @"</a></li><li><a href=""#"" onclick=""Shop.showSendToAFriend(\'#{displayGroup.displayGroup_id}\',this); return false;"">" + sendEmail + @"</a></li> </ul>    <ul class=""headline personal-shopper-headline"" style=""display:none""></ul>    <p class=""description"">#{text.grpdesc}</p><div class=""style active-style""><div class=""description-variation""><div class=""scrollbar""><div class=""handle""></div></div><div id=""divInfor"">#{text.vardesc}</div></div><div class=""style-information""><div class=""style-number""><label id=""lbAdd""  style=""cursor:pointer"" onclick=""return SetCookie(\'/User/FavoriteEdit.aspx?url=\'+escape(window.location.href)+\'&title=" + HttpUtility.UrlEncode(title) + @"&aid=" + aid + @"&thumbnail=" + thumb + @"\',document.getElementById(\'divInfor\').innerHTML) "">" + addFavorite + @"</label></div><div class=""style-number""><a style=""color: rgb(117, 101, 85); text-decoration: none; margin-left: 5px;"" href=""/User/index.aspx"">" + account + @"</A></div><div class=""style-number""><A style=""color: rgb(117, 101, 85); text-decoration: none; margin-left: 5px;"" href=""/User/FavoriteList.aspx"">" + favorite + @"</A></div><div class=""style-number"">#{display_style_number}</div></div> <div class=""clear""></div></div><div class=""configuration-info""><p class=""availability""></p><div class=""button addtobag"" style=""display:none"" onclick=""Shop.addToBag(this)""> <div class=""content""></div>      <div class=""end""></div>    </div>    <div class=""button backorder"" style=""display:none"" onclick=""Shop.submitBackorder(this)"">      <div class=""content""></div>      <div class=""end""></div>    </div>    <div class=""button checkout"" style=""display:none"" onclick=""Shop.gotoShoppingBag(this)"">      <div class=""content""></div>      <div class=""end""></div>    </div>    <p class=""info"" style=""display:block""></p>  </div></div>',
				                collection: 'variations',
				                templates: {
					                sizes_template: {
						                templateString: '<div class=""size"">  <span class=""size-info""></span> <select class=""size-select"" onchange=""Shop.selectSize(this);""><option>#{displayGroup.systemtexts.size_select}</option> #{_templates.option_template.renderedString} </select>   </div>',
						                templates: {
							                option_template: {
								                templateString: '<option value=""#{sku}"">#{sizename}</option>',
								                collection: ""skus""
							                }
						                }
					                }
				                }
			                },
						                personalshopper_template: {
				                templateString: '<ul class=\""functions\""><li><a href=\""#\"" onclick=\""Shop.closeShopper(this); return false\"">close</a></li></ul><p style=\""padding-bottom:5px\"">u.s. continental clients only</p><form action=\""#\"" method=\""post\"" onsubmit=\""Shop.submitShopper(this); return false;\""><div class=\""form-row \""><input type=\""radio\"" name=\""title\"" value=\""先生\"">&#160;先生  <input type=\""radio\"" name=\""title\"" value=\""女士\"">&#160;女士  </div><div class=\""form-row \"">first name<br/><input type=\""text\"" size=\""30\"" name=\""firstName\"" value=\""\"" class=\""field\""/></div><div class=\""form-row \"">last name<br/><input type=\""text\"" size=\""30\"" name=\""lastName\"" value=\""\"" class=\""field\""/></div><div class=\""form-row \"">email<br/><input type=\""text\"" size=\""30\"" name=\""email\"" value=\""\"" class=\""field\""/></div><div class=\""form-row \"">confirm email<br/><input type=\""text\"" size=\""30\"" name=\""confirmEmail\"" value=\""\"" class=\""field\""/></div><div class=\""form-row \"">subject<br/><select name=\""subject\"" class=\""field\""><option value=\"" \"">select</option><option value=\""product availability\"">product availability</option><option value=\""product information\"">product information</option><option value=\""purchase policy\"">purchase policy</option><option value=\""order status\"">order status</option><option value=\""returns/exchange\"">returns/exchange</option><option value=\""repairs\"">repairs</option><option value=\""other\"">other</option></select></div><div class=\""form-row \"">message<br/><textarea name=\""requestBody\"" rows=\""5\"" cols=\""25\"" class=\""field\""></textarea></div><ul class=\""functions bottom\""><li><div class=\""button\"" onclick=\""$(this).up(\'form\').onsubmit()\""><div class=\""content\"">send request</div><div class=\""end\""> </div></div></li></ul></form>'
			                }
                         }");
        }

        void BuildImageList(StringBuilder sb, List<ShopWindowImage> list)
        {
            if (list.Count > 0)
            {
                ShopWindowImage orign = list[0];
                sb.Append(",leadStyle:");
                sb.Append(BuildImage(orign));
                sb.Append(",variations:[");
                for (int i = 1; i < list.Count; i++)
                {
                    ShopWindowImage vari = list[i];
                    sb.Append(BuildImage(vari));
                }
                sb.Append("]");
            }
        }

        string BuildImage(ShopWindowImage img)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("{0}:'{1}'", "style_number", img.StyleNumber);
            //sb.AppendFormat(",{0}:'{1}'", "display_style_number", img.StyleNumber);
            sb.AppendFormat(",{0}:'{1}'", "display_style_number", "");

            sb.Append(",images:{");
            sb.Append("mpi:[");
            for (int i = 0; i < img.Full.Count; i++)
            {
                if (i != 0)
                    sb.Append(",");
                sb.AppendFormat("'{0}'", img.Full[i]);
            }
            sb.Append("]");
            sb.AppendFormat(",is360:{0}", img.Zoom.Count);
            sb.AppendFormat(",{0}:'{1}'", "full", img.Full.Count > 0 ? img.Full[0] : "");
            sb.Append(",zoom:[");
            for (int i = 0; i < img.Zoom.Count; i++)
            {
                if (i != 0)
                    sb.Append(",");
                sb.AppendFormat("'{0}'", img.Zoom[i]);
            }
            sb.Append("]");
            sb.AppendFormat(",{0}:'{1}'", "panel_thumb", img.Thumbnail);
            sb.AppendFormat(",{0}:'{1}'", "miniThumb", img.MinThumbnail);
            sb.AppendFormat(",{0}:'{1}'", "thumb", img.Thumbnail);
            sb.Append("}");

            sb.Append(",text:{");
            sb.AppendFormat("{0}:'{1}'", "grpdesc", img.Name);
            sb.AppendFormat(",{0}:'{1}'", "vardesc", img.Desc);
            sb.Append("}");

            sb.Append(",path:'/spring-summer-10/handbags/'");
            sb.Append(",collection:'Spring Summer'");
            sb.Append(",department:'HANDBAGS'");

            sb.Append("}");
            return sb.ToString();
        }
    }

    public class ShopWindowImage
    {
        public ShopWindowImage()
        {
            Zoom = new List<string>();
            Full = new List<string>();
        }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string StyleNumber { get; set; }

        public string Thumbnail { get; set; }

        public string MinThumbnail { get; set; }

        public List<string> Zoom { get; set; }

        public List<string> Full { get; set; }

        public string ToJson()
        {
            return "";
        }

    }


}
