using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AnnotationWebApp.Data;
using AnnotationWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnnotationWebApp.Models.TumorImage;
using AnnotationWebApp.Models.Account;

namespace AnnotationWebApp.Services
{
    public partial class TumorImageManager:ITumorImageManager
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TumorImageManager> _logger;
        private readonly IAppConfig _appConfig;

        public TumorImageManager(AppDbContext dbContext, ILogger<TumorImageManager> logger, IAppConfig appConfig)
        {
            _dbContext = dbContext;
            _logger = logger;
            _appConfig = appConfig;
        }

        /// <summary>
        /// Check existence of video file
        /// <para>It should be note that It check only id in DB, not physical file.</para>
        /// </summary>
        /// <param name="videoId">Video file id</param>
        /// <returns>True if exist or false</returns>
        public bool CheckExistenceOfVideo(string videoId)
        {
            return _dbContext.EndoscopeVideos.Any(video => video.Id == videoId);
        }

        /// <summary>
        /// Check existence of image file
        /// <para>It should be note that It check only id in DB, not physical file.</para>
        /// </summary>
        /// <param name="imageId">Image file id</param>
        /// <returns>True if exist or false</returns>
        public bool CheckExistenceOfImage(string imageId)
        {
            return _dbContext.StillCutImages.Any(img => img.Id == imageId);
        }

        /// <summary>
        /// Provide Un-cropped (tumor is not yet defined) video list
        /// </summary>
        /// <param name="numTake">Number of take (load) video from the DB</param>
        /// <returns></returns>
        public async Task<List<EndoscopeVideo>> GetUnCompleteVideosAsync(int numTake)
        {

            if (numTake <= 10)
            {
                numTake = _appConfig.MaxNumOfTakeImageFromDb;
            }

            var unCompletVideos = await _dbContext.EndoscopeVideos
                .AsNoTracking()
                .OrderBy(i => i.UploadTime)
                .Where(i => i.IsAllImageTreated == false)
                .Take(numTake)
                .ToListAsync();


            return unCompletVideos;
        }


        /// <summary>
        /// Provide cropped (tumor had been defined) video list
        /// </summary>
        /// <param name="numTake">Number of take (load) video from the DB</param>
        /// <returns></returns>
        public async Task<List<EndoscopeVideo>> GetCompleteVideosAsync(int numTake)
        {

            if (numTake <= 10)
            {
                numTake = _appConfig.MaxNumOfTakeImageFromDb;
            }

            var completVideos = await _dbContext.EndoscopeVideos
                .AsNoTracking()
                .OrderBy(i => i.UploadTime)
                .Where(i => i.IsAllImageTreated == true)
                .Take(numTake)
                .ToListAsync();


            return completVideos;
        }


        /// <summary>
        /// Provide all video list
        /// </summary>
        /// <param name="numTake">Number of take (load) video from the DB</param>
        /// <returns></returns>
        public async Task<List<EndoscopeVideo>> GetAllVideosAsync(int numTake)
        {

            if (numTake <= 10)
            {
                numTake = _appConfig.MaxNumOfTakeImageFromDb;
            }

            var videos = await _dbContext.EndoscopeVideos
                .AsNoTracking()
                .OrderBy(i => i.UploadTime)
                .Take(numTake)
                .ToListAsync();

            return videos;
        }

        public async Task<bool> UpdateOrCheckAnnotationDone(string videoId)
        {
            var numImage = await _dbContext.StillCutImages.Where(i => i.VideoId == videoId).CountAsync();
            var numCropDone = await _dbContext.StillCutImages.Where(i => i.VideoId == videoId && i.IsCropComplete == true).CountAsync();

            if(numImage == numCropDone && numImage != 0)
            {
                var video = await _dbContext.EndoscopeVideos.Where(i => i.Id == videoId).FirstOrDefaultAsync();
                video.IsAllImageTreated = true;

                _dbContext.EndoscopeVideos.Attach(video).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                // Nothing to do
                return false;
            }
        }

        public int GetTotalNumberOfImages(string videoId)
        {
            return _dbContext.StillCutImages.Where(i => i.VideoId == videoId).Count();
        }

        public int GetTotalNumberOfCroppedImages(string videoId)
        {
            return _dbContext.StillCutImages.Where(i => i.VideoId == videoId && i.IsCropComplete == true).Count();
        }
        public int GetTotalNumberOfUnCroppedImages(string videoId)
        {
            return _dbContext.StillCutImages.Where(i => i.VideoId == videoId && i.IsCropComplete == false).Count();
        }

        /// <summary>
        /// Provide specific video
        /// </summary>
        /// <param name="videoId">Video Id</param>
        /// <returns></returns>
        public async Task<EndoscopeVideo> GetVideoAsync(string videoId)
        {

            var video = await _dbContext.EndoscopeVideos
                .AsNoTracking()
                .Where(i => i.Id == videoId)
                .FirstOrDefaultAsync();

            return video;
        }

    }
}
