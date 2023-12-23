using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketLand_project.ViewModels
{
    public class BookingViewModel
    {
        public int TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public int MemberId { get; set; }
        public int BookingStatus { get; set; }
        
        
    }
}
