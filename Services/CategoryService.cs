using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class CategoryService
    {
        private readonly IDatabaseService _databaseService;

        public CategoryService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _databaseService.Query<Category>(
                "SELECT * FROM Categories WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<Category> GetAllCategoriesAAA()
        {
            return _databaseService.Query<Category>(
                "SELECT * FROM Categories WHERE Deleted IS NULL"
            );
        }
        public Category GetCategoryById(int categoryId)
        {
            return _databaseService.QuerySingle<Category>(
                "SELECT * FROM Categories WHERE CategoryID = @CategoryID AND Deleted IS NULL",
                new { CategoryID = categoryId }
            );
        }

        public void AddCategory(Category category)
        {
            _databaseService.Execute(
                @"INSERT INTO Categories (CategoryName, Description, Created) 
                  VALUES (@CategoryName, @Description, @Created)",
                new
                {
                    category.CategoryName,
                    category.Description,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateCategory(Category category)
        {
            _databaseService.Execute(
                @"UPDATE Categories 
                  SET CategoryName = @CategoryName, Description = @Description, Modified = @Modified 
                  WHERE CategoryID = @CategoryID AND Deleted IS NULL",
                new
                {
                    category.CategoryName,
                    category.Description,
                    Modified = DateTime.UtcNow,
                    category.CategoryID
                }
            );
        }

        public void DeleteCategory(int categoryId)
        {
            _databaseService.Execute(
                @"UPDATE Categories 
                  SET Deleted = @Deleted 
                  WHERE CategoryID = @CategoryID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    CategoryID = categoryId
                }
            );
        }
    }
}
