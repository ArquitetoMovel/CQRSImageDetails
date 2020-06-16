using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Queries
{

    public interface ViewImageDetails { }

    public class ImageDetails : ViewImageDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class ImagesDetailsTotal : ViewImageDetails
    {
        public int Total { get; set; }
    }

    public class ImagesDetailsIDs : ViewImageDetails
    {
        public int Id { get; set; }
    }

    public class ImageDetailsGroupName : ViewImageDetails
    {
        public string Name { get; set; }
        public int Total { get; set; }
    }
}
