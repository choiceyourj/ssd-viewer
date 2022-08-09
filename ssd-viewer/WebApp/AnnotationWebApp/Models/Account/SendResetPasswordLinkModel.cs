using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnnotationWebApp.Models.Account
{
    public class SendResetPasswordLinkModel
    {
        [Required(ErrorMessage = "이메일 주소를 입력하여 주세요")]
        [EmailAddress]
        [Display(Name = "이메일 주소")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
