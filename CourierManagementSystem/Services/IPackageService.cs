namespace CourierManagementSystem.Services
{
    public interface IPackageService
    {
        Task<List<Package>> GetAllPackages();
        Task<Package> GetSinglePackage(int id);
        Task<List<Package>> AddPackage(Package comCost);
        Task<List<Package>?> UpdatePackage(int id, Package request);
        Task<List<Package>?> DeletePackage(int id);
    }
}
