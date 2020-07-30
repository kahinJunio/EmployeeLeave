using AutoMapper;
using EmployeeLeave.Contracts;
using EmployeeLeave.Data;
using EmployeeLeave.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeLeave.Controllers
{
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
        public ActionResult Index()
        {
            var leavetypes = _repo.FindAll().ToList();
            var model = _maper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public ActionResult Details(int id)
        {
            if (!_repo.IsExist(id))
            {
                return NotFound();
            }
            var leavetype = _repo.FindById(id);
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
        public ActionResult Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leavetype = _maper.Map<LeaveType>(model);
                leavetype.DateCreated = DateTime.Now;
                var isSuccess=_repo.Create(leavetype);
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
        public ActionResult Edit(int id)
        {
            if (!_repo.IsExist(id))
            {
                return NotFound();
            }
            var leavetype = _repo.FindById(id);
            var model = _maper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeVM model)
        {
            try
            {
                //validate the form
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leavetype = _maper.Map<LeaveType>(model);
                var isSuccess = _repo.Update(leavetype);
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
        public ActionResult Delete(int id)
        {
            var leavetype = _repo.FindById(id);
            if (leavetype == null)
            {
                return NotFound();
            }
            var isSucess = _repo.Delete(leavetype);
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
                var leavetype = _repo.FindById(id);
                if (leavetype == null)
                {
                    return NotFound();
                }
                var isSucess = _repo.Delete(leavetype);
                if (!isSucess)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
