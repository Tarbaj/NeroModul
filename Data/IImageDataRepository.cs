using picturpictur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace picturpictur.Data
{
    public interface IImageDataRepository
    {
        IEnumerable<UserImage> GetImages(int moduleId, int userId);
        UserImage GetImage(int imageId);
        int AddImage(UserImage image);
        void DeleteImage(int imageId);
    }
}
