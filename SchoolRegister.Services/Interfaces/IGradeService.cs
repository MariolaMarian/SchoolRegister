using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        GradeVM AddGradeToStudent(GradeForStudentAddDTO gradeForStudentAddDTO);
        GradesReportVM GetGradesReportForStudent(GetGradesDTO getGradesDTO);
    }
}
