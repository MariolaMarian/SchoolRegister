using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRegister.Web.Seed
{
    public static class Seed
    {
        public static void SeedAll(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext applicationDbContext)
        {
            SeedGroups(applicationDbContext);
            SeedRoles(roleManager);
            SeedUsers(userManager, applicationDbContext);
        }
        private static void SeedGroups(ApplicationDbContext applicationDbContext)
        {
            if (!applicationDbContext.Groups.Any())
            {
                var groups = new List<Group>
                {
                    new Group{Name="1A"},
                    new Group{Name="2B"},
                    new Group{Name="1C"}
                };

                applicationDbContext.AddRange(groups);
                applicationDbContext.SaveChanges();
            }
        }

        public static void SeedRoles(RoleManager<Role> roleManager)
        {
            var roles = new List<Role>
            {
                new Role{Name="Admin",RoleValue=RoleValue.Admin},
                new Role{Name="Teacher",RoleValue=RoleValue.Teacher},
                new Role{Name="Parent",RoleValue=RoleValue.Parent},
                new Role{Name="Student", RoleValue=RoleValue.Student}
            };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void SeedUsers(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            var admin = new User
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "Admin",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(admin, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(admin, "Admin").Wait();

            var teacher1 = new Teacher
            {
                Email = "teacher1@gmail.com",
                UserName = "teacher1@gmail.com",
                FirstName = "Anna",
                LastName = "Malinowska",
                Title = "prof",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(teacher1, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(teacher1, "Teacher").Wait();

            var teacher2 = new Teacher
            {
                Email = "teacher2@gmail.com",
                UserName = "teacher2@gmail.com",
                FirstName = "Andrzej",
                LastName = "Kot",
                Title = "mgr",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(teacher2, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(teacher2, "Teacher").Wait();

            var teacher3 = new Teacher
            {
                Email = "teacher3@gmail.com",
                UserName = "teacher3@gmail.com",
                FirstName = "Wioletta",
                LastName = "Nudna",
                Title = "mgr",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(teacher3, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(teacher3, "Teacher").Wait();

            var parent1 = new Parent
            {
                Email = "parent1@gmail.com",
                UserName = "parent1@gmail.com",
                FirstName = "Antoni",
                LastName = "Kowalski",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(parent1, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(parent1, "Parent").Wait();

            var parent2 = new Parent
            {
                Email = "parent2@gmail.com",
                UserName = "parent2@gmail.com",
                FirstName = "Joanna",
                LastName = "Nowak",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(parent2, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(parent2, "Parent").Wait();

            var parent3 = new Parent
            {
                Email = "parent3@gmail.com",
                UserName = "parent3@gmail.com",
                FirstName = "Albert",
                LastName = "Zielony",
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(parent3, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(parent3, "Parent").Wait();

            var student1 = new Student
            {
                Email = "student1@gmail.com",
                UserName = "student1@gmail.com",
                FirstName = "Damian",
                LastName = "Zielony",
                GroupId = applicationDbContext.Groups.First(g => g.Id != 0).Id,
                ParentId = applicationDbContext.Users.First(g => g.LastName == "Zielony").Id,
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(student1, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(student1, "Student").Wait();

            var student2 = new Student
            {
                Email = "student2@gmail.com",
                UserName = "student2@gmail.com",
                FirstName = "Anna",
                LastName = "Nowak",
                GroupId = applicationDbContext.Groups.First(g => g.Id != 0).Id,
                ParentId = applicationDbContext.Users.First(g => g.LastName == "Nowak").Id,
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(student2, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(student2, "Student").Wait();

            var student3 = new Student
            {
                Email = "student3@gmail.com",
                UserName = "student3@gmail.com",
                FirstName = "Zygmunt",
                LastName = "Nowak",
                GroupId = applicationDbContext.Groups.First(g => g.Id != 0).Id,
                ParentId = applicationDbContext.Users.First(g => g.LastName == "Nowak").Id,
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(student3, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(student3, "Student").Wait();

            var student4 = new Student
            {
                Email = "student4@gmail.com",
                UserName = "student4@gmail.com",
                FirstName = "Zygmunt",
                LastName = "Kowalski",
                GroupId = applicationDbContext.Groups.First(g => g.Id != 0).Id,
                ParentId = applicationDbContext.Users.First(g => g.LastName == "Kowalski").Id,
                RegistrationDate = DateTime.Now
            };
            userManager.CreateAsync(student4, "Pa$$w0rd").Wait();
            userManager.AddToRoleAsync(student4, "Student").Wait();
        }
    }
}
