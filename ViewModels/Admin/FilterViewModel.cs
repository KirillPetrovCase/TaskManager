namespace TaskManager.ViewModels.Admin
{
    public class FilterViewModel
    {
        public string SelectedUser { get; private set; }
        public string SelectedPerformer { get; private set; }

        public FilterViewModel(string name = "", string performer = "")
        {
            SelectedUser = name;
            SelectedPerformer = performer;
        }
    }
}