using System;

namespace MawethuGym.Web.Models;

public class CheckIn
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Method { get; set; } = "Face"; // Face, QR, Manual
        public bool IsOverride { get; set; }
    }
