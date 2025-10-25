using Linksy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Repositories
{
    public interface IBarcodeRepository
    {
        Task CreateAsync(Barcode barcode, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
