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

namespace picturpictur.Controllers
{
    [DnnHandleError]
    public class ImageController : DnnController
    {
        private readonly ImageService _imageService;
        private readonly ImageProcessor _processor = new ImageProcessor();

        public ImageController()
        {
            _imageService = new ImageService();
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
            //ezen még dolgozni kel !!!!
            return View(new UserImage { CreatedOnDate = DateTime.Today});
        }

        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ActionResult Edit(UserImage userImage)
        {
            try
            {
                _imageService.CreateImage(userImage);
                return RedirectToDefaultRoute();
            }
            catch (ArgumentException)
            {
                return RedirectToDefaultRoute();
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var userImage = _imageService.GetImage(id);
                return View(userImage);
            }
            catch (ArgumentException)
            {
                return RedirectToDefaultRoute();
            }
        }

        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
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
