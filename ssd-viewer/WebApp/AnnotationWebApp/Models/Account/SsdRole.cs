using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationWebApp.Models.Account
{
    public class SsdRole: IdentityRole
    {
        public string? RoleDescription { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
