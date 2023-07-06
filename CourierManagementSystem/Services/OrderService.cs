namespace CourierManagementSystem.Services
{
    public class OrderService : IOrderService
    {

        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> AddOrder(Order order)
        {
            _context.orders.Add(order);
            await _context.SaveChangesAsync();
            return await _context.orders.ToListAsync();
        }

        public async Task<List<Order>?> DeleteOrder(int id)
        {
            var order = await _context.orders.FindAsync(id);
            if (order is null)
                return null;

            _context.orders.Remove(order);
            await _context.SaveChangesAsync();

            return await _context.orders.ToListAsync();
        }


        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.orders.ToListAsync();
            return orders;
        }

        public async Task<Order?> GetSingleOrder(int id)
        {
            var order = await _context.orders.FindAsync(id);
            if (order is null)
                return null;

            return order;
        }

        public async Task<List<Order>?> UpdateOrder(int id, Order request)
        {
            var order = await _context.orders.FindAsync(id);
            if (order is null)
                return null;

            order.Status = request.Status;
            order.Package = order.Package;
            order.PackageId= request.PackageId;
            order.Cost = request.Cost;
            order.Discount= request.Discount;

            await _context.SaveChangesAsync();

            return await _context.orders.ToListAsync();
        }
    }
}
