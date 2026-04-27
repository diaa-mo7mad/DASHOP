using DASHOP.BLL.Service;
using DASHOP.DAL.DTO.Request;
using DASHOP.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IStringLocalizer<SharedResource> localizer, ICategoryService categoryService)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var response = _categoryService.GetAllCategories();
            return Ok(new { Message = _localizer["Success"].Value, response });

        }
       
    }
}
