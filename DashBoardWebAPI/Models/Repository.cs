using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DashBoardWebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using System.Diagnostics;
namespace DashBoardWebAPI.Models
{
    public class Repository
    {
        static IHubContext<MyHub> _myHubContext;
        static int counter = 0;

        static string connstring = @"data source=DESKTOP-MH76MER\SQLEXPRESS;initial catalog=Dashboard;integrated security=True;" +
    "MultipleActiveResultSets=True";
        static SqlConnection con = new SqlConnection(connstring);


        static Repository()
        {
            SqlDependency.Start(connstring);
            con.Open();

        }
        public Repository(IHubContext<MyHub> myHubContext)
        {
            
            _myHubContext = myHubContext;
        }

        public List<Training> GetTrainingDetails()
        {
            List<Training> trainings = new List<Training>();
             String query = "select  [dbo].[Training].[TrainingName],[dbo].[Training].[Lvl], [dbo].[Training].[CourseID], [dbo].[Training].[TrainingID], [dbo].[Training].[StartDate], [dbo].[Training].[EndDate],[dbo].[Training].[Credits] from [dbo].[Training] ";
            using (var command = new SqlCommand(query, con))
                {

                    var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    { 

                        DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        Training tr = new Training();
                        tr.TrainingID = int.Parse(row["TrainingID"].ToString());
                        tr.StartDate = DateTime.Parse(row["StartDate"].ToString());
                        tr.EndDate = DateTime.Parse(row["EndDate"].ToString());
                        tr.Credits = int.Parse(row["Credits"].ToString());
                        tr.courseID = int.Parse(row["CourseID"].ToString());
                        tr.Name = row["TrainingName"].ToString();
                        tr.Level = int.Parse(row["Lvl"].ToString());

                        trainings.Add(tr);

                    }
                }


                
            }
            return trainings;


        }

        public Employee getEmployeeDetails(int id)
        {
            Employee emp = new Employee();
            

            String query = "select [dbo].[Employee].[Name] from [dbo].[Employee] where [dbo].[Employee].[EmployeeID] = " + id;
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        emp.EmployeeName = row["Name"].ToString();

                    }
                }
            }
                

            
            return emp;
        }


        public Course getCourseDetails(int id)
        {
            Course course = new Course();
             String query = "select [dbo].[Course].[CourseID], [dbo].[Course].[Name] from [dbo].[Course] where [dbo].[Course].[CourseID] = " + id;
            using (var command = new SqlCommand(query, con))
                {

                    var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();

                        adapter.Fill(ds);

                        DataTable dTable = ds.Tables[0];

                        foreach (DataRow row in dTable.Rows)
                        {
                            course.CourseID = int.Parse(row["CourseID"].ToString());
                            course.CourseName = row["Name"].ToString();

                        }
                    }
                }
            

           

            return course;
        }



        public Training GetTrainingDetails(int id)
        {
            Training tr = new Training();


            

            String query = "select [dbo].[Training].[CourseID],[dbo].[Training].[TrainingName],[dbo].[Training].[Lvl], [dbo].[Training].[TrainingID], [dbo].[Training].[StartDate], [dbo].[Training].[EndDate],[dbo].[Training].[Credits] from [dbo].[Training] where [dbo].[Training].[TrainingID] = " + id;
            
            using (var command = new SqlCommand(query, con))
                {
                    var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);


                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                       DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        DataTable dTable = ds.Tables[0];

                        foreach (DataRow row in dTable.Rows)
                        {
                            tr.TrainingID = int.Parse(row["TrainingID"].ToString());
                            tr.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            tr.EndDate = DateTime.Parse(row["EndDate"].ToString());
                            tr.Credits = int.Parse(row["Credits"].ToString());
                            tr.courseID = int.Parse(row["CourseID"].ToString());
                            tr.Name = row["TrainingName"].ToString();
                            tr.Level = int.Parse(row["Lvl"].ToString());




                        }
                    }
                }
            
            tr.Topics = GetAllTopics(tr.TrainingID);
            tr.Trainers = GetTrainerIds(tr.TrainingID);
            tr.Participants = GetParticipantIds(tr.TrainingID);
            tr.Prerequisites = GetPrerequisites(tr.TrainingID);
            return tr;

        }

        public List<Topic> GetTopicDetails(int trainerid)
        {
            string query = "select [dbo].[Topic].[StartDate], [dbo].[Topic].[EndDate], [dbo].[Topic].[TopicName], [dbo].[Topic].[TopicID], [dbo].[Topic].[TrainingID] from [dbo].[Topic] where [dbo].[Topic].[TrainerID]=" + trainerid;
            List<Topic> topics = new List<Topic>();
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        Topic t = new Topic();
                        t.TopicName = row["TopicName"].ToString();
                        t.StartDate = DateTime.Parse(row["StartDate"].ToString());
                        t.EndDate = DateTime.Parse(row["EndDate"].ToString());
                        t.TopicID = int.Parse(row["TopicID"].ToString());
                        t.TrainingID = int.Parse(row["TrainingID"].ToString());
                        topics.Add(t);
                    }
                }
            }

            return topics;


        }

        public List<int> GetPrerequisites(int trainingid)
        {
            String query = "select [dbo].[Prerequisite].[PrerequisiteTrainingID] from [dbo].[Prerequisite] where [dbo].[Prerequisite].[TrainingID] = " + trainingid;

            List<int> prerequisites = new List<int>();
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        prerequisites.Add(int.Parse(row["PrerequisiteTrainingID"].ToString()));
                    }
                }
            }

            return prerequisites;
         }


        public int AddMeeting(Meeting meeting)
        {
            String query = "insert into Meeting values(" + meeting.MeetingID + ",'" + meeting.Name + "', '" + meeting.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' , '" + meeting.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' , '" + meeting.Description + "'," + meeting.scheduledBy + ",'"+meeting.Status+"')";
            var command = new SqlCommand(query, con);
            counter = 0;

            int err = 1;
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }

            return err;

        }

        public List<Meeting> GetAllMeetings()
        {
            String query = "select [dbo].[Meeting].[MeetingID],[dbo].[Meeting].[Name], [dbo].[Meeting].[StartDate], [dbo].[Meeting].[EndDate], [dbo].[Meeting].[Description], [dbo].[Meeting].[ScheduledBy], [dbo].[Meeting].[Status] from [dbo].[Meeting]";
            List<Meeting> meetings = new List<Meeting>();
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        Meeting meeting = new Meeting();
                        meeting.MeetingID = int.Parse(row["MeetingID"].ToString());
                        meeting.Name = row["Name"].ToString();
                        meeting.StartDate = DateTime.Parse(row["StartDate"].ToString());

                        meeting.EndDate = DateTime.Parse(row["EndDate"].ToString());

                        meeting.Description = row["Description"].ToString();
                        meeting.scheduledBy = int.Parse(row["ScheduledBy"].ToString());
                        meeting.Status = row["Status"].ToString();
                        meetings.Add(meeting);
                    }
                }


            }
            foreach (var meeting in meetings)
            {
                meeting.TrainersRequestedToPostpone = getTrainersRequestForMeeting(meeting.MeetingID);
                meeting.TrainersEnrolled = getTrainersEnrolledForMeeting(meeting.MeetingID);
            }

            return meetings;

        }



        public int AddTrainingData(Training tr)
        {

            String query = "insert into Training values(" + tr.TrainingID + ", '" + tr.Name + "', '" + tr.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + tr.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " + tr.courseID + ", " + tr.Credits + ", " + tr.Level + ")";
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                foreach(var topic in tr.Topics)
                {
                    AddTopicData(tr.TrainingID, topic);
                    
                }

                foreach(var trainer in tr.Trainers)
                {
                    AddTrainer(tr.TrainingID, trainer);
                }

               foreach(var pr in tr.Prerequisites)
                {
                    AddPrerequisite(tr.TrainingID, pr);
                }
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            counter = 0;

            return err;

        }
        public int AddParticipant(int trainingid, int participantid)
        {
            String query = "insert into Training_Participant values(" + trainingid + "," + participantid + ")";
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }

            return err;

        }

        public int AddTrainer(int trainingid, int trainerid)
        {
            String query = "insert into Training_Trainer values(" + trainingid + "," + trainerid + ")";
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }

            return err;

        }

        public int AddTopicData(int trainingid, Topic topic)
        {
            String query = "insert into Topic values(" + topic.TopicID + ", " + trainingid + ", '"+topic.TopicName+"', '" + topic.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + topic.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " + topic.TrainerID + ", " + topic.DurationDays + ", " + topic.DurationHours + ")";
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }

            return err;

        }


        public int DeleteTraining(int id)
        {
            Training isTrainingAvailable = GetTrainingDetails(id);
            if (isTrainingAvailable.courseID == 0)
            {
                return 2;
            }

            
            String query = "delete from Training where TrainingID = " + id;
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public int DeletePrerequisite(int trainingid, int prerequisiteid)
        {
            String query = "delete from Prerequisite where TrainingID = " + trainingid + " AND PrerequisiteTrainingID = " + prerequisiteid;
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public int DeleteAllPrerequisites(int trainingid)
        {
            String query = "delete from Prerequisite where TrainingID = " + trainingid;
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;
        }

        public int AddPrerequisite(int trainingid, int prerequisiteID)
        {
            String query = "insert into Prerequisite values(" + trainingid + ", " + prerequisiteID + ")";
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;
        }

        public int UpdateMeetingStatus(int id, string status)
        {
            string query = "update Meeting set Status='" + status + "' where MeetingID=" + id;
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }


        public int UpdateMeeting(int id, Meeting meeting)
        {
            DeleteAllTrainersEnrolled(id);
            DeleteAllTrainersRequested(id);
            String query="update Meeting set Name = '"+meeting.Name+"', StartDate='"+meeting.StartDate.ToString("yyyy-MM-dd HH:mm:ss") +"', EndDate='"+ meeting.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "', Description='"+meeting.Description+"', ScheduledBy="+meeting.scheduledBy+", Status='"+meeting.Status+"' where MeetingID = "+id;

            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public int DeleteAllTrainersEnrolled(int id)
        {
            String query = "delete from Trainer_Meeting where MeetingID = " + id;
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }
        public int DeleteAllTrainersRequested(int id)
        {
            String query = "delete from Meeting_Request where MeetingID = " + id;
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public int UpdateTraining(int id, Training newtraining)
        {
            Training isTrainingAvailable = GetTrainingDetails(id);

            if (isTrainingAvailable.courseID == 0)
            {
                return 2;
            }

            
            String query = "update Training set TrainingName='" + newtraining.Name + "', StartDate='" + newtraining.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "', EndDate='" + newtraining.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "', CourseID=" + newtraining.courseID + ", Credits=" + newtraining.Credits + ", Lvl=" + newtraining.Level + " where TrainingID =" + id;
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                foreach(var topic in newtraining.Topics)
                {
                    UpdateTopic(newtraining.TrainingID, topic.TopicID, topic);
                    
                }
                RemoveAllTrainers(newtraining.TrainingID);
                foreach (var trainer in newtraining.Trainers)
                {
                    AddTrainer(newtraining.TrainingID, trainer);
                }

                
                RemoveAllTopics(newtraining.TrainingID);
                foreach(var topic in newtraining.Topics)
                {
                    AddTopicData(newtraining.TrainingID, topic);
                }
                DeleteAllPrerequisites(newtraining.TrainingID);
                foreach(var pr in newtraining.Prerequisites)
                {
                    AddPrerequisite(newtraining.TrainingID, pr);
                }

                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            counter = 0;

            return err;

        }
        public int RemoveAllParticipants(int trainingid)
        {
            string query = "delete from Training_Participant where TrainingID = " + trainingid;
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }



        public int RemoveAllTopics(int trainingid)
        {
            string query = "delete from Topic where TrainingID = " + trainingid;
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public int RemoveAllTrainers(int trainingid)
        {
            string query = "delete from Training_Trainer where TrainingID = " + trainingid;
            var command = new SqlCommand(query, con);
            int err = 1;
            

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public List<Topic> GetAllTopics(int trainingid)
        {
            List<Topic> topics = new List<Topic>();
            String query = "select [dbo].[Topic].[TopicID], [dbo].[Topic].[TopicName], [dbo].[Topic].[StartDate], [dbo].[Topic].[EndDate], [dbo].[Topic].[TrainerID], [dbo].[Topic].[DurationDays], [dbo].[Topic].[DurationHours] from [dbo].[Topic] where [dbo].[Topic].[TrainingID] =" + trainingid;
            using (var command = new SqlCommand(query, con))
                {
                    var dependency = new SqlDependency(command);

                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        DataSet ds = new DataSet();

                        adapter.Fill(ds);

                        DataTable dTable = ds.Tables[0];

                        foreach (DataRow row in dTable.Rows)
                        {
                            Topic topic = new Topic();
                            topic.TopicID = int.Parse(row["TopicID"].ToString());
                            topic.TopicName = row["TopicName"].ToString();
                            topic.StartDate = DateTime.Parse(row["StartDate"].ToString());
                            topic.EndDate = DateTime.Parse(row["EndDate"].ToString());
                            topic.TrainerID = int.Parse(row["TrainerID"].ToString());
                            topic.DurationDays = int.Parse(row["DurationDays"].ToString());
                            topic.DurationHours = int.Parse(row["DurationHours"].ToString());


                            topics.Add(topic);
                        }
                    }
                }
            
            return topics;
        }

        public List<int> GetTrainingsOfTrainer(int trainerID)
        {
            StackTrace stackTrace = new StackTrace();
            string name = stackTrace.GetFrame(1).GetMethod().Name;
            List<int> TrainingIDs = new List<int>();
            String query = "select [dbo].[Training_Trainer].[TrainingID] from [dbo].[Training_Trainer] where [dbo].[Training_Trainer].[TrainerID] = " + trainerID;

            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        TrainingIDs.Add(int.Parse(row["TrainingID"].ToString()));
                    }
                }


            }

            return TrainingIDs;
        }

        public Meeting GetMeetingDetails(int meetingID)
        {
            String query = "select [dbo].[Meeting].[Name], [dbo].[Meeting].[StartDate], [dbo].[Meeting].[EndDate], [dbo].[Meeting].[Description], [dbo].[Meeting].[ScheduledBy], [dbo].[Meeting].[Status] from [dbo].[Meeting] where [dbo].[Meeting].[MeetingID] = " + meetingID;
            Meeting meeting = new Meeting();
            meeting.MeetingID = meetingID;
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        meeting.Name = row["Name"].ToString();
                        meeting.StartDate = DateTime.Parse(row["StartDate"].ToString());

                        meeting.EndDate = DateTime.Parse(row["EndDate"].ToString());

                        meeting.Description = row["Description"].ToString();
                        meeting.scheduledBy = int.Parse(row["ScheduledBy"].ToString());
                        meeting.Status = row["Status"].ToString();
                    }
                }


            }

            meeting.TrainersEnrolled = getTrainersEnrolledForMeeting(meetingID);

            return meeting;

        }

        public int AddMeetingRequest(int trainerid, int meetingid)
        {
            String query = "insert into Meeting_Request values("+trainerid+","+meetingid+")";

            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }
        public int EnrollToMeeting(int trainerid, int meetingid)
        {
            String query = "insert into Trainer_Meeting values(" + trainerid + "," + meetingid + ")";

            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public List<int> getTrainersRequestForMeeting(int meetingID)
        {
            List<int> Trainers = new List<int>();
            String query = "select [dbo].[Meeting_Request].[EmployeeID] from [dbo].[Meeting_Request] where [dbo].[Meeting_Request].[MeetingID] = " + meetingID;
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        Trainers.Add(int.Parse(row["EmployeeID"].ToString()));
                    }
                }


            }

            return Trainers;

        }

        public List<int> getTrainersEnrolledForMeeting(int meetingID)
        {
            List<int> Trainers = new List<int>();
            String query = "select [dbo].[Trainer_Meeting].[EmployeeID] from [dbo].[Trainer_Meeting] where [dbo].[Trainer_Meeting].[MeetingID] = " + meetingID;
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        Trainers.Add(int.Parse(row["EmployeeID"].ToString()));
                    }
                }


            }

            return Trainers;

        }

        public List<Meeting> GetMeetingsOfTrainer(int trainerid)
        {

            List<Meeting> Meetings = new List<Meeting>();
            String query = "select [dbo].[Trainer_Meeting].[MeetingID] from [dbo].[Trainer_Meeting] where [dbo].[Trainer_Meeting].[EmployeeID] = " + trainerid;
            using (var command = new SqlCommand(query, con))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {

                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    DataTable dTable = ds.Tables[0];

                    foreach (DataRow row in dTable.Rows)
                    {
                        Meetings.Add(GetMeetingDetails(int.Parse(row["MeetingID"].ToString())));
                    }
                }


            }

            return Meetings;

        }


        public List<int> GetTrainerIds(int trainingid)
        {
            List<int> TrainerIDs = new List<int>();

            String query = "select [dbo].[Training_Trainer].[TrainerID] from [dbo].[Training_Trainer] where [dbo].[Training_Trainer].[TrainingID] = " + trainingid;
            using (var command = new SqlCommand(query, con))
                {
                    var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        DataSet ds = new DataSet();

                        adapter.Fill(ds);

                        DataTable dTable = ds.Tables[0];

                        foreach (DataRow row in dTable.Rows)
                        {
                            TrainerIDs.Add(int.Parse(row["TrainerID"].ToString()));
                        }
                    }


                }
            
            return TrainerIDs;
        }


        public List<int> GetParticipantIds(int trainingID)
        {
            List<int> ParticipantIds = new List<int>();

            string query = "select [dbo].[Training_Participant].[ParticipantID] from [dbo].[Training_Participant] where [dbo].[Training_Participant].[TrainingID] = " + trainingID;
            
            using (var command = new SqlCommand(query, con))
                {
                    var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        DataSet ds = new DataSet();

                        adapter.Fill(ds);

                        DataTable dTable = ds.Tables[0];

                        foreach (DataRow row in dTable.Rows)
                        {
                            ParticipantIds.Add(int.Parse(row["ParticipantID"].ToString()));
                        }
                    }
                }
            
            return ParticipantIds;

        }

        public int RemoveTrainer(int trainingID, int trainerID)
        {
            String query = "delete from Training_Trainer where TrainingID = " + trainingID + " AND TrainerID = " + trainerID;
            
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;


        }

        public int RemoveTopic(int trainingID, int topicID)
        {
            String query = "delete from Topic where TopicID=" + topicID + " AND TrainingID = " + trainingID;
            
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;


        }

        public int UpdateTopic(int trainingID,int TopicID, Topic updatedTopic)
        {
            
            
            String query = "update Topic set TopicName='" + updatedTopic.TopicName + "', StartDate='" + updatedTopic.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "', EndDate='" + updatedTopic.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "', TrainerID=" + updatedTopic.TrainerID + ", DurationDays =" + updatedTopic.DurationDays + ", DurationHours=" + updatedTopic.DurationHours + " where TrainingID =" + trainingID+" AND TopicID = "+TopicID;
            var command = new SqlCommand(query, con);
            int err = 1;
            
            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }
        public List<Assignment> GetAssignmentSubmissionCount(int trainingid)
        { 
            string query = "select[dbo].[Assignment].[AssignmentID], [dbo].[Assignment].[SubmissionCount] from[dbo].[Assignment] where[dbo].[Assignment].[TrainingID] = " + trainingid;
            List<Assignment> assignmnets = new List<Assignment>();
            using (var command = new SqlCommand(query, con))
            {

                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                DataTable dTable = ds.Tables[0];

                foreach (DataRow row in dTable.Rows)
                {
                    Assignment ass = new Assignment();
                    ass.AssignmentID = (int.Parse(row["AssignmentID"].ToString()));
                    ass.SubmissionCount = (int.Parse(row["SubmissionCount"].ToString()));
                    assignmnets.Add(ass);
                }


            }

            return assignmnets;

        }

        public List<Analytics> GetAttendance(int trainingID)
        {
            string query = "select [dbo].[Analytics].[AttendanceDate], [dbo].[Analytics].[Attendance] from [dbo].[Analytics] where [dbo].[Analytics].[TrainingID]=" + trainingID;

            List<Analytics> analytics = new List<Analytics>();
            

            using (var command = new SqlCommand(query, con))
            {

                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                DataTable dTable = ds.Tables[0];

                foreach (DataRow row in dTable.Rows)
                {
                    Analytics a = new Analytics();
                    a.AttendanceDate = DateTime.Parse(row["AttendanceDate"].ToString());
                    
                    a.attendance = (int.Parse(row["Attendance"].ToString()));
                    analytics.Add(a);

                }


            }

            
            return analytics;

        }


        public string GetUserNameFromEmployeeId(int id)
        {
            String query = "select [dbo].[Employee].[UserName] from [dbo].[Employee] where [dbo].[Employee].[EmployeeID] = " + id;

            string username = ""; 
            
            using (var command = new SqlCommand(query, con))
            {

                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                DataTable dTable = ds.Tables[0];

                foreach (DataRow row in dTable.Rows)
                {
                    username = row["UserName"].ToString();
                    
                }


            }
            return username;
        }


        public Employee GetEmployeeCredentials(string UserName)
        {
            string query = "select [dbo].[Employee].[Name], [dbo].[Employee].[EmployeeID], [dbo].[Employee].[isAdmin] from [dbo].[Employee] where [dbo].[Employee].[UserName] ='" + UserName + "'";
            Employee emp = new Employee();

            
            using (var command = new SqlCommand(query, con))
            {

                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                DataTable dTable = ds.Tables[0];

                foreach (DataRow row in dTable.Rows)
                {
                    emp.EmployeeName = row["Name"].ToString();
                    emp.isAdmin = int.Parse(row["isAdmin"].ToString());
                    emp.password = "";
                    emp.EmployeeID = int.Parse(row["EmployeeID"].ToString());

                }


            }
            return emp;


        }


        public EmployeeValidation GetEmployeeValidation(int id)
        {
            string query = "select [dbo].[EmployeeValidation].[Question1], [dbo].[EmployeeValidation].[Question2], [dbo].[EmployeeValidation].[Question3] from [dbo].[EmployeeValidation] where [dbo].[EmployeeValidation].[EmployeeID] = " + id;
            EmployeeValidation e = new EmployeeValidation();
            using (var command = new SqlCommand(query, con))
            {

                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                DataTable dTable = ds.Tables[0];

                foreach (DataRow row in dTable.Rows)
                {
                    e.Question1 = row["Question1"].ToString();
                    e.Question2 = row["Question2"].ToString();
                    e.Question3 = row["Question3"].ToString();


                }


            }

            return e;
        }

        public int UpdateEmployeeDetails(int EmployeeID, Employee emp)
        {


            String query = "update Employee set password='" + emp.password + "' where EmployeeID =" + EmployeeID ;
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;

        }

        public int AddEmployee(Employee emp)
        {
            emp.EmployeeID = EmployeeCount() + 1;
            String query = "insert into Employee values("+emp.EmployeeID+", '"+emp.EmployeeName+"', "+emp.isAdmin+", '"+emp.password+"')";
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;
        }

        public int AddEmployeeValidation(int id, EmployeeValidation ev)
        {
            String query = "insert into EmployeeValidation values(" + id + ", '" + ev.Question1 + "', '" + ev.Question2 + "', '" + ev.Question3 + "')";
            var command = new SqlCommand(query, con);
            int err = 1;
            counter = 0;

            try
            {
                command.ExecuteNonQuery();
                err = 0;
            }
            catch (Exception e)
            {

                err = 1;
            }
            return err;
        }


        public int EmployeeCount()
        {
            string query = "select [dbo].[Employee].[EmployeeID] from [dbo].[Employee]";
            List<int> employeeids = new List<int>();
            using (var command = new SqlCommand(query, con))
            {

                var dependency = new SqlDependency(command);
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                DataTable dTable = ds.Tables[0];

                foreach (DataRow row in dTable.Rows)
                {
                    employeeids.Add(int.Parse(row["EmployeeID"].ToString()));

                }


            }

            return employeeids.Select(e => e).OrderByDescending(e=>e).FirstOrDefault();


        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            
            if (e.Type == SqlNotificationType.Change && counter == 0)
            {
                
               _myHubContext.Clients.All.SendAsync("updateMessages");
                ++counter;
            }
            

            


        }


    }
}
