using askaway.Models;
using askaway.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askaway.ViewComponents
{


    public class NavbarViewComponent: ViewComponent
    {
        private ICategoryRepository categoryRepository;

        public NavbarViewComponent()
        {
            this.categoryRepository = new CategoryRepository(new askawayDbContext());
        }

        public IViewComponentResult Invoke()
        {
            var categories = from c in categoryRepository.GetCategories()
                             select c;

            return View(categories);
        }
    }
}
