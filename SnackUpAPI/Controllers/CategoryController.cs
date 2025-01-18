using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult GetAllCategories()
    {
        try
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching categories: {ex.Message}");
        }
    }

  
    [HttpGet("{id}")]
    public IActionResult GetCategoryById(int id)
    {
        try
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");
            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching category: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult AddCategory([FromBody] Category category)
    {
        try
        {
            _categoryService.AddCategory(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryID }, category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding category: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, [FromBody] Category category)
    {
        try
        {
            category.CategoryID = id;
            _categoryService.UpdateCategory(category);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating category: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        try
        {
            _categoryService.DeleteCategory(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting category: {ex.Message}");
        }
    }
}
