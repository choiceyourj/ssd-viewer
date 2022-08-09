namespace AnnotationWebApp.Helpers
{
    public static class CssHelper
    {
        public static string IsSelected(bool flag)
        {
            if (flag)
            {
                return "active";
            }
            else
            {
                return "";
            }
        }
    }
}
