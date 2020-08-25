using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Models
{
    public class LeaveHistoryVM
    {
        public int Id { get; set; }

        public EmployeeVM RequestingEmployee { get; set; }
        public string RequestingEmployeeId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public LeaveTypeVM LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime Dateactioned { get; set; }
        public bool? Approved { get; set; }

        public EmployeeVM ApprovedBy { get; set; }
        public string ApprovedById { get; set; }
        
    }
    public class AdminLeaveHistoryViewVM
    {
        [Display(Name ="Total number of requests")]
        public int TotalRequests { get; set; }
        [Display(Name = "Approved requests")]
        public int ApprovedRequests { get; set; }
        [Display(Name = "Pending requests")]
        public int PendingRequests { get; set; }
        [Display(Name = "Rejected requests")]
        public int RejectedRequests { get; set; }
        public List<LeaveHistoryVM> LeaveRequests { get; set; }
    }
    public class CreateLeaveHistoriesVM
    {
        [Display(Name ="Start date")]
        [Required]
        public string StartDate { get; set; }
        [Display(Name = "End date ")]
        [Required]
        public string EndDate { get; set; }
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        [Display(Name ="Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name = "Request comments")]
        public string Comments { get; set; }
        public bool Cancel { get; set; }
    }
    public class EmployeeLeavehistoryVM
    {
        public List<LeaveAllocationVM>LeaveAllocations { get; set; }
        public List<LeaveHistoryVM> LeaveHistories { get; set; }
    }
}
