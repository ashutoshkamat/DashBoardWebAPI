using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardWebAPI.Models
{
    public class Meeting
    {
        public int MeetingID { get; set; }
        public string Name { get; set; } = "";
        public DateTime StartDate { get; set; } = new DateTime();
        public DateTime EndDate { get; set; } = new DateTime();
        public string Description { get; set; } = "";
        public int scheduledBy { get; set; }
        public string Status { get; set; } = "";
        public List<int> TrainersEnrolled { get; set; } = new List<int>();
        public List<int> TrainersRequestedToPostpone { get; set; } = new List<int>();
    }
}
