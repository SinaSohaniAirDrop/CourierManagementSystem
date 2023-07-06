namespace CourierManagementSystem.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrders();
        Task<Order?> GetSingleOrder(int id);
        Task<List<Order>> AddOrder(Order order);
        Task<List<Order>?> UpdateOrder(int id, Order request);
        Task<List<Order>?> DeleteOrder(int id);
    }
}
