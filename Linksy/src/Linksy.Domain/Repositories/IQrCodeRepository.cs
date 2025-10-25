using Linksy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Repositories
{
    public interface IQrCodeRepository
    {
        Task CreateAsync(QrCode qrCode, CancellationToken cancellationToken = default);
        Task DeleteAsync(int qrCodeId, bool includeUrlInDeletion, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
