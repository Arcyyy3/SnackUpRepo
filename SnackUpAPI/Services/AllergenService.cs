using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class AllergenService
    {
        private readonly DatabaseService _databaseService;

        public AllergenService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Allergen> GetAllAllergens()
        {
            return _databaseService.Query<Allergen>(
                "SELECT * FROM Allergens WHERE Deleted IS NULL"
            );
        }

        public Allergen GetAllergenById(int id)
        {
            return _databaseService.QuerySingle<Allergen>(
                "SELECT * FROM Allergens WHERE AllergenID = @AllergenID AND Deleted IS NULL",
                new { AllergenID = id }
            );
        }

        public void AddAllergen(Allergen allergen)
        {
            _databaseService.Execute(
                @"INSERT INTO Allergens (Name, Description, Created) 
                  VALUES (@Name, @Description, @Created)",
                new
                {
                    allergen.Name,
                    allergen.Description,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateAllergen(Allergen allergen)
        {
            _databaseService.Execute(
                @"UPDATE Allergens 
                  SET Name = @Name, Description = @Description, Modified = @Modified 
                  WHERE AllergenID = @AllergenID AND Deleted IS NULL",
                new
                {
                    allergen.Name,
                    allergen.Description,
                    Modified = DateTime.UtcNow,
                    allergen.AllergenID
                }
            );
        }

        public void DeleteAllergen(int id)
        {
            _databaseService.Execute(
                @"UPDATE Allergens 
                  SET Deleted = @Deleted 
                  WHERE AllergenID = @AllergenID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    AllergenID = id
                }
            );
        }
    }
}
