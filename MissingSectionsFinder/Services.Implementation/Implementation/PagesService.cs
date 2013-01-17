using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Contracts;
using System.ComponentModel.Composition;
using Entities;

namespace Services.Implementation.Implementation
{
    [Export(typeof(IRegexService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PagesService : IPagesService
    {
        #region Member Variables
        
        readonly IRegexService _regexService; 

        #endregion

        #region Constructor
        
        [ImportingConstructor]
        public PagesService(IRegexService regexService)
        {
            _regexService = regexService;
        } 

        #endregion

        #region Public Methods
        
        IEnumerable<Result> IPagesService.GetMissingSections(IEnumerable<Page> pages)
        {
            Page defaultLayoutPage = GetDefaultLayoutPage(pages);

            Dictionary<string, IEnumerable<string>> pagesWithSections = GetLayoutSections(pages);

            foreach (var page in pages)
            {
                IEnumerable<string> missingSections = FindMissingSections(defaultLayoutPage, pagesWithSections, page);

                yield return new Result { PageName = page.Name, Sections = missingSections };
            }
        } 

        #endregion

        #region Private Methods

        private Page GetDefaultLayoutPage(IEnumerable<Page> pages)
        {
            Page viewStart = pages.Where(p => p.Name.Contains("_ViewStart")).SingleOrDefault();

            if (viewStart == null)
            {
                return new Page { };
            }

            string defaultLayoutPageName = _regexService.GetLayoutPageName(viewStart.Content);

            Page defaultLayoutPage = pages.Where(p => p.Name == defaultLayoutPageName).SingleOrDefault();

            if (defaultLayoutPage == null)
            {
                return new Page { };
            }

            return defaultLayoutPage;
        }

        private Dictionary<string, IEnumerable<string>> GetLayoutSections(IEnumerable<Page> pages)
        {
            var pagesWithSections = new Dictionary<string, IEnumerable<string>>();

            foreach (var page in pages)
            {
                IEnumerable<string> pageSections = _regexService.GetSectionsInLayout(page.Content);

                if (pageSections.Any())
                {
                    pagesWithSections[page.Name] = pageSections;
                }
            }

            return pagesWithSections;
        }

        private IEnumerable<string> FindMissingSections(Page defaultLayoutPage, Dictionary<string, IEnumerable<string>> pagesWithSections, Page page)
        {
            string pageLayout = _regexService.GetLayoutPageName(page.Content);
            IEnumerable<string> pageSections = _regexService.GetSectionsInPage(page.Content);
            IEnumerable<string> missingSections = new string[] { };

            if (string.IsNullOrEmpty(pageLayout))
            {
                missingSections = pagesWithSections[defaultLayoutPage.Name].Except(pageSections);
            }
            else if (pageLayout != "null")
            {
                missingSections = pagesWithSections[pageLayout].Except(pageSections);
            }

            return missingSections;
        } 

        #endregion
    }
}