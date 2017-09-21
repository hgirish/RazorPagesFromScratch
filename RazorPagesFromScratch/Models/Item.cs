using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace RazorPagesFromScratch.Models
{
    public class Item //: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You can't have an empty list item")]
      //  [CustomRemoteValidation(AdditionalFields ="ListId")]
       [Remote(action: "ValidateText", controller:"Home",AdditionalFields ="ListId")]
        public string Text { get; set; }
        public TodoList List { get; set; }
        public int ListId { get; set; }

    
    }
}
