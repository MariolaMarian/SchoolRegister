using AutoMapper;
using SchoolRegister.DAL.EF;
using System;

namespace SchoolRegister.Services.Services
{
    public abstract class BaseService : IDisposable
    {
        protected readonly IMapper _mapper;
        protected readonly ApplicationDbContext _dbContext;
        private bool _disposed;
        public BaseService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _disposed = false;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
