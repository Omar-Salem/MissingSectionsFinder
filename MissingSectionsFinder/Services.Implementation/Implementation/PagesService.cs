using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Contracts;
using System.ComponentModel.Composition;
using Entities;

namespace Services.Implementation.Implementation
{
    [Export(typeof(IPagesService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PagesService : IPagesService
    {
        #region Member Variables

        readonly IRegexService _regexService;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public PagesService([Import("regexService", typeof(IRegexService))]IRegexService regexService)
        {
            _regexService = regexService;
        }

        #endregion

        #region Public Methods

        IEnumerable<Result> IPagesService.GetMissingSections(IEnumerable<Area> areas, IEnumerable<Page> pages)
        {
            IEnumerable<Result> results = GetMissingSectionsInPages(pages);

            foreach (Area area in areas)
            {
                IEnumerable<Result> missingSections = GetMissingSectionsInPages(area.Pages);
                results = results.Concat(missingSections);
            }

            return results;
        }

        #endregion

        #region Private Methods

        IEnumerable<Result> GetMissingSectionsInPages(IEnumerable<Page> pages)
        {
            Page defaultLayoutPage = GetDefaultLayoutPage(pages);

            Dictionary<string, IEnumerable<string>> pagesWithSections = GetLayoutSections(pages);

            foreach (var page in pages)
            {
                IEnumerable<string> missingSections = FindMissingSections(defaultLayoutPage, pagesWithSections, page);

                yield return new Result { PageName = page.Name, Sections = missingSections };
            }
        }

        private Page GetDefaultLayoutPage(IEnumerable<Page> pages)
        {
            Page viewStart = pages.Where(p => p.Name.Contains("_ViewStart")).SingleOrDefault();

            if (viewStart == null)
            {
                return new Page { Name = "", Content = "" };
            }

            string defaultLayoutPageName = _regexService.GetLayoutPageName(viewStart.Content);

            Page defaultLayoutPage = pages.Where(p => p.Name == defaultLayoutPageName).SingleOrDefault();

            if (defaultLayoutPage == null)
            {
                return new Page { Name = "", Content = "" };
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

            if (string.IsNullOrEmpty(pageLayout) && pagesWithSections.ContainsKey(defaultLayoutPage.Name))
            {
                missingSections = pagesWithSections[defaultLayoutPage.Name].Except(pageSections);
            }
            else if (pagesWithSections.ContainsKey(pageLayout))
            {
                missingSections = pagesWithSections[pageLayout].Except(pageSections);
            }

            return missingSections;
        }

        #endregion
    }
}