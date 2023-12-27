using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketLand_project.ViewModels
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int MovieId { get; set; }
        public int RoomNumber { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
