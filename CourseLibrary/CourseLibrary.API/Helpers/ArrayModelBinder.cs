using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            // GUID
            // value isn't null or white space
            // and the type of the model is enumerable
            // Get the enumerable type and a converter
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            // convert each item in the value list as a Model value
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();

            // Create an array of that type, and set it as the Model value
            var typeValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typeValues, 0);
            bindingContext.Model = typeValues;

            //return a successful result, passing in the Model
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
