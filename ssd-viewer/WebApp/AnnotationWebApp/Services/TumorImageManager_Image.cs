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
    public partial class TumorImageManager : ITumorImageManager
    {

        /// <summary>
        /// Provide Un-cropped (tumor is not yet defined) image list
        /// </summary>
        /// <param name="videoId">Video Id</param>
        /// <param name="numTake">Number of take (load) images from the DB</param>
        /// <returns></returns>
        public async Task<List<StillCutImage>> GetUnCroppedImagesAsync(string videoId, int numTake)
        {

            if (string.IsNullOrWhiteSpace(videoId))
            {
                _logger.LogWarning("VideoId Cannot be null. Return null");
                return null;
            }

            if (numTake <= 0)
            {
                numTake = _appConfig.MaxNumOfTakeImageFromDb;
            }

            List<StillCutImage> unCroppedImages = await _dbContext.StillCutImages
                .AsNoTracking()
                .Include(i => i.TumorPositions)
                .Where(i => i.IsCropComplete == false && i.VideoId == videoId).Take(numTake)
                .ToListAsync();

            return unCroppedImages;
        }

        /// <summary>
        /// Provide cropped (tumor was defined) image list
        /// </summary>
        /// <param name="videoId">Video Id</param>
        /// <param name="numTake">Number of take (load) images from the DB</param>
        /// <returns></returns>
        public async Task<List<StillCutImage>> GetCroppedImagesAsync(string videoId, int numTake)
        {

            if (string.IsNullOrWhiteSpace(videoId))
            {
                _logger.LogWarning("VideoId Cannot be null. Return null");
                return null;
            }

            if (numTake <= 0)
            {
                numTake = _appConfig.MaxNumOfTakeImageFromDb;
            }

            List<StillCutImage> unCroppedImages = await _dbContext.StillCutImages
                .AsNoTracking()
                .Include(i => i.TumorPositions)
                .Where(i => i.IsCropComplete == true && i.VideoId == videoId).Take(numTake)
                .ToListAsync();

            return unCroppedImages;
        }



        /// <summary>
        /// Provide all image list belong tager video
        /// </summary>
        /// <param name="videoId">Video Id</param>
        /// <param name="numTake">Number of take (load) images from the DB</param>
        /// <returns></returns>
        public async Task<List<StillCutImage>> GetAllImagesAsync(string videoId, int numTake)
        {

            if (string.IsNullOrWhiteSpace(videoId))
            {
                _logger.LogWarning("VideoId Cannot be null. Return null");
                return null;
            }

            if (numTake <= 0)
            {
                numTake = _appConfig.MaxNumOfTakeImageFromDb;
            }

            List<StillCutImage> unCroppedImages = await _dbContext.StillCutImages
                .AsNoTracking()
                .Include(i => i.TumorPositions)
                .Where(i =>  i.VideoId == videoId).Take(numTake)
                .ToListAsync();

            return unCroppedImages;
        }



        public async Task<string> InsertTumorPosition(string imageId, string userId, List<TumorPosInputModel> tumorPosInputs)
        {

            var tgImage = await _dbContext.StillCutImages
                .Include(i => i.TumorPositions)
                .Where(i => i.Id == imageId)
                .FirstOrDefaultAsync();


            int numRect = tumorPosInputs.Count;

            if(numRect > 0)
            {
                for (int i = 0; i < numRect; i++)
                {
                    tgImage.TumorPositions.Add(new TumorPosition
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImageId = imageId,
                        Order = i,
                        StartX = tumorPosInputs[i].StartX,
                        StartY = tumorPosInputs[i].StartY,
                        EndX = tumorPosInputs[i].EndX,
                        EndY = tumorPosInputs[i].EndY,
                        Width = tumorPosInputs[i].Width,
                        Height = tumorPosInputs[i].Height,
                    });
                }

                tgImage.IsCropComplete = true;
                tgImage.LastUpdateTime = DateTime.Now;
                tgImage.UserId = userId;
            }

            try
            {
                _dbContext.StillCutImages.Attach(tgImage).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"New tumor position has been insert for VideoId: {tgImage.VideoId}");
                return tgImage.VideoId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fail to update database for VideoId: {tgImage.VideoId}");
                return "";
            }

        }

        public async Task<string> UpdateTumorPosition(string imageId, string userId, List<TumorPosInputModel> tumorPosInputs)
        {
            var tgImage = await _dbContext.StillCutImages
                .Include(i => i.TumorPositions)
                .Where(i => i.Id == imageId)
                .FirstOrDefaultAsync();

            // First Clear All Exist Tumor Position
            tgImage.TumorPositions.Clear();

            int numRect = tumorPosInputs.Count;

            if (numRect > 0)
            {
                for (int i = 0; i < numRect; i++)
                {
                    tgImage.TumorPositions.Add(new TumorPosition
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImageId = imageId,
                        Order = i,
                        StartX = tumorPosInputs[i].StartX,
                        StartY = tumorPosInputs[i].StartY,
                        EndX = tumorPosInputs[i].EndX,
                        EndY = tumorPosInputs[i].EndY,
                        Width = tumorPosInputs[i].Width,
                        Height = tumorPosInputs[i].Height,
                    });
                }

                tgImage.IsCropComplete = true;
                tgImage.LastUpdateTime = DateTime.Now;
                tgImage.UserId = userId;
            }

            try
            {
                _dbContext.StillCutImages.Attach(tgImage).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"New tumor position has been insert for VideoId: {tgImage.VideoId}");
                return tgImage.VideoId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fail to update database for VideoId: {tgImage.VideoId}");
                return "";
            }

        }

        public async Task<string> DeleteTumorPosition(string imageId, string userId, string positionId)
        {

            var tgImage = await _dbContext.StillCutImages
                .Include(i => i.TumorPositions)
                .Where(i => i.Id == imageId)
                .FirstOrDefaultAsync();

            int numTumorPosition = tgImage.TumorPositions.Count();
            var tgTumorPosition = tgImage.TumorPositions.Where(i => i.Id == positionId).FirstOrDefault();

            tgImage.TumorPositions.Remove(tgTumorPosition);
            tgImage.LastUpdateTime = DateTime.Now;
            tgImage.UserId = userId;

            if (numTumorPosition == 1)
            {
                tgImage.IsCropComplete = false; // Restore flag
            }

            try
            {
                _dbContext.StillCutImages.Attach(tgImage).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Selected tumor position has been deleted for ImageId: {tgImage.Id}");
                return tgImage.VideoId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fail to delete database for ImageId: {tgImage.Id}");

                return string.Empty;
            }
        }

        public async Task<string> DeleteAllTumorPosition(string imageId, string userId)
        {
            var tgImage = await _dbContext.StillCutImages
                .Include(i => i.TumorPositions)
                .Where(i => i.Id == imageId)
                .FirstOrDefaultAsync();

            tgImage.TumorPositions.Clear();
            tgImage.LastUpdateTime = DateTime.Now;
            tgImage.IsCropComplete = false; // Restore flag
            tgImage.UserId = userId;

            try
            { 
                _dbContext.StillCutImages.Attach(tgImage).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"All tumor position has been deleted for ImageId: {tgImage.Id}");
                return tgImage.VideoId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fail to delete database for ImageId: {tgImage.Id}");

                return string.Empty;
            }
        }

        public async Task<List<TumorPosition>> GetAllTumorPositions(string imageId)
        {
            List<TumorPosition> positions = await _dbContext.TumorPositions.Where(i => i.ImageId == imageId).ToListAsync();
            return positions;
        }


        public string GetImageFilePath(string imageId)
        {
            var imgRelLocation = _dbContext.StillCutImages.AsNoTracking().Where(i => i.Id == imageId).Select(i => i.ImageFileLocation).FirstOrDefault();

            if (imgRelLocation == null)
            {
                return "";
            }

            string filePath = "";

            // Construct Path
            string storageRoot = _appConfig.Storage.StorageRootPath;
            string imageFolder = _appConfig.Storage.StillCutImageFolder;

            filePath = Path.Combine(storageRoot, imageFolder, imgRelLocation);

            // Check Existence
            if (System.IO.File.Exists(filePath))
            {
                _logger.LogInformation($"Provide file : {filePath}");
                return filePath;
            }
            else
            {
                _logger.LogError($"Can't fine image file ({imageId}) in {filePath}");
                return "";
            }
        }




    }
}
