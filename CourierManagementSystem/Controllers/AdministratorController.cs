﻿using CourierManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
        private readonly IPackageService _packageService;
        private readonly IOrderService _orderService;

        public AdministratorController(IRequestService requestService, UserManager<IdentityUser> userManager, 
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
            request.IsDone = false;
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
            if (request.Type == "Suggestion")
                request.Type = "Criticism";
            else
                request.Type = "Suggestion";
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
            if (string.IsNullOrEmpty(package.DeliveryCity) || string.IsNullOrEmpty(package.PickupCity) 
                || string.IsNullOrEmpty(package.DeliveryLocation) || string.IsNullOrEmpty(package.PickupLocation))
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
            if (string.IsNullOrEmpty(request.DeliveryCity) || string.IsNullOrEmpty(request.PickupCity)
                || string.IsNullOrEmpty(request.DeliveryLocation) || string.IsNullOrEmpty(request.PickupLocation)) 
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
            if(order.PackageId == 0)
                return NotFound("PackageId cannot be empty.");
            var package = await _packageService.GetSinglePackage(order.PackageId);
            if (package is null)
                return NotFound("Package cannot be empty.");
            order.Package = package;
            double estimatedPrice = await InnerEstimatePrice(order);
            order.Cost = estimatedPrice;
            var result = await _orderService.AddOrder(order);
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
            var result = await _orderService.DeleteOrder(id);
            if (result is null)
                return NotFound("Order not found.");

            return Ok(result);
        }

        [HttpPut]
        [Route("RegisterOrder")]
        public async Task<ActionResult<List<Order>>> RegisterOrder(int id)
        {
            var order = await _orderService.GetSingleOrder(id);
            if (order is null)
                return NotFound("Order cannot be found.");
            order.Status = "Registered";
            var result = await _orderService.UpdateOrder(id, order);
            if (result is null)
                return NotFound("Order not found.");

            return Ok("Order registered");
        }

        [HttpGet]
        [Route("EstimatePrice")]
        public async Task<ActionResult<string>> EstimatePrice(int orderId)
        {
            try
            {
                Order order = await _orderService.GetSingleOrder(orderId);
                if(order is null)
                    return NotFound("Order not found!");
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
                if(order.Discount != null)
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
                string message = "The size must be in: ";
                foreach (string size in sizes)
                {
                    message += size + " ";
                }
                return message;
            }
            else
                return "ok";
        }

        protected async Task<string> CheckPackageValue(Package package)
        {
            var insurances = await _insuranceService.GetAllInsurances();
            List<double> values = new List<double>();
            foreach (var insurance in insurances)
            {
                values.Add(insurance.MaxVal);
            }
            var item = values[values.Count - 1];
            if (item < package.Value)
            {
                string message = "The value must be less than: ";
                message += item;
                return message;
            }
            else
                return "ok";
        }

        protected async Task<string> CheckPackageWeight(Package package)
        {
            var weightDists = await _weightDistService.GetAllWeightDists();
            List<double> weights = new List<double>();
            foreach (var weightDist in weightDists)
            {
                weights.Add(weightDist.MaxWeight);
            }
            var item = weights[weights.Count - 1];
            if (item < package.Weight)
            {
                string message = "The weight must be less than: ";
                message += item;
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
