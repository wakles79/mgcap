using MGCap.DataAccess.Abstract.Repository;
using MGCap.DataAccess.Implementation.Context;
using MGCap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MGCap.Domain.Utils;
using System.Threading.Tasks;
using System.Linq;
using MGCap.Domain.ViewModels.DataViewModels.Customer;
using Microsoft.EntityFrameworkCore;
using MGCap.Domain.ViewModels.ScheduleWorkorder;
using Dapper;



using MGCap.Domain.ViewModels.Calendar;
using MGCap.Domain.ViewModels.Common;
using MGCap.Domain.ViewModels.WorkOrder;
using MGCap.Domain.ViewModels.WorkOrderBillingReport;

namespace MGCap.DataAccess.Implementation.Repositories
{

    public class SchedulerRepository : BaseRepository<Scheduler, int>, ISchedulerRepository
    {
        protected readonly IBaseDapperRepository _baseDapperRepository;
        //protected readonly IBaseDapperRepository _baseDapperRepository;

        public SchedulerRepository(
            MGCapDbContext dbContext, IBaseDapperRepository baseDapperRepository)
            : base(dbContext)
        {
            _baseDapperRepository = baseDapperRepository;
        }


        public async Task<DataSource<Scheduler>> ReadAllDapperAsync(DataSourceRequest request, Guid? scheduleGuid, int? scheduleID)
        {
            // TODO: Refactor this
            var result = new DataSource<Scheduler>
            {
                Payload = new List<Scheduler>(),
                Count = 0
            };

            string query = @" select 
                    [dbo].[schedule_WorkOrder].[scheduleID],
                    [dbo].[schedule_WorkOrder].[scheduleType],
                    [dbo].[schedule_WorkOrder].[scheduleDuration],
                    [dbo].[schedule_WorkOrder].[recurrencePattern],
                    [dbo].[schedule_WorkOrder].[recurrenceType],
                    [dbo].[schedule_WorkOrder].[recurrenceFrequency],
                    [dbo].[schedule_WorkOrder].[recurrenceDayValue],
                    [dbo].[schedule_WorkOrder].[recurrenceCustomValue],
                    [dbo].[schedule_WorkOrder].[dueDays],
                    [dbo].[schedule_WorkOrder].[rangeRecurrenceType],
                    [dbo].[schedule_WorkOrder].[rangeRecurrenceOccurrences],
                    [dbo].[schedule_WorkOrder].[rangeRecurrenceStartDate],
                    [dbo].[schedule_WorkOrder].[rangeRecurrenceEndDate],
                    [dbo].[schedule_WorkOrder].[isApproved],
                    [dbo].[schedule_WorkOrder].[createdDate],
                    [dbo].[schedule_WorkOrder].[UpdatedDate],
                    [dbo].[schedule_WorkOrder].[createdBy],
                    [dbo].[schedule_WorkOrder].[UpdatedBy] from schedule_WorkOrder ";

            var payload = await _baseDapperRepository.QueryAsync<Scheduler>(query, null, System.Data.CommandType.Text);
            result.Payload = payload;

            return result;
        }



        public override async Task<IQueryable<Scheduler>> ReadAllAsync(Func<Scheduler, bool> filter)
        {
            return await Task.Factory.StartNew(() =>
            {
                return Entities
                        .Include(ent => ent.scheduleDuration)
                        .Include(ent => ent.recurrencePattern)
                        .Include(ent => ent.recurrencePattern)
                        .Include(ent => ent.rangeRecurrenceStartDate)
                        .Include(ent => ent.rangeRecurrenceEndDate)
                            .Where(ent => filter.Invoke(ent));
            });
        }

        public  Scheduler ReadByScheduleId(DataSourceRequest request, int scheduleId)
        {
            var result = new Scheduler();
            

            string query = @" select * from schedule_WorkOrder Where scheduleID = " + scheduleId;

            var payload =  _baseDapperRepository.Query<Scheduler>(query, null, System.Data.CommandType.Text);
            result = payload.FirstOrDefault();
            
            return result;
        }

        public int ScheduleWorkorderEnd(DataSourceRequest request, int ScheduleID, int WorkOrderID)
        {
            DynamicParameters parameters = new DynamicParameters();


            parameters.Add("@pScheduleID", ScheduleID);
            parameters.Add("@pWorkOrderID", WorkOrderID);
            parameters.Add("@pEndDate", request.DateTo);

            int result = this._baseDapperRepository.Execute("ScheduleWorkorderEnd", parameters, System.Data.CommandType.StoredProcedure);
            //result.Count = payload.FirstOrDefault()?.Count ?? 0;
                       return result;
        }


        public async Task<DataSource<ScheduleWorkOrderGridViewModel>> ReadAllScheduleWorkOrdersDapperAsync(DataSourceRequestCalendar request, int companyId)
        {
            var result = new DataSource<ScheduleWorkOrderGridViewModel>
            {
                Payload = new List<ScheduleWorkOrderGridViewModel>(),
                Count = 0
            };

            DynamicParameters parameters = new DynamicParameters();

            request.TypeId = request.TypeId.HasValue ? (request.TypeId == -1 ? null : request.TypeId) : null;

            parameters.Add("@pStartDate", request.DateFrom);
            parameters.Add("@pEndDate", request.DateTo);
            parameters.Add("@BuildingId", request.BuildingId);
            parameters.Add("@TypeId", request.TypeId);
            parameters.Add("@pageNumber", request.PageNumber);
            parameters.Add("@pageSize", request.PageSize);
            //var payload = await this._baseDapperRepository.QueryAsync<ScheduleWorkOrderGridViewModel>("getScheduleWorkOrderList", parameters, System.Data.CommandType.StoredProcedure);
            //result.Payload = payload;
           // result.Count = payload.FirstOrDefault()?.Count ?? : 0;
           // result.Count = 0;
            string  sql = @"EXEC getScheduleWorkOrderList @pStartDate, @pEndDate, @BuildingId, @TypeId, @pageNumber, @pageSize ; EXEC getScheduleWorkOrderListCount @pStartDate, @pEndDate, @BuildingId, @TypeId, @pageNumber, @pageSize ";
            using (var conn = _baseDapperRepository.GetConnection())
            {
                //using (var multi = await conn.QueryMultipleAsync(sql, new { request.DateFrom, request.DateTo, request.BuildingId, request.TypeId, request.PageNumber, request.PageSize }))
                using (var multi = await conn.QueryMultipleAsync(sql, parameters))
                {
                    var response = multi.Read<ScheduleWorkOrderGridViewModel>();
                    if (response?.Any() == true)
                    {
                        result.Count = multi.ReadSingleOrDefault<int>();
                        result.Payload = response;
                    }
                }
            }
            return result;
        }
    }
}