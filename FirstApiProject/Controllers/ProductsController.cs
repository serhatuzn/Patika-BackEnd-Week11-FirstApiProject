using FirstApiProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new List<Product>()
        {
            new Product { Id = 1, Name = "Keyboard", Price = 20.00m },
            new Product { Id = 2, Name = "Mouse", Price = 10.00m },
            new Product { Id = 3, Name = "Monitor", Price = 200.00m }
        };

        [HttpGet]
        public IEnumerable<Product> Get() // Hepsini al dememiz için IEnumerable<Product> Get() şeklinde tanımladık.
        {
            return _products;
        }

        [HttpGet("{id}")] // Id'ye göre al dememiz için [HttpGet("{id}")] şeklinde tanımladık.
        public IActionResult Get(int id) // Id'ye göre al dememiz için IActionResult<Product> GetAction(int id) şeklinde tanımladık.
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product) // Yeni bir ürün eklemek için Post metodu oluşturduk.
        {
            var id = _products.Max(p => p.Id) + 1; // Id'yi otomatik olarak arttırmak için.
            product.Id = id;
            _products.Add(product);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product); // CreatedAtAction metodu ile yeni eklenen ürünün bilgilerini döndürdük.
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Product product) // Güncelleme işlemi için Put metodu oluşturduk.
        {
            if (product == null || id != product.Id) // Eğer ürün yoksa veya id'ler uyuşmuyorsa BadRequest döndürdük.
                return BadRequest();

            var existingProduct = _products.FirstOrDefault(p => p.Id == id); // Güncellenecek ürünü bulduk.

            if (existingProduct is null) // Eğer ürün yoksa NotFound döndürdük.
                return NotFound();

            existingProduct.Name = product.Name; // Güncelle
            existingProduct.Price = product.Price; // Güncelle

            return Ok(existingProduct); // Güncellenen ürünü döndürdük.
        }

        [HttpDelete]
        public IActionResult Delete(int id) // Silme işlemi için Delete metodu oluşturduk.
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id); // Silinecek ürünü bulduk.
            if (existingProduct == null) // Eğer ürün yoksa NotFound döndürdük.
                return NotFound();

            _products.Remove(existingProduct); // Ürünü listeden sildik.

            return NoContent(); // İşlem başarılı olduğu için NoContent döndürdük.
        }
    }
}
