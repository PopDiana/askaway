using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askaway.Models;

namespace askaway.Repositories
{
    public class CategoryRepository: ICategoryRepository, IDisposable
    {
        private askawayDbContext context;

        public CategoryRepository(askawayDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetCategories()
        {

            return context.Categories.ToList().OrderBy(c => c.CategoryName);
        }

        public Category GetCategoryByID(int id)
        {
            return context.Categories.Find(id);

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
