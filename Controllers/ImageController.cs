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

namespace picturpictur.Controllers
{
    [DnnHandleError]
    public class ImageController : DnnController
    {
        private readonly ImageProcessor _processor = new ImageProcessor();

        public ActionResult Index()
        {
            var images = ImageManager.Instance.GetItems(ModuleContext.ModuleId, User.UserID);
            return View(images);
        }

        

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                byte[] fileData;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
                file.InputStream.Position = 0;
                string hexColor = _processor.GetTopColor(file.InputStream);

                var userImg = new UserImage
                {
                    ModuleId = ModuleContext.ModuleId,
                    UserId = User.UserID,
                    FileName = file.FileName,
                    ImageData = fileData,
                    TopColorHex = hexColor,
                    CreatedOnDate = DateTime.Now
                };

                ImageManager.Instance.CreateItem(userImg);
            }
            return Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        public ActionResult Delete(int imageId)
        {
            ImageManager.Instance.DeleteItem(imageId, ModuleContext.ModuleId);
            return Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}
