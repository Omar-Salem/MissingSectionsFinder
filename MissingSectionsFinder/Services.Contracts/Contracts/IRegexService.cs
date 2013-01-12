using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IRegexService
    {
        IEnumerable<string> GetSectionsInLayout(string pageContent);
        bool CheckPageHasNullLayout(string content);
        string GetLayoutPageName(string pageContent);
        IEnumerable<string> GetSectionsInPage(string pageContent);
    }
}