using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;

[ApiController]
[Route("api/Products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        try
        {
            var products = _productService.GetAllProducts();
            return Ok(products); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        try
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found."); // HTTP 404
            return Ok(product); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching product ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("ProductIDByName/{name}")]
    public IActionResult GetProductIDByName(string name)
    {
        try
        {
            var product = _productService.GetProductIDByName(name);
            if (product == null)
                return NotFound($"Product with name {name} not found."); // HTTP 404
            return Ok(product); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching name {name}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("details-by-name")]
    public IActionResult GetProductDetailsByName([FromQuery] string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Il nome del prodotto è obbligatorio."); // HTTP 400
            }

            var (productId, price) = _productService.GetProductDetailsByName(name);
            if (productId == 0)
            {
                return NotFound($"Prodotto con nome '{name}' non trovato."); // HTTP 404
            }

            return Ok(new
            {
                ProductID = productId,
                Price = price
            }); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero dei dettagli per il prodotto '{name}': {ex.Message}");
            return StatusCode(500, "Errore interno del server"); // HTTP 500
        }
    }

    [HttpGet("producer/{producerId}")]
    public IActionResult GetProductByProducerID(int producerId)
    {
        try
        {
            var products = _productService.GetProductByProducerID(producerId);
            if (products == null || !products.Any())
                return NotFound($"No products found for Producer ID {producerId}."); // HTTP 404
            return Ok(products); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products for Producer ID {producerId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("image/{name}")]
    public IActionResult GetImagePathByName(string name)
    {
        try
        {
            var imagePath = _productService.GetImagePathByName(name);
            if (imagePath == null)
                return NotFound($"Image path for product {name} not found."); // HTTP 404
            return Ok(imagePath); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching image path for product {name}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("filter")]
    public IActionResult GetProductsByFilter([FromQuery] int? producerId, [FromQuery] string category)
    {
        try
        {
            var products = _productService.GetProductsByFilter(producerId, category);
            return Ok(products); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering products: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody] Product product)
    {
        try
        {
            _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product); // HTTP 201
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding product: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            product.ProductID = id;
            _productService.UpdateProduct(product);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("with-stock-byCategory")]
    public IActionResult GetProductsWithStockByCategory([FromQuery] string category)
    {
        try
        {
            // Verifica che la categoria non sia null o vuota
            if (string.IsNullOrEmpty(category))
            {
                return BadRequest("La categoria è obbligatoria.");
            }

            // Ottieni i prodotti filtrati per categoria
            var productsWithStock = _productService.GetProductsWithStockByCategory(category);
            return Ok(productsWithStock); // HTTP 200 con i prodotti filtrati
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero dei prodotti con stock: {ex.Message}");
            return StatusCode(500, "Errore interno del server.");
        }
    }
    [HttpGet("preferred-products/{userId}")]
    public IActionResult GetPreferredProducts(int userId)
    {
        try
        {
            var products = _productService.GetPreferredProducts(userId);

            if (products == null || !products.Any())
            {
                return NotFound("No preferred products found for this user.");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            // Log exception if needed
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    [HttpGet("most-purchased-products")]
    public IActionResult GetMostPurchasedProducts()
    {
        try
        {
            var products = _productService.GetMostPurchasedProducts();

            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            // Log exception if needed
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("product-page/{productId}")]
    public IActionResult GetProductPageData(int productId)
    {
        try
        {
            var productPageData = _productService.GetProductPageData(productId);
            return Ok(productPageData); // Restituisce HTTP 200 con i dati del prodotto
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero dei dati della pagina prodotto: {ex.Message}");
            return NotFound(new { Message = ex.Message }); // HTTP 404 se il prodotto non viene trovato
        }
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            _productService.DeleteProduct(id);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
