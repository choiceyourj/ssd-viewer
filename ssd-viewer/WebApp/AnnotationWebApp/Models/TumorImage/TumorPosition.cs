using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AnnotationWebApp.Models.Account;

namespace AnnotationWebApp.Models.TumorImage
{
    public class TumorPosition:TumorPosInputModel
    {              
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Display ORDER, If there are multiple crop rectangles.
        /// <para>It should be note that order always start ZERO.</para>
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Foregine Key
        /// </summary>
        [JsonPropertyName("imageId")]
        public string ImageId { get; set; }

        [JsonPropertyName("image")]
        [JsonIgnore]
        public virtual StillCutImage Image { get; set; }
    }
}
