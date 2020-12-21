using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardWebAPI.Models
{
    public class Training
    {
        public int TrainingID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Credits { get; set; }
        public int courseID { get; set; }
        public string Name {get;set;}
        public int Level { get; set; }

        public List<Topic> Topics { get; set; } = new List<Topic>();
        public List<int> Trainers { get; set; } = new List<int>();
        public List<int> Participants { get; set; } = new List<int>();
        public List<int> Prerequisites { get; set; } = new List<int>();
        

        
    }
}
