using TaskManager.Data.Contracts;

namespace TaskManager.ViewModels.Admin
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Post { get; set; }
        public string Placement { get; set; }
        public Role Role { get; set; }
    }
}