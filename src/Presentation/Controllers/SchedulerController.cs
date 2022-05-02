using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGCap.Business.Abstract.ApplicationServices;
using AutoMapper;
using MGCap.Domain.Utils;
using MGCap.Presentation.Filters;
//using MGCap.Presentation.ViewModels.DataViewModels.Scheduler;
using MGCap.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MGCap.Domain.ViewModels.Schedulers;
using System.Net;
using MGCap.Domain.ViewModels.ScheduleWorkorder;
//using MGCap.Domain.ViewModels.ScheduleCategories;

namespace MGCap.Presentation.Controllers
{
    public class SchedulerController : BaseEntityController<Scheduler, int>
    {
        private ISchedulerApplicationService _schedulerApplicationService;
        public new ISchedulerApplicationService AppService => base.AppService as ISchedulerApplicationService;

        public SchedulerController(
            IEmployeesApplicationService employeeAppService,
          ISchedulerApplicationService schedulerApplicationService,
          IMapper mapper
            ) : base(employeeAppService, schedulerApplicationService, mapper)
        {

        }


        // GET: api/scheduler/readall
        [HttpGet]
        [AllowAnonymous]

        public async Task<JsonResult> ReadAll(DataSourceRequestScheduler request, Guid? scheduleGuid, int? scheduleID)
        {

            var dataSource = await this.AppService.ReadAllDapperAsync(request, scheduleGuid, scheduleID);
            return new JsonResult(dataSource);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddScheduler([FromBody] SchedulerCreateViewModel schedulerVM)
        {
            if (this.ModelState.IsValid)
            {
                if (schedulerVM == null)
                {
                    return BadRequest(this.ModelState);
                }

                try
                {
                    var reportObj = this.Mapper.Map<SchedulerCreateViewModel, Scheduler>(schedulerVM);
                    await this.AppService.AddAsync(reportObj);

                    await this.AppService.SaveChangesAsync();
                    var result = this.Mapper.Map<Scheduler, Scheduler>(reportObj);

                    return new JsonResult(result);
                }
                catch (Exception ex) // unexpected exception
                {
                    return this.BadRequest(this.ModelState);
                }
            }

            return this.BadRequest(this.ModelState);
        }




        [HttpPut]
        [AllowAnonymous]
        //[PermissionsFilter("UpdateCleaningReports")]
        public async Task<IActionResult> UpdateScheduler([FromBody] SchedulerUpdateViewModel schedulerVM)
        {
            if (this.ModelState.IsValid)
            {
                if (schedulerVM == null)
                {
                    return BadRequest(this.ModelState);
                }
                try
                {
                    var reportObj = await this.AppService.SingleOrDefaultAsync(ent => ent.scheduleID == schedulerVM.scheduleID);
                    if (reportObj == null)
                    {
                        return BadRequest(this.ModelState);
                    }
                    this.Mapper.Map(schedulerVM, reportObj);
                    await this.AppService.UpdateAsync(reportObj);
                    await this.AppService.SaveChangesAsync();
                }
                catch(Exception ex)
                {

                }
               
                return Ok();
            }
            return this.BadRequest(this.ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ReadByScheduleId(DataSourceRequestScheduler request, int scheduleId)
        {
            var scheduler = this.AppService.ReadByScheduleId(request, scheduleId);
            return new JsonResult(scheduler);
        }


        [HttpGet]
        [AllowAnonymous]
        public JsonResult ScheduleWorkorderEnd(DataSourceRequestScheduler request, int ScheduleID, int WorkOrderID)
        {
                try
                {
                    int result = this.AppService.ScheduleWorkorderEnd(request, ScheduleID, WorkOrderID);
                    return new JsonResult("Updated records");
                }
                catch (Exception ex) // unexpected exception
                {
                return new JsonResult("Not Updated"); 
                }
        
        }


        [HttpDelete]
       // [PermissionsFilter("DeleteScheduler")]
        public async Task<IActionResult> DeleteScheduler(int id)
        {
            var obj = this.AppService.SingleOrDefault(wo => wo.scheduleID == id);
            if (obj == null)
            {
                return StatusCode((int)HttpStatusCode.NoContent, "Error finding work order");
            }
            try
            {
                await this.AppService.RemoveAsync(obj);
                await this.AppService.SaveChangesAsync();
                return this.Ok();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed, "This work order is being referenced by other entities");
            }
        }

        [HttpGet]
        [PermissionsFilter("ReadScheduleList")]
        //        public async Task<ActionResult<DataSource<WorkOrderCalendarGridViewModel>>> ReadAllScheduleWorkorders(DataSourceRequestCalendar request)

        public async Task<ActionResult<DataSource<ScheduleWorkOrderGridViewModel>>> ReadAllScheduleWorkOrders(DataSourceRequestCalendar request)
        {
            try
            {
                var result = await this.AppService.ReadAllScheduleWorkOrdersDapperAsync(request);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
