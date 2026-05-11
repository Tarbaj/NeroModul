using System;
using System.Collections.Generic;
using System.Linq;
using picturpictur.Data;
using picturpictur.Models;

namespace picturpictur.Components
{
    public class ImageService
    {
        private readonly IImageDataRepository _repository;

        public ImageService(IImageDataRepository repository)
        {
            _repository = repository;
        }

        public ImageService() : this(new SqlDataProvider())
        {
        }

        public IEnumerable<UserImage> GetUserImages(int moduleId, int userId)
        {
            return _repository.GetImages(moduleId, userId)
                .OrderBy(e => e.CreatedOnDate);
        }

        public UserImage GetImage(int imageId)
        {
            var userImage = _repository.GetImage(imageId);
            if (userImage == null)
                throw new ArgumentException($"Nem létezik kép {imageId} azonosítóval.");
            return userImage;
        }

        public int CreateImage(UserImage userImage)
        {
            if (userImage == null) throw new ArgumentNullException(nameof(userImage));
            string bvin = string.Empty;
            string ImageFileSmall = string.Empty;
            (bvin, ImageFileSmall) = _repository.GetProductBvin(userImage.TopColor);
            userImage.Bvin = bvin;
            userImage.ImageFileSmall = ImageFileSmall;
            return _repository.AddImage(userImage);
        }

        public void DeleteImage(int imageId)
        {
            var userImage = _repository.GetImage(imageId);
            if(userImage == null) throw new ArgumentException("A törölni kívánt kép nem található");
            _repository.DeleteImage(imageId);
        }
    }
}