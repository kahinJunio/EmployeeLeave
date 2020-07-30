using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
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

        public bool Create(LeaveHistory entity)
        {
            _db.LeaveHistories.Add(entity);
            return Save();
        }

        public bool Delete(LeaveHistory entity)
        {
            _db.LeaveHistories.Remove(entity);
            return Save();
        }

        public ICollection<LeaveHistory> FindAll()
        {
            var record = _db.LeaveHistories.ToList();
            return record;
        }

        public LeaveHistory FindById(int id)
        {
            return _db.LeaveHistories.Find(id);
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


        public bool Update(LeaveHistory entity)
        {
            _db.LeaveHistories.Update(entity);
            return Save();
        }
    }
}
