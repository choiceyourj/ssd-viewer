using System.Text.Json.Serialization;

namespace AnnotationWebApp.Models
{
    public class TumorPosInputModel
    {

        /// <summary>
        /// Start coordinates on the X-AXIS of the crop rectangle
        /// </summary>
        [JsonPropertyName("startX")]
        public int StartX { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        /// <summary>
        /// Start coordinates on the Y-AXIS of the crop rectangle
        /// </summary>
        [JsonPropertyName("startY")]
        public int StartY { get; set; }


        /// <summary>
        /// End coordinates on the X-AXIS of the crop rectangle
        /// </summary>
        [JsonPropertyName("endX")]
        public int EndX { get; set; }

        /// <summary>
        /// End coordinates on the Y-AXIS of the crop rectangle
        /// </summary>
        [JsonPropertyName("endY")]
        public int EndY { get; set; }
    }
}
