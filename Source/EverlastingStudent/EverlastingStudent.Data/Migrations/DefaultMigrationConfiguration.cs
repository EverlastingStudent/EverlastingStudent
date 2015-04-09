namespace EverlastingStudent.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Models;
    using EverlastingStudent.Models.FreelanceProjects;

    internal sealed class DefaultMigrationConfiguration : DbMigrationsConfiguration<EverlastingStudentDbContext>
    {
        public DefaultMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EverlastingStudentDbContext context)
        {
            if (!context.Homeworks.Any())
            {
                this.SeedHomeworks(context);
            }

            if (!context.BaseFreelanceProjects.Any())
            {
                this.SeedBaseFreelanceProjects(context);
            }
        }

        private void SeedHomeworks(EverlastingStudentDbContext context)
        {
            context.Homeworks.Add(new Homework
            {
                Title = "JavaScript Syntax (If statemants, operations, variables...)",
                Type = TypeOfDifficulty.Easy
            });
            context.Homeworks.Add(new Homework
            {
                Title = "Java Syntax (If statemants, operations, variables...)",
                Type = TypeOfDifficulty.Easy
            });
            context.Homeworks.Add(new Homework
            {
                Title = "PHP Syntax (If statemants, operations, variables...)",
                Type = TypeOfDifficulty.Medium
            });
            context.Homeworks.Add(new Homework
            {
                Title = "C# Syntax (If statemants, operations, variables...)",
                Type = TypeOfDifficulty.Hard
            });
            context.Homeworks.Add(new Homework
            {
                Title = "JavaScript Loops (for, forin, while...)",
                Type = TypeOfDifficulty.Easy
            });
            context.Homeworks.Add(new Homework
            {
                Title = "JavaScript OOP (classical, prototype)",
                Type = TypeOfDifficulty.Medium
            });
            context.Homeworks.Add(new Homework
            {
                Title = "JavaScript Functions",
                Type = TypeOfDifficulty.Hard
            });
            context.Homeworks.Add(new Homework
            {
                Title = "HTML Talbes",
                Type = TypeOfDifficulty.Easy
            });
            context.Homeworks.Add(new Homework
            {
                Title = "Responsive Design",
                Type = TypeOfDifficulty.Hard
            });
            context.Homeworks.Add(new Homework
            {
                Title = "SASS, LESS, Jekyll...",
                Type = TypeOfDifficulty.Medium
            });
            context.Homeworks.Add(new Homework
            {
                Title = "CSS Selectors",
                Type = TypeOfDifficulty.Medium
            });
            context.Homeworks.Add(new Homework
            {
                Title = "Regular Expressions (build in functions)",
                Type = TypeOfDifficulty.Medium
            });
            context.Homeworks.Add(new Homework
            {
                Title = "PHP - Apache (server setup, XAMPP, LAMPP, MAMPP)",
                Type = TypeOfDifficulty.Easy
            });
            context.Homeworks.Add(new Homework
            {
                Title = "Java Object Oriented Programming",
                Type = TypeOfDifficulty.Hard
            });
            context.Homeworks.Add(new Homework
            {
                Title = "DataBases - SQL (Structured  Query Language)",
                Type = TypeOfDifficulty.Easy
            });
            context.Homeworks.Add(new Homework
            {
                Title = "Database (SELECT, UPDATE, INSERT, DELETE)",
                Type = TypeOfDifficulty.Medium
            });
            context.Homeworks.Add(new Homework
            {
                Title = "High Quality code - Design Patterns",
                Type = TypeOfDifficulty.Hard
            });
            context.Homeworks.Add(new Homework
            {
                Title = "High Quality code - SOLID",
                Type = TypeOfDifficulty.Hard
            });

            context.SaveChanges();
        }

        private void SeedBaseFreelanceProjects(EverlastingStudentDbContext context)
        {
            context.BaseFreelanceProjects.Add(new BaseFreelanceProject()
            {
                Title = "Create new Office.",
                Content = "Bla-bla-bla",
                RequireExperience = 0,
                EnergyCost = 10,
                MoneyGain = 3,
                ExperienceGain = 2,
                SolveDurabationInHours = 6,
                IsActive = true,
                OpenForTakenDatetime = new DateTime(1999, 1, 1),
                CloseForTakenDatetime = new DateTime(2020, 1, 1),
                IsDeleted = false,
                DeletedOn = null
            });

            context.BaseFreelanceProjects.Add(new BaseFreelanceProject()
            {
                Title = "Create new Autocad.",
                Content = "Open Source",
                RequireExperience = 0,
                EnergyCost = 22,
                MoneyGain = 33,
                ExperienceGain = 5,
                SolveDurabationInHours = 2,
                IsActive = true,
                OpenForTakenDatetime = new DateTime(2002, 3, 3),
                CloseForTakenDatetime = new DateTime(2016, 2, 2),
                IsDeleted = false,
                DeletedOn = null
            });

            context.BaseFreelanceProjects.Add(new BaseFreelanceProject()
            {
                Title = "Create new Php version",
                Content = "ahahhx",
                RequireExperience = 999,
                EnergyCost = 12,
                MoneyGain = 323,
                ExperienceGain = 65,
                SolveDurabationInHours = 22,
                IsActive = true,
                OpenForTakenDatetime = new DateTime(2012, 7, 8),
                CloseForTakenDatetime = new DateTime(2017, 2, 2),
                IsDeleted = false,
                DeletedOn = null
            });

            context.SaveChanges();
        }
    }
}
