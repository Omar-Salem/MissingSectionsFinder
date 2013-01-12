using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using Services.Contracts;

namespace Services.Implementation
{
    [Export(typeof(IRegexService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RegexService : IRegexService
    {
        #region IRegexService

        IEnumerable<string> IRegexService.GetSectionsInLayout(string pageContent)
        {
            throw new System.NotImplementedException();
        }

        string IRegexService.GetLayoutPageName(string pageContent)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> IRegexService.GetSectionsInPage(string pageContent)
        {
            throw new System.NotImplementedException();
        }
        
        bool IRegexService.CheckPageHasNullLayout(string content)
        {
            Regex regex = new Regex(@"Layout[\n|\s]*=[\n|\s]*null");//Layout=null
            return regex.IsMatch(content);
        }

        #endregion IRegexService
    }
}