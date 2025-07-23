using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Application.DTOs
{
    public class CategoryViewCountDto
    {
        public string CategoryName { get; set; }
        public int TotalViews { get; set; }
    }
}
