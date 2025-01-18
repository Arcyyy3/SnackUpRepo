using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class ProducerService
    {
        private readonly DatabaseService _databaseService;

        public ProducerService(DatabaseService databaseService)
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
                @"INSERT INTO Producers (Name, ContactInfo, Address, Created, Modified, Deleted) 
                  VALUES (@Name, @ContactInfo, @Address, @Created, NULL, NULL)",
                new
                {
                    producer.Name,
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
                  SET Name = @Name, ContactInfo = @ContactInfo, Address = @Address, 
                      Modified = @Modified 
                  WHERE ProducerID = @ProducerID AND Deleted IS NULL",
                new
                {
                    producer.Name,
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
