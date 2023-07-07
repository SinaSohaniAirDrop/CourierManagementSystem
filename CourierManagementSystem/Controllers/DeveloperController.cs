using CourierManagementSystem.Models;
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
        private readonly IOrderService _orderService;

        public DeveloperController(IRequestService requestService, UserManager<IdentityUser> userManager, 
            IPackagingService packagingService, IComCostService comCostService, IInsuranceService insuranceService, 
            IWeightDistService weightDistService, IPackageService packageService, IOrderService orderService)
        {
            _requestService = requestService;
            _userManager = userManager;
            _packagingService = packagingService;
            _comCostService = comCostService;
            _insuranceService = insuranceService;
            _weightDistService = weightDistService;
            _packageService = packageService;
            _orderService = orderService;
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
            request.IsDone = false;
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

        [HttpGet]
        [Route("GetAllPackages")]
        public async Task<ActionResult<List<Package>>> GetAllPackages()
        {
            return await _packageService.GetAllPackages();
        }

        [HttpGet]
        [Route("GetSinglePackage")]
        public async Task<ActionResult<Package>> GetSinglePackage(int id)
        {
            var result = await _packageService.GetSinglePackage(id);
            if (result is null)
                return NotFound("Package not found.");

            return Ok(result);
        }

        [HttpPost]
        [Route("AddPackage")]
        public async Task<ActionResult<List<Package>>> AddPackage(Package package)
        {
            if (package.DeliveryCity == null || package.PickupCity == null || package.DeliveryLocation == null || package.PickupLocation == null)
                return NotFound("PickupCity/DeliveryCity/PickupLocation/DeliveryLocation cannot be empty");
            string checkMessage = await CheckPackageSize(package);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            checkMessage = await CheckPackageValue(package);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            checkMessage = await CheckPackageWeight(package);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            if (string.IsNullOrEmpty(package.ReceiverId) || string.IsNullOrEmpty(package.SenderId))
                return NotFound("SenderId and receiverId cannot be empty.");
            var sender = await _userManager.FindByIdAsync(package.SenderId);
            var receiver = await _userManager.FindByIdAsync(package.ReceiverId);
            if (sender == null || receiver == null)
                return NotFound("Sender or receiver not found.");
            package.Sender = sender;
            package.Receiver = receiver;
            var result = await _packageService.AddPackage(package);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdatePackage")]
        public async Task<ActionResult<List<Package>>> UpdatePackage(int id, Package request)
        {
            if (request.DeliveryCity == null || request.PickupCity == null || request.DeliveryLocation == null || request.PickupLocation == null)
                return NotFound("PickupCity/DeliveryCity/PickupLocation/DeliveryLocation cannot be empty");
            string checkMessage = await CheckPackageSize(request);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            checkMessage = await CheckPackageValue(request);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            checkMessage = await CheckPackageWeight(request);
            if (checkMessage != "ok")
                return NotFound(checkMessage);
            if (string.IsNullOrEmpty(request.ReceiverId) || string.IsNullOrEmpty(request.SenderId))
                return NotFound("SenderId and receiverId cannot be empty.");
            var message = await CheckPackage(id);
            if (message != "ok")
                return NotFound(message);
            var sender = await _userManager.FindByIdAsync(request.SenderId);
            var receiver = await _userManager.FindByIdAsync(request.ReceiverId);
            if (sender == null || receiver == null)
                return NotFound("Sender or receiver not found.");
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

        [HttpGet]
        [Route("GetAllOrders")]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return await _orderService.GetAllOrders();
        }

        [HttpGet]
        [Route("GetSingleOrder")]
        public async Task<ActionResult<Order>> GetSingleOrder(int id)
        {
            var result = await _orderService.GetSingleOrder(id);
            if (result is null)
                return NotFound("Order not found.");

            return Ok(result);
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<ActionResult<List<Order>>> AddOrder(Order order)
        {
            if (order.PackageId == 0)
                return NotFound("PackageId cannot be empty.");
            var package = await _packageService.GetSinglePackage(order.PackageId);
            if (package is null)
                return NotFound("Package cannot be empty.");
            var user = await GetUserId();
            if (package.SenderId != user.Id)
                return NotFound("Package cannot be found.");
            order.Package = package;
            double estimatedPrice = await InnerEstimatePrice(order);
            order.Cost = estimatedPrice;
            var result = await _packageService.AddPackage(package);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateOrder")]
        public async Task<ActionResult<List<Order>>> UpdateOrder(int id, Order request)
        {
            if (request.PackageId == 0)
                return NotFound("PackageId cannot be empty.");
            var package = await _packageService.GetSinglePackage(request.PackageId);
            if (package is null)
                return NotFound("Package cannot be found.");
            var user = await GetUserId();
            if (package.SenderId != user.Id)
                return NotFound("Package cannot be found.");
            double estimatedPrice = await InnerEstimatePrice(request);
            request.Cost = estimatedPrice;
            var result = await _orderService.UpdateOrder(id, request);
            if (result is null)
                return NotFound("Order not found.");

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteOrder")]
        public async Task<ActionResult<List<Order>>> DeleteOrder(int id)
        {
            var order = await _orderService.GetSingleOrder(id);
            if (order is null)
                return NotFound("Order cannot be found.");
            var package = await _packageService.GetSinglePackage(id);
            var user = await GetUserId();
            if (package.SenderId != user.Id)
                return NotFound("Order cannot be found.");
            var result = await _orderService.DeleteOrder(id);
            if (result is null)
                return NotFound("Order not found.");

            return Ok(result);
        }

        [HttpGet]
        [Route("EstimatePrice")]
        public async Task<ActionResult<string>> EstimatePrice(Order order)
        {
            try
            {
                int packageId = order.PackageId;
                Package package = await _packageService.GetSinglePackage(packageId);
                double estimatedPrice = 0;
                List<ComCost> comCosts = await _comCostService.GetAllComCosts();
                ComCost comCost = comCosts.First();
                if (comCost == null)
                    return NotFound("ComCost not found!");
                estimatedPrice += comCost.FixedCost;
                List<Packaging> packagingCosts = await _packagingService.GetAllPackagings();
                Packaging packagingCost = packagingCosts.Where(x => x.Size == package.Size).First();
                if (packagingCost != null)
                    estimatedPrice += packagingCost.PackagingCost;
                else
                    return NotFound("Your package dimensions is not allowed!");
                //double volume = width * lenght * height;
                List<WeightDist> weightDists = await _weightDistService.GetAllWeightDists();
                WeightDist weightDist = weightDists.Where(x => x.MinWeight <= package.Weight && x.MaxWeight > package.Weight).First();
                if (weightDist != null)
                {
                    if (package.IsNeighboringCity)
                        estimatedPrice += weightDist.NeighboringProvince;
                    else
                        estimatedPrice += weightDist.OtherProvince;
                }
                else
                    return NotFound("Your package weight is not allowed!");
                List<Insurance> insurances = await _insuranceService.GetAllInsurances();
                Insurance insurance = insurances.Where(x => x.MinVal <= package.Value && x.MaxVal > package.Value).First();
                if (insurance != null)
                    estimatedPrice += insurance.Tariff;
                else
                    return NotFound("Your package value is not allowed!");
                estimatedPrice += 0.01 * package.Value + comCost.HQCost;
                if (package.PickupCity == package.DeliveryCity)
                    estimatedPrice += comCost.InsiderFee / 100 * estimatedPrice;
                else
                    estimatedPrice += comCost.OutsiderFee / 100 * estimatedPrice;
                estimatedPrice += estimatedPrice / 100 * comCost.tax;
                if (order.Discount != null)
                {
                    var discountValue = order.Discount * estimatedPrice / 100;
                    estimatedPrice -= discountValue;
                }
                return Ok(estimatedPrice);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        protected async Task<double> InnerEstimatePrice(Order order)
        {
            int packageId = order.PackageId;
            Package package = await _packageService.GetSinglePackage(packageId);
            double estimatedPrice = 0;
            List<ComCost> comCosts = await _comCostService.GetAllComCosts();
            ComCost comCost = comCosts.First();
            if (comCost != null)
                estimatedPrice += comCost.FixedCost;
            List<Packaging> packagingCosts = await _packagingService.GetAllPackagings();
            Packaging packagingCost = packagingCosts.Where(x => x.Size == package.Size).First();
            if (packagingCost != null)
                estimatedPrice += packagingCost.PackagingCost;
            //double volume = width * lenght * height;
            List<WeightDist> weightDists = await _weightDistService.GetAllWeightDists();
            WeightDist weightDist = weightDists.Where(x => x.MinWeight <= package.Weight && x.MaxWeight > package.Weight).First();
            if (weightDist != null)
            {
                if (package.IsNeighboringCity)
                    estimatedPrice += weightDist.NeighboringProvince;
                else
                    estimatedPrice += weightDist.OtherProvince;
            }
            List<Insurance> insurances = await _insuranceService.GetAllInsurances();
            Insurance insurance = insurances.Where(x => x.MinVal <= package.Value && x.MaxVal > package.Value).First();
            if (insurance != null)
                estimatedPrice += insurance.Tariff;
            estimatedPrice += 0.01 * package.Value + comCost.HQCost;
            if (package.PickupCity == package.DeliveryCity)
                estimatedPrice += comCost.InsiderFee / 100 * estimatedPrice;
            else
                estimatedPrice += comCost.OutsiderFee / 100 * estimatedPrice;
            estimatedPrice += estimatedPrice / 100 * comCost.tax;
            if (order.Discount != null)
            {
                var discountValue = order.Discount * estimatedPrice / 100;
                estimatedPrice -= discountValue;
            }
            return estimatedPrice;
        }

        protected string CheckRequestType(Request request)
        {
            string[] types = { "Criticism", "Suggestion" };
            if (!types.Contains(request.Type))
            {
                string message = "The type must be in: ";
                foreach (string type in types)
                {
                    message += type + " ";
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

        protected async Task<string> CheckPackageSize(Package package)
        {
            var packagings = await _packagingService.GetAllPackagings();
            List<string> sizes = new List<string>();
            foreach (var packaging in packagings)
            {
                sizes.Add(packaging.Size);
            }
            if (!sizes.Contains(package.Size))
            {
                string message = "The size must be less than: ";
                var item = sizes[sizes.Count - 1];
                message += item;
                return message;
            }
            else
                return "ok";
        }

        protected async Task<string> CheckPackageValue(Package package)
        {
            var insurances = await _insuranceService.GetAllInsurances();
            List<string> values = new List<string>();
            foreach (var insurance in insurances)
            {
                values.Add(insurance.MaxVal.ToString());
            }
            if (!values.Contains(package.Size))
            {
                string message = "The value must be less than: ";
                var item = values[values.Count - 1];
                message += item;
                return message;
            }
            else
                return "ok";
        }

        protected async Task<string> CheckPackageWeight(Package package)
        {
            var weightDists = await _weightDistService.GetAllWeightDists();
            List<string> weights = new List<string>();
            foreach (var weightDist in weightDists)
            {
                weights.Add(weightDist.MaxWeight.ToString());
            }
            if (!weights.Contains(package.Size))
            {
                string message = "The weight must be less than: ";
                var item = weights[weights.Count - 1];
                message += item;
                return message;
            }
            else
                return "ok";
        }
    }
}
