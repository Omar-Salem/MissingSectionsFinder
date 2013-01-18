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

        IEnumerable<Result> IPagesService.GetMissingSections(IEnumerable<Page> pages)
        {
            pages = GetNonPartialPages(pages);

            var layoutSections = new Dictionary<string, IEnumerable<string>>();

            foreach (var currentPage in pages)
            {
                if (currentPage.Name.Contains("_ViewStart"))
                {
                    continue;
                }

                Page layoutPage = GetLayoutPage(pages, currentPage);

                if (layoutPage != null)
                {
                    if (!layoutSections.ContainsKey(layoutPage.Name))
                    {
                        layoutSections[layoutPage.Name] = _regexService.GetSectionsInLayout(layoutPage.Content);
                    }

                    IEnumerable<string> pageSections = _regexService.GetSectionsInPage(currentPage.Content);
                    IEnumerable<string> missingSections = layoutSections[layoutPage.Name].Except(pageSections);

                    if (missingSections.Any())
                    {
                        yield return new Result { PageName = currentPage.Name, Sections = missingSections };
                    }
                }

            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<Page> GetNonPartialPages(IEnumerable<Page> pages)
        {
            var allPartialPages = new HashSet<string>();

            foreach (var page in pages)
            {
                IEnumerable<string> partialPage = _regexService.GetPartialPageNames(page.Content);

                foreach (var item in partialPage)
                {
                    if (!allPartialPages.Contains(item))
                    {
                        allPartialPages.Add(item);
                    }
                }
            }
            foreach (var page in pages)
            {
                bool isNormalPage = true;

                foreach (string partialPage in allPartialPages)
                {
                    if (page.Name.EndsWith(partialPage + ".cshtml") || page.Name.EndsWith(partialPage + ".vbhtml"))
                    {
                        isNormalPage = false;
                        break;
                    }
                }

                if (isNormalPage)
                {
                    yield return page;
                }
            }

        }

        private Page GetLayoutPage(IEnumerable<Page> pages, Page currentPage)
        {
            string layoutPageName = _regexService.GetLayoutPageName(currentPage.Content);

            if (layoutPageName == "null")
            {
                return null;
            }

            if (string.IsNullOrEmpty(layoutPageName))
            {
                Page viewStart = pages.Where(p => p.Area == currentPage.Area && p.Name.Contains("_ViewStart")).SingleOrDefault();

                if (viewStart == null)
                {
                    viewStart = pages.Where(p => string.IsNullOrEmpty(p.Area) && p.Name.Contains("_ViewStart")).SingleOrDefault();
                }

                if (viewStart != null)
                {
                    layoutPageName = _regexService.GetLayoutPageName(viewStart.Content);
                }

                if (string.IsNullOrEmpty(layoutPageName))
                {
                    return null;
                }
            }

            Page layout = pages.Where(p => p.Area == currentPage.Area && p.Name.Contains(layoutPageName)).SingleOrDefault();

            if (layout == null)
            {
                var xxxxx = pages.Where(p => string.IsNullOrEmpty(p.Area) && p.Name.Contains(layoutPageName)).ToArray();
                layout = pages.Where(p => string.IsNullOrEmpty(p.Area) && p.Name.Contains(layoutPageName)).SingleOrDefault();
            }

            return layout;
        }

        #endregion
    }
}