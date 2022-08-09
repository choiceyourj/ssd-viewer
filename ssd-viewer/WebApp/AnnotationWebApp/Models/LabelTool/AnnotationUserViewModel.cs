namespace AnnotationWebApp.Models.LabelTool
{
    public class AnnotationUserViewModel
    {
        public AnnotationUserViewModel()
        {
            VideoList = new List<VideoUserViewModel>();
            ImageList = new List<ImageUserViewModel>();
        }
        public IList<VideoUserViewModel> VideoList { get; set; }
        public IList<ImageUserViewModel> ImageList { get; set; }
        
        public string WorkImageId { get; set; }
        public string PositionJsonStr { get; set; }
    }
}
