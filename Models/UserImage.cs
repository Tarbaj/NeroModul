using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace picturpictur.Models
{
    public class UserImage
    {
        public int ImageId { get; set; }

        public int FileId { get; set; }

        public int UserId { get; set; }

        public int ModuleId { get; set; }

        public string TopColorHex { get; set; }

        public string TopColor { get; set; }

        public DateTime CreatedOnDate { get; set; }
    }
}
