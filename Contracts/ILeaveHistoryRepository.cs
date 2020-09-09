using EmployeeLeave.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Contracts
{
    public interface ILeaveHistoryRepository:IRepositoryBase<LeaveHistory>
    {
       Task<ICollection<LeaveHistory>> GetEmployeeByLeaveHistory(string employeeid);
    }
}
