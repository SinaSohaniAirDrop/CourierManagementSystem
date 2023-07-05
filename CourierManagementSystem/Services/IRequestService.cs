namespace CourierManagementSystem.Services
{
    public interface IRequestService
    {
        Task<List<Request>> GetAllRequests();
        Task<Request?> GetSingleRequest(int id);
        Task<List<Request>> AddRequest(Request request);
        Task<List<Request>?> UpdateRequest(int id, Request request);
        Task<List<Request>?> DeleteRequest(int id);
    }
}
