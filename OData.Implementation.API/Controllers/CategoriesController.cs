using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using OData.Implementation.API.Models;

namespace OData.Implementation.API.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(_context.Categories.AsQueryable());
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult GetCategory([FromODataUri] int key)
        {
            return Ok(_context.Categories.Where(x => x.Id == key));
        }

        [ODataRoute(("Categories({id})/Products({item})"))]
        [EnableQuery]
        [HttpGet]
        public IActionResult ProductById([FromODataUri] int id, [FromODataUri] int item)
        {
            return Ok(_context.Products.Where(x => x.CategoryId == id && x.Id == item));
        }

        [ODataRoute(("Categories({id})/Products"))]
        [EnableQuery]
        [HttpGet]
        public IActionResult ProductsByCategory([FromODataUri] int id)
        {
            return Ok(_context.Products.Where(x => x.CategoryId == id));
        }

        [HttpPost]
        public IActionResult TotalProductPrice([FromODataUri] int key)
        {
            var total = _context.Products.Where(x => x.CategoryId == key).Sum(x => x.Price);
            return Ok(total);
        }

        [HttpPost]
        public IActionResult TotalProductPriceAll()
        {
            var total = _context.Products.Sum(x => x.Price);
            return Ok(total);
        }

        [HttpPost]
        public IActionResult TotalProductPriceWithParam(ODataActionParameters parameters)
        {
            int categoryId = (int)parameters["categoryId"];
            var total = _context.Products.Where(x => x.CategoryId == categoryId).Sum(x => x.Price);
            return Ok(total);
        }

        [HttpPost]
        public IActionResult Total(ODataActionParameters parameters)
        {
            int? a = (int?)parameters["categoryId"];
            int? b = (int?)parameters["categoryId"];
            int? c = (int?)parameters["categoryId"];
            var total = a + b + c;
            return Ok(total);
        }

        [HttpGet]
        public IActionResult CategoryCount()
        {
            return Ok(_context.Categories.Count());
        }
    }
}
