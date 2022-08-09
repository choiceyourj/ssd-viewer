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

    [Authorize(Roles = "Admin, ADMIN")]
    public class AdminController : Controller
    {
        private readonly ITumorImageManager _imgMan;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<SsdUser> _userManager;
        private readonly IAppConfig _appConfig;

        public AdminController(ITumorImageManager imgMan, ILogger<AdminController> logger, UserManager<SsdUser> userManager, IAppConfig appConfig)
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
                else if (videoExist && imageExist)
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
            var videoList = await _imgMan.GetAllVideosAsync(_appConfig.MaxNumOfTakeVideoFromDb);

            if (videoList.Count == 0)
            {
                return vm;
            }

            string videoId = videoList.FirstOrDefault().Id;
            foreach (var item in videoList)
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

            var imageList = await _imgMan.GetAllImagesAsync(videoId, _appConfig.MaxNumOfTakeImageFromDb);

            foreach (var item in imageList)
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
                vm.PositionJsonStr = await BuildPositionToJsonString(vm.WorkImageId);
            }
            return vm;
        }

        private async Task<Models.LabelTool.AnnotationUserViewModel> BuildViewModelAsync(string videoId)
        {
            Models.LabelTool.AnnotationUserViewModel vm = new Models.LabelTool.AnnotationUserViewModel();

            // Do not select query. Use loop to prevent exception
            var videoList = await _imgMan.GetAllVideosAsync(_appConfig.MaxNumOfTakeVideoFromDb);

            foreach (var item in videoList)
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

            var imageList = await _imgMan.GetAllImagesAsync(videoId, _appConfig.MaxNumOfTakeImageFromDb);
            foreach (var item in imageList)
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
                vm.PositionJsonStr = await BuildPositionToJsonString(vm.WorkImageId);
            }

            return vm;
        }

        private async Task<Models.LabelTool.AnnotationUserViewModel> BuildViewModelAsync(string videoId, string imageId)
        {
            Models.LabelTool.AnnotationUserViewModel vm = new Models.LabelTool.AnnotationUserViewModel();

            // Do not select query. Use loop to prevent exception
            var videoList = await _imgMan.GetAllVideosAsync(_appConfig.MaxNumOfTakeVideoFromDb);
            foreach (var item in videoList)
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

            var imageList = await _imgMan.GetAllImagesAsync(videoId, _appConfig.MaxNumOfTakeImageFromDb);
            foreach (var item in imageList)
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
            vm.PositionJsonStr = await BuildPositionToJsonString(vm.WorkImageId);

            return vm;
        }

        private async Task<string> BuildPositionToJsonString(string imageId)
        {
            List<TumorPosition> tumorPositions = new List<TumorPosition>();
            tumorPositions = await _imgMan.GetAllTumorPositions(imageId);

            string jsonStr = string.Empty;
            List<TumorPosInputModel> posVM = new List<TumorPosInputModel>();

            if(tumorPositions != null)
            {
                foreach(var item in tumorPositions)
                {
                    posVM.Add(new TumorPosInputModel
                    {
                        StartX = item.StartX,
                        StartY = item.StartY,
                        EndX = item.EndX,
                        EndY = item.EndY,
                        Height = item.Height,
                        Width = item.Width,
                    });
                }

                jsonStr = JsonSerializer.Serialize(posVM);

                return jsonStr;
            }
            return jsonStr;
        }



        /*
         * 고객사 요청에 의해 Ajax를 사용하지 않고 input tag에 json string을 전달함.
         * 반드시 tumorPosition을 deserialize 해야함.
         */
        [HttpPost("UpdateTumorPosition")]
        public async Task<IActionResult> UpdateTumorPosition(string imageId, string tumorPosition)
        {
            var user = await _userManager.GetUserAsync(User);

            bool isExist = _imgMan.CheckExistenceOfImage(imageId);
            if (!isExist)
            {
                return Redirect("/Error/NotFound");
            }

            List<TumorPosInputModel> tumorPos = new List<TumorPosInputModel>();
            string videoId = string.Empty;
            try
            {
                tumorPos = JsonSerializer.Deserialize<List<TumorPosInputModel>>(tumorPosition);

                videoId = await _imgMan.UpdateTumorPosition(imageId, user.Id, tumorPos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect("/Error/NotFound");
            }

            return Redirect($"/Admin/Index?videoId={videoId}");
        }
    }
}
