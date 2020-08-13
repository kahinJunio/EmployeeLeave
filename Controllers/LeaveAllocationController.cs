using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
using EmployeeLeave.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeave.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        //Dependency injection 
        private readonly ILeaveTypeRepository _LeaveTyperepo;
        private readonly ILeaveAllocationRepository _LeaveAllocationrepo;
        private readonly IMapper _maper;
        private readonly UserManager<Employee> _userManager;
        public LeaveAllocationController(ILeaveTypeRepository LeaveTyperepo, ILeaveAllocationRepository LeaveAllocationrepo,
            IMapper maper, UserManager<Employee> userManager)
        {
            _LeaveAllocationrepo = LeaveAllocationrepo;
            _LeaveTyperepo = LeaveTyperepo;
            _maper = maper;
            _userManager = userManager;
        }
        // GET: LeaveAllocationController
        public ActionResult Index()
        {
            var leavetypes = _LeaveTyperepo.FindAll().ToList();
            var MappedLeaveType = _maper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            var model = new CreateLeaveAllocationVm
            {
                LeaveTypes = MappedLeaveType,
                NumbersUpdated = 0
            };
            return View(model);
        }
        public ActionResult SetLeave(int id)
        {
            var leavetype = _LeaveTyperepo.FindById(id);
            var employee = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach(var emp in employee)
            {
                if (_LeaveAllocationrepo.CheckAllocation(id, emp.Id))
                    break;
                var allocation = new LeaveAllocationVM
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = emp.Id,
                    LeaveTypeId = id,
                    NumberOfDays=leavetype.DefaultDays,
                    Period =DateTime.Now.Year
                };
                var leaveAllocation = _maper.Map<LeaveAllocation>(allocation);
                _LeaveAllocationrepo.Create(leaveAllocation);
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: LeaveAllocationController/Details/5
        public ActionResult Details(string id)
        {
            var employee = _maper.Map<EmployeeVM> (_userManager.FindByIdAsync(id).Result);
            var period = DateTime.Now.Year;
            var allocations =_maper.Map<List<LeaveAllocationVM>>(_LeaveAllocationrepo.GetLeaveAllocationsByEmployee(id));
            var model = new ViewAllocationVM
            {
                Employee = employee,
                Allocations = allocations,
            };
            return View(model);
        }
        public ActionResult ListEmployees()
        {
            var employees= _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _maper.Map<List<EmployeeVM>>(employees);
            return View(model);
        }
        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public ActionResult Edit(int id)
        {
            var leaveAllocation =_LeaveAllocationrepo.FindById(id);
            var model = _maper.Map<EditLeaveAllocationVM>(leaveAllocation);
            return View(model);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record =_LeaveAllocationrepo.FindById(model.Id);
                record.NumberOfDays = model.NumberOfDays;
                var IsSucess = _LeaveAllocationrepo.Update(record);
                if (!IsSucess)
                {
                    ModelState.AddModelError("", "Error while Saving");
                    return View(model);
                }
                return RedirectToAction(nameof(Details),new{ id=model.EmployeeId});
            }
            catch
            {
                return View(model);
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
