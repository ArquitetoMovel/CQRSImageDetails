using MediatrSampleDB.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatrSampleDB
{
    class Program
    {
        //static async Task Main(string[] args)
        //{

        //}

        static void Main(string[] args)
        {
            var repo = new DBPostgres();
            foreach (var item in new DirectoryInfo(@"D:\Fotos Pai").GetFiles("*.jpg"))
            {
                repo.InsertImages(new Commands.CreateNewImageCommand { Name = item.Name, Path = item.FullName });
            }

        }
    }
}
