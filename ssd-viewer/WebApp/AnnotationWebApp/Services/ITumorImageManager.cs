using AnnotationWebApp.Models;
using AnnotationWebApp.Models.TumorImage;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AnnotationWebApp.Services
{
    public interface ITumorImageManager
    {

        /// <summary>
        /// Check existence of video file
        /// <para>It should be note that It check only id in DB, not physical file.</para>
        /// </summary>
        /// <param name="videoId">Video file id</param>
        /// <returns>True if exist or false</returns>
        bool CheckExistenceOfVideo(string videoId);

        /// <summary>
        /// Check existence of image file
        /// <para>It should be note that It check only id in DB, not physical file.</para>
        /// </summary>
        /// <param name="imageId">Image file id</param>
        /// <returns>True if exist or false</returns>
        bool CheckExistenceOfImage(string imageId);

        Task<List<EndoscopeVideo>> GetUnCompleteVideosAsync(int numTake);

        Task<List<EndoscopeVideo>> GetCompleteVideosAsync(int numTake);

        Task<List<EndoscopeVideo>> GetAllVideosAsync(int numTake);

        Task<bool> UpdateOrCheckAnnotationDone(string videoId);

        int GetTotalNumberOfImages(string videoId);
        int GetTotalNumberOfCroppedImages(string videoId);
        int GetTotalNumberOfUnCroppedImages(string videoId);



        /// <summary>
        /// Provide all image list belong tager video
        /// </summary>
        /// <param name="videoId">Video Id</param>
        /// <param name="numTake">Number of take (load) images from the DB</param>
        /// <returns></returns>
        Task<List<StillCutImage>> GetAllImagesAsync(string videoId, int numTake);

        Task<List<StillCutImage>> GetUnCroppedImagesAsync(string videoId, int numTake);

        Task<List<StillCutImage>> GetCroppedImagesAsync(string videoId, int numTake);

        Task<List<TumorPosition>> GetAllTumorPositions(string imageId);

        /// <summary>
        /// Insert new tumor position (rectangle coordinate) to target image.
        /// </summary>
        /// <param name="imageId">Image id where to define tumor position</param>
        /// <param name="tumorPosition">Tumor Position Input Model</param>
        /// <returns>Video ID where the image belong</returns>
        Task<string> InsertTumorPosition(string imageId, string userId, List<TumorPosInputModel> tumorPosition);


        /// <summary>
        /// Update tumor position (rectangle coordinate) to target image.
        /// <para>CATUION: EXIST tumor position will be deleted.</para>
        /// </summary>
        /// <param name="imageId">Image id where to define tumor position</param>
        /// <param name="tumorPosition">Tumor Position Input Model</param>
        /// <returns>Video ID where the image belong</returns>
        Task<string> UpdateTumorPosition(string imageId, string userId, List<TumorPosInputModel> tumorPosInputs);
        
        /// <summary>
        /// Delete single tumor position in target image.
        /// </summary>
        /// <param name="imageId">Image id that has tumor position</param>
        /// <param name="positionId">Tumor position Id in DB</param>
        /// <returns>Video Id where the image belong</returns>
        Task<string> DeleteTumorPosition(string imageId, string userId, string positionId);
        
        /// <summary>
        /// Delete All tumor position in target image.
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        Task<string> DeleteAllTumorPosition(string imageId, string userId);
        
        
        string GetImageFilePath(string imageId);
    }
}
