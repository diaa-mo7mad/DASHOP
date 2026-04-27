using DASHOP.BLL.Service;
using DASHOP.DAL.DTO.Request;
using DASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DASHOP.PL.Areas.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IStringLocalizer<SharedResource> localizer, ICategoryService categoryService)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _categoryService.CreateCategory(request);
            return Ok(new { Message = _localizer["Success"].Value, response });
        }
    }
}
