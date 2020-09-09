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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Schema;

namespace EmployeeLeave.Controllers
{
    [Authorize]
    public class LeaveHistoriesController : Controller
    {
        //dependency injection
        private readonly ILeaveHistoryRepository _leaveHistoryRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        public LeaveHistoriesController
            (
            ILeaveHistoryRepository leaveHistoryRepo,
            ILeaveTypeRepository leaveTypeRepo,
            IMapper mapper,
            ILeaveAllocationRepository leaveAllocRepo,
            UserManager<Employee> userManager
            )
        {
            _leaveHistoryRepo = leaveHistoryRepo;
            _mapper = mapper;
            _userManager = userManager;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocRepo = leaveAllocRepo;
        }
        // GET: LeaveHistoriesController
        [Authorize(Roles ="Administrator")]
        public async Task<ActionResult> Index()
        {
            var leaveRequests = await _leaveHistoryRepo.FindAll();
            var leaveRequestModel = _mapper.Map<List<LeaveHistoryVM>>(leaveRequests);
            var model = new AdminLeaveHistoryViewVM
            {
                TotalRequests = leaveRequestModel.Count,
                ApprovedRequests = leaveRequestModel.Where(q => q.Approved == true).Count(),
                PendingRequests= leaveRequestModel.Where(q => q.Approved ==null).Count(),
                RejectedRequests= leaveRequestModel.Where(q => q.Approved ==false).Count(),
                LeaveRequests= leaveRequestModel
            };
            return View(model);
        }
        public async Task<ActionResult> MyLeave()
        {
            var employee = await _userManager.GetUserAsync(User);
            var employeeid = employee.Id;
            var allocations =await _leaveAllocRepo.GetLeaveAllocationsByEmployee(employeeid);
            var leaverequests =await _leaveHistoryRepo.GetEmployeeByLeaveHistory(employeeid);
            var leaveAllocationModel = _mapper.Map<List<LeaveAllocationVM>>(allocations);
            var leaveRequestModel = _mapper.Map<List<LeaveHistoryVM>>(leaverequests);
            var model = new EmployeeLeavehistoryVM
            {
                LeaveAllocations = leaveAllocationModel,
                LeaveHistories = leaveRequestModel
            };
            return View(model);
        }
        // GET: LeaveHistoriesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var leaveRequest =await _leaveHistoryRepo.FindById(id);
            var model = _mapper.Map<LeaveHistoryVM>(leaveRequest);
            return View(model);
        }
        public async Task<ActionResult> ApproveRequest(int id)
        {
            try
            {
                var user =await _userManager.GetUserAsync(User);
                var leaveRequest =await _leaveHistoryRepo.FindById(id);

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.Dateactioned = DateTime.Now;

                var leavetype = leaveRequest.LeaveTypeId;
                var employeeid = leaveRequest.RequestingEmployeeId;
                var allocation =await _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employeeid, leavetype);
                int daysrequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                allocation.NumberOfDays -= daysrequested;

               await _leaveAllocRepo.Update(allocation);
               await _leaveHistoryRepo.Update(leaveRequest);
               return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index),"Home");
            }
           
        } 
        public async Task<ActionResult> CancelRequest(int id)
        {
            var leaveRequest = await _leaveHistoryRepo.FindById(id);
            leaveRequest.Cancel = true;
            await _leaveHistoryRepo.Update(leaveRequest);
            return RedirectToAction("MyLeave");

        }
        public async Task<ActionResult> RejectRequest(int id)
        {
            try
            {
                var user =await _userManager.GetUserAsync(User);
                var leaveRequest =await _leaveHistoryRepo.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.Dateactioned = DateTime.Now;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }
        // GET: LeaveHistoriesController/Create
        public async Task<ActionResult> Create()
        {
            var leaveTypes =await _leaveTypeRepo.FindAll();
            var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });
            var model = new CreateLeaveHistoriesVM
            {
                LeaveTypes = leaveTypeItems
            };
            return View(model);
        }

        // POST: LeaveHistoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveHistoriesVM model)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes =await _leaveTypeRepo.FindAll();
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypeItems;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start date cannot be further in the future than end date");
                    return View(model);
                }
                var employee =await _userManager.GetUserAsync(User);//retreiving current user
                var allocation=await _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id,model.LeaveTypeId);
                int DaysRequested =(int) (endDate.Date -startDate.Date).TotalDays;
                if (DaysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "Choose a valid number of days");
                    return View(model);
                }
                var leaveRequestModel = new LeaveHistoryVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate=startDate,
                    EndDate=endDate,
                    Approved=null,
                    DateRequested=DateTime.Now,
                    Dateactioned=DateTime.Now,
                    LeaveTypeId=model.LeaveTypeId,
 
                };
                var leaveRequest = _mapper.Map<LeaveHistory>(leaveRequestModel);
                var IsSuccess =await _leaveHistoryRepo.Create(leaveRequest);
                if (!IsSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong in submitting the form..");
                    return View(model);
                }
                return RedirectToAction("MyLeave");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong..");
                return View();
            }
        }

        // GET: LeaveHistoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveHistoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: LeaveHistoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveHistoriesController/Delete/5
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
