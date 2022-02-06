using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("[action]")]
        public JsonResult PostUser([FromBody] User userData)
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    response = context.User.Add(userData).ToString();
                    context.SaveChanges();  result = new
                {
                    response
                };
                    
                }
                catch(Exception ex)
                {
                    result = ex.Message.ToString();
                }
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);

        }
        [HttpGet("[action]")]
        public JsonResult GetUsers()
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                result = new
                {
                    response = (from u in context.User join r in context.Role on u.Role_id equals r.Role_id into joinData from user_role in joinData.DefaultIfEmpty() join dl in context.Driving_License on u.Driving_id equals dl.Driving_id into joinData2 from user_driving in joinData2.DefaultIfEmpty() select new { u.User_id, u.Firstname, u.Lastname, u.user_Address, u.Password, u.dateOfBirth, u.CitizenId, role = user_role, driving = user_driving }).ToList()
                };
            }
            return new JsonResult(result);
        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetUserById([FromRoute]int id)
        {
            dynamic result = null;
            using(var context = new DB(_configuration))
            {
                var joinUser = (from u in context.User join r in context.Role on u.Role_id equals r.Role_id into joinData from user_role in joinData.DefaultIfEmpty() join dl in context.Driving_License on u.Driving_id equals dl.Driving_id into joinData2 from user_driving in joinData2.DefaultIfEmpty() select new {u.User_id, u.Firstname, u.Lastname, u.user_Address, u.Password, u.dateOfBirth, u.CitizenId,role=user_role,driving=user_driving });
                result = new
                {
                    response = joinUser.Where(e => e.User_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }
        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteUserById([FromRoute] int id)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var user = context.User.Where(e => e.User_id == id).FirstOrDefault();
                     response = context.User.Remove(user).ToString();
                    context.SaveChanges();
                }catch(Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);

        }

        [HttpPut("[action]/{id}")]
        public JsonResult PutUserById([FromRoute] int id,[FromBody] User userData)
        {
            dynamic result = null;
            dynamic response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var user = context.User.Where(e => e.User_id == id).FirstOrDefault();
                    if (user == null) return new JsonResult(result);
                    user.Firstname = userData.Firstname;
                    user.Lastname = userData.Lastname;
                    user.Password = userData.Password; 
                    user.Role_id = userData.Role_id;
                    user.user_Address = userData.user_Address;
                    user.CitizenId = userData.CitizenId;
                    user.dateOfBirth = userData.dateOfBirth;
                    user.Driving_id= userData.Driving_id;
                    user.user_Phone = userData.user_Phone;
                    response = context.User.Update(user).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);

        }

    }
}
