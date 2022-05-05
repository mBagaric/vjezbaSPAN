using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    public class CourseForCreationDto : CourseForManupulationDto//: IValidatableObject
    {

        /*[Required(ErrorMessage = "You shoulf fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't be more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The description shouldn't be more than 1500 characters.")]
        public string Description { get; set; }*/

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "The provider description should be different form the title.",
        //            new[] { "CourseForCreationDto" });
        //    }
        //}
    }
}
