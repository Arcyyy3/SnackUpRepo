using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;
using Microsoft.AspNetCore.Authorization;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class AllergenService
    {
        private readonly IDatabaseService _databaseService;

        public AllergenService(IDatabaseService databaseService)
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
                @"INSERT INTO Allergens (AllergenName, Description, Created) 
                  VALUES (@AllergenName, @Description, @Created)",
                new
                {
                    allergen.AllergenName,
                    allergen.Description,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateAllergen(Allergen allergen)
        {
            _databaseService.Execute(
                @"UPDATE Allergens 
                  SET AllergenName = @AllergenName, Description = @Description, Modified = @Modified 
                  WHERE AllergenID = @AllergenID AND Deleted IS NULL",
                new
                {
                    allergen.AllergenName,
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
