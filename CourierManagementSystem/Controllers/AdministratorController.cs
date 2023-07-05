using CourierManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourierManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly UserManager<IdentityUser> _userManager;
        public AdministratorController(IRequestService requestService, UserManager<IdentityUser> userManager)
        {
            _requestService = requestService;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetAllRequests")]
        public async Task<ActionResult<List<Request>>> GetAllRequests()
        {
            return await _requestService.GetAllRequests();
        }

        [HttpGet]
        [Route("GetSingleRequestById")]
        public async Task<ActionResult<Request>> GetSingleRequestById(int id)
        {
            var result = await _requestService.GetSingleRequest(id);
            if (result is null)
                return NotFound("Request not found.");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMyRequests")]
        public async Task<ActionResult<Request>> GetMyRequests()
        {
            var user = await GetUserId();
            var result = await _requestService.GetAllRequests();
            if (result is null)
                return NotFound("Requests not found.");
            List<Request> requests = result.Where(x => x.UserId == user.Id).ToList();
            if (requests is null)
                return NotFound("Requests not found.");
            return Ok(requests);
        }

        [HttpPost]
        [Route("AddRequest")]
        public async Task<ActionResult<List<Request>>> AddRequest(Request request)
        {
            string checkMessage = CheckRequestType(request);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            var user = await GetUserId();
            request.UserId = user.Id;
            request.User = user;
            var result = await _requestService.AddRequest(request);
            return Ok(result);
        }

        [HttpPut]
        [Route("EditRequest")]
        public async Task<ActionResult<List<Request>>> EditRequest(int id, Request request)
        {
            string checkMessage = CheckRequestType(request);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            var result = await _requestService.UpdateRequest(id, request);
            if (result is null)
                return NotFound("Request not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteRequest")]
        public async Task<ActionResult<List<Request>>> DeleteRequest(int id)
        {
            var result = await _requestService.DeleteRequest(id);
            if (result is null)
                return NotFound("Request not found.");

            return Ok(result);
        }

        [HttpPost]
        [Route("ChangeRequestType")]
        public async Task<ActionResult<List<Request>>> ChangeRequestType(int id)
        {
            var request = await _requestService.GetSingleRequest(id);
            if (request is null)
                return NotFound("Request not found.");
            if (request.Type == 1)
                request.Type = 2;
            else
                request.Type = 1;
            var result = await _requestService.UpdateRequest(id, request);
            return Ok(result);
        }
        [HttpPost]
        [Route("ChangeRequestState")]
        public async Task<ActionResult<List<Request>>> ChangeRequestState(int id)
        {
            var request = await _requestService.GetSingleRequest(id);
            if (request is null)
                return NotFound("Request not found.");
            if (request.IsDone == true)
                request.IsDone = false;
            else
                request.IsDone = true;
            var result = await _requestService.UpdateRequest(id, request);
            return Ok(result);
        }
        protected string CheckRequestType(Request request)
        {
            int[] types = { 0, 1 };
            if (!types.Contains(request.Type))
            {
                string message = "The type must be in:";
                foreach (int type in types)
                {
                    message += type.ToString();
                }
                return message;
            }
            else
                return "ok";
        }
        protected async Task<IdentityUser> GetUserId()
        {
            var username = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }
    }
}
