using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
using EmployeeLeave.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;
        //dependency injection
        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;

        }

        public bool CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(q => q.LeaveTypeId == leaveTypeId && q.EmployeeId == employeeId && q.Period == period).Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            var record = _db.LeaveAllocations.
                Include(q=>q.LeaveType)
                .ToList();
            return record;
        }

        public LeaveAllocation FindById(int id)
        {
            var allocation= _db.LeaveAllocations
                .Include(q=>q.LeaveType)
                .Include(q=>q.Employee)
                .FirstOrDefault(q=>q.Id==id);
            return allocation;
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string EmployeeId)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .Where(q=>q.EmployeeId==EmployeeId && q.Period==period)
                .ToList();
        }

        public LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string EmployeeId, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .FirstOrDefault(q => q.EmployeeId == EmployeeId && q.Period == period && q.LeaveTypeId == leavetypeid);
        }

        public bool IsExist(int id)
        {
            var exists = _db.LeaveTypes.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
