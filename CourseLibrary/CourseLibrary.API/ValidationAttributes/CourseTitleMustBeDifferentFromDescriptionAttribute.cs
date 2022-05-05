using System.ComponentModel.DataAnnotations;
using System;
using CourseLibrary.API.Model;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var course = (CourseForManupulationDto)validationContext.ObjectInstance;

            if(course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(CourseForManupulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
