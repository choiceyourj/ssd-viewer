namespace AnnotationWebApp.Models.LabelTool
{
    public class AnnotationUserInputModel
    {
        public AnnotationUserInputModel()
        {
            TumorPosition = new List<TumorPosInputModel>();
        }

        public string videoId { get; set; }
        public string ImageId { get; set; }
        public IList<TumorPosInputModel> TumorPosition { get; set; }
    }
}
