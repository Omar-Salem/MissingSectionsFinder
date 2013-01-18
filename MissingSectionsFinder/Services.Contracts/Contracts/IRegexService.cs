using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IRegexService
    {
        IEnumerable<string> GetSectionsInLayout(string pageContent);
        string GetLayoutPageName(string pageContent);
        IEnumerable<string> GetChildViews(string pageContent);
        IEnumerable<string> GetSectionsInPage(string pageContent);
    }
}