using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DashBoardWebAPI.Models;
using System.Dynamic;
using Microsoft.AspNetCore.SignalR;
using DashBoardWebAPI.Hubs;

namespace DashBoardWebAPI.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        static Repository repo;

        public TrainingController(IHubContext<MyHub> myHubContext)
        {
            repo = new Repository(myHubContext);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repo.GetTrainingDetails());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(repo.GetTrainingDetails(id));
        }

        [HttpGet("Course/{id}")]
        public IActionResult GetCourseName(int id)
        {
            return Ok(repo.getCourseDetails(id));
        }

        [HttpGet("Employee/{id}")]
        public IActionResult GetEmployeeName(int id)
        {
            return Ok(repo.getEmployeeDetails(id));
        }
        [HttpGet("Assignment/{id}")]
        public IActionResult GetAssignmentSubmissionCount(int id)
        {
            return Ok(repo.GetAssignmentSubmissionCount(id));
        }

        [HttpGet("Attendance/{id}")]
        public IActionResult GetAttendance(int id)
        {
            return Ok(repo.GetAttendance(id));
        }


        [HttpPost]
        public IActionResult Post([FromBody] Training value)
        {
            if (value != null)
            {
                int err = repo.AddTrainingData(value);
                if (err == 0)
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Training value)
        {
            if (value != null)
            {
                int status = repo.UpdateTraining(id, value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int status = repo.DeleteTraining(id);
            if (status == 2)
            {
                return NotFound();
            }
            if (status == 1) //error in sql
            {
                return BadRequest();
            }
            return Ok(repo.GetTrainingDetails());


        }

        [HttpDelete("{id}/Trainer/{Trainerid}")]
        public IActionResult Delete(int id, int Trainerid)
        {
            int status = repo.RemoveTrainer(id, Trainerid);

            if (status == 2)
            {
                return NotFound();
            }
            if (status == 1) //error in sql
            {
                return BadRequest();
            }
            return Ok(repo.GetTrainingDetails(id));

        }


        [HttpDelete("{id}/Topic/{TopicID}")]
        public IActionResult DeleteTopic(int id, int TopicID)
        {
            int status = repo.RemoveTopic(id, TopicID);
            if (status == 2)
            {
                return NotFound();
            }
            if (status == 1) //error in sql
            {
                return BadRequest();
            }
            return Ok(repo.GetTrainingDetails(id));

        }


        [HttpPut("{id}/Topic/{TopicID}")]
        public IActionResult Put(int id,int TopicID, [FromBody] Topic value)
        {
            if (value != null)
            {
                int status = repo.UpdateTopic(id,TopicID, value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("EmployeeDetails/{id}")]
        public IActionResult Get(string id)
        {
            return Ok(repo.GetEmployeeCredentials(id));
        }
        [HttpGet("EmployeeUserName/{id}")]
        public IActionResult GetEmpUserName(int id)
        {
            return Ok(repo.GetUserNameFromEmployeeId(id));
        }


        [HttpGet("EmployeeValidation/{id}")]
        public IActionResult GetValidationDetails(int id)
        {
            return Ok(repo.GetEmployeeValidation(id));
        }

        [HttpPost("Employee")]
        public IActionResult Post([FromBody] Employee value)
        {
            if (value != null)
            {
                int err = repo.AddEmployee(value);
                if (err == 0)
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }
        }



        [HttpPut("UpdatePassword/{id}")]
        public IActionResult Put(int id, [FromBody] Employee value)
        {
            if (value != null)
            {
                int status = repo.UpdateEmployeeDetails(id,value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("AddParticipant/{id}")]
        public IActionResult AddParticipant(int id, [FromBody] int value)
        {
            if (value != 0)
            {
                int status = repo.AddParticipant(id, value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("EnrollToMeeting/{id}")]
        public IActionResult EnrollToMeeting(int id, [FromBody] int value)
        {
            if (value != 0)
            {
                int status = repo.EnrollToMeeting(value, id);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("RequestPostponment/{id}")]
        public IActionResult RequestPostponment(int id, [FromBody] int value)
        {
            if (value != 0)
            {
                int status = repo.AddMeetingRequest(value, id);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateMeetingStatus/{id}")]
        public IActionResult UpdateMeetingStatus(int id, [FromBody] string value)
        {
            if (value != null)
            {
                int status = repo.UpdateMeetingStatus(id, value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateMeeting/{id}")]
        public IActionResult UpdateMeeting(int id, [FromBody] Meeting value)
        {
            if (value != null)
            {
                int status = repo.UpdateMeeting(id, value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("AddEmployeeValidation/{id}")]
        public IActionResult Put(int id, [FromBody] EmployeeValidation value)
        {
            if (value != null)
            {
                int status = repo.AddEmployeeValidation(id, value);
                if (status == 2)
                {
                    return NotFound();
                }
                if (status == 1) //error in sql
                {
                    return BadRequest();
                }
                else
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);

                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("Prerequisites/{id}")]
        public IActionResult DeleteTopic(int id)
        {
            int status = repo.DeleteAllPrerequisites(id);
            if (status == 2)
            {
                return NotFound();
            }
            if (status == 1) //error in sql
            {
                return BadRequest();
            }
            return Ok(repo.GetTrainingDetails(id));

        }

        [HttpGet("TopicDetails/{id}")]
        public IActionResult TopicDetails(int id)
        {
            return Ok(repo.GetTopicDetails(id));
        }

        [HttpGet("TrainingsOfTrainer/{id}")]
        public IActionResult GetTrainingsOfTrainer(int id)
        {
            return Ok(repo.GetTrainingsOfTrainer(id));
        }

        [HttpGet("MeetingsOfTrainer/{id}")]
        public IActionResult GetMeetingsOfTrainer(int id)
        {
            return Ok(repo.GetMeetingsOfTrainer(id));
        }

        [HttpGet("AllMeetings")]
        public IActionResult GetAllMeetings()
        {
            return Ok(repo.GetAllMeetings());
        }

        [HttpPost("AddMeeting")]
        public IActionResult Post([FromBody] Meeting value)
        {
            if (value != null)
            {
                int err = repo.AddMeeting(value);
                if (err == 0)
                {
                    Uri uri = new Uri("https://localhost:44397/api/Training/");
                    return Created(uri, value);
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }
        }



    }
}
