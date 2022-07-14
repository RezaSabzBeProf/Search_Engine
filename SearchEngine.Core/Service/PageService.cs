using CategoryData.SearchEngineIssueClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using SearchEngine.Core.Service.Interface;
using SearchEngine.Datalayer.Context;
using SearchEngine.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Core.Service
{
    public class PageService : IPageService
    {
   
        EngineDbContext _context;
        public PageService(EngineDbContext context)
        {
            _context = context;
        }

        public List<Page> Search(string q)
        {
            var model1 = _context.Pages.Where(p=> p.IsDone == true && p.IsImage == false && p.title.Contains(q)).Take(3);
            string area1 = PredictIssue(new SearchEngineIssue
            {
                Title = q,
                Description = q
            });
            var first = model1.Take(1).SingleOrDefault();
            string area2 = "";
            if(first != null && first.area != "null")
            {
                area2 = first.area;
            }
            var model2 = _context.Pages.Where(p => p.IsDone == true && p.IsImage == false && p.area.Contains(area1)).Take(3);
            var model3 = _context.Pages.Where(p => p.IsDone == true && p.IsImage == false && p.area.Contains(area2)).Take(3);
            List<Page> result = new List<Page>();
            foreach (var page in model1)
                result.Add(new Page
                {
                    title = page.title,
                    area = page.area,
                    url = page.url
                });
            foreach (var page in model2)
                result.Add(new Page
                {
                    title = page.title,
                    area = page.area,
                    url = page.url
                });
            foreach (var page in model3)
                result.Add(new Page
                {
                    title = page.title,
                    area = page.area,
                    url = page.url,
                });

            return result;
        }

        public List<Page> SearchImage(string q)
        {
            var model1 = _context.Pages.Where(p => p.IsDone == true && p.IsImage == true && p.title.Contains(q)).Take(3);
            string area1 = PredictIssue(new SearchEngineIssue
            {
                Title = q,
                Description = q
            });
            var first = model1.Take(1).SingleOrDefault();
            string area2 = "";
            if (first != null && first.area != "null")
            {
                area2 = first.area;
            }
            var model2 = _context.Pages.Where(p => p.IsDone == true && p.IsImage == true && p.area.Contains(area1)).Take(3);
            var model3 = _context.Pages.Where(p => p.IsDone == true && p.IsImage == true && p.area.Contains(area2)).Take(3);
            List<Page> result = new List<Page>();
            foreach (var page in model1)
                result.Add(new Page
                {
                    title = page.title,
                    area = page.area,
                    url = page.url,
                    ImagePageUrl = page.ImagePageUrl
                });
            foreach (var page in model2)
                result.Add(new Page
                {
                    title = page.title,
                    area = page.area,
                    url = page.url,
                    ImagePageUrl = page.ImagePageUrl
                });
            foreach (var page in model3)
                result.Add(new Page
                {
                    title = page.title,
                    area = page.area,
                    url = page.url,
                    ImagePageUrl = page.ImagePageUrl
                });

            return result;
        }

        string PredictIssue(SearchEngineIssue singleIssue)
{
            string _appPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string _modelPath;
            ITransformer _trainedModel;
            IDataView _trainingDataView;
            _modelPath = Path.Combine(_appPath, "..", "..", "..", "..", "Models", "model.zip");

            MLContext _mlContext;
            PredictionEngine<SearchEngineIssue, IssuePrediction> _predEngine;
            _mlContext = new MLContext(seed: 0);

            ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

            _predEngine = _mlContext.Model.CreatePredictionEngine<SearchEngineIssue, IssuePrediction>(loadedModel);

            var prediction = _predEngine.Predict(singleIssue);

            return prediction.Area;

        }
    }
}
