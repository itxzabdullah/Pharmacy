using System;

namespace DomainModels
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedAt { get; set; }

        // Constructor for convenience
        public User(int userId, string username, string passwordHash, string email,
                    string fullName, string address, string phoneNumber,
                    string userType, DateTime createdAt)
        {
            UserID = userId;
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            FullName = fullName;
            Address = address;
            PhoneNumber = phoneNumber;
            UserType = userType;
            CreatedAt = createdAt;
        }

        // Empty constructor for flexibility
        public User() { }
    }
}