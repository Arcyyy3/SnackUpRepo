using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

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
    [HttpGet("products-grouped-by-category")]
    public IActionResult GetProductsGroupedByCategory()
    {
        var result = _productService.GetProductsGroupedByCategory();
        return Ok(result);
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
    [HttpGet("SupplierProduct{photoLinkProdotto}")]
    public IActionResult GetProduct(string photoLinkProdotto)
    {
        var product = _productService.GetProductDetails(photoLinkProdotto);

        if (product == null)
        {
            return NotFound(new { message = "Prodotto non trovato" });
        }

        return Ok(product);
    }

    [HttpGet("BundleCatalog")]
    public IActionResult BundleCatalog()
    {
        try
        {
            var bundles = _productService.GetBundleCatalog(); // CORRETTO: Adesso è UNA LISTA
            if (bundles == null || !bundles.Any())
                return NotFound("Nessun bundle trovato"); // HTTP 404

            return Ok(bundles); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore nel catalogo bundle: {ex.Message}");
            return StatusCode(500, "Errore interno del server");
        }
    }


    [HttpGet("details-by-name")]
    public IActionResult GetProductDetailsByName(string name)
    {
        try
        {
            var ProductDetails = _productService.GetProductDetailsByName(name);
            return Ok(ProductDetails);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving orders: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
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
    [HttpGet("PromotionProductByUser/{userId}")]
    public IActionResult GetProductPromotionsByUser(int userId)
    {
        try
        {
            var products = _productService.GetProductPromotionsByUserID(userId);

            // Se l'oggetto è vuoto
            if ((products.BundleProducts.Count == 0) && (products.RegularProducts.Count == 0))
            {
                return NotFound(new { message = $"UserID {userId} non associato a nessun producer o nessun prodotto." });
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products for UserID {userId}: {ex.Message}");
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
    [HttpGet("most-purchased-bundle")]
    public IActionResult GetMostPurchasedBundles()
    {
        try
        {
            var products = _productService.GetMostPurchasedBundles();

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
            return Ok(productPageData); // HTTP 200
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
    // 1) Aggiorna Nome
    [HttpPut("{id}/update-name")]
    public IActionResult UpdateProductName(int id, [FromBody] UpdateNameRequest request)
    {
        try
        {
            _productService.UpdateProductName(id, request.NewName);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product name for ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // 2) Aggiorna Prezzo
    [HttpPut("{id}/update-price")]
    public IActionResult UpdateProductPrice(int id, [FromBody] UpdatePriceRequest request)
    {
        try
        {
            _productService.UpdateProductPrice(id, request.NewPrice);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product price for ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // 3) Aggiorna Categorie, Allergeni, Descrizione, Dettagli
    [HttpPut("{id}/update-details")]
    public IActionResult UpdateProductAllergensCategoriesDescDetails(int id, [FromBody] UpdateDetailsRequest request)
    {
        try
        {
            _productService.UpdateProductDetailsAndAssociations(
                id,
                request.NewCategories,
                request.NewAllergens,
                request.NewDescription,
                request.NewDetails
            );
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product details for ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // 4) Aggiorna Stock (QuantityAvailable)
    [HttpPut("{id}/update-stock")]
    public IActionResult UpdateProductStock(int id, [FromBody] UpdateStockRequest request)
    {
        try
        {
            _productService.UpdateProductStock(id, request.NewQuantityAvailable);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product stock for ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    public class UpdateNameRequest
    {
        public string NewName { get; set; }
    }

    public class UpdatePriceRequest
    {
        public decimal NewPrice { get; set; }
    }

    public class UpdateDetailsRequest
    {
        public List<string> NewCategories { get; set; }
        public List<string> NewAllergens { get; set; }
        public string NewDescription { get; set; }
        public string NewDetails { get; set; }
    }

    public class UpdateStockRequest
    {
        public int NewQuantityAvailable { get; set; }
    }




}
