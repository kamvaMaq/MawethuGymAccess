using System;

namespace MawethuGym.Web.Models;

public class Staff
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "FrontDesk"; // "Admin" or "FrontDesk"
        public string PasswordHash { get; set; } = string.Empty;
    }