using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ProducerService
    {
        private readonly IDatabaseService _databaseService;

        public ProducerService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Producer> GetAllProducers()
        {
            return _databaseService.Query<Producer>(
                "SELECT * FROM Producers WHERE Deleted IS NULL"
            );
        }

        public Producer GetProducerById(int id)
        {
            return _databaseService.QuerySingle<Producer>(
                "SELECT * FROM Producers WHERE ProducerID = @ProducerID AND Deleted IS NULL",
                new { ProducerID = id }
            );
        }

        public void AddProducer(Producer producer)
        {
            _databaseService.Execute(
                @"INSERT INTO Producers (ProducerName, ContactInfo, Address, Created, Modified, Deleted) 
                  VALUES (@ProducerName, @ContactInfo, @Address, @Created, NULL, NULL)",
                new
                {
                    producer.ProducerName,
                    producer.ContactInfo,
                    producer.Address,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateProducer(Producer producer)
        {
            _databaseService.Execute(
                @"UPDATE Producers 
                  SET ProducerName = @ProducerName, ContactInfo = @ContactInfo, Address = @Address, 
                      Modified = @Modified 
                  WHERE ProducerID = @ProducerID AND Deleted IS NULL",
                new
                {
                    producer.ProducerName,
                    producer.ContactInfo,
                    producer.Address,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    producer.ProducerID
                }
            );
        }

        public void DeleteProducer(int id)
        {
            _databaseService.Execute(
                @"UPDATE Producers 
                  SET Deleted = @Deleted 
                  WHERE ProducerID = @ProducerID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    ProducerID = id
                }
            );
        }
    }
}
