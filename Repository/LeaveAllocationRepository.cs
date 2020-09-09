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

        public async Task<bool> CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.Where(q => q.LeaveTypeId == leaveTypeId && q.EmployeeId == employeeId && q.Period == period).Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
           await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            var record = await _db.LeaveAllocations.
                Include(q=>q.LeaveType)
                .ToListAsync();
            return record;
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            var allocation=await _db.LeaveAllocations
                .Include(q=>q.LeaveType)
                .Include(q=>q.Employee)
                .FirstOrDefaultAsync(q=>q.Id==id);
            return allocation;
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string EmployeeId)
        {
            var period = DateTime.Now.Year;
            var leaveAlloc = await FindAll();
            return leaveAlloc.Where(q => q.EmployeeId == EmployeeId && q.Period == period)
                .ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string EmployeeId, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            var allocation = await FindAll();
            return allocation.FirstOrDefault(q => q.EmployeeId == EmployeeId && q.Period == period && q.LeaveTypeId == leavetypeid);
        }

        public async Task<bool> IsExist(int id)
        {
            var exists =await _db.LeaveTypes.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
