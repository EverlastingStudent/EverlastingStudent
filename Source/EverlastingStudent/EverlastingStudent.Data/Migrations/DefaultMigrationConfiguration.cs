namespace EverlastingStudent.Data.Migrations
{
    using System.Data.Entity.Migrations;

    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Models;

    internal sealed class DefaultMigrationConfiguration : DbMigrationsConfiguration<EverlastingStudentDbContext>
    {
        public DefaultMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EverlastingStudentDbContext context)
        {
            this.SeedHomeworks(context);
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
    }
}
