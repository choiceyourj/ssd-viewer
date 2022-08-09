namespace AnnotationWebApp.Models.LabelTool
{
    public class VideoUserViewModel
    {
        public string VideoId { get; set; }
        public string VideoDisplayName { get; set; }

        /// <summary>
        /// Number of remain frame for crop (define tumor position)
        /// </summary>
        public int NumOfRemainFrame { get; set; }

        /// <summary>
        /// Total number of frame for crop
        /// </summary>
        public int NumOfTotalFrame { get; set; }
        public bool IsSelected { get; set; }
        public string Description { get; set; }
    }
}
