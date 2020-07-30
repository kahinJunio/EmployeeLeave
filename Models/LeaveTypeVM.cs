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
        public string Name { get; set; }
        [Display(Name="Date created")]
        public DateTime? DateCreated { get; set; }
    }
}
