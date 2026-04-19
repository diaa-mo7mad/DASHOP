using DASHOP.DAL.DTO.Request;
using DASHOP.DAL.DTO.Response;
using DASHOP.DAL.Models;
using DASHOP.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.BLL.Service
{
    public interface ICategoryService
    {
        public List<CategoryResponse> GetAllCategories();
        CategoryResponse CreateCategory(CategoryRequest request);
    }
}
