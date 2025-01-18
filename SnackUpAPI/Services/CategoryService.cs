using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class CategoryService
    {
        private readonly DatabaseService _databaseService;

        public CategoryService(DatabaseService databaseService)
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
                @"INSERT INTO Categories (Name, Description, Created) 
                  VALUES (@Name, @Description, @Created)",
                new
                {
                    category.Name,
                    category.Description,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateCategory(Category category)
        {
            _databaseService.Execute(
                @"UPDATE Categories 
                  SET Name = @Name, Description = @Description, Modified = @Modified 
                  WHERE CategoryID = @CategoryID AND Deleted IS NULL",
                new
                {
                    category.Name,
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
