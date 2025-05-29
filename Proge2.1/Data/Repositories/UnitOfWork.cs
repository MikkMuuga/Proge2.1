using Proge2._1.Models;
using Proge2._1.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Proge2._1.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICustomerRepository CustomerRepository { get; }
        public IBudgetRepository BudgetRepository { get; }
        public IMaterialsRepository MaterialsRepository { get; }
        public IServicessRepository ServicesRepository { get; }
        public IMachinesRepository MachinesRepository { get; }
        public ICommentRepository CommentRepository { get; }

        public UnitOfWork(ApplicationDbContext context,
            ICustomerRepository customerRepository,
            IBudgetRepository budgetRepository,
            IMaterialsRepository materialsRepository,
            IServicessRepository servicesRepository,
            IMachinesRepository machinesRepository,
            ICommentRepository commentRepository)
        {
            _context = context;

            CustomerRepository = customerRepository;
            BudgetRepository = budgetRepository;
            MaterialsRepository = materialsRepository;
            ServicesRepository = servicesRepository;
            MachinesRepository = machinesRepository;
            CommentRepository = commentRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }


        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

    }
}
