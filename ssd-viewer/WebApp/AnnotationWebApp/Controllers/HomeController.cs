using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AnnotationWebApp.Models;
using AnnotationWebApp.Data;
using AnnotationWebApp.Services;
using Microsoft.AspNetCore.Identity;
using AnnotationWebApp.Models.Account;
using System.Text.Json;
using AnnotationWebApp.Models.TumorImage;
using Microsoft.AspNetCore.Authorization;

namespace AnnotationWebApp.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {

        private readonly ITumorImageManager _imgMan;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<SsdUser> _userManager;
        private readonly IAppConfig _appConfig;

        public HomeController(ITumorImageManager imgMan, ILogger<HomeController> logger, UserManager<SsdUser> userManager, IAppConfig appConfig)
        {
            _imgMan = imgMan;
            _logger = logger;
            _userManager = userManager;
            _appConfig = appConfig;

        }


        [HttpGet]
        public async Task<IActionResult> Index(string videoId, string imageId)
        {

            Models.LabelTool.AnnotationUserViewModel vm = new Models.LabelTool.AnnotationUserViewModel();

            if (string.IsNullOrEmpty(videoId) && string.IsNullOrEmpty(imageId))
            {
                // Initial page load
                _logger.LogInformation("Initial data set load.");
                vm = await BuildViewModelAsync();
            }
            else
            {
                // User request
                bool videoExist = _imgMan.CheckExistenceOfVideo(videoId);
                bool imageExist = _imgMan.CheckExistenceOfImage(imageId);

                if (videoExist && !imageExist)
                {
                    _logger.LogInformation($"Dataset load for videoId {videoId}");
                    vm = await BuildViewModelAsync(videoId);
                }
                else if(videoExist && imageExist)
                {
                    _logger.LogInformation($"Dataset load for videoId {videoId} and imageId {imageId}");
                    vm = await BuildViewModelAsync(videoId, imageId);
                }
                else
                {
                    _logger.LogError("User request unknown or invalid videoId and imageId");
                    return NotFound();
                }
            }

            return View(vm);
        }


        private async Task<Models.LabelTool.AnnotationUserViewModel> BuildViewModelAsync()
        {
            Models.LabelTool.AnnotationUserViewModel vm = new Models.LabelTool.AnnotationUserViewModel();

            // Do not select query. Use loop to prevent exception
            var unCompleteVideoList = await _imgMan.GetUnCompleteVideosAsync(_appConfig.MaxNumOfTakeVideoFromDb);

            if (unCompleteVideoList.Count == 0)
            {
                return vm;
            }

            string videoId = unCompleteVideoList.FirstOrDefault().Id;
            foreach (var item in unCompleteVideoList)
            {
                vm.VideoList.Add(new Models.LabelTool.VideoUserViewModel
                {
                    VideoId = item.Id,
                    NumOfTotalFrame = _imgMan.GetTotalNumberOfImages(item.Id),
                    NumOfRemainFrame = _imgMan.GetTotalNumberOfUnCroppedImages(item.Id),
                    Description = (string.IsNullOrEmpty(item.Description) ? "Not Available": item.Description),
                    IsSelected = (videoId == item.Id ? true : false),
                    VideoDisplayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.UploadTime.ToString("yy-dd-MM HH:mm:ss") : item.DisplayName,
                });
            }

            var unCompleteImageList = await _imgMan.GetUnCroppedImagesAsync(videoId, _appConfig.MaxNumOfTakeImageFromDb);
            foreach (var item in unCompleteImageList)
            {
                vm.ImageList.Add(new Models.LabelTool.ImageUserViewModel
                {
                    ImageId = item.Id,
                    VideoId = item.VideoId,
                    IsSelected = false,
                    ImageDisplayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.ImageCreatedTime.ToString("yy-dd-MM HH:mm:ss") : item.DisplayName,
                });
            }

            if(vm.ImageList.Count > 0)
            {
                vm.ImageList.FirstOrDefault().IsSelected = true;

                vm.WorkImageId = vm.ImageList.FirstOrDefault().ImageId;
            }
            return vm;
        }

        private async Task<Models.LabelTool.AnnotationUserViewModel> BuildViewModelAsync(string videoId)
        {
            Models.LabelTool.AnnotationUserViewModel vm = new Models.LabelTool.AnnotationUserViewModel();

            // Do not select query. Use loop to prevent exception
            var unCompleteVideoList = await _imgMan.GetUnCompleteVideosAsync(_appConfig.MaxNumOfTakeVideoFromDb);

            foreach (var item in unCompleteVideoList)
            {
                vm.VideoList.Add(new Models.LabelTool.VideoUserViewModel
                {
                    VideoId = item.Id,
                    NumOfTotalFrame = _imgMan.GetTotalNumberOfImages(item.Id),
                    NumOfRemainFrame = _imgMan.GetTotalNumberOfUnCroppedImages(item.Id),
                    Description = (string.IsNullOrEmpty(item.Description) ? "Not Available" : item.Description),
                    IsSelected = (videoId == item.Id ? true : false),
                    VideoDisplayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.UploadTime.ToString("yy-dd-MM HH:mm:ss") : item.DisplayName,
                });
            }

            var unCompleteImageList = await _imgMan.GetUnCroppedImagesAsync(videoId, _appConfig.MaxNumOfTakeImageFromDb);
            foreach (var item in unCompleteImageList)
            {
                vm.ImageList.Add(new Models.LabelTool.ImageUserViewModel
                {
                    ImageId = item.Id,
                    VideoId = item.VideoId,
                    IsSelected = false,
                    ImageDisplayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.ImageCreatedTime.ToString("yy-dd-MM HH:mm:ss") : item.DisplayName,
                });
            }

            if (vm.ImageList.Count > 0)
            {
                vm.ImageList.FirstOrDefault().IsSelected = true;
                vm.WorkImageId = vm.ImageList.FirstOrDefault().ImageId;
            }

            return vm;
        }

        private async Task<Models.LabelTool.AnnotationUserViewModel> BuildViewModelAsync(string videoId, string imageId)
        {
            Models.LabelTool.AnnotationUserViewModel vm = new Models.LabelTool.AnnotationUserViewModel();

            // Do not select query. Use loop to prevent exception
            var unCompleteVideoList = await _imgMan.GetUnCompleteVideosAsync(_appConfig.MaxNumOfTakeVideoFromDb);
            foreach (var item in unCompleteVideoList)
            {
                vm.VideoList.Add(new Models.LabelTool.VideoUserViewModel
                {
                    VideoId = item.Id,
                    NumOfTotalFrame = _imgMan.GetTotalNumberOfImages(item.Id),
                    NumOfRemainFrame = _imgMan.GetTotalNumberOfUnCroppedImages(item.Id),
                    Description = (string.IsNullOrEmpty(item.Description) ? "Not Available" : item.Description),
                    IsSelected = (videoId == item.Id ? true : false),
                    VideoDisplayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.UploadTime.ToString("yy-dd-MM HH:mm:ss") : item.DisplayName,
                });
            }

            var unCompleteImageList = await _imgMan.GetUnCroppedImagesAsync(videoId, _appConfig.MaxNumOfTakeImageFromDb);
            foreach (var item in unCompleteImageList)
            {
                vm.ImageList.Add(new Models.LabelTool.ImageUserViewModel
                {
                    ImageId = item.Id,
                    VideoId = item.VideoId,
                    IsSelected = (imageId == item.Id ? true : false),
                    ImageDisplayName = string.IsNullOrWhiteSpace(item.DisplayName) ? item.ImageCreatedTime.ToString("yy-dd-MM HH:mm:ss") : item.DisplayName,
                });

            }

            vm.WorkImageId = imageId;

            return vm;
        }

        /*
         * 고객사 요청에 의해 Ajax를 사용하지 않고 input tag에 json string을 전달함.
         * 반드시 tumorPosition을 deserialize 해야함.
         */
        [HttpPost("InsertTumorPosition")]
        public async Task<IActionResult> InsertTumorPosition(string imageId, string tumorPosition)
        {
           var user = await _userManager.GetUserAsync(User);

            bool isExist = _imgMan.CheckExistenceOfImage(imageId);
            if(!isExist)
            {
                return Redirect("/Error/NotFound");
            }

            List<TumorPosInputModel> tumorPos = new List<TumorPosInputModel>();
            string videoId = string.Empty;
            try
            {
                tumorPos = JsonSerializer.Deserialize<List<TumorPosInputModel>>(tumorPosition);

                videoId = await _imgMan.InsertTumorPosition(imageId, user.Id, tumorPos);

                var checkAnnotationWorkDone = await _imgMan.UpdateOrCheckAnnotationDone(videoId);

                if(checkAnnotationWorkDone)
                {
                    _logger.LogInformation($"Annotation work done for video {videoId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect("/Error/NotFound");
            }

            return Redirect($"/Home/Index?videoId={videoId}");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}