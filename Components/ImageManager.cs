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

using DotNetNuke.Data;
using DotNetNuke.Framework;
using picturpictur.Models;
using System.Collections.Generic;

namespace picturpictur.Components
{
    internal interface IImageManager
    {
        void CreateItem(UserImage t);
        void DeleteItem(int itemId, int moduleId);
        void DeleteItem(UserImage t);
        IEnumerable<UserImage> GetItems(int moduleId, int userId);
        UserImage GetItem(int itemId, int moduleId);
        void UpdateItem(UserImage t);
    }

    internal class ImageManager : ServiceLocator<IImageManager, ImageManager>, IImageManager
    {
        public void CreateItem(UserImage t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<UserImage>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(UserImage t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<UserImage>();
                rep.Delete(t);
            }
        }

        public IEnumerable<UserImage> GetItems(int moduleId, int userId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<UserImage>();
                return rep.Find("WHERE ModuleId = @0 AND UserId = @1", moduleId, userId);
            }
        }

        public UserImage GetItem(int itemId, int moduleId)
        {
            UserImage t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<UserImage>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(UserImage t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<UserImage>();
                rep.Update(t);
            }
        }

        protected override System.Func<IImageManager> GetFactory()
        {
            return () => new ImageManager();
        }
    }
}
