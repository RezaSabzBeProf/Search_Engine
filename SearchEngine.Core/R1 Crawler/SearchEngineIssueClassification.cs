using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryData
{
    // <SnippetAddUsings>
    using Microsoft.ML.Data;
    // </SnippetAddUsings>

    namespace SearchEngineIssueClassification
    {
        // <SnippetDeclareTypes>
        public class SearchEngineIssue
        {
            [LoadColumn(0)]
            public string Area { get; set; }
            [LoadColumn(1)]
            public string Title { get; set; }
            [LoadColumn(2)]
            public string Description { get; set; }
            [LoadColumn(3)]
            public bool IsTrue { get; set; }
        }

        public class IssuePrediction
        {
            [ColumnName("PredictedLabel")]
            public string Area;
        }
        // </SnippetDeclareTypes>
    }
}
