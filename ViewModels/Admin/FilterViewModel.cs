namespace TaskManager.ViewModels.Admin
{
    public class FilterViewModel
    {
        public string SelectedName { get; private set; }

        public FilterViewModel(string name)
        {
            SelectedName = name;
        }
    }
}