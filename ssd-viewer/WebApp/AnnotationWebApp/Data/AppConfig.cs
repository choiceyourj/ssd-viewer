using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationWebApp.Data
{
    public class AppConfig:IAppConfig
    {
        public DataStorage Storage { get; set; }
        public int MaxNumOfTakeImageFromDb { get; set; }
        public int MaxNumOfTakeVideoFromDb { get; set; }
        public string NotFoundImagePath { get; set; }
    }
}
