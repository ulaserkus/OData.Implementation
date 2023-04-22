using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using OData.Implementation.API.Models;

namespace OData.Implementation.API.Controllers
{
    [ODataRoutePrefix("Products")]
    public class ProductsController : ODataController
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery(PageSize = 2)]
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.AsQueryable());
        }

        [ODataRoute("({id})")]
        [EnableQuery]
        [HttpGet]
        public IActionResult GetProduct([FromODataUri] int id)
        {
            return Ok(_context.Products.Where(x => x.Id == id));
        }

        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created(product);
        }

        [HttpPut]
        public IActionResult PutProduct(int key, [FromBody] Product product)
        {
            product.Id = key;
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int key)
        {
            var product = _context.Products.Find(key);
            if (product == null)
                return NotFound();
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost]
        public IActionResult Login(ODataActionParameters parameters)
        {
            Login? login = parameters["UserLogin"] as Login;
            return Ok(login);
        }

        [HttpGet]
        public IActionResult MultiplyFunction([FromODataUri] int a1, [FromODataUri] int a2, [FromODataUri] int a3)
        {
            return Ok(a1 * a2 * a3);
        }

        [HttpGet]
        public IActionResult KdvHesapla(int key, [FromODataUri] decimal kdv)
        {
            var product = _context.Products.Find(key);
            return Ok(product?.Price + (product?.Price * kdv));
        }
    }
}
