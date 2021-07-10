using TaskManager.Data.Contracts;

namespace TaskManager.ViewModels.Admin
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState DateSort { get; set; }
        public SortState Current { get; set; }

        public SortViewModel(SortState sortState)
        {
            NameSort = sortState == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            DateSort = sortState == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            Current = sortState;
        }
    }
}