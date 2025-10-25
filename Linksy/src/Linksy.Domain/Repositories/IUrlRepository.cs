﻿using Linksy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Repositories
{
    public interface IUrlRepository
    {
        Task<Url?> GetUrlAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<Url, object?>>[] inclues);
        Task CreateAsync(Url url, CancellationToken cancellationToken = default);
        Task<bool> IsUrlCodeInUseAsync(string code, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
