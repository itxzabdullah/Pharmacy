using System.Collections.Generic;
using DomainModels;

namespace DataAccessLayer
{
    public interface IDal
    {
        void InsertOrder(int userId, List<OrderDetailDTO> orderDetails, decimal totalAmount, string paymentMethod);
        bool VerifyUser(string username, string password, string userType);
    }
}