using User.Managment.Service.Models;

namespace User.Managment.Service.Services
{
    public interface IEmailManagerService
    {
        void SendEmail(Message message);
    }
}
