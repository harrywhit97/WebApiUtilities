using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebApiUtilities.Abstract;
using WebApiUtilities.Concrete;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Testing
{
    public abstract class AbstractTestClass : IDisposable
    {
        public Mock<IClock> Clock;
        public DateTimeOffset DateTimeDefualt;
        public TDbContext Context;
        public CancellationToken CancToken => new CancellationToken();
        public IMapper Mapper;
        IConfigurationProvider _configuration;

        [TestInitialize]
        public virtual void Initialize<TDbContext>(DateTimeOffset defaultDateTime)
            where TDbContext : DbContext
        {

            DateTimeDefualt = defaultDateTime;
            Clock = Utils.GetMockDateTime(DateTimeDefualt);

            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = _configuration.CreateMapper();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            Mapper = new Mapper(configuration);
        }

        TDbContext GetDbContextMock<TDbContext>()
            where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);

            var isAuditingDbConext = dbContextType.GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAuditingDbContext))
                    .GetGenericArguments()
                    .FirstOrDefault();

            if (isAuditingDbConext)
                return MakeAuditingContext<TDbContext>();

            Context = DbContextMocker<TDbContext>.GetTestingDbContext(DateTime.Object);
        }

        TDbContext MakeAuditingContext<TDbContext>()
            where TDbContext : AuditingDbContext, new()
        {
            return AuditingDbContextMocker<TDbContext>.GetTestingDbContext(Clock.Object);
        }


        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        public abstract void Seed(TDbContext dbContext);
    }
}
