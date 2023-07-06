using Microsoft.AspNetCore.Http.HttpResults;

namespace CourierManagementSystem.Services
{
    public class RequestService : IRequestService
    {
        private readonly DataContext _context;

        public RequestService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Request>> AddRequest(Request request)
        {
            _context.requests.Add(request);
            await _context.SaveChangesAsync();
            return await _context.requests.ToListAsync();
        }

        public async Task<List<Request>?> DeleteRequest(int id)
        {
            var request = await _context.requests.FindAsync(id);
            if (request is null)
                return null;

            _context.requests.Remove(request);
            await _context.SaveChangesAsync();

            return await _context.requests.ToListAsync();
        }


        public async Task<List<Request>> GetAllRequests()
        {
            var requests = await _context.requests.ToListAsync();
            return requests;
        }

        public async Task<Request?> GetSingleRequest(int id)
        {
            var request = await _context.requests.FindAsync(id);
            if (request is null)
                return null;

            return request;
        }

        public async Task<List<Request>?> UpdateRequest(int id, Request request)
        {
            var requestSingle = await _context.requests.FindAsync(id);
            if (requestSingle is null)
                return null;
            requestSingle.Type = request.Type;
            requestSingle.Text = request.Text;
            requestSingle.RegistrationDate = request.RegistrationDate;
            requestSingle.IsDone = request.IsDone;

            await _context.SaveChangesAsync();

            return await _context.requests.ToListAsync();
        }
    }
}
