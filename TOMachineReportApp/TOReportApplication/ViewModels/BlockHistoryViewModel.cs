using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class BlockHistoryViewModel: ViewModelBase, IBlockHistoryViewModel
    {
        public ISettingsAndFilterPanelViewModel<BlockHistoryModel> SettingsAndFilterPanelViewModel { get; }

        public BlockHistoryViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger) : base(container)
        {
            SettingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel<BlockHistoryModel>(container, repository, logger);
        }

        public void Dispose()
        {
      
        }
    }
}
