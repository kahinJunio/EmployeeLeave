using EmployeeLeave.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Contracts
{
    public interface ILeaveAllocationRepository:IRepositoryBase<LeaveAllocation>
    {
      Task<bool> CheckAllocation(int leaveTypeId, String employeeId);
      Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string EmployeeId);
      Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string EmployeeId,int leavetypeid);
    }
}
