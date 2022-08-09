using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnnotationWebApp.Models.Account
{
    public class ResetPasswordModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        [Required(ErrorMessage = "이메일 주소를 입력하여 주세요")]
        [EmailAddress]
        [Display(Name = "이메일 주소")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "비밀번호를 입력하여 주세요.")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "비밀번호를 한번 더 입력하여 주세요.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "입력한 비밀번호가 서로 일치하지 않습니다.")]
        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
