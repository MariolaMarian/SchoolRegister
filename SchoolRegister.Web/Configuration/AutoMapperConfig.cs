using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRegister.BLL.Entities;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Linq;

namespace SchoolRegister.Web.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //SUBJECT
            CreateMap<SubjectForCreateOrUpdateDTO, Subject>();

            CreateMap<SubjectGroup, SubjectVM>()
                .ForMember(dest => dest.Name, x => x.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Subject.Id));

            CreateMap<Subject, SubjectVM>()
                .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
                .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

            CreateMap<SubjectVM, SubjectForCreateOrUpdateDTO>();

            CreateMap<Group, GroupVM>()
                .ForMember(dest => dest.Subjects, x => x.MapFrom(src => src.SubjectGroups));

            CreateMap<SubjectVM, SelectListItem>()
                .ForMember(dest => dest.Text, y => y.MapFrom(z => z.Name))
                .ForMember(dest => dest.Value, y => y.MapFrom(z => z.Id));

            //USER
            CreateMap<User, StudentVM>();

            //TEACHER
            CreateMap<Teacher, TeacherVM>();

            CreateMap<TeacherVM, SelectListItem>()
                .ForMember(dest => dest.Text, y => y.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Value, y => y.MapFrom(z => z.Id));

            //STUDENT
            CreateMap<Student, StudentVM>()
                .ForMember(dest => dest.GroupName, x => x.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.ParentName,
                    x => x.MapFrom(src => $"{src.Parent.FirstName} {src.Parent.LastName}"));

            CreateMap<StudentVM, SelectListItem>()
                .ForMember(dest => dest.Text, y => y.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Value, y => y.MapFrom(z => z.Id));

            CreateMap<Student, GradesReportVM>()
                .ForMember(dest => dest.StudentLastName, y => y.MapFrom(src => src.LastName))
                .ForMember(dest => dest.StudentFirstName, y => y.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.GroupName, y => y.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.ParentName, y => y.MapFrom(src => $"{src.Parent.FirstName} {src.Parent.LastName}"))
                .ForMember(dest => dest.StudentGradesPerSubject, y => y.MapFrom(src => src.Grades
                    .GroupBy(g => g.Subject.Name)
                    .Select(g => new { SubjectName = g.Key, Grades = g.Select(gl => gl.GradeValue).ToList() })
                    .ToDictionary(x => x.SubjectName, x => x.Grades)));

            //PARENT
            CreateMap<Parent, ParentVM>();

            CreateMap<ParentVM, SelectListItem>()
                .ForMember(dest => dest.Text, y => y.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Value, y => y.MapFrom(z => z.Id));

            //GRADE
            CreateMap<GradeForStudentAddDTO, Grade>()
                .ForMember(dest => dest.DateOfIssue, y => y.MapFrom(src => DateTime.Now));

            CreateMap<Grade, GradeVM>()
                .ForMember(dest => dest.StudentName, y => y.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}"));

            //GROUP

            CreateMap<GroupForCreateOrUpdateDTO, Group>();

            CreateMap<GroupVM, GroupForCreateOrUpdateDTO>();

            CreateMap<GroupVM, SelectListItem>()
                .ForMember(dest => dest.Text, y => y.MapFrom(z => z.Name))
                .ForMember(dest => dest.Value, y => y.MapFrom(z => z.Id));
        }
    }
}
