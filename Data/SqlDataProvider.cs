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
        public (string, string) GetProductBvin(string topColor)
        {
            string bvin = string.Empty;
            string ImageFileSmall = string.Empty;
            try
            {
                using (IDataReader reader = SqlHelper.ExecuteReader(
                _connectionString,
                CommandType.StoredProcedure,
                _databaseOwner + _objectQualifier + "hcc_Product_GetRandomByColorName",
                new System.Data.SqlClient.SqlParameter("@ColorName", topColor)))
                {
                    if (reader.Read())
                    {
                        bvin = Null.SetNullString(reader["bvin"]);
                        ImageFileSmall = Null.SetNullString(reader["ImageFileSmall"]);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return (bvin, ImageFileSmall);
        }
        private string GetFullyQualifiedName(string name)
        {
            return _databaseOwner + _objectQualifier + "pictur_Images_" + name;
        }
        public IEnumerable<UserImage> GetImages(int moduleId, int userId)
        {
            var images = new List<UserImage>();

            using (IDataReader reader = SqlHelper.ExecuteReader(
                _connectionString,
                CommandType.StoredProcedure,
                GetFullyQualifiedName("GetImages"),
                new System.Data.SqlClient.SqlParameter("@ModuleId", moduleId),
                new System.Data.SqlClient.SqlParameter("@UserId", userId)))
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
                new System.Data.SqlClient.SqlParameter("@TopColorHex", userImage.TopColorHex),
                new System.Data.SqlClient.SqlParameter("@TopColor", userImage.TopColor),
                new System.Data.SqlClient.SqlParameter("@AltColorHex", userImage.AltColorHex),
                new System.Data.SqlClient.SqlParameter("@AltColor", userImage.AltColor),
                new System.Data.SqlClient.SqlParameter("@Bvin", userImage.Bvin),
                new System.Data.SqlClient.SqlParameter("@ImageFileSmall", userImage.ImageFileSmall),
                new System.Data.SqlClient.SqlParameter("@AltBvin", userImage.AltBvin),
                new System.Data.SqlClient.SqlParameter("@AltImageFileSmall", userImage.AltImageFileSmall));

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
        public IEnumerable<ColorsApi> GetColors(int moduleId)
        {
            var colors = new List<ColorsApi>();

            using (IDataReader reader = SqlHelper.ExecuteReader(
                _connectionString,
                CommandType.StoredProcedure,
                GetFullyQualifiedName("GetColors"),
                new System.Data.SqlClient.SqlParameter("@ModuleId", moduleId)))
            {
                while (reader.Read())
                {
                    colors.Add(FillColorsApi(reader));
                }
            }
            return colors;
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
                TopColor = Null.SetNullString(reader["TopColor"]),
                AltColorHex = Null.SetNullString(reader["AltColorHex"]),
                AltColor = Null.SetNullString(reader["AltColor"]),
                Bvin = Null.SetNullString(reader["bvin"]),
                ImageFileSmall = Null.SetNullString(reader["ImageFileSmall"]),
                AltBvin = Null.SetNullString(reader["Altbvin"]),
                AltImageFileSmall = Null.SetNullString(reader["AltImageFileSmall"]),
                CreatedOnDate = Null.SetNullDateTime(reader["CreatedOnDate"])
            };
        }
        private static ColorsApi FillColorsApi(IDataReader reader)
        {
            return new ColorsApi
            {
                TopColor = Null.SetNullString(reader["TopColor"]),
                AltColor = Null.SetNullString(reader["AltColor"])
            };
        }
    }
}