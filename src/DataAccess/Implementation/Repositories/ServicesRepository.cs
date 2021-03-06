using Dapper;
using MGCap.DataAccess.Abstract.Repository;
using MGCap.DataAccess.Implementation.Context;
using MGCap.Domain.Models;
using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGCap.DataAccess.Implementation.Repositories
{
    public class ServicesRepository : BaseRepository<Service, int>, IServicesRepository
    {
        protected readonly IBaseDapperRepository _baseDapperRepository;
        
        private readonly ISchedulerRepository SchedulerRepository;
        /// <summary>
        ///     Gets dB representation of the object
        ///     of type TEntity.
        /// </summary>
        public DbSet<Scheduler> Entities { get; }
        public ServicesRepository(
            MGCapDbContext dbContext, IBaseDapperRepository baseDapperRepository) 
            : base(dbContext)
        {
            _baseDapperRepository = baseDapperRepository;
            
            this.Entities = (this.DbContext as MGCapDbContext).Set<Scheduler>();
        }


        /// <summary>
        ///     Adds an object to the table
        /// </summary>
        /// <param name="obj">The object to be added</param>
        /// <returns>Returns the <paramref name="obj"/> after being inserted</returns>
        public Scheduler Add(Scheduler obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("The given object must not be null");
            }

            this.Entities.Add(obj);
            return obj;
        }

        /// <summary>
        ///     Asynchronously adds an object to the table
        /// </summary>
        /// <param name="obj">The object to be added</param>
        /// <returns>Returns the <paramref name="obj"/> after being inserted</returns>
        public async Task<Scheduler> AddAsync(Scheduler obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The given object must not be null");
            }

            await this.Entities.AddAsync(obj);
            return obj;
        }

        public async Task<Scheduler> AddCleaningReportItemAsync(Scheduler obj)
        {
            var scheduler = await SchedulerRepository.AddAsync(obj);

            if (scheduler != null)
            {
                var itemLog = new List<ItemLogEntry>();
                itemLog.Add(new ItemLogEntry()
                {
                    ActivityType = "Added"
                });

                //this.RegisterLogActivity(
                //    obj.CleaningReportId,
                //    CleaningReportActivityType.ItemUpdated,
                //    itemLog: itemLog);
            }

            return scheduler;
        }


        public async Task<DataSource<ServiceListViewModel>> ReadAllCboDapperAsync(DataSourceRequest request, int companyId, int? id = null)
        {
            var result = new DataSource<ServiceListViewModel>
            {
                Payload = new List<ServiceListViewModel>(),
                Count = 0
            };

            string query = $@"
                        declare @index int;
                        declare @maxIndex int;
                        declare @total int;

                        IF ISNULL(@id, 0) <= 0 OR ISNULL(@filter, '') <> ''
                        BEGIN
                            select @index =  @pageNumber;
                        END
                        ELSE
                        BEGIN
                        SELECT @index = [Index] - 1 FROM ( 
                            SELECT 
                                [dbo].[Services].ID as ID, 
                                [dbo].[Services].[CompanyId] as CompanyId,
                                ROW_NUMBER() OVER (PARTITION BY CompanyId Order BY [dbo].[Services].[Name], [dbo].[Services].ID ) as [Index]
                            FROM [dbo].[Services]
                            ) payload
                        WHERE ID = @id;
                        END

                        SELECT @total = COUNT(*) FROM [dbo].[Services] WHERE [dbo].[Services].[CompanyId]= @companyId;

                        --max(0, @total-@pageSize)
                        SELECT @maxIndex = IIF(@pageSize > @total, 0, @total - @pageSize);

                        --safety check
                        SELECT @index = ISNULL(@index, 0);

                        --min(@index, @maxIndex)
                        SELECT @index = IIF(@index > @maxIndex, @maxIndex, @index)

                        SELECT 
                            [dbo].[Services].[ID],
                            [dbo].[Services].[Name],
                            [dbo].[Services].[UnitFactor],
                            [dbo].[Services].[UnitPrice],
                            [dbo].[Services].[MinPrice]
                        FROM [dbo].[Services]
                        WHERE [dbo].[Services].[CompanyId]= @companyId AND
                            ISNULL([dbo].[Services].[Name], '') 
                                LIKE '%' + ISNULL(@filter, '') + '%'
                        ORDER BY Name, ID

                        OFFSET @index ROWS
                        FETCH NEXT @pageSize ROWS ONLY;";

            var pars = new DynamicParameters();
            pars.Add("@id", id);
            pars.Add("@companyId", companyId);
            pars.Add("@filter", request.Filter);
            pars.Add("@pageNumber", request.PageNumber);
            pars.Add("@pageSize", request.PageSize);

            var payload = await _baseDapperRepository.QueryAsync<ServiceListViewModel>(query, pars);
            result.Count = payload.FirstOrDefault()?.Count ?? 0;
            result.Payload = payload;
            
            return result;
        }

        public async Task<DataSource<ServiceGridViewModel>> ReadAllDapperAsync(DataSourceRequest request, int companyId)
        {
            var result = new DataSource<ServiceGridViewModel>
            {
                Payload = new List<ServiceGridViewModel>(),
                Count = 0
            };

            string query = $@"
                        -- payload query
                         SELECT *, [Count] = COUNT (*) OVER() FROM (
                             SELECT 
                                 S.[ID],      
                                 S.[MinPrice],
                                 S.[Name],
                                 S.[UnitFactor],
                                 S.[UnitPrice]
                             FROM [dbo].[Services] as S 
                             WHERE S.CompanyId = @CompanyId AND
	                               ISNULL(S.Name, '') LIKE '%' + ISNULL(@filter, '') + '%'
                          ) payload 
                          ORDER BY ID
                          OFFSET @pageSize * @pageNumber ROWS
                          FETCH NEXT @pageSize ROWS ONLY ";

            var pars = new DynamicParameters();
            pars.Add("@companyId", companyId);
            pars.Add("@filter", request.Filter);
            pars.Add("@pageNumber", request.PageNumber);
            pars.Add("@pageSize", request.PageSize);

            var payload = await _baseDapperRepository.QueryAsync<ServiceGridViewModel>(query, pars);
            result.Count = payload.FirstOrDefault()?.Count ?? 0;
            result.Payload = payload;

            return result;
        }
    }
}
