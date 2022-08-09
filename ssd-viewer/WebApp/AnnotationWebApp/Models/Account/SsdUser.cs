using Microsoft.AspNetCore.Identity;
using AnnotationWebApp.Models.TumorImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationWebApp.Models.Account
{
    /// <summary>
    /// KatechEMF User Identity Model
    /// </summary>
    public class SsdUser : IdentityUser
    {
        public string UserDisplayName { get; set; }

        public virtual List<EndoscopeVideo> EndoscopeVideos { get; set; }

        public virtual List<StillCutImage> StillCutImages { get; set; }
    }
}
