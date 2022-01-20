using System;
using System.Collections.Generic;

namespace askaway.Models
{
    public partial class Category
    {
        public Category()
        {
            Questions = new HashSet<Question>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
