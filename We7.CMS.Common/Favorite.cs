using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    [Serializable]
    public class Favorite
    {
        public string FavoriteID { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string AccountID { get; set; }

        public string Tag { get; set; }

        public string Thumbnail { get; set; }

        public string ArticleID { get; set; }
    }
}
