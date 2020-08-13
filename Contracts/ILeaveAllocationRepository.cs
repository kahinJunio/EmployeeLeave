using EmployeeLeave.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Contracts
{
    public interface ILeaveAllocationRepository:IRepositoryBase<LeaveAllocation>
    {
       bool CheckAllocation(int leaveTypeId, String employeeId);
        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string EmployeeId);
    }
}
