using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SnackUpAPI
{
    public class AmazonSESHelper
    {
        private static readonly string accessKey = "AKIA2KX4BWDD4WEPJYZY";
        private static readonly string secretKey = "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";
        private static readonly RegionEndpoint region = RegionEndpoint.EUCentral1;

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new AmazonSimpleEmailServiceClient(accessKey, secretKey, region))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = "snackupitalia@gmail.com", // Deve essere un'email verificata
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { toEmail }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Text = new Content(body)
                        }
                    }
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    Console.WriteLine($"Email inviata con successo! MessageId: {response.MessageId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore nell'invio email: {ex.Message}");
                }
            }
        }
    }
}