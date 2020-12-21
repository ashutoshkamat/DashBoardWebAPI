using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardWebAPI.Models
{
    public class Topic
    {
        public int TopicID { get; set; }
        public int TrainingID { get; set; }
        public string TopicName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TrainerID { get; set; }
        public int DurationDays { get; set; }
        public int DurationHours { get; set; }
    }
}
