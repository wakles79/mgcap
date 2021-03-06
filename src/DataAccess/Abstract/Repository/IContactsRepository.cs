// -----------------------------------------------------------------------
// <copyright file="IContactsRepository.cs" company="Axzes">
// This source code is Copyright Axzes and MAY NOT be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from Axzes (https://www.axzes.com).
// </copyright>
// -----------------------------------------------------------------------

using MGCap.Domain.Models;
using MGCap.Domain.Utils;
using MGCap.Domain.ViewModels.Contact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MGCap.DataAccess.Abstract.Repository
{
    /// <summary>
    ///     Contains the declaration of the base
    ///     functionalities for the repositories
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of entity that the actual implementation
    ///     of this interface handles
    /// </typeparam>
    /// <typeparam name="TKey">
    ///     The type of the <typeparamref name="TEntity"/>'s Primary Key
    /// </typeparam>
    public interface IContactsRepository : IBaseRepository<Contact, int>
    {
        /// <summary>
        ///     Asynchronously read all the elements in the
        ///     table that <see cref="Contact"/> represents
        ///     applying a filter
        /// </summary>
        /// <returns>A list with all the Employees with the given filter</returns>
        new Task<Contact> SingleOrDefaultAsync(Func<Contact, bool> filter);
        Task<DataSource<ContactLogsViewModel>> ReadAllDapperAsync(DataSourceRequest request, int companyId);
        Task<DataSource<ContactListBoxViewModel>> ReadAllCboDapperAsync(DataSourceRequest request, int companyId, int? id);
    }
}
