using System;
using System.Collections.Generic;
using System.Linq;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class SchoolService
    {
        private readonly DatabaseService _databaseService;

        public SchoolService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<School> GetAllSchools()
        {
            return _databaseService.Query<School>("SELECT * FROM Schools WHERE Deleted IS NULL");
        }

        public School GetSchoolById(int schoolId)
        {
            return _databaseService.QuerySingleOrDefault<School>(
                "SELECT * FROM Schools WHERE SchoolID = @SchoolID AND Deleted IS NULL",
                new { SchoolID = schoolId }
            );
        }

        public int GetSchoolIDByAddressAndName(string address, string name)
        {
            return _databaseService.QuerySingleOrDefault<int>(
                "SELECT SchoolID FROM Schools WHERE Address = @Address AND Name = @Name AND Deleted IS NULL",
                new { Address = address, Name = name }
            );
        }

        public List<string> GetCityList()
        {
            return _databaseService.Query<string>(
                "SELECT DISTINCT Address FROM Schools WHERE Deleted IS NULL"
            ).ToList();
        }

        public List<String> GetSchoolsInCity(string city)
        {
            return _databaseService.Query<String>(
                "SELECT Name FROM Schools WHERE Address LIKE @City AND Deleted IS NULL",
                new { City = $"%{city}%" }
            ).ToList();
        }

        public int? GetProducerIDBySchoolID(int schoolId)
        {
            return _databaseService.QuerySingleOrDefault<int?>(
                "SELECT ProducerID FROM Schools WHERE SchoolID = @SchoolID AND Deleted IS NULL",
                new { SchoolID = schoolId }
            );
        }

        public void AddSchool(School school)
        {
            _databaseService.Execute(
                @"INSERT INTO Schools (Name, Address, ProducerID, Created, Modified, Deleted) 
                  VALUES (@Name, @Address, @ProducerID, @Created, NULL, NULL)",
                new
                {
                    school.Name,
                    school.Address,
                    school.ProducerID,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateSchool(School school)
        {
            _databaseService.Execute(
                @"UPDATE Schools 
                  SET Name = @Name, Address = @Address, ProducerID = @ProducerID, 
                      Modified = @Modified 
                  WHERE SchoolID = @SchoolID AND Deleted IS NULL",
                new
                {
                    school.Name,
                    school.Address,
                    school.ProducerID,
                    Modified = DateTime.UtcNow,
                    school.SchoolID
                }
            );
        }

        public void DeleteSchool(int schoolId)
        {
            _databaseService.Execute(
                @"UPDATE Schools 
                  SET Deleted = @Deleted 
                  WHERE SchoolID = @SchoolID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    SchoolID = schoolId
                }
            );
        }
    }
}
