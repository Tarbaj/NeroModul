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

namespace picturpictur.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {

        //public ActionResult Delete(int itemId)
        //{
        //    ImageManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
        //    return RedirectToDefaultRoute();
        //}

        //public ActionResult Edit(int itemId = -1)
        //{
        //    DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

        //    var userlist = UserController.GetUsers(PortalSettings.PortalId);
        //    var users = from user in userlist.Cast<UserInfo>().ToList()
        //                select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

        //    ViewBag.Users = users;

        //    var item = (itemId == -1)
        //         ? new Image { ModuleId = ModuleContext.ModuleId }
        //         : ImageManager.Instance.GetItem(itemId, ModuleContext.ModuleId);

        //    return View(item);
        //}

        //[HttpPost]
        //[DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        //public ActionResult Edit(Image item)
        //{
        //    if (item.ItemId == -1)
        //    {
        //        item.CreatedByUserId = User.UserID;
        //        item.CreatedOnDate = DateTime.UtcNow;
        //        item.LastModifiedByUserId = User.UserID;
        //        item.LastModifiedOnDate = DateTime.UtcNow;

        //        ImageManager.Instance.CreateItem(item);
        //    }
        //    else
        //    {
        //        var existingItem = ImageManager.Instance.GetItem(item.ImageId, item.ModuleId);
        //        existingItem.LastModifiedByUserId = User.UserID;
        //        existingItem.LastModifiedOnDate = DateTime.UtcNow;
        //        existingItem.ItemName = item.ItemName;
        //        existingItem.ItemDescription = item.ItemDescription;
        //        existingItem.AssignedUserId = item.AssignedUserId;

        //        ImageManager.Instance.UpdateItem(existingItem);
        //    }

        //    return RedirectToDefaultRoute();
        //}

        //[ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        //public ActionResult Index()
        //{
        //    var items = ImageManager.Instance.GetItems(ModuleContext.ModuleId);
        //    return View(items);
        //}
    }
}
