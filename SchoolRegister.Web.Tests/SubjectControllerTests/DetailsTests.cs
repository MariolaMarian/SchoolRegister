using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SchoolRegister.BLL.Entities;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VMs;
using SchoolRegister.Web.Controllers;
using System;
using System.Linq.Expressions;
using Xunit;

namespace SchoolRegister.Web.Tests
{
    public class DetailsTests
    {
        private readonly Mock<ILoggerFactory> _loggerFactory;
        private readonly SubjectVM _correctSubjectVM;

        public DetailsTests()
        {
            _correctSubjectVM = new SubjectVM() { Name = "Name" };
            _loggerFactory = new Mock<ILoggerFactory>();
        }

        
        [Fact]
        public void DetailsReturnsErrorViewWhenSubjectVMIsNULL()
        {
            //Assign
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(() => null);
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, null, null, null, null, _loggerFactory.Object);

            //Act
            var expected = "Error";
            var result = (ViewResult)_subjectController.Details(0);

            //Assert
            Assert.Equal(expected, result.ViewName);
        }
        

        [Fact]
        public void DetailsReturnsDetailsViewWithSubjectVMWhenSubjectVMIsNotNull()
        {
            //Assign
            Mock<ISubjectService> _subjectServiceMock = new Mock<ISubjectService>();
            _subjectServiceMock.Setup(s => s.GetSubject(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(_correctSubjectVM);
            SubjectController _subjectController = new SubjectController(_subjectServiceMock.Object, null, null, null, null, _loggerFactory.Object);

            //Act
            var expectedModel = _correctSubjectVM;
            var result = (ViewResult)_subjectController.Details(1);

            //Assert

            Assert.Equal(expectedModel, result.Model);
            Assert.NotEqual("Error", result.ViewName);
        }

    }
}
