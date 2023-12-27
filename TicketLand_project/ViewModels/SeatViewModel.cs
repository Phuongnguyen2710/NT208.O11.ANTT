using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketLand_project.Models;

namespace TicketLand_project.ViewModels
{
    public class SeatViewModel
    {
        public schedule Schedule { get; set; }
        public List<seat> Seats { get; set; }
        public List<string> ReservedSeats { get; set; } // Danh sách các ghế đã chọn
        public List<int> ReservedSeatIds { get; set; }

    }
}