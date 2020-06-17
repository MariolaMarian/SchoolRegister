using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.BLL.Entities
{
    public enum GradeScale
    {
        [Display(Name = "NDST")]
        NDST = 2,
        [Display(Name = "DST")]
        DST = 3,
        [Display(Name = "DB")]
        DB = 4,
        [Display(Name = "BDB")]
        BDB = 5
    }
}
