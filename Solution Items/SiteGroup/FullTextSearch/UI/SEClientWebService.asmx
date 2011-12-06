<%@ WebService Language="C#" Class="SEClientWebService" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using We7.Plugin.FullTextSearch;
using WebEngine2007.SE;

[WebService(Namespace = "http://WebEngine2007.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SEClientWebService  : System.Web.Services.WebService {

    [WebMethod]
    public string ping() {
        return "tong";
    }

    [WebMethod]
    public QueryArticle[] IndexArticleList(int count)
    {
        return IndexerDataProvider.Instance().IndexArticleList(count);
    }
    
    [WebMethod]
    public string[] DeleteArticleList(int count)
    {
        return IndexerDataProvider.Instance().DeleteArticleList(count);
    }
    
    [WebMethod]
    public void UnlockArticles(string[] list)
    {
        IndexerDataProvider.Instance().UnlockArticles(list);
    }
    
    [WebMethod]
    public void UnlockAllArticles()
    {
        IndexerDataProvider.Instance().UnlockAllArticles();
    }
    
    [WebMethod]
    public void DeleteArticles(string[] list)
    {
        IndexerDataProvider.Instance().DeleteArticles(list);
    }

    [WebMethod]
    public string SiteName()
    {
        return IndexerDataProvider.Instance().SiteName();
    }

    [WebMethod]
    public string SiteUrl()
    {
        return IndexerDataProvider.Instance().SiteUrl();
    }
    
}

