using System;

namespace MawethuGym.Web.Models;

public class Payment
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = "Cash"; // Cash, Card, EFT
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        // Which staff accepted/processed this payment
        public int StaffId { get; set; }
        public Staff? Staff { get; set; }
    }
