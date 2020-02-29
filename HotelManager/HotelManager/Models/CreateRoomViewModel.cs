using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelManager.Models
{
    public class CreateRoomViewModel
    {
        public string Number { get; set; }
        [DataType(DataType.Text,ErrorMessage = "Type can contian only characters")]
        public string Type { get; set; }
        public int Capacity { get; set; }
        public double ChildrenPrice { get; set; }
        public double AdultsPrice { get; set; }
    }
}
