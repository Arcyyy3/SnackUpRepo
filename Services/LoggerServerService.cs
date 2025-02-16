using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class LoggerServerService
    {
        private readonly IDatabaseService _databaseService;

        public LoggerServerService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public void AddLoggerServer(LoggerServer logger )
        {
            _databaseService.Execute(
                @"INSERT INTO Logger (Created, LogType, LogMessage, StackTrace, LogSource) 
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
