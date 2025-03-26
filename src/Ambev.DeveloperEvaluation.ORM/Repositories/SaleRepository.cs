using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Products)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }


        public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        public async Task<Sale> UpdateRangeAsync(Sale sale, CancellationToken cancellationToken = default)
        {

            // Retrieves the existing entity in the database
            var existingSale = await _context.Sales
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == sale.Id, cancellationToken);

            if (existingSale == null)
                throw new ValidationException("Sale not found.");

            // Updates simple entity properties
            _context.Entry(existingSale).CurrentValues.SetValues(sale);

            existingSale.Products.ToList().Clear();
            foreach (var product in sale.Products)
            {
                existingSale.AddProduct(product);
            }

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            if (affectedRows == 0)
                throw new DbUpdateConcurrencyException("The update operation did not affect any rows. The entity might have been modified or deleted since it was loaded.");

            return existingSale;

        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

    }

}
