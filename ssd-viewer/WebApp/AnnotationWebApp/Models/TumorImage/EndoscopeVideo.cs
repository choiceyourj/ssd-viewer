using AnnotationWebApp.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnnotationWebApp.Models.TumorImage
{
    public class EndoscopeVideo
    {

        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("videoFileLocation")]
        public string VideoFileLocation { get; set; }

        [JsonPropertyName("totalNumberOfFrame")]
        public int TotalNumberOfFrame { get; set; }

        [JsonPropertyName("isAllImageTreated")]
        public bool IsAllImageTreated { get; set; }

        [JsonPropertyName("uploadTime")]
        public DateTime UploadTime { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }



        //
        //========== Relations ==========

        [JsonPropertyName("user")]
        [JsonIgnore]
        public virtual SsdUser User { get; set; }

        [JsonPropertyName("stillCutImages")]
        public virtual IList<StillCutImage> StillCutImages { get; set; }
    }
}
