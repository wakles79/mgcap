using MGCap.Domain.Models;
using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.ScheduleWorkorder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MGCap.DataAccess.Abstract.Repository
{
    //public interface ISchedulerRepository 
    //{
    //    Scheduler Add(Scheduler obj);
    //    Task<Scheduler> AddAsync(Scheduler obj);
    //    Task<DataSource<Scheduler>> ReadAllDapperAsync(DataSourceRequest request, Guid? scheduleGuid, int? scheduleID);


    //    DbSet<Scheduler> Entities { get; }

    //}


    public interface ISchedulerRepository : IBaseRepository<Scheduler, int>
    {

        Scheduler Add(Scheduler obj);

        Task<Scheduler> AddAsync(Scheduler obj);
        Task<DataSource<Scheduler>> ReadAllDapperAsync(DataSourceRequest request, Guid? scheduleGuid, int? scheduleID);

        Task<DataSource<ScheduleWorkOrderGridViewModel>> ReadAllScheduleWorkOrdersDapperAsync(DataSourceRequestCalendar request, int companyId);

        Scheduler ReadByScheduleId(DataSourceRequest request,int scheduleId);

        int ScheduleWorkorderEnd(DataSourceRequest request, int ScheduleID, int WorkOrderID);



    }

}
