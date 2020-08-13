using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Models
{
    public class LeaveTypeVM
    {
        //abstruction of the db
       
        public int Id { get; set; }
        [Required]
        [Range(1,25,ErrorMessage ="Please enter a valid number")]
        [Display(Name ="Default Number of Days")]
        public int DefaultDays { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Date created")]
        public DateTime? DateCreated { get; set; }
    }
}
