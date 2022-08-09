using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnnotationWebApp.Models.Account
{
    public class SignUpViewModel:SignUpInputModel
    {
        public bool IsSignUpEnabled { get; set; } // Temporarly Value

        // TODO. External Login scheme. ex: kakao
        public string Message { get; set; }

    }
}
