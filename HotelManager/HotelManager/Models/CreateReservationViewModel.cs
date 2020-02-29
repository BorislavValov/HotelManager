using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManager.Models
{
    public class CreateReservationViewModel
    {
        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public bool BreakfastIncluded { get; set; }

        public bool IsAllInclusive { get; set; }

        public int ChildrenCount { get; set; }
        public int AdultsCount { get; set; }
    }
}
