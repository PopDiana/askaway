using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;

namespace askaway.Repositories
{
    public interface ICategoryRepository: IDisposable
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryByID(int categoryId);
    }
}
