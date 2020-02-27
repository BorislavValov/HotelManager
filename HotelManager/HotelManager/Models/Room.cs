using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelManager.Models
{
    public class Room
    {
        public Room()
        {
            Reservations = new List<Reservation>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public double AdultsPrice { get; set; }

        [Required]
        public double ChildrenPrice { get; set; }

        [Required]
        public string Number { get; set; }

        public bool? IsAvailable { get; set; }

        List<Reservation> Reservations { get; set; }
    }
}
