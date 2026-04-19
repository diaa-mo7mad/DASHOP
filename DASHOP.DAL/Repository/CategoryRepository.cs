using DASHOP.DAL.Data;
using DASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category Create(Category requset)
        {
            _context.Categories.Add(requset);   
            _context.SaveChanges();
            return requset;
        }

        public List<Category> GetAll()
        {
           
            return _context.Categories.Include(c=>c.Translations).ToList();
        }
    }
}
