using System;
using System.Windows.Controls;
using RemoverPackage.Control.ViewModels;
using Services.Contracts;
using System.ComponentModel.Composition;

namespace MissingSectionsFinder.Control
{
    public partial class SearchControl : UserControl
    {
        public SearchControl(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ContainerProvider.Container.ComposeExportedValue<IServiceProvider>("serviceProvider", serviceProvider);
            var visualStudioService = ContainerProvider.Container.GetExportedValue<IVisualStudioService>();

            var regexService = ContainerProvider.Container.GetExportedValue<IRegexService>();

            ContainerProvider.Container.ComposeExportedValue<IRegexService>("regexService", regexService);
            var pageService = ContainerProvider.Container.GetExportedValue<IPagesService>();

            this.DataContext = new SearchViewModel(visualStudioService, regexService, pageService);
        }
    }
}
