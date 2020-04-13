using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace App
{
    public sealed class SchoolDbContext : DbContext
    {
        public SchoolDbContext(string connectionString, bool useConsoleLogger, EventDispatcher eventDispatcher) : base()
        {
            _connectionString = connectionString;
            _useConsoleLogger = useConsoleLogger;
            _eventDispatcher = eventDispatcher;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        private readonly string _connectionString;
        private readonly bool _useConsoleLogger;
        private readonly EventDispatcher _eventDispatcher;

        private readonly Type[] EnumerationTypes = new[] { typeof(Course), typeof(Suffix) };

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_connectionString)
                .UseLazyLoadingProxies();

            if (_useConsoleLogger)
            {
                optionsBuilder
                    .UseLoggerFactory(CreateLoggerFactory())
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(x =>
            {
                x.ToTable("Student").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("StudentID");
                x.Property(p => p.Email)
                    .HasConversion(p => p.Value, p => Email.Create(p).Value);
                x.OwnsOne(p => p.Name, p =>
                    {
                        p.Property<long?>("NameSuffixId").HasColumnName("NameSuffixId");
                        p.Property(pp => pp.First).HasColumnName("FirstName");
                        p.Property(pp => pp.Last).HasColumnName("LastName");
                        p.HasOne(pp => pp.Suffix).WithMany().HasForeignKey("NameSuffixId").IsRequired(false);
                    });
                x.HasOne(p => p.FavoriteCourse).WithMany();
                x.HasMany(p => p.Enrollments).WithOne(p => p.Student)
                    .OnDelete(DeleteBehavior.Cascade)
                    .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Course>(x =>
            {
                x.ToTable("Course").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("CourseID");
                x.Property(p => p.Name);
                    //.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });

            modelBuilder.Entity<Enrollment>(x =>
            {
                x.ToTable("Enrollment").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("EnrollmentID");
                x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
                x.HasOne(p => p.Course).WithMany();
                x.Property(p => p.Grade);
            });

            modelBuilder.Entity<Suffix>(x =>
            {
                x.ToTable("Suffix").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("SuffixId");
                x.Property(p => p.Name);
                    //.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });
        }

        public override int SaveChanges()
        {
            var enumerationEntries = ChangeTracker.Entries()
                .Where(x => EnumerationTypes.Contains(x.Entity.GetType()));

            foreach(var entry in enumerationEntries)
            {
                entry.State = EntityState.Unchanged;
            }

            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is AggregateRoot)
                .Select(x => (AggregateRoot)x.Entity)
                .ToList();

            var result = base.SaveChanges();

            foreach(var entity in entities)
            {
                // dispatch events
                _eventDispatcher.Dispatch(entity.DomainEvents);

                // clear events
                entity.ClearDomainEvents();
            }

            return result;
        }
    }
}
