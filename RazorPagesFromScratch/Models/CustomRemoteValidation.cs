using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RazorPagesFromScratch.Models
{
    public class CustomRemoteValidationAttribute : RemoteAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var db = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            PropertyInfo additionalPropertyName =
                validationContext.ObjectInstance.GetType().GetProperty(AdditionalFields);
            object additionalPropertyValue =
                additionalPropertyName.GetValue(validationContext.ObjectInstance, null);

            bool validateName = db.Items.Any(
                x => x.Text == (string)value && x.ListId == (int)additionalPropertyValue);
            if (validateName )
            {
                return new ValidationResult("The text already exist", new string[] { "Text" });
            }
            return ValidationResult.Success;
        }
       

    }
}
