using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnnotationWebApp.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "이메일 주소를 입력하여 주세요")]
        [EmailAddress]
        [Display(Name = "이메일 주소")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "비밀번호를 입력하여 주세요.")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Display(Name = "로그인 유지")]
        [JsonPropertyName("remember")]
        public bool RememberMe { get; set; }
    }
}
