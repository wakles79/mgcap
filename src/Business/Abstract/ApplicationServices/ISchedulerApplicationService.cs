using MGCap.Domain.Models;
using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.ScheduleWorkorder;
//using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MGCap.Business.Abstract.ApplicationServices
{
    public interface ISchedulerApplicationService : IBaseApplicationService<Scheduler, int>
    {
        Task<DataSource<Scheduler>> ReadAllDapperAsync(DataSourceRequest request, Guid? scheduleGuid, int? scheduleID);
        Scheduler ReadByScheduleId(DataSourceRequest request, int scheduleId);

        int ScheduleWorkorderEnd(DataSourceRequest request, int ScheduleID, int WorkOrderID);

        Task<DataSource<ScheduleWorkOrderGridViewModel>> ReadAllScheduleWorkOrdersDapperAsync(DataSourceRequestCalendar request);

    }

}
