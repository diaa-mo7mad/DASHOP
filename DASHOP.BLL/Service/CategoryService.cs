using Azure.Core;
using DASHOP.DAL.DTO.Request;
using DASHOP.DAL.DTO.Response;
using DASHOP.DAL.Models;
using DASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public CategoryResponse CreateCategory(CategoryRequest requset)
        {
            var result = requset.Adapt<Category>();
            _categoryRepository.Create(result);
            return result.Adapt<CategoryResponse>();
        }

        public List<CategoryResponse> GetAllCategories()
        {
            var category = _categoryRepository.GetAll();
            var response = category.Adapt<List<CategoryResponse>>();
            return response;
        }
    }
}
