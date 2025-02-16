using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ProducerUsersService
    {
        private readonly IDatabaseService _databaseService;

        public ProducerUsersService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public string GetProducerByID(int id)
        {
            string Producer = _databaseService.QuerySingle<string>(
                "SELECT PS.ProducerID FROM ProducerUsers as PS WHERE PS.UserID =@UserID AND PS.Deleted IS NULL",
                new { UserID = id }
            );
            return Producer;
        }
    }
}