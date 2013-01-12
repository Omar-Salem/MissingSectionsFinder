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
        IEnumerable<Result> IPagesService.GetMissingSections(IEnumerable<Page> pages)
        {
            throw new NotImplementedException();
        }
    }
}
