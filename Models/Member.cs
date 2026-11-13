namespace MawethuGym.Web.Models;

public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string MemberNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PlanType { get; set; } = "1 Month"; // 1 Month, 3 Months, 6 Months
        public DateTime ExpiryDate { get; set; }
        public bool BiometricConsentGiven { get; set; }
        public string? FaceDescriptorJson { get; set; } // 128-point vector descriptor

        // Client-specific additions
        public double WeightGoalKg { get; set; } = 75.0;
        public double CurrentWeightKg { get; set; } = 80.0;

        // Employee Audit Trail: Which staff registered this client
        public int? RegisteredByStaffId { get; set; }
        public Staff? RegisteredByStaff { get; set; }
    }
