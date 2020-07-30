using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
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
            var record = _db.LeaveAllocations.ToList();
            return record;
        }

        public LeaveAllocation FindById(int id)
        {
            return _db.LeaveAllocations.Find(id);
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
