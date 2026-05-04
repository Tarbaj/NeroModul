/*
' Copyright (c) 2026 Nero
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using picturpictur.Components;
using picturpictur.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.IO;
using DotNetNuke.Security;
using System.Diagnostics;

namespace picturpictur.Controllers
{
    [DnnHandleError]
    public class ImageController : DnnController
    {
        private readonly ImageService _imageService;
        private readonly ImageProcessor _processor;

        public ImageController()
        {
            _imageService = new ImageService();
            _processor = new ImageProcessor();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var images = _imageService.GetUserImages(ModuleContext.ModuleId, User.UserID);
            return View(images);
        }

        [HttpGet]
        public ActionResult Selected(int id)
        {
            try
            {
                var selectedImage = _imageService.GetImage(id);
                return View(selectedImage);
            }
            catch (ArgumentException)
            {
                return RedirectToDefaultRoute();
            }
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View(new UserImage { CreatedOnDate = DateTime.Today});
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase attachment)
        {
            try
            {
                int fileId = 0;
                string topColorHex = "#000000";
                if (attachment != null && attachment.ContentLength > 0)
                {
                    var allowed = new[] { "image/jpeg", "image/png" };
                    if (!Array.Exists(allowed, t => t == attachment.ContentType) || attachment.ContentLength >10*1024*1024)
                    {
                        return RedirectToDefaultRoute();
                    }
                    fileId = _processor.UploadImage(attachment, PortalSettings.PortalId);
                    topColorHex = _processor.GetTopColor(attachment.InputStream);
                }
                var userImage = new UserImage()
                {
                    FileId          = fileId,
                    UserId          = User.UserID,
                    ModuleId        = ModuleContext.ModuleId,
                    TopColorHex     = topColorHex
                };

                _imageService.CreateImage(userImage);
                return RedirectToDefaultRoute();
            }
            catch (ArgumentException)
            {
                return RedirectToDefaultRoute();
            }
        }

        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var userImage = _imageService.GetImage(id);
        //        return View(userImage);
        //    }
        //    catch (ArgumentException)
        //    {
        //        return RedirectToDefaultRoute();
        //    }
        //}

        //[HttpPost]
        //[System.Web.Mvc.ValidateAntiForgeryToken]
        //[ActionName("Delete")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var imageToDelete = _imageService.GetImage(id);
                _processor.DeleteImage(imageToDelete.FileId);
                _imageService.DeleteImage(id);
            }
            catch (ArgumentException ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
            return RedirectToDefaultRoute();
        }
    }
}
