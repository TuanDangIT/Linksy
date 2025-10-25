using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Repositories
{
    internal class BarcodeRepository : IBarcodeRepository
    {
        private readonly LinksyDbContext _dbContext;

        public BarcodeRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Barcode barcode, CancellationToken cancellationToken = default)
        {
            await _dbContext.Barcodes.AddAsync(barcode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
