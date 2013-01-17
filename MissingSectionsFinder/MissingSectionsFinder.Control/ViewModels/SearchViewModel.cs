namespace RemoverPackage.Control.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Entities;
    using RemoverPackage.Control.Commands;
    using Services.Contracts;
    using Services.Implementation;
    using EnvDTE;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    public class SearchViewModel : ViewModelBase
    {
        #region Member Variables

        private readonly IVisualStudioService _visualStudioService;
        private readonly IRegexService _regexService;
        private readonly IPagesService _pagesService;
        private ObservableCollection<Result> _results;

        #endregion Member Variables

        #region Constructor

        [ImportingConstructor]
        public SearchViewModel(IVisualStudioService visualStudioService, IRegexService regexService, IPagesService pagesService)
        {
            this._visualStudioService = visualStudioService;
            this._regexService = regexService;
            this._pagesService = pagesService;
            Results = new ObservableCollection<Result>();
        }

        #endregion Constructor

        #region Public Properties

        public ObservableCollection<Result> Results
        {
            get { return _results; }
            set
            {
                if (_results != value)
                {
                    _results = value;
                    OnPropertyChanged("Results");
                }
            }
        }

        #endregion Public Properties

        #region Commands

        public ICommand FindDuplicates
        {
            get { return new RelayCommand(FindDuplicatesExecute, CanFindDuplicatesExecute); }
        }

        #endregion Commands

        #region Commands Methods

        private void FindDuplicatesExecute()
        {
            var webProject = _visualStudioService.GetWebProject();

            if (webProject == null)
            {
                return;
            }

            var pages = _visualStudioService.GetPages(webProject.Project.ProjectItems);
            Results = new ObservableCollection<Result>(_pagesService.GetMissingSections(pages));
        }

        private bool CanFindDuplicatesExecute()
        {
            return true;
        }

        #endregion Commands Methods
    }
}