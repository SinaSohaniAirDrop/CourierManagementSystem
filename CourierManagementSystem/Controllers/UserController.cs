using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CourierManagementSystem.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPackagingService _packagingService;
        private readonly IComCostService _comCostService;
        private readonly IInsuranceService _insuranceService;
        private readonly IWeightDistService _weightDistService;
        private readonly IPackageService _packageService;

        public UserController(IRequestService requestService, UserManager<IdentityUser> userManager, 
            IPackagingService packagingService, IComCostService comCostService, IInsuranceService insuranceService, 
            IWeightDistService weightDistService, IPackageService packageService)
        {
            _requestService = requestService;
            _userManager = userManager;
            _packagingService = packagingService;
            _comCostService = comCostService;
            _insuranceService = insuranceService;
            _weightDistService = weightDistService;
            _packageService = packageService;
        }

        [HttpGet]
        [Route("GetSingleRequestById")]
        public async Task<ActionResult<Request>> GetSingleRequestById(int id)
        {
            var user = await GetUserId();
            var result = await _requestService.GetSingleRequest(id);
            if (result is null)
                return NotFound("Request not found.");
            if (result.UserId == user.Id)
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
                return NotFound("Request deleted!");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMyPackages")]
        public async Task<ActionResult<Package>> GetMyPackages()
        {
            var user = await GetUserId();
            var result = await _packageService.GetAllPackages();
            if (result is null)
                return NotFound("Packages not found.");
            List<Package> packages = result.Where(x => x.SenderId == user.Id).ToList();
            if (packages is null)
                return NotFound("Packages not found.");
            return Ok(packages);
        }

        [HttpPost]
        [Route("AddPackage")]
        public async Task<ActionResult<List<Package>>> AddPackage(Package package)
        {
            if (string.IsNullOrEmpty(package.ReceiverId) || string.IsNullOrEmpty(package.SenderId))
                return NotFound("SenderId and receiverId cannot be empty.");
            var sender = await _userManager.FindByIdAsync(package.SenderId);
            var receiver = await _userManager.FindByIdAsync(package.ReceiverId);
            package.Sender = sender;
            package.Receiver = receiver;
            var result = await _packageService.AddPackage(package);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdatePackage")]
        public async Task<ActionResult<List<Package>>> UpdatePackage(int id, Package request)
        {
            if (string.IsNullOrEmpty(request.ReceiverId) || string.IsNullOrEmpty(request.SenderId))
                return NotFound("SenderId and receiverId cannot be empty.");
            var message = await CheckPackage(id);
            if (message != "ok")
                return NotFound(message);
            var sender = await _userManager.FindByIdAsync(request.SenderId);
            var receiver = await _userManager.FindByIdAsync(request.ReceiverId);
            request.Sender = sender;
            request.Receiver = receiver;
            var result = await _packageService.UpdatePackage(id, request);
            if (result is null)
                return NotFound("Package not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletePackage")]
        public async Task<ActionResult<List<Package>>> DeletePackage(int id)
        {
            var message = await CheckPackage(id);
            if (message != "ok")
                return NotFound(message);
            var result = await _packageService.DeletePackage(id);
            if (result is null)
                return NotFound("Package not found.");

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

        protected async Task<string> CheckPackage(int id)
        {
            var user = await GetUserId();
            var singleRequest = await _packageService.GetSinglePackage(id);
            if (singleRequest is null)
                return "Package not found.";
            if (singleRequest.SenderId != user.Id)
                return "Request not found.";
            return "ok";
        }

    }
}
