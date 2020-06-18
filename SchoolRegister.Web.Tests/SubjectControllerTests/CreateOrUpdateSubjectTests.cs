using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using SchoolRegister.BLL.Entities;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using SchoolRegister.Web.Controllers;
using SchoolRegister.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace SchoolRegister.Web.Tests.SubjectControllerTests
{
    public class CreateOrUpdateSubjectTests
    {
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<ITeacherService> _teacherServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IStringLocalizer<SubjectController>> _localizerMock;
        private readonly SubjectVM _correctSubjectVM;
        private readonly SubjectForCreateOrUpdateDTO _correctSubjectDTO;
        private readonly string _alreadyExistsErrorDescription;

        public CreateOrUpdateSubjectTests()
        {
            _correctSubjectVM = new SubjectVM() { Id = 1, Name = "Name", Description = "Description" };
            _correctSubjectDTO = new SubjectForCreateOrUpdateDTO() { Id = 1, Name = "Name", Description = "Description", TeacherId = 4 };
            _alreadyExistsErrorDescription = "This subject already exists!";

            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _teacherServiceMock = new Mock<ITeacherService>();
            _teacherServiceMock.Setup(ts => ts.GetTeachers(null)).Returns(() => ListOfTeachersVMs());
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(m => m.Map<SubjectForCreateOrUpdateDTO>(_correctSubjectVM)).Returns(() => _correctSubjectDTO);
            _localizerMock = new Mock<IStringLocalizer<SubjectController>>();
            _localizerMock.Setup(l => l["This subject already exists"]).Returns(() => 
                new LocalizedString("This subject already exists", _alreadyExistsErrorDescription));
        }

        [Fact]
        public void WhenIdIsNullShouldModelBeNullAndViewShouldNotBeError()
        {
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => null);
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, _teacherServiceMock.Object, null, _mapperMock.Object, _localizerMock.Object, _loggerFactoryMock.Object);

            int? id = null;

            SubjectForCreateOrUpdateDTO expectedModel = null;

            var result = (ViewResult)_subjectController.CreateOrUpdateSubject(id);

            Assert.Equal(expectedModel, result.Model);
            Assert.NotEqual("Error", result.ViewName);
        }

        [Fact]
        public void WhenIdIsNotNullButSubjectVMIsNullShouldReturnError()
        {
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => null);
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, _teacherServiceMock.Object, null, _mapperMock.Object, _localizerMock.Object, _loggerFactoryMock.Object);

            int? id = 99;

            var result = (ViewResult)_subjectController.CreateOrUpdateSubject(id);

            Assert.Equal("Error", result.ViewName);
        }

        [Fact]
        public void WhenIdIsNotNullAndSubjectVMIsNotNullShouldReturnViewWithModelDTO()
        {
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => _correctSubjectVM);
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, _teacherServiceMock.Object, null, _mapperMock.Object, _localizerMock.Object, _loggerFactoryMock.Object);

            int? id = 99;

            var expected = _correctSubjectDTO;
            var result = (ViewResult)_subjectController.CreateOrUpdateSubject(id);

            Assert.NotEqual("Error", result.ViewName);
            Assert.Equal(expected, result.Model);
        }

        [Fact]
        public void POSTShouldCallCreateOrUpdateOnceAdndRedirectToIndexIfModelStateIsValidAndNotExistsAlready()
        {
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => _correctSubjectVM);
            _subjectServiceMock.Setup(s => s.CreateOrUpdate(_correctSubjectDTO)).Returns(() => _correctSubjectVM);  //returns object so it was succesfully created
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, _teacherServiceMock.Object, null, _mapperMock.Object, _localizerMock.Object, _loggerFactoryMock.Object);

            var result = (RedirectToActionResult)_subjectController.CreateOrUpdateSubject(_correctSubjectDTO);

            _subjectServiceMock.Verify(m => m.CreateOrUpdate(_correctSubjectDTO), Times.Once);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void POSTShouldCallCreateOrUpdateOnceAdndRedirectToIndexIfModelStateIsValidAndAlreadyExistsOrAnotherServiceError()
        {
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => _correctSubjectVM);
            _subjectServiceMock.Setup(s => s.CreateOrUpdate(_correctSubjectDTO)).Returns(() => null); //returns null so it was not created
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, _teacherServiceMock.Object, null, _mapperMock.Object, _localizerMock.Object, _loggerFactoryMock.Object);

            var result = (ViewResult)_subjectController.CreateOrUpdateSubject(_correctSubjectDTO);

            _subjectServiceMock.Verify(m => m.CreateOrUpdate(_correctSubjectDTO), Times.Once);
            Assert.Equal(_alreadyExistsErrorDescription, ((ErrorViewModel)(result.Model)).Description);
            Assert.Equal("Error", (result.ViewName));
        }

        [Fact]
        public void POSTShouldReturnSameViewWhenStateIsNotValid()
        {
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => _correctSubjectVM);
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, _teacherServiceMock.Object, null, _mapperMock.Object, _localizerMock.Object, _loggerFactoryMock.Object);
            _subjectController.ModelState.AddModelError("test", "test");

            var result = (ViewResult)_subjectController.CreateOrUpdateSubject(new SubjectForCreateOrUpdateDTO());

            Assert.Equal("CreateOrUpdateSubject", result.ViewName);
        }


        private List<TeacherVM> ListOfTeachersVMs()
        {
            return new List<TeacherVM>();
        }
    }
}
