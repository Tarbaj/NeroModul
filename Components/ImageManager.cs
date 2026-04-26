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
        void CreateItem(Image t);
        void DeleteItem(int itemId, int moduleId);
        void DeleteItem(Image t);
        IEnumerable<Image> GetItems(int moduleId);
        Image GetItem(int itemId, int moduleId);
        void UpdateItem(Image t);
    }

    internal class ImageManager : ServiceLocator<IImageManager, ImageManager>, IImageManager
    {
        public void CreateItem(Image t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Image>();
                rep.Insert(t);
            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
        }

        public void DeleteItem(Image t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Image>();
                rep.Delete(t);
            }
        }

        public IEnumerable<Image> GetItems(int moduleId)
        {
            IEnumerable<Image> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Image>();
                t = rep.Get(moduleId);
            }
            return t;
        }

        public Image GetItem(int itemId, int moduleId)
        {
            Image t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Image>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(Image t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Image>();
                rep.Update(t);
            }
        }

        protected override System.Func<IImageManager> GetFactory()
        {
            return () => new ImageManager();
        }
    }
}
