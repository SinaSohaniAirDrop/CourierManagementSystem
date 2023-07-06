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
        private readonly IPackagingService _packagingService;
        private readonly IComCostService _comCostService;
        private readonly IInsuranceService _insuranceService;
        private readonly IWeightDistService _weightDistService;
        private readonly IPackageService _packageService;

        public DeveloperController(IRequestService requestService, UserManager<IdentityUser> userManager, 
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

        [HttpGet]
        [Route("GetAllPackagings")]
        public async Task<ActionResult<List<Packaging>>> GetAllPackagings()
        {
            return await _packagingService.GetAllPackagings();
        }

        [HttpGet]
        [Route("GetSinglePackaging")]
        public async Task<ActionResult<Packaging>> GetSinglePackaging(int id)
        {
            var result = await _packagingService.GetSinglePackaging(id);
            if (result is null)
                return NotFound("Packaging not found.");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllComCosts")]
        public async Task<ActionResult<List<ComCost>>> GetAllComCosts()
        {
            return await _comCostService.GetAllComCosts();
        }

        [HttpGet]
        [Route("GetSingleComCost")]
        public async Task<ActionResult<ComCost>> GetSingleComCost(int id)
        {
            var result = await _comCostService.GetSingleComCost(id);
            if (result is null)
                return NotFound("ComCost not found.");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllInsurances")]
        public async Task<ActionResult<List<Insurance>>> GetAllInsurances()
        {
            return await _insuranceService.GetAllInsurances();
        }

        [HttpGet]
        [Route("GetSingleInsurance")]
        public async Task<ActionResult<Insurance>> GetSingleInsurance(int id)
        {
            var result = await _insuranceService.GetSingleInsurance(id);
            if (result is null)
                return NotFound("Insurance not found.");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllWeightDists")]
        public async Task<ActionResult<List<WeightDist>>> GetAllWeightDists()
        {
            return await _weightDistService.GetAllWeightDists();
        }

        [HttpGet]
        [Route("GetSingleWeightDist")]
        public async Task<ActionResult<WeightDist>> GetSingleWeightDist(int id)
        {
            var result = await _weightDistService.GetSingleWeightDist(id);
            if (result is null)
                return NotFound("WeightDist not found.");

            return Ok(result);
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
