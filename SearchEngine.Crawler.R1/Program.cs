using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CategoryData.SearchEngineIssueClassification;
using Microsoft.ML;
using SearchEngine.Core.R1_Crawler.Model;
using SearchEngine.Crawler.R1.Repository;
using SearchEngine.Datalayer.Entities;
using System.Net;
using System.Text.RegularExpressions;

string _appPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
string _modelPath = Path.Combine(_appPath,"..", "..", "..", "..", "Models", "model.zip");
ITransformer _trainedModel;
IDataView _trainingDataView;
ICrawlerService _crawlerService = new CrawlerService();
MLContext _mlContext;
PredictionEngine<SearchEngineIssue, IssuePrediction> _predEngine;

_mlContext = new MLContext(seed: 0);


var pageList = _crawlerService.GetAllPageNotDone();
foreach (var page in pageList)
{
    //R1CrawlerEngine r1CrawlerEngine = new R1CrawlerEngine(page.url);
    string html = GetHtmlFromPage(page.url);
    string h1Text = GetPageH1(html);
    List<string> links = GetAllPageLink(html);
    string metaDesc = GetMetaDesc(html);
    AddSomeNewLinks(links);
    SearchEngineIssue mlModel = new SearchEngineIssue();
    if(h1Text == "error")
    {
        UpdateThisPage(new Page
        {
            title = "none",
            url = page.url,
            IsDone = true,
            area = "none"
        });
    }
    else
    {
        mlModel.Title = h1Text;
        mlModel.Description = metaDesc;
        string area = PredictIssue(mlModel);
        UpdateThisPage(new Page
        {
            title = h1Text,
            url = page.url,
            IsDone = true,
            area = area
        });
    }
    
}


void AddSomeNewLinks(List<string> links)
{
    _crawlerService.AddNewLinks(links);
}
void UpdateThisPage(Page page)
{
    _crawlerService.UpdatePage(page);
}
string PredictIssue(SearchEngineIssue singleIssue)
{
    
    ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

    _predEngine = _mlContext.Model.CreatePredictionEngine<SearchEngineIssue, IssuePrediction>(loadedModel);
  
    var prediction = _predEngine.Predict(singleIssue);

    return prediction.Area;

}
R1CrawlerResult DoWork(string url)
{
    string html = GetHtmlFromPage(url);
    if (html == "error")
    {
        return new R1CrawlerResult
        {
            h1Text = "error"
        };
    }
    string h1Text = GetPageH1(html);
    List<string> getLinks = GetAllPageLink(html);
    string metaDesc = GetMetaDesc(html);
    return new R1CrawlerResult
    {
        h1Text = h1Text,
        Links = getLinks,
        MetaDesc = metaDesc
    };
}
string GetHtmlFromPage(string url)
{
    try
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.UserAgent = "R1 Web Crawler";

        WebResponse response = request.GetResponse();

        Stream stream = response.GetResponseStream();

        StreamReader reader = new StreamReader(stream);
        string htmlText = reader.ReadToEnd();
        return htmlText;

    }
    catch
    {
        return "error";
    }

}
string GetPageH1(string html)
{
    try
    {
        HtmlParser parser = new HtmlParser();
        IHtmlDocument document = parser.ParseDocument(html);
        var x = document.GetElementsByTagName("h1");
        string h1Text = x[0].InnerHtml;
        return h1Text;
    }
    catch
    {
        return "none";
    }
}
List<string> GetAllPageLink(string html)
{
    Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");
    ISet<string> newLinks = new HashSet<string>();
    List<string> result = new List<string>();
    foreach (var match in regexLink.Matches(html))
    {
        if (!newLinks.Contains(match.ToString()))
        {
            newLinks.Add(match.ToString());
            result.Add(match.ToString());
        }
    }
    return result;
}
string GetMetaDesc(string html)
{
    try
    {
        HtmlParser parser = new HtmlParser();
        IHtmlDocument document = parser.ParseDocument(html);
        string x = document.QuerySelector("meta[name='description']").GetAttribute("content");
        return x;
    }
    catch
    {
        return "none";
    }


}
