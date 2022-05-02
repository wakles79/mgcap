using MGCap.Business.Abstract.ApplicationServices;
using MGCap.Domain.Models;
using MGCap.DataAccess.Abstract.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
//using Octopus.Client.Repositories;
using ISchedulerRepository = MGCap.DataAccess.Abstract.Repository.ISchedulerRepository;
using System.Threading.Tasks;
using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.ScheduleWorkorder;

namespace MGCap.Business.Implementation.ApplicationServices
{
    public class SchedulerApplicationService : BaseApplicationService<Scheduler, int>, ISchedulerApplicationService

    {
        private readonly int CompanyId;
        public new ISchedulerRepository Repository => base.Repository as ISchedulerRepository;
       // private readonly IWorkOrdersRepository WorkOrdersRepository;
        public SchedulerApplicationService(ISchedulerRepository repository) : base(repository)
        {

        }



        public async Task<DataSource<Scheduler>> ReadAllDapperAsync(DataSourceRequest request, Guid? scheduleGuid, int? scheduleID)
        {
            return await Repository.ReadAllDapperAsync(request, scheduleGuid, scheduleID);
        }

        public Scheduler ReadByScheduleId(DataSourceRequest request, int scheduleId)
        {
            return  Repository.ReadByScheduleId(request, scheduleId);
        }

        public int ScheduleWorkorderEnd(DataSourceRequest request, int ScheduleID, int WorkOrderID)
        {
            return Repository.ScheduleWorkorderEnd(request, ScheduleID, WorkOrderID);
        }

        public async Task<DataSource<ScheduleWorkOrderGridViewModel>> ReadAllScheduleWorkOrdersDapperAsync(DataSourceRequestCalendar request)
        {
            var result = await this.Repository.ReadAllScheduleWorkOrdersDapperAsync(request, this.CompanyId);

            //result.Payload = (inspections.Payload).Concat(tickets.Payload);
            //result.Count = inspections.Payload.Count() + tickets.Payload.Count();

            return result;
        }


    }


}
