using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AnnotationWebApp.Models.Account;

namespace AnnotationWebApp.Models.TumorImage
{
    public class StillCutImage
    {

        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Number of tumors in this image.
        /// </summary>
        [JsonPropertyName("numberOfTumors")]
        public int NumberOfTumors
        { 
            get
            {
                int numTumors = 0;
                if(TumorPositions != null)
                {
                    numTumors = TumorPositions.Count;
                }
                return numTumors;
            }
        }

        /// <summary>
        /// Releative path where the image is stored
        /// </summary>
        [JsonPropertyName("imageFileLocation")]
        public string ImageFileLocation { get; set; }


        [JsonPropertyName("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// Date and time the image was created
        /// </summary>
        [JsonPropertyName("imageCreatedTime")]
        public DateTime ImageCreatedTime { get; set; }


        /// <summary>
        /// Whether image cropping (finding tumor) is complete.
        /// </summary>
        [JsonPropertyName("isCropComplete")]
        public bool IsCropComplete { get; set; }


        [JsonPropertyName("videoId")]
        public string VideoId { get; set; }


        //
        //========== Relations ==========

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// XY Coordinate list of tumor position in this image
        /// </summary>
        [JsonPropertyName("tumorPositions")]
        public virtual IList<TumorPosition> TumorPositions { get; set; }


        [JsonPropertyName("video")]
        [JsonIgnore]
        public virtual EndoscopeVideo Video { get; set; }


        [JsonPropertyName("user")]
        [JsonIgnore]
        public virtual SsdUser User { get; set; }
    }
}
