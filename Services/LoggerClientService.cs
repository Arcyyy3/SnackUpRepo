using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class LoggerClientService
    {
        private readonly IDatabaseService _databaseService;

        public LoggerClientService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public void AddLoggerClient(LoggerClient logger)
        {
            _databaseService.Execute(
                @"INSERT INTO LoggerClient (Created, LogType, LogMessage, StackTrace, LogSource) 
                  VALUES ( @Created,@LogType, @LogMessage,@StackTrace,@LogSource)",
                new
                {
                    Created = DateTime.UtcNow,
                    logger.LogType,
                    logger.LogMessage,
                    logger.StackTrace,
                    logger.LogSource
                }
            );
        }

    }
}
