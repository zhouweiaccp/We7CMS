using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using System.IO;

namespace We7.CMS.WebControls
{
    public class ShopWindowProvider : BaseWebControl
    {
        protected List<ShopWindow> Data = new List<ShopWindow>();
        protected int Index;
        public List<Channel> CurChannels;

        #region 属性

        public ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        public string WebUrl
        {
            get
            {
                string url=Request.Url.AbsoluteUri;
                url = url.Substring(0, url.IndexOf(Request.Url.PathAndQuery));
                url = url.Replace(Request.Url.AbsolutePath, "");
                return url;
            }
        }

        public string UCPath
        {
            get
            {
                return Page.ResolveUrl(AppRelativeTemplateSourceDirectory);
            }
        }

        public int WindowWidth
        {
            get
            {
                return 471 + 325 * Column;
            }
        }

        public int Column
        {
            get
            {
                return Data.Count % 6 == 0 ? Data.Count / 6 : (Data.Count / 6 + 1);
            }
        }
        public int LastCount
        {
            get
            {
                int count = Data.Count % 6;
                return (count==0&&Column>0)?6:count;
            }
        }

        string bindColumnID = "{31061604-2ad9-4ca9-afd9-94618c7194ce}";
        /// <summary>
        /// 控件绑定ID
        /// </summary>
        public string BindColumnID
        {
            get { return bindColumnID; }
            set { bindColumnID = value; }
        }

        /// <summary>
        /// 栏目ID
        /// </summary>
        protected string ChannelID
        {
            get { return ChannelHelper.GetChannelIDFromURL(); }
        }

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        protected int Level
        {
            get
            {
                int i = 0;
                string l = Request["l"];
                Int32.TryParse(l, out i);
                return i < 2 ? 2 : i;
            }
        }

        protected List<Channel> FirstChannel;

        protected List<Channel> SecondChannel;

        protected List<Channel> ThirdChannel;

        protected Channel Root,First, Second, Third, Forth;

        #endregion

        public void BuildMenu()
        {
            if (String.IsNullOrEmpty(BindColumnID))
                throw new Exception("请设置根栏目");
            Root = ChannelHelper.GetChannel(BindColumnID,null);
            switch (Level)
            {
                case 1:
                    Load1Level();
                    break;
                case 2:
                    Load2Level();
                    break;
                case 3:
                    Load3Level();
                    break;
                case 4:
                    Load4Level();
                    break;
            }
            if (FirstChannel == null)
                FirstChannel = new List<Channel>();
            if (SecondChannel == null)
                SecondChannel = new List<Channel>();
            if (ThirdChannel == null)
                ThirdChannel = new List<Channel>();
        }

        void Load4Level()
        {
            LoadData(ChannelID);
            Third = ChannelHelper.GetChannel(ChannelID, null);
            Second = ChannelHelper.GetChannel(Third.ParentID, null);
            First = ChannelHelper.GetChannel(Second.ParentID, null);
            SecondChannel = ChannelHelper.GetChannels(First.ID);

            if (SecondChannel != null && SecondChannel.Count > 0)
            {
                ThirdChannel = ChannelHelper.GetChannels(SecondChannel[0].ID);
                //TODO::加上数据访问
            }
        }

        void Load3Level()
        {
            LoadData(ChannelID);
            Second = ChannelHelper.GetChannel(ChannelID, null);
            First = ChannelHelper.GetChannel(Second.ParentID, null);
            SecondChannel = ChannelHelper.GetChannels(First.ID);

            if (SecondChannel != null && SecondChannel.Count > 0)
            {
                ThirdChannel = ChannelHelper.GetChannels(SecondChannel[0].ID);
                //TODO::加上数据访问
            }
        }

        void Load2Level()
        {
            LoadData(ChannelID);
            First = ChannelHelper.GetChannel(ChannelID, null);
            SecondChannel = ChannelHelper.GetChannels(ChannelID);
            if (SecondChannel != null && SecondChannel.Count > 0)
            {
                ThirdChannel = ChannelHelper.GetChannels(SecondChannel[0].ID);
                Second = ChannelHelper.GetChannel(SecondChannel[0].ID, null);
                //TODO::加上数据访问
            }
        }

        void Load1Level()
        {
            FirstChannel = ChannelHelper.GetChannels(BindColumnID);
            LoadData(BindColumnID);
        }


        public string ActiveSecond(string id)
        {
            return (Second != null && Second.ID == id || Second == null && string.IsNullOrEmpty(id)) ? "active" : "";
        }

        public string ActiveThird(string id)
        {
            return (Third != null && Third.ID == id||Third==null&&string.IsNullOrEmpty(id)) ? "active" : "";
        }

        public void LoadData(string cid)
        {
            List<Article> list = ArticleHelper.QueryArticlesByChannel(cid, true);
            if (list == null)
                return;
            int i = 0;
            foreach (Article a in list)
            {
                string guid = i++.ToString();
                ShopWindow win = new ShopWindow();
                Data.Add(win);
                win.GroupID = guid;
                win.LinkID = a.ID+"_"+guid;

                ShopWindowImage img = new ShopWindowImage();
                img.StyleNumber = a.ID.Trim('{').Trim('}').Split('-')[4];
                img.Name = a.Title;
                img.Desc = a.Content;
                img.Thumbnail = GetThumbUrl(a.Thumbnail,"_pt");
                img.MinThumbnail = GetThumbUrl(a.Thumbnail, "_pm");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "f");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "b");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "l");
                AddThumb(img.Full, img.Zoom, a.Thumbnail, "r");
                win.Images.Add(img);
            }
        }

        public void AddThumb(List<string> Full, List<string> Zoom, string path, string ext)
        {
            string f=GetThumbUrl(path, "_p"+ext+"f");
            string z = GetThumbUrl(path, "_p" + ext + "z");
            if (!String.IsNullOrEmpty(f) && !String.IsNullOrEmpty(z))
            {
                Full.Add(f);
                Zoom.Add(z);
            }
        }

        public string GetThumbUrl(string path,string add)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            int i=path.LastIndexOf('.');
            path=path.Substring(0, i) + add + path.Substring(i);
            string s=path.TrimStart('~').TrimStart('/');
            s="~/"+s;
            if (File.Exists(Server.MapPath(s)))
                return path;
            return "";
        }

        public List<Channel> GetChannels(string id)
        {
            return ChannelHelper.GetChannels(id)??new List<Channel>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IncludeJavaScript("prototype.js", "scriptaculous.js", "localize.js", "sitestuff.js", "minibag.js", "cookie.js",
                "ext.js", "app.js", "menu-cn.js", "shop.js", "zoom.js", "data.js", "rtw.js", "eluminate.js", "cmdatatagutils.js", "cm_custom.js");
            BuildMenu();
        }
    }

}
