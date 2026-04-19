using DASHOP.DAL.Data;
using DASHOP.DAL.DTO.Request;
using DASHOP.DAL.DTO.Response;
using DASHOP.DAL.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult Index()
        {   var category=_context.Categories.Include(c=>c.Translations).ToList();   
            var response = category.Adapt<List<CategoryResponse>>();

            return Ok(new { Message=_localizer["Success"].Value, response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request) {
            var result = request.Adapt<Category>();
            _context.Categories.Add(result);
            _context.SaveChanges();


            return Ok(new { Message = _localizer["Success"].Value });

        
        }

       
    }
}
