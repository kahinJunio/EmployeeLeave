using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Repository
{
    public class LeaveHistoryRepository : ILeaveHistoryRepository
    {
        private readonly ApplicationDbContext _db;
        //dependency injection
        public LeaveHistoryRepository(ApplicationDbContext db)
        {
            _db = db;

        }

        public async Task<bool> Create(LeaveHistory entity)
        {
            await _db.LeaveHistories.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveHistory entity)
        {
            _db.LeaveHistories.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveHistory>> FindAll()
        {
            var record =await _db.LeaveHistories
                .Include(q=>q.RequestingEmployee)
                .Include(q=>q.ApprovedBy)
                .Include(q=>q.LeaveType)
                .ToListAsync();
            return record;
        }

        public async Task<LeaveHistory> FindById(int id)
        {
            var record= await _db.LeaveHistories
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id==id);
            return record;
        }

        public async Task<ICollection<LeaveHistory>> GetEmployeeByLeaveHistory(string employeeid)
        {
            var leaveRequest = await FindAll();
               return leaveRequest.Where(q => q.RequestingEmployeeId == employeeid)
                .ToList();
        }

        public async Task<bool> IsExist(int id)
        {
            var exists =await _db.LeaveTypes.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes =await _db.SaveChangesAsync();
            return changes > 0;
        }


        public async Task<bool> Update(LeaveHistory entity)
        {
            _db.LeaveHistories.Update(entity);
            return await Save();
        }
    }
}
