using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/[controller]")]
public class CategoryProductController : ControllerBase
{
    private readonly CategoryProductService _categoryProductService;

    public CategoryProductController(CategoryProductService categoryProductService)
    {
        _categoryProductService = categoryProductService;
    }

    [HttpGet("GetDrink")]
    public IActionResult GetDrink()
    {
        try
        {
            var products = _categoryProductService.GetDrink();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching GetDrink: {ex.Message}");
        }
    }
    [HttpGet("products-by-category/{categoryId}")]
    public IActionResult GetProductsByCategoryId(int categoryId)
    {
        try
        {
            var products = _categoryProductService.GetProductsByCategoryId(categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching products: {ex.Message}");
        }
    }
    [HttpGet("categories-by-product/{productId}")]
    public IActionResult GetCategoriesByProductId(int productId)
    {
        try
        {
            var categories = _categoryProductService.GetCategoriesByProductId(productId);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching categories: {ex.Message}");
        }
    }

    [HttpPost("add-category-to-product")]
    public IActionResult AddCategoryToProduct([FromBody] CategoryProduct categoryProduct)
    {
        try
        {
            _categoryProductService.AddCategoryToProduct(categoryProduct.ProductID, categoryProduct.CategoryID);
            return Ok("Category linked to product successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error linking category to product: {ex.Message}");
        }
    }

    [HttpDelete("remove-category-from-product")]
    public IActionResult RemoveCategoryFromProduct([FromBody] CategoryProduct categoryProduct)
    {
        try
        {
            _categoryProductService.RemoveCategoryFromProduct(categoryProduct.ProductID, categoryProduct.CategoryID);
            return Ok("Category unlinked from product successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error unlinking category from product: {ex.Message}");
        }
    }
}
