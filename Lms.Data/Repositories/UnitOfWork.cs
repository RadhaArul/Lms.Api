using AutoMapper;
using Lms.Core.Repositories;
using Lms.Data.Data;

namespace Lms.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ILmsRepository LmsRepo { get; set; }

        public LmsApiContext db;

        public UnitOfWork(LmsApiContext context)
        {
            db = context;
            LmsRepo = new LmsRepository(db);
        }
        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
