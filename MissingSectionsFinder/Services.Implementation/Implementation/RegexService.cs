﻿using System.Collections.Generic;
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

        IEnumerable<string> IRegexService.GetChildViews(string pageContent)
        {
            List<string> partialViews = GetChildViews(pageContent, @"Html.RenderPartial\([^,\)]*");
            List<string> partialActions = GetChildViews(pageContent, @"Html.RenderAction\([^,\)]*");

            partialViews.AddRange(partialActions);

            return partialViews;
        }

        IEnumerable<string> IRegexService.GetSectionsInLayout(string pageContent)
        {
            List<string> sections = new List<string>();

            sections.AddRange(FindSections(pageContent, @"@RenderSection\([^,]*,[\n|\s]*required[^:]*:[\n|\s]*true"));
            sections.AddRange(FindSections(pageContent, @"@RenderSection\([^,]*,[\n|\s]*true"));
            sections.AddRange(FindSections(pageContent, @"@RenderSection\([^,]*\)"));

            return sections;
        }

        string IRegexService.GetLayoutPageName(string pageContent)
        {
            var reg = new Regex(@"Layout[^;]*");
            var match = reg.Match(pageContent);

            reg = new Regex(@"[^/]*[s|b]html");
            string pageName = reg.Match(match.Value).Value;

            if (pageName == string.Empty)
            {
                reg = new Regex(@"=[^;]*");
                pageName = reg.Match(match.Value).Value;
                pageName = pageName.Replace("=", string.Empty);
                pageName = pageName.Replace(" ", string.Empty);
            }

            return pageName;
        }

        IEnumerable<string> IRegexService.GetSectionsInPage(string pageContent)
        {
            List<string> sections = new List<string>();

            var reg = new Regex(@"@section[^{]*");
            var matches = reg.Matches(pageContent);

            foreach (Match item in matches)
            {
                sections.Add(item.Value.Replace(@"@section", string.Empty).Replace(" ", string.Empty));
            }

            return sections;
        }

        #endregion IRegexService

        #region Private Methods

        private List<string> GetChildViews(string pageContent, string pattern)
        {
            List<string> pageNames = new List<string>();
            var reg = new Regex(pattern);
            var matches = reg.Matches(pageContent);

            foreach (Match item in matches)
            {
                int startIdx = item.Value.IndexOf('(');
                string partialView = item.Value.Substring(++startIdx, item.Value.Length - startIdx);
                startIdx = partialView.LastIndexOf('.');

                if (startIdx != -1)//In case of T4MVC templates:MVC.Reports.Shared.Views._Filters
                {
                    partialView = partialView.Substring(++startIdx, partialView.Length - startIdx);
                }
                else
                {
                    partialView = partialView.Replace("\"", string.Empty);
                }

                pageNames.Add(partialView);
            }

            return pageNames;
        }

        private IEnumerable<string> FindSections(string pageContent, string reg)
        {
            List<string> sections = new List<string>();
            var regex = new Regex(reg);
            MatchCollection matches = regex.Matches(pageContent);

            foreach (Match item in matches)
            {
                yield return GetSectionName(item.Value);
            }
        }

        private string GetSectionName(string match)
        {
            match = match.Replace("@RenderSection(", "");
            int idx = match.IndexOf(',');

            if (idx == -1)
            {
                idx = match.IndexOf(')');
            }

            return match.Substring(0, idx).Replace("\"", string.Empty);
        }

        #endregion
    }
}