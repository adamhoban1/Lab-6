using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Student_Class_Library; // Your class library namespace

namespace Student_MVC_App.Data
{
    public class StudentsContext : DbContext
    {
        public StudentsContext() { }

        public StudentsContext(DbContextOptions<StudentsContext> options) : base(options) { }

        public DbSet<Appusers> users { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<project> projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<AppRole> roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite($"Data Source=C:\\Users\\joemh\\source\\repos\\adamhoban1\\lab-6\\student Console App2\\student.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Appusers to use int Id (not the inherited Guid Id from IdentityUser<Guid>)
            modelBuilder.Entity<Appusers>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // Configure composite key for StudentCourse
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            // Configure relationships for StudentCourse
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many: Appusers ↔ Team
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Members)
                .WithMany(u => u.teams)
                .UsingEntity<Dictionary<string, object>>(
                    "UserTeam",
                    j => j.HasOne<Appusers>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Team>().WithMany().HasForeignKey("TeamId"),
                    j => j.HasKey("UserId", "TeamId")
                );

            // One-to-many: Appusers → Project
            modelBuilder.Entity<project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.projects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Project → Team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Teams)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required fields and constraints
            modelBuilder.Entity<Appusers>().Property(u => u.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Appusers>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<project>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Team>().Property(t => t.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Course>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Course>().Property(c => c.department).IsRequired();
            modelBuilder.Entity<Course>().Property(c => c.lecturer).IsRequired();
        }
    }

    public class StudentsContextFactory : IDesignTimeDbContextFactory<StudentsContext>
    {
        public StudentsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentsContext>();
            optionsBuilder.UseSqlite($"Data Source=C:\\Users\\joemh\\source\\repos\\adamhoban1\\lab-6\\student Console App2\\student.db");

            return new StudentsContext(optionsBuilder.Options);
        }
    }
}
