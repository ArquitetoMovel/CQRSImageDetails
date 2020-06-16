using CQRSImageDetails.Repository;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Queries
{
    public class ImagesDetailsQueries : IImagesDetailsQueries
    {
        private readonly RepositoryPostgres _repository;

        public ImagesDetailsQueries()
        {
            _repository = new RepositoryPostgres();
        }


        public Task<IEnumerable<ImageDetails>> GetImageDetails() =>
        Task.Run(() =>
        {
            ImageDetails parseReader(NpgsqlDataReader reader)
            {
                var imgDetails = new ImageDetails
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Path = reader.GetString(2)
                };
                return imgDetails;
            }

            return _repository.SelectImagesdetails<ImageDetails>("SELECT Id, Name, Path FROM tb_images", parseReader);
        });

        public Task<IEnumerable<ImagesDetailsIDs>> GetImageIds() =>
        Task.Run(() =>
        {
            ImagesDetailsIDs parseReader(NpgsqlDataReader reader)
            {
                var imgDetails = new ImagesDetailsIDs
                {
                    Id = reader.GetInt32(0)
                };
                return imgDetails;
            }

            return _repository.SelectImagesdetails<ImagesDetailsIDs>("SELECT id FROM tb_images", parseReader);
        });

        public Task<ImagesDetailsTotal> ImagesDetailsTotal() =>
        Task.Run(() =>
        {
            return new ImagesDetailsTotal { Total = _repository.TotalImagesdetails("SELECT COUNT(1) FROM tb_images") };
        });
    }
}
