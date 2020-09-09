using AutoMapper;
using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
using EmployeeLeave.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class LeaveTypesController : Controller
    {
        //Dependency injection 
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _maper;

        public LeaveTypesController(ILeaveTypeRepository repo, IMapper maper)
        {
            _repo = repo;
            _maper = maper;
        }
        // GET: LeaveTypesController
       
        public async Task<ActionResult> Index()
        {
            var leavetypes =await _repo.FindAll();
            var model = _maper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes.ToList());
            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var IsExist =await _repo.IsExist(id);
            if (!IsExist)
            {
                return NotFound();
            }
            var leavetype =await _repo.FindById(id);
            var model = _maper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leavetype = _maper.Map<LeaveType>(model);
                leavetype.DateCreated = DateTime.Now;
                var isSuccess=await _repo.Create(leavetype);
                if (!isSuccess)
                {
                    ModelState.AddModelError(" ", "Something went wrong...");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(" ", "Something went wrong...");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var IsExist = await _repo.IsExist(id);
            if (!IsExist)
            {
                return NotFound();
            }
            var leavetype =await _repo.FindById(id);
            var model = _maper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeVM model)
        {
            try
            {
                //validate the form
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leavetype = _maper.Map<LeaveType>(model);
                var isSuccess = await _repo.Update(leavetype);
                if (!isSuccess)
                {
                    ModelState.AddModelError(" ", "Something went wrong...");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(" ", "Something went wrong...");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var leavetype =await _repo.FindById(id);
            if (leavetype == null)
            {
                return NotFound();
            }
            var isSucess =await _repo.Delete(leavetype);
            if (!isSucess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,LeaveTypeVM model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
