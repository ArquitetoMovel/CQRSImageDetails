using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Queries
{
    public interface IImagesDetailsQueries
    {
        Task<ImagesDetailsTotal> ImagesDetailsTotal();

        Task<IEnumerable<ImageDetails>> GetImageDetails();

        Task<IEnumerable<ImagesDetailsIDs>> GetImageIds();
    }
}
