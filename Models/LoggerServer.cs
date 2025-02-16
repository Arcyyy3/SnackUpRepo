using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackUpAPI.Models
{
	public class LoggerServer
	{
		[Key]
		public int ID { get; set; } // Chiave primaria autonumerata

		public DateTime Created { get; set; } = DateTime.UtcNow; // Data di creazione

		[Required]
		public int LogType { get; set; } // Legame con Products

		[Required]
		[MaxLength(4000)]
		public string LogMessage { get; set; } // Motivo del cambiamento


		[Required]
		[MaxLength(4000)]
		public string StackTrace { get; set; } // Motivo del cambiamento


		[Required]
		[MaxLength(4000)]
		public string LogSource { get; set; } // Motivo del cambiamento


	}
}
