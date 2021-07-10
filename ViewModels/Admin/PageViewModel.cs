using System;

namespace TaskManager.ViewModels.Admin
{
    public class PageViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public bool HasNext => CurrentPage < TotalPages;

        public bool HasPrevious => CurrentPage > 1;

        public PageViewModel(int currentPage, int totalPages, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalPages / (double)pageSize);
        }
    }
}