using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Student_Class_Library;
using System;
namespace Student_Console_App2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using var db = new StudentsContext();
                
                // Ensure database is created
                db.Database.EnsureCreated();
                
                // Seed Users
                if (!db.users.Any())
                {
                    Console.WriteLine("Seeding database...");
                    
                    // Create users
                    var user1 = new Appusers 
                    { 
                        Name = "Alice Smith", 
                        Age = 25, 
                        UserName = "alice",
                        Email = "alice@example.com",
                        NormalizedUserName = "ALICE",
                        NormalizedEmail = "ALICE@EXAMPLE.COM",
                        EmailConfirmed = true
                    };
                    
                    var user2 = new Appusers 
                    { 
                        Name = "Bob Lee", 
                        Age = 30, 
                        UserName = "bob",
                        Email = "bob@example.com",
                        NormalizedUserName = "BOB",
                        NormalizedEmail = "BOB@EXAMPLE.COM",
                        EmailConfirmed = true
                    };

                    // Add users first to generate IDs
                    db.users.AddRange(user1, user2);
                    db.SaveChanges();

                    // Create projects
                    var project1 = new project 
                    { 
                        Name = "Project A", 
                        Description = "Important project", 
                        OwnerId = user1.Id
                    };
                    
                    var project2 = new project 
                    { 
                        Name = "Project B", 
                        Description = "Another project", 
                        OwnerId = user2.Id
                    };

                    db.projects.AddRange(project1, project2);
                    db.SaveChanges();

                    // Create teams
                    var team1 = new Team 
                    { 
                        Name = "Team Alpha", 
                        ProjectId = project1.Id
                    };
                    
                    var team2 = new Team 
                    { 
                        Name = "Team Beta", 
                        ProjectId = project2.Id
                    };

                    db.Teams.AddRange(team1, team2);
                    db.SaveChanges();

                    // Create courses
                    var course1 = new Course 
                    { 
                        Name = "Database Systems",
                        department = "Computer Science",
                        lecturer = "Dr. Smith"
                    };
                    
                    var course2 = new Course 
                    { 
                        Name = "Operating Systems", 
                        department = "Computer Science",
                        lecturer = "Prof. Johnson"
                    };
                    
                    var course3 = new Course 
                    { 
                        Name = "Calculus I",
                        department = "Mathematics",
                        lecturer = "Dr. Brown"
                    };

                    db.Courses.AddRange(course1, course2, course3);
                    db.SaveChanges();

                    // Create student-course relationships
                    var sc1 = new StudentCourse { StudentId = user1.Id, CourseId = course1.Id };
                    var sc2 = new StudentCourse { StudentId = user1.Id, CourseId = course3.Id };
                    var sc3 = new StudentCourse { StudentId = user2.Id, CourseId = course1.Id };
                    var sc4 = new StudentCourse { StudentId = user2.Id, CourseId = course2.Id };

                    db.StudentCourses.AddRange(sc1, sc2, sc3, sc4);
                    db.SaveChanges();

                    // Assign users to teams
                    team1.Members.Add(user1);
                    team1.Members.Add(user2);
                    team2.Members.Add(user2);
                    
                    db.SaveChanges();
                    
                    Console.WriteLine("Database seeded successfully!");
                    Console.WriteLine($"Created {db.users.Count()} users");
                    Console.WriteLine($"Created {db.projects.Count()} projects");
                    Console.WriteLine($"Created {db.Teams.Count()} teams");
                    Console.WriteLine($"Created {db.Courses.Count()} courses");
                    Console.WriteLine($"Created {db.StudentCourses.Count()} student-course enrollments");
                }
                else
                {
                    Console.WriteLine("Database already contains data.");
                    Console.WriteLine($"Users: {db.users.Count()}");
                    Console.WriteLine($"Projects: {db.projects.Count()}");
                    Console.WriteLine($"Teams: {db.Teams.Count()}");
                    Console.WriteLine($"Courses: {db.Courses.Count()}");
                    Console.WriteLine($"Enrollments: {db.StudentCourses.Count()}");
                    // Get all projects with their teams and members
                    var projects = db.projects
                        .Include(p => p.Teams)
                            .ThenInclude(t => t.Members)
                        .ToList();

                    foreach (var project in projects)
                    {
                        Console.WriteLine($"Project: {project.Name} (Owner: {project.Owner.Name})");
                        foreach (var team in project.Teams)
                        {
                            Console.WriteLine($"  Team: {team.Name}");
                            foreach (var member in team.Members)
                            {
                                Console.WriteLine($"    Member: {member.Name}");
                            }
                        }
                    }

                    // Query: Users in more than 1 team
                    var multiTeamUsers = db.users
                        .Include(u => u.teams)
                        .Where(u => u.teams.Count > 1)
                        .ToList();

                    Console.WriteLine("\nUsers in multiple teams:");
                    foreach (var user in multiTeamUsers)
                    {
                        Console.WriteLine(user.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
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
}
