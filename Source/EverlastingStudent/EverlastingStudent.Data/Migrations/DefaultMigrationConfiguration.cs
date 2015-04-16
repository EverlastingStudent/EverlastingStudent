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

            if (!context.Courses.Any())
            {
                this.SeedCourses(context);
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

        private void SeedCourses(EverlastingStudentDbContext context)
        {
            context.Courses.Add(new Course()
            {
                Title = "Programming Basics",
                Lectures = 
                { 
                    new Lecture()
                    {
                         Title = "Introduction to C#",
                         DurationInMinutes = 1,
                         CoefficientKnowledgeGain = 0.01
                    },
                    new Lecture(){
                         Title = "Console Input and Output",
                         DurationInMinutes = 1,
                         CoefficientKnowledgeGain = 0.01
                    },
                    new Lecture(){
                         Title = "Conditional Statements",
                         DurationInMinutes = 1,
                         CoefficientKnowledgeGain = 0.01
                    }
                },
                Exam = new Exam
                {
                    ExamDurationInMinutes = 30,
                    RequireExpForExam = 100
                }
            });
            context.Courses.Add(new Course()
            {
                Title = "Java Basics",
                Lectures = 
                { 
                    new Lecture()
                    {
                         Title = "Introduction to Java",
                         DurationInMinutes = 10,
                         CoefficientKnowledgeGain = 0.015
                    },
                    new Lecture(){
                         Title = "Loops",
                         DurationInMinutes = 10,
                         CoefficientKnowledgeGain = 0.015
                    },
                    new Lecture(){
                         Title = "Strings",
                         DurationInMinutes = 10,
                         CoefficientKnowledgeGain = 0.015
                    },
                    new Lecture(){
                         Title = "Arrays",
                         DurationInMinutes = 10,
                         CoefficientKnowledgeGain = 0.015
                    }
                },
                Exam = new Exam
                {
                    ExamDurationInMinutes = 45,
                    RequireExpForExam = 200
                }
            });
            context.Courses.Add(new Course()
            {
                Title = "Object-Oriented Programming",
                Lectures = 
                { 
                    new Lecture()
                    {
                         Title = "Introduction to OOP",
                         DurationInMinutes = 15,
                         CoefficientKnowledgeGain = 0.02
                    },
                    new Lecture(){
                         Title = "Defining Classes",
                         DurationInMinutes = 15,
                         CoefficientKnowledgeGain = 0.02
                    },
                    new Lecture(){
                         Title = "Inheritance and Encapsulation",
                         DurationInMinutes = 25,
                         CoefficientKnowledgeGain = 0.02
                    },
                    new Lecture(){
                         Title = "Abstraction and Polymorphism",
                         DurationInMinutes = 25,
                         CoefficientKnowledgeGain = 0.02
                    }
                },
                Exam = new Exam
                {
                    ExamDurationInMinutes = 60,
                    RequireExpForExam = 400
                }
            });
            context.Courses.Add(new Course()
            {
                Title = "High-Quality Code",
                Lectures = 
                { 
                    new Lecture()
                    {
                         Title = "Quality Identifiers and Methods",
                         DurationInMinutes = 15,
                         CoefficientKnowledgeGain = 0.02
                    },
                    new Lecture(){
                         Title = "Code Refacturing",
                         DurationInMinutes = 15,
                         CoefficientKnowledgeGain = 0.02
                    },
                    new Lecture(){
                         Title = "Unit Testing and Mocking",
                         DurationInMinutes = 25,
                         CoefficientKnowledgeGain = 0.02
                    },
                    new Lecture(){
                         Title = "Design Patterns",
                         DurationInMinutes = 25,
                         CoefficientKnowledgeGain = 0.02
                    }
                },
                Exam = new Exam
                {
                    ExamDurationInMinutes = 75,
                    RequireExpForExam = 600
                }
            });
            context.Courses.Add(new Course()
            {
                Title = "HTML Basics"
            });
            context.Courses.Add(new Course()
            {
                Title = "CSS Basics"
            });
            context.Courses.Add(new Course()
            {
                Title = "PHP Basics"
            });
            context.Courses.Add(new Course()
            {
                Title = "JavaScript Basics"
            });
            context.Courses.Add(new Course()
            {
                Title = "Advanced JavaScript"
            });
            context.Courses.Add(new Course()
            {
                Title = "JavaScript Applications"
            });
            context.Courses.Add(new Course()
            {
                Title = "AngularJS"
            });

            context.SaveChanges();

            var count = context.Courses.Count();

            for (int i = 1; i < count; i++)
            {
                var course = context.Courses.Where(c => c.Id == i).First();
                var nextCourse = context.Courses.Where(c => c.Id == i+1).First();
                course.NextCourse = nextCourse;
            }

            context.SaveChanges();
        }
    }
}
