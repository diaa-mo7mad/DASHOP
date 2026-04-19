using DASHOP.BLL.Service;
using DASHOP.DAL.Data;
using DASHOP.DAL.DTO.Request;
using DASHOP.DAL.DTO.Response;
using DASHOP.DAL.Models;
using DASHOP.DAL.Repository;
using DASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
       
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResource> localizer)
        {
           
            _categoryService = categoryService;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult Index()
        {  
            var response = _categoryService.GetAllCategories();

            return Ok(new { Message=_localizer["Success"].Value, response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request) {
            
            var response = _categoryService.CreateCategory(request);



            return Ok(new { Message = _localizer["Success"].Value });

        
        }

       
    }
}
