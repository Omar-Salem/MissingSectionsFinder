namespace Services.Contracts
{
    using System.Collections.Generic;
    using Entities;
    using EnvDTE;

    public interface IVisualStudioService
    {
        WebProject GetWebProject();
        IEnumerable<Area> GetAreas(ProjectItems item);
        IEnumerable<Page> GetPages(ProjectItems item);
    }
}