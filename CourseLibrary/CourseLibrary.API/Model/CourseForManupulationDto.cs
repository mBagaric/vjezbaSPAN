using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    [CourseTitleMustBeDifferentFromDescription(
        ErrorMessage = "Title must be different from description.")]
    public abstract class CourseForManupulationDto
    {
        [Required(ErrorMessage = "You shoulf fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't be more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The description shouldn't be more than 1500 characters.")]
        public virtual string Description { get; set; }
    }
}
