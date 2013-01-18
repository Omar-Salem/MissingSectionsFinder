﻿namespace Services.Contracts
{
    using System.Collections.Generic;
    using Entities;
    using EnvDTE;

    public interface IPagesService
    {
        IEnumerable<Result> GetMissingSections(IEnumerable<Area> areas, IEnumerable<Page> pages);
    }
}