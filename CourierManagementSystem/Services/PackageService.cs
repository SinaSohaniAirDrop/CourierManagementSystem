namespace CourierManagementSystem.Services
{
    public class PackageService : IPackageService
    {

        private readonly DataContext _context;

        public PackageService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Package>> AddPackage(Package package)
        {
            _context.packages.Add(package);
            await _context.SaveChangesAsync();
            return await _context.packages.ToListAsync();
        }

        public async Task<List<Package>?> DeletePackage(int id)
        {
            var package = await _context.packages.FindAsync(id);
            if (package is null)
                return null;

            _context.packages.Remove(package);
            await _context.SaveChangesAsync();

            return await _context.packages.ToListAsync();
        }


        public async Task<List<Package>> GetAllPackages()
        {
            var package = await _context.packages.ToListAsync();
            return package;
        }

        public async Task<Package?> GetSinglePackage(int id)
        {
            var package = await _context.packages.FindAsync(id);
            if (package is null)
                return null;

            return package;
        }

        public async Task<List<Package>?> UpdatePackage(int id, Package request)
        {
            var package = await _context.packages.FindAsync(id);
            if (package is null)
                return null;

            package.Weight = request.Weight;
            package.Size = request.Size;
            package.PickupDate = request.PickupDate;
            package.DeliveryDate = request.DeliveryDate;
            package.PickupCity = request.PickupCity;
            package.DeliveryDate = request.DeliveryDate;
            package.PickupLocation = request.PickupLocation;
            package.DeliveryLocation = request.DeliveryLocation;
            package.IsNeighboringCity = request.IsNeighboringCity;
            package.SenderId = request.SenderId;
            package.Sender = request.Sender;
            package.ReceiverId = request.ReceiverId;
            package.Receiver = request.Receiver;
            package.Value = request.Value;

            await _context.SaveChangesAsync();

            return await _context.packages.ToListAsync();
        }
    }
}
