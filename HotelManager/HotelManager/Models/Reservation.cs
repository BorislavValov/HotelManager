using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManager.Models
{
    public class Reservation
    {
        public Reservation()
        {
            Clients = new List<Client>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        [Required]
        public virtual Room Room { get; set; }

        [Required]
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        [Required]
        public virtual Client Client{ get; set; }

        public List<Client> Clients { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        public bool BreakfastIncluded { get; set; }

        [Required]
        public bool IsAllInclusive { get; set; }

        [Required]
        public double TotalPrice { get; set; }
    }
}
