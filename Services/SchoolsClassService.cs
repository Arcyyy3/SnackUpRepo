using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class SchoolClassService
    {
        private readonly IDatabaseService _databaseService;

        public SchoolClassService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<SchoolClass> GetAllSchoolClasses()
        {
            return _databaseService.Query<SchoolClass>("SELECT * FROM SchoolClasses");
        }

        public IEnumerable<SchoolClass> GetSchoolClassesBySchoolId(int schoolId)
        {
            return _databaseService.Query<SchoolClass>(
                "SELECT * FROM SchoolClasses WHERE SchoolID = @SchoolID",
                new { SchoolID = schoolId }
            );
        }
        public int GetSchoolClassesIDByParameters(int schoolId,int classyear,string classsection)
        {
            return _databaseService.QuerySingle<int>(
            "SELECT SchoolClassID FROM SchoolClasses WHERE SchoolID =@SchoolID AND ClassYear =  @ClassYear AND ClassSection =@ClassSection ;",
                new { SchoolID = schoolId, ClassYear = classyear, ClassSection = classsection }

                );
        }
    
        public List<string> GetClassSectionByParameters(int schoolId, int classyear)
        {
            return _databaseService.Query<string>(
            "SELECT ClassSection FROM SchoolClasses WHERE SchoolID =@SchoolID AND ClassYear =  @ClassYear ;",
                new { SchoolID = schoolId, ClassYear = classyear }

                ).ToList();
        }

        public List<ClassYearSection> GetClassYearAndSectionByUserID(int userID)
        {
            return _databaseService.Query<ClassYearSection>(
                @"SELECT 
         sc.ClassYear,
         sc.ClassSection
         FROM 
         Users u
         INNER JOIN 
         SchoolClasses sc ON u.SchoolClassID = sc.SchoolClassID
         WHERE 
         u.UserID = @UserID
         AND u.Deleted IS NULL 
         AND sc.Deleted IS NULL;",
                new { UserID = userID }
            ).ToList();
        }


        public SchoolClass GetSchoolClassById(int id)
        {
            return _databaseService.QuerySingle<SchoolClass>(
                "SELECT * FROM SchoolClasses WHERE SchoolClassID = @SchoolClassID",
                new { SchoolClassID = id }
            );
        }

        public void AddSchoolClass(SchoolClass schoolClass)
        {
            _databaseService.Execute(
                @"INSERT INTO SchoolClasses (SchoolID, ClassYear,ClassSection) 
                  VALUES (@SchoolID, @ClassYear,@ClassSection)",
                schoolClass
            );
        }
        public void UpdateSchoolClass(SchoolClass schoolClass)
        {
            _databaseService.Execute(
                @"UPDATE SchoolClasses 
                  SET SchoolID = @SchoolID, ClassYear = @ClassYear , ClassSection = @ClassSection 
                  WHERE SchoolClassID = @SchoolClassID",
                schoolClass
            );
        }

        public void DeleteSchoolClass(int id)
        {
            _databaseService.Execute(
                "DELETE FROM SchoolClasses WHERE SchoolClassID = @SchoolClassID",
                new { SchoolClassID = id }
            );
        }
        public class ClassYearSection
        {
            public int classYear { get; set; }
            public string classSection { get; set; }
        }

    }
}
