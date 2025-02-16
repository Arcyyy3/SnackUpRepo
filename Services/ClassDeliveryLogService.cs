using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ClassDeliveryLogService
    {
        private readonly IDatabaseService _databaseService;

        public ClassDeliveryLogService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<ClassDeliveryLog> GetAll()
        {
            return _databaseService.Query<ClassDeliveryLog>(
                "SELECT * FROM ClassDeliveryLogs"
            );
        }

        public IEnumerable<ClassDeliveryLog> GetLogsByClassDeliveryCodeId(int classDeliveryCodeId, string moment)

        {

            return _databaseService.Query<ClassDeliveryLog>(
                "SELECT CD.UserID, CD.CodeType FROM ClassDeliveryLogs AS CD WHERE CD.ClassDeliveryCodeID = @ClassDeliveryCodeID AND CD.CodeType=@CodeType",
                new { ClassDeliveryCodeID = classDeliveryCodeId, CodeType=moment }
            );
        }

        public void Add(ClassDeliveryLog log)
        {
            _databaseService.Execute(
                @"INSERT INTO ClassDeliveryLogs (ClassDeliveryCodeID, UserID, CodeType, Timestamp)
                  VALUES (@ClassDeliveryCodeID, @UserID, @CodeType, @Timestamp)",
                new
                {
                    log.ClassDeliveryCodeID,
                    log.UserID,
                    log.CodeType,
                    Timestamp = DateTime.UtcNow
                }
            );
        }

    }
}
