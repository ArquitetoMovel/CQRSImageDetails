using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Queries
{

    public interface GenericImageDetails { }

    public class ImageDetails : GenericImageDetails
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class ImagesDetailsTotal : GenericImageDetails
    {
        public int Total { get; set; }
    }

    public class ImagesDetailsIDs : GenericImageDetails
    {
        public int Id { get; set; }
    }
}
