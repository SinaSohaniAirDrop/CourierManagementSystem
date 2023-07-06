using Microsoft.AspNetCore.Http;

namespace CourierManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPackagingService _packagingService;
        private readonly IComCostService _comCostService;
        private readonly IInsuranceService _insuranceService;
        private readonly IWeightDistService _weightDistService;

        public AdministratorController(IRequestService requestService, UserManager<IdentityUser> userManager, 
            IPackagingService packagingService, IComCostService comCostService, IInsuranceService insuranceService, 
            IWeightDistService weightDistService)
        {
            _requestService = requestService;
            _userManager = userManager;
            _packagingService = packagingService;
            _comCostService = comCostService;
            _insuranceService = insuranceService;
            _weightDistService = weightDistService;
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

        [HttpPost]
        [Route("AddPackaging")]
        public async Task<ActionResult<List<Packaging>>> AddPackaging(Packaging packaging)
        {
            var result = await _packagingService.AddPackaging(packaging);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdatePackaging")]
        public async Task<ActionResult<List<Packaging>>> UpdatePackaging(int id, Packaging request)
        {
            var result = await _packagingService.UpdatePackaging(id, request);
            if (result is null)
                return NotFound("Packaging not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletePackaging")]
        public async Task<ActionResult<List<Packaging>>> DeletePackaging(int id)
        {
            var result = await _packagingService.DeletePackaging(id);
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

        [HttpPost]
        [Route("AddComCost")]
        public async Task<ActionResult<List<ComCost>>> AddComCost(ComCost comCost)
        {
            var result = await _comCostService.AddComCost(comCost);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateComCost")]
        public async Task<ActionResult<List<ComCost>>> UpdateComCost(int id, ComCost request)
        {
            var result = await _comCostService.UpdateComCost(id, request);
            if (result is null)
                return NotFound("ComCost not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteComCost")]
        public async Task<ActionResult<List<ComCost>>> DeleteComCost(int id)
        {
            var result = await _comCostService.DeleteComCost(id);
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

        [HttpPost]
        [Route("AddInsurance")]
        public async Task<ActionResult<List<Insurance>>> AddInsurance(Insurance insurance)
        {
            var result = await _insuranceService.AddInsurance(insurance);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateInsurance")]
        public async Task<ActionResult<List<Insurance>>> UpdateInsurance(int id, Insurance request)
        {
            var result = await _insuranceService.UpdateInsurance(id, request);
            if (result is null)
                return NotFound("Insurance not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteInsurance")]
        public async Task<ActionResult<List<Insurance>>> DeleteInsurance(int id)
        {
            var result = await _insuranceService.DeleteInsurance(id);
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

        [HttpPost]
        [Route("AddWeightDist")]
        public async Task<ActionResult<List<WeightDist>>> AddWeightDist(WeightDist weightDist)
        {
            var result = await _weightDistService.AddWeightDist(weightDist);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateWeightDist")]
        public async Task<ActionResult<List<WeightDist>>> UpdateWeightDist(int id, WeightDist request)
        {
            var result = await _weightDistService.UpdateWeightDist(id, request);
            if (result is null)
                return NotFound("WeightDist not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteWeightDist")]
        public async Task<ActionResult<List<WeightDist>>> DeleteWeightDist(int id)
        {
            var result = await _weightDistService.DeleteWeightDist(id);
            if (result is null)
                return NotFound("WeightDist not found.");

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
