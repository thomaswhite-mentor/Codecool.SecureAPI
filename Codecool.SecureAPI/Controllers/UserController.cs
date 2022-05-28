using Codecool.SecureAPI.Model;
using Codecool.SecureAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codecool.SecureAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Policy = "MustBeAdmin")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> GetAll()
        {
            return Ok(UserDataStore.Current.Users);
        }

        [HttpGet("{id}", Name="GetUser")]
        public ActionResult<UserViewModel> GetUser(int id)
        {
            var result = UserDataStore.Current.Users.FirstOrDefault(x=>x.Id==id);
            if (result == null)
            {
               return NotFound();
            }    
            return Ok(result);
        }

        [HttpPost]
        public ActionResult AddUser([FromForm] UserViewModel userViewModel)
        {
            var createdResource = new UserViewModel() { Id = userViewModel.Id, Name = userViewModel.Name, Role = userViewModel.Role };
            UserDataStore.Current.Users.Add(createdResource);
            return CreatedAtRoute("GetUser", new {id = userViewModel.Id },createdResource);
        }
    }
}
