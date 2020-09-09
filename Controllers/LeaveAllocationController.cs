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
        public async Task<ActionResult> Index()
        {
            var leavetypes =await _LeaveTyperepo.FindAll();
            var MappedLeaveType = _maper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes.ToList());
            var model = new CreateLeaveAllocationVm
            {
                LeaveTypes = MappedLeaveType,
                NumbersUpdated = 0
            };
            return View(model);
        }
        public async Task<ActionResult> SetLeave(int id)
        {
            var leavetype =await _LeaveTyperepo.FindById(id);
            var employee =await _userManager.GetUsersInRoleAsync("Employee");
            foreach(var emp in employee)
            {
                if (await _LeaveAllocationrepo.CheckAllocation(id, emp.Id))
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
                await _LeaveAllocationrepo.Create(leaveAllocation);
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: LeaveAllocationController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var employee =_maper.Map<EmployeeVM> ( await _userManager.FindByIdAsync(id));
            var period = DateTime.Now.Year;
            var allocations =_maper.Map<List<LeaveAllocationVM>>(await _LeaveAllocationrepo.GetLeaveAllocationsByEmployee(id));
            var model = new ViewAllocationVM
            {
                Employee = employee,
                Allocations = allocations,
            };
            return View(model);
        }
        public async Task<ActionResult> ListEmployees()
        {
            var employees=await _userManager.GetUsersInRoleAsync("Employee");
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
        public async Task<ActionResult> Edit(int id)
        {
            var leaveAllocation = await _LeaveAllocationrepo.FindById(id);
            var model = _maper.Map<EditLeaveAllocationVM>(leaveAllocation);
            return View(model);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditLeaveAllocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record =await _LeaveAllocationrepo.FindById(model.Id);
                record.NumberOfDays = model.NumberOfDays;
                var IsSucess = await _LeaveAllocationrepo.Update(record);
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
