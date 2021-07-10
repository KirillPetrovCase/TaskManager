using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.ViewModels.Admin
{
    public class ArchiveViewModel
    {
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

        public IEnumerable<ArchiveOrderRecord> Records { get; set; }
    }
}