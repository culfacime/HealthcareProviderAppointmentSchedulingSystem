using Healthcare.Core.DB;
using Healthcare.Core.Repositories;
using Healthcare.Core.UnitOfWorks;
using Healthcare.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Repository.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly HealthcareDbContext _context;
    public UnitOfWork(HealthcareDbContext context)
    {
        _context = context;
    }
    public void Commit()
    {
        _context.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public IGenericRepository<T> GenericRepository<T>() where T : class
    {
        return new GenericRepository<T>(_context);
    }
}
