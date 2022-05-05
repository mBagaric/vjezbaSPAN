using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    public class CourseForUpdateDto: CourseForManupulationDto
    {
        [Required(ErrorMessage = "You should fill out the description")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
