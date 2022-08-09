using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationWebApp.Data
{
    public interface IAppConfig
    {

        /// <summary>
        /// Tumor image storage path and folder
        /// </summary>
        DataStorage Storage { get; set; }

        /// <summary>
        /// Maximum number when loading images from DB
        /// </summary>
        int MaxNumOfTakeImageFromDb { get; set; }

        /// <summary>
        /// Maximum number when loading viedo from DB
        /// </summary>
        int MaxNumOfTakeVideoFromDb { get; set; }

        string NotFoundImagePath { get; set; }
    }
}
