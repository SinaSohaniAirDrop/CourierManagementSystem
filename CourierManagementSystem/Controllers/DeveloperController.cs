using Microsoft.AspNetCore.Http;

namespace CourierManagementSystem.Controllers
{
    [Authorize(Roles = "Developer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly UserManager<IdentityUser> _userManager;
        public DeveloperController(IRequestService requestService, UserManager<IdentityUser> userManager)
        {
            _requestService = requestService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetSingleRequestById")]
        public async Task<ActionResult<Request>> GetSingleRequestById(int id)
        {
            var user = await GetUserId();
            var result = await _requestService.GetSingleRequest(id);
            if (result is null)
                return NotFound("Request not found.");
            if(result.UserId == user.Id)
                return Ok(result);
            else
                return NotFound("Request not found.");
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
            return Ok("Request added!");
        }

        [HttpPut]
        [Route("EditRequest")]
        public async Task<ActionResult<List<Request>>> EditRequest(int id, Request request)
        {
            var user = await GetUserId();
            string checkMessage = CheckRequestType(request);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            checkMessage = await CheckRequest(id);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            request.IsDone = false;
            var result = await _requestService.UpdateRequest(id, request);

            return Ok("Request edited!");
        }

        [HttpDelete]
        [Route("DeleteRequest")]
        public async Task<ActionResult<List<Request>>> DeleteRequest(int id)
        {
            string checkMessage = await CheckRequest(id);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            var result = await _requestService.DeleteRequest(id);
            if (result is null)
                return NotFound("Request not found.");

            return Ok("Request deleted.");
        }

        protected string CheckRequestType(Request request)
        {
            int[] types = { 0, 1 };
            if (!types.Contains(request.Type))
            {
                string message = "The type must be in: ";
                foreach (int type in types)
                {
                    message += type.ToString() + " ";
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

        protected async Task<string> CheckRequest(int id)
        {
            var user = await GetUserId();
            var singleRequest = await _requestService.GetSingleRequest(id);
            if (singleRequest is null)
                return "Request not found.";
            if (singleRequest.UserId != user.Id)
                return "Request not found.";
            return "ok";
        }
    }
}
