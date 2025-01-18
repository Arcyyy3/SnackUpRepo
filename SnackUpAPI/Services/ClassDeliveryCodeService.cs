using System;
using System.Collections.Generic;
using System.Diagnostics;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class ClassDeliveryCodeService
    {
        private readonly DatabaseService _databaseService;

        public ClassDeliveryCodeService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<ClassDeliveryCode> GetAll()
        {
            return _databaseService.Query<ClassDeliveryCode>(
                "SELECT * FROM ClassDeliveryCodes WHERE Deleted IS NULL"
            );
        }

        public ClassDeliveryCode GetById(int id)
        {
            return _databaseService.QuerySingle<ClassDeliveryCode>(
                "SELECT * FROM ClassDeliveryCodes WHERE ClassDeliveryCodeID = @ClassDeliveryCodeID AND Deleted IS NULL",
                new { ClassDeliveryCodeID = id }
            );
        }
        public IEnumerable<Order> GetOrdersByClassId(int schoolClassId)
        {
            return _databaseService.Query<Order>(
                @"SELECT * 
                  FROM Orders 
                  WHERE SchoolClassID = @SchoolClassID AND Deleted IS NULL",
                new { SchoolClassID = schoolClassId }
            );
        }
        public User GetUserById(int userId)
        {
            return _databaseService.QuerySingle<User>(
                "SELECT * FROM Users WHERE UserID = @UserID AND Deleted IS NULL",
                new { UserID = userId }
            );
        }

        public object GetDeliveryCode(int userID, string currentMoment)
        {
            if (string.IsNullOrEmpty(currentMoment))
            {
                return new { Message = "Current moment is required." };
            }

            string query = currentMoment switch
            {
                "First" => @"
            SELECT Code1 
            FROM ClassDeliveryCodes AS CD
            INNER JOIN Users AS U ON CD.SchoolClassID = U.SchoolClassID
            WHERE U.UserID = @UserID AND CD.Deleted IS NULL",
                "Second" => @"
            SELECT Code2 
            FROM ClassDeliveryCodes AS CD
            INNER JOIN Users AS U ON CD.SchoolClassID = U.SchoolClassID
            WHERE U.UserID = @UserID AND CD.Deleted IS NULL",
                _ => null
            };

            if (query == null)
            {
                return new { Message = "Invalid moment of the day." };
            }

            try
            {
                var code = _databaseService.QuerySingle<string>(query, new { UserID = userID });
                return  code;
            }
            catch (Exception ex)
            {
                return new { Message = "An error occurred while retrieving the delivery code.", Details = ex.Message };
            }
        }

        public string GetUserName(int userID)
        {
            string query = @"SELECT Name FROM Users WHERE UserID = @UserID";

            return _databaseService.QuerySingle<string>(query, new { UserID = userID });
        }

        public object GetCodeInformation(int userID, string currentMoment)
        {
            string query = currentMoment switch
            {
                "First" => @"SELECT TOP 1 
                       CD.ClassDeliveryCodeID
                     FROM 
                       ClassDeliveryCodes AS CD
                     WHERE 
                       CD.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
                       AND CD.Deleted IS NULL",
                "Second" => @"SELECT TOP 1 
                        CD.ClassDeliveryCodeID
                     FROM 
                        ClassDeliveryCodes AS CD
                     WHERE 
                        CD.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
                        AND CD.Deleted IS NULL",
                _ => null
            };

            if (query == null)
            {
                return new { Message = "Invalid moment of the day." };
            }

            try
            {
                var classDeliveryCodeID = _databaseService.QuerySingle<int>(query, new { UserID = userID });

                return new CodeInformation
                {
                    ClassDeliveryCodeID = classDeliveryCodeID,
                    Moment = currentMoment
                };
            }
            catch (Exception ex)
            {
                return new { Message = "Error retrieving code information.", Details = ex.Message };
            }
        }

        public object ConfirmCodeRetrieval(int userID, string currentMoment)
        {
            string updateQuery = currentMoment switch
            {
                "First" => "UPDATE ClassDeliveryCodes SET RetrievedCode1 = 1, Modified = GETDATE() WHERE SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) AND Deleted IS NULL",
                "Second" => "UPDATE ClassDeliveryCodes SET RetrievedCode2 = 1, Modified = GETDATE() WHERE SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) AND Deleted IS NULL",
                _ => null
            };

            if (updateQuery == null)
            {
                return new { Message = "Invalid moment of the day." };
            }

            try
            {
                _databaseService.Execute(updateQuery, new { UserID = userID });

                string userNameQuery = "SELECT Name FROM Users WHERE UserID = @UserID";
                var userName = _databaseService.QuerySingle<string>(userNameQuery, new { UserID = userID });

                _databaseService.Execute(
                    @"INSERT INTO ClassDeliveryLogs (ClassDeliveryCodeID, UserID, CodeType, Timestamp)
              SELECT ClassDeliveryCodeID, @UserID, @CodeType, GETDATE()
              FROM ClassDeliveryCodes
              WHERE SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) AND Deleted IS NULL",
                    new
                    {
                        UserID = userID,
                        CodeType = currentMoment == "First" ? "Code1" : "Code2"
                    }
                );

                return new { Message = "Code retrieval confirmed.", RetrievedBy = userName };
            }
            catch (Exception ex)
            {
                return new { Message = "Error confirming code retrieval.", Details = ex.Message };
            }
        }
        public CodeStatusResponse GetCodeStatus(int userID, string currentMoment)
        {
            Debug.WriteLine($"Momento in entrata: {currentMoment}");

            string query = currentMoment switch
            {
                "First" => @"SELECT TOP 1 
                      CD.RetrievedCode1 AS IsRetrieved, 
                      U.Name AS RetrievedBy
                  FROM 
                      ClassDeliveryCodes AS CD
                  INNER JOIN 
                      ClassDeliveryLogs AS CL ON CD.ClassDeliveryCodeID = CL.ClassDeliveryCodeID
                  INNER JOIN 
                      Users AS U ON CL.UserID = U.UserID
                  WHERE 
                      CD.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) 
                      AND CL.CodeType = 'Code1' 
                      AND CD.Deleted IS NULL
                  ORDER BY CL.Timestamp DESC",
                "Second" => @"SELECT TOP 1 
                       CD.RetrievedCode2 AS IsRetrieved, 
                       U.Name AS RetrievedBy
                   FROM 
                       ClassDeliveryCodes AS CD
                   INNER JOIN 
                       ClassDeliveryLogs AS CL ON CD.ClassDeliveryCodeID = CL.ClassDeliveryCodeID
                   INNER JOIN 
                       Users AS U ON CL.UserID = U.UserID
                   WHERE 
                       CD.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) 
                       AND CL.CodeType = 'Code2' 
                       AND CD.Deleted IS NULL
                   ORDER BY CL.Timestamp DESC",
                _ => null
            };
            if(query == null)
            {
                return new CodeStatusResponse
                {
                    IsRetrieved = false,
                    RetrievedBy = "Invalid",
                };
            }
            try
            {
                return _databaseService.QuerySingle<CodeStatusResponse>(query, new { UserID = userID });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving code status: {ex.Message}");
                return new CodeStatusResponse { IsRetrieved = false, RetrievedBy = "N/A" }; // Default response
            }
        }

        public bool IsCodeAlreadyRetrieved(int userID, string currentMoment)
        {
            string query = currentMoment switch
            {
                "First" => @"SELECT RetrievedCode1 FROM ClassDeliveryCodes 
                     WHERE SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) 
                     AND Deleted IS NULL",
                "Second" => @"SELECT RetrievedCode2 FROM ClassDeliveryCodes 
                      WHERE SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID) 
                      AND Deleted IS NULL",
                _ => null
            };
            if (query == null)
            {
                return false;
            }
            try
            {
                return _databaseService.QuerySingle<bool>(query, new { UserID = userID });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking code retrieval: {ex.Message}");
                return false; // Ritorna false in caso di errore
            }
        }

        public async Task<string> IsCodeAlreadyRetrievedWithName(int userID, string currentMoment)
        {
            string query = currentMoment switch
            {
                 "First" => @"
                    SELECT 
                        CASE 
                            WHEN CD.RetrievedCode1 = 1 THEN CONCAT(U.Name, ' ', U.Surname)
                            ELSE 'non ritirato'
                        END AS Status
                    FROM 
                        ClassDeliveryCodes AS CD
                    LEFT JOIN 
                        ClassDeliveryLogs AS CL ON CD.ClassDeliveryCodeID = CL.ClassDeliveryCodeID AND CL.CodeType = 'Code1' AND CAST(CL.Timestamp AS DATE) = CAST(DATEADD(DAY, 0, GETDATE()) AS DATE)
                    LEFT JOIN 
                        Users AS U ON CL.UserID = U.UserID
                    WHERE 
                        CD.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
                        AND CD.Deleted IS NULL",
                "Second" => @"
                    SELECT 
                        CASE 
                            WHEN CD.RetrievedCode2 = 1 THEN CONCAT(U.Name, ' ', U.Surname)
                            ELSE 'non ritirato'
                        END AS Status
                    FROM 
                        ClassDeliveryCodes AS CD
                    LEFT JOIN 
                        ClassDeliveryLogs AS CL ON CD.ClassDeliveryCodeID = CL.ClassDeliveryCodeID AND CL.CodeType = 'Code2' AND CAST(CL.Timestamp AS DATE) = CAST(DATEADD(DAY, 0, GETDATE()) AS DATE)
                    LEFT JOIN 
                        Users AS U ON CL.UserID = U.UserID
                    WHERE 
                        CD.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
                        AND CD.Deleted IS NULL",
                _ => null
            };

            if (query == null)
            {
                return "invalid moment";
            }

            try
            {
                // Usa `await` per eseguire la query in modo asincrono
                return await _databaseService.QuerySingleAsync<string>(query, new { UserID = userID });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving code status: {ex.Message}");
                return "Errore nel recupero dello stato del codice";
            }
        }


        public void Add(ClassDeliveryCode code)
        {
            _databaseService.Execute(
                @"INSERT INTO ClassDeliveryCodes (SchoolClassID, Code1, Code2, RetrievedCode1, RetrievedCode2, Created, Modified, Deleted)
                  VALUES (@SchoolClassID, @Code1, @Code2, @RetrievedCode1, @RetrievedCode2, @Created, NULL, NULL)",
                new
                {
                    code.SchoolClassID,
                    code.Code1,
                    code.Code2,
                    code.RetrievedCode1,
                    code.RetrievedCode2,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void Update(ClassDeliveryCode code)
        {
            _databaseService.Execute(
                @"UPDATE ClassDeliveryCodes 
                  SET SchoolClassID = @SchoolClassID, Code1 = @Code1, Code2 = @Code2, 
                      RetrievedCode1 = @RetrievedCode1, RetrievedCode2 = @RetrievedCode2, 
                      Modified = @Modified 
                  WHERE ClassDeliveryCodeID = @ClassDeliveryCodeID AND Deleted IS NULL",
                new
                {
                    code.SchoolClassID,
                    code.Code1,
                    code.Code2,
                    code.RetrievedCode1,
                    code.RetrievedCode2,
                    Modified = DateTime.UtcNow,
                    code.ClassDeliveryCodeID
                }
            );
        }

        public void Delete(int id)
        {
            _databaseService.Execute(
                @"UPDATE ClassDeliveryCodes 
                  SET Deleted = @Deleted 
                  WHERE ClassDeliveryCodeID = @ClassDeliveryCodeID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    ClassDeliveryCodeID = id
                }
            );
        }
        public class CodeInformation
        {
            public int ClassDeliveryCodeID { get; set; }
            public string Moment { get; set; }
        }
        public class CodeStatusResponse
        {
            public bool IsRetrieved { get; set; }
            public string RetrievedBy { get; set; }
        }


    }
}

