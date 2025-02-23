using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SnackUpAPI.Models;
using SnackUpAPI.Db_Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class AllergenService
    {
        private readonly DbSnackUpProjectContext _context;

        public AllergenService(DbSnackUpProjectContext context)
        {
            _context = context; // Dependency Injection
        }

        public IEnumerable<SnackUpAPI.Db_Models.Allergen> GetAllAllergens()
        {
            try
            {
                return _context.Allergens.Where(a => a.Deleted == null).ToList();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}
