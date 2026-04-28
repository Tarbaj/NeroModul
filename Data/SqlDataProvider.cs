using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;
using picturpictur.Models;

namespace picturpictur.Data
{
    public class SqlDataProvider : IImageDataRepository
    {
        private const string ProviderType = "data";
        private readonly string _connectionString;
        private readonly string _databaseOwner;
        private readonly string _objectQualifier;

        public SqlDataProvider()
        {
            var providerConfig = ProviderConfiguration.GetProviderConfiguration(ProviderType);
            var objProvider = (Provider)providerConfig.Providers[providerConfig.DefaultProvider];

            _connectionString = Config.GetConnectionString();
            _databaseOwner = objProvider.Attributes["databaseOwner"];
            _objectQualifier = objProvider.Attributes["objectQualifier"];

            if (!string.IsNullOrEmpty(_databaseOwner)&&!_databaseOwner.EndsWith("."))
            {
                _databaseOwner += ".";
            }
        }

        private string GetFullyQualifiedName(string name)
        {
            return _databaseOwner + _objectQualifier + "pictur_Images_" + name;
        }
        public IEnumerable<UserImage> GetImages(int moduleId)
        {
            var images = new List<UserImage>();

            using (IDataReader reader = SqlHelper.ExecuteReader(
                _connectionString,
                CommandType.StoredProcedure,
                GetFullyQualifiedName("GetImages"),
                new System.Data.SqlClient.SqlParameter("@ModuleId", moduleId)))
            {
                while (reader.Read())
                {
                    images.Add(FillUserImage(reader));
                }
            }
            return images;
        }

        public UserImage GetImage(int imageId)
        {
            UserImage userImage = null;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                _connectionString,
                CommandType.StoredProcedure,
                GetFullyQualifiedName("GetImage"),
                new System.Data.SqlClient.SqlParameter("@ImageId", imageId)))
            {
                if (reader.Read())
                {
                    userImage = FillUserImage(reader);
                }
            }
            return userImage;
        }

        public int AddImage(UserImage userImage)
        {
            object result = SqlHelper.ExecuteScalar(
                _connectionString,
                CommandType.StoredProcedure,
                GetFullyQualifiedName("AddImage"),
                new System.Data.SqlClient.SqlParameter("@FileId", userImage.FileId),
                new System.Data.SqlClient.SqlParameter("@UserId", userImage.UserId),
                new System.Data.SqlClient.SqlParameter("@ModuleId", userImage.ModuleId),
                new System.Data.SqlClient.SqlParameter("@TopColorHex", userImage.TopColorHex));

            return Convert.ToInt32(result);
        }

        public void DeleteImage(int imageId)
        {
            SqlHelper.ExecuteNonQuery(
                _connectionString,
                CommandType.StoredProcedure,
                GetFullyQualifiedName("DeleteImage"),
                new System.Data.SqlClient.SqlParameter("@ImageId", imageId));
        }

        private static UserImage FillUserImage(IDataReader reader)
        {
            return new UserImage
            {
                ImageId = Null.SetNullInteger(reader["ImageId"]),
                FileId = Null.SetNullInteger(reader["FileId"]),
                UserId = Null.SetNullInteger(reader["UserId"]),
                ModuleId = Null.SetNullInteger(reader["ModuleId"]),
                TopColorHex = Null.SetNullString(reader["TopColorHex"]),
                CreatedOnDate = Null.SetNullDateTime(reader["CreatedOnDate"])
            };
        }
    }
}