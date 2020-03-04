using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.ViewModel
{
    public class ResponseList<T>
    {
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public T Data { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemsCount { get; set; }
    }
}
