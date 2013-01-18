namespace Services.Contracts
{
    using System.Collections.Generic;
    using Entities;
    using EnvDTE;

    public interface IVisualStudioService
    {
        WebProject GetWebProject();
        IEnumerable<Page> GetPages(ProjectItems item);
    }
}