using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using picturpictur.Components;
using picturpictur.Models;

namespace picturpictur.Controllers
{
    public class ImageApiController : DnnApiController
    {
        private readonly ImageService _imageService;

        public ImageApiController()
        {
            _imageService = new ImageService();
        }
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetColors(int moduleId)
        {
            try
            {
                var colors = _imageService.GetColors(moduleId);
                return Request.CreateResponse(HttpStatusCode.OK, colors);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Hiba a színek lekérdezésekor");
            }
        }
    }
}
