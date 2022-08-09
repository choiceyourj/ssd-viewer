using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using AnnotationWebApp.Services;
using System.Net.Mime;
using AnnotationWebApp.Models.Account;

namespace AnnotationWebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private readonly ITumorImageManager _imgMan;
        private readonly ILogger<DataSetController> _logger;
        private readonly UserManager<SsdUser> _userManager;
        private readonly IAppConfig _appConfig;

        public DataSetController(ITumorImageManager imgMan, ILogger<DataSetController> logger, UserManager<SsdUser> userManager, IAppConfig appConfig)
        {
            _imgMan = imgMan;
            _logger = logger;
            _userManager = userManager;
            _appConfig = appConfig;

        }

        [HttpGet("Videos/UnComplete/{numTake}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetUnCompleteVideos(int numTake)
        {

            var videos = await _imgMan.GetUnCompleteVideosAsync(numTake);

            return Ok(videos);
        }

        [HttpGet("Videos/Complete/{numTake}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetCompleteVideos(int numTake)
        {
            var videos = await _imgMan.GetCompleteVideosAsync(numTake);

            return Ok(videos);
        }

        [HttpGet("Videos/All/{numTake}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetAllVideos(int numTake)
        {
            var videos = await _imgMan.GetAllVideosAsync(numTake);

            return Ok(videos);
        }

        [HttpGet("Images/UnComplete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetUnCompleteImages(string videoId, int numTake)
        {
            var images = await _imgMan.GetUnCroppedImagesAsync(videoId, numTake);

            return Ok(images);
        }

        [HttpGet("Images/Complete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetCompleteImages(string videoId, int numTake)
        {
            var images = await _imgMan.GetCroppedImagesAsync(videoId, numTake);

            return Ok(images);
        }

        [HttpGet("Images/All")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetAllImages(string videoId, int numTake)
        {
            var images = await _imgMan.GetAllImagesAsync(videoId, numTake);

            return Ok(images);
        }


        [HttpGet("Image/{imageId}")]
        public async Task<ActionResult> GetImage(string imageId)
        {
            string filePath = "";

            // Get Releative path from database
            if (!_imgMan.CheckExistenceOfImage(imageId))
            {
                return NotFound();
            }

            filePath = _imgMan.GetImageFilePath(imageId);

            _logger.LogInformation($"Provide file ({filePath}) to client.");

            return PhysicalFile(filePath, "image/jpg");
        }


    }
}
