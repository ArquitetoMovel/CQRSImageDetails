using CQRSImageDetails.Commands;
using CQRSImageDetails.Queries;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Repository
{
    public class RepositoryPostgres 
    {
        private readonly NpgsqlConnection _pgdbConn;
        public RepositoryPostgres()
        {
            var connString = "Host=localhost;Username=postgres;Password=dbtest1;Database=postgres";
            _pgdbConn= new NpgsqlConnection(connString);
        }

        public bool DeleteImageDetails(int id)
        {
            var commit = true;
            try
            {
                _pgdbConn.Open();
               
                try
                {
                    // Insert some data
                    using (var cmd = new NpgsqlCommand("DELETE FROM tb_images WHERE Id=@id", _pgdbConn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    commit = false;
                }

            }
            finally
            {
                _pgdbConn.Close();
              //  Task.Delay(5000);
            }
            return commit;
        }

        public IEnumerable<T> SelectImagesdetails<T>(string query, Func<NpgsqlDataReader, T> resultModel) 
            where T : ViewImageDetails, new()
        {
            _pgdbConn.Open();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _pgdbConn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return resultModel(reader);
                        }
                    }
                }
            }
            finally
            {
                _pgdbConn.Close();
            }
            
        
        }

        public int TotalImagesdetails(string query)
        {
            _pgdbConn.Open();
            try
            {
                using (var cmd = new NpgsqlCommand(query, _pgdbConn))
                {
                    return Int32.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            finally
            {
                _pgdbConn.Close();
            }
            

        }
        public int ImageID { get;  private set; }
        public bool InsertImageDetails(CreateNewImageCommand createNewImageCommand)
        {
 
            var commit = true;
            try
            {
                _pgdbConn.Open();
                // next id
                var nextId = 0;
                using (var cmd = new NpgsqlCommand("SELECT count(1) FROM tb_images", _pgdbConn))
                {
                    try
                    {
                        nextId = Int32.Parse(cmd.ExecuteScalar().ToString())+1;
                    }
                    catch(NpgsqlException)
                    {
                        commit = false;
                    }
                    catch (Exception e)
                    {
                        nextId = 1;
                    }
                  
                }
                try
                {
                    ImageID = nextId;

                    // Insert some data
                    using (var cmd = new NpgsqlCommand("INSERT INTO tb_images (id,name,path) VALUES (@id,@name,@path)", _pgdbConn))
                    {
                        cmd.Parameters.AddWithValue("id", nextId);
                        cmd.Parameters.AddWithValue("name", createNewImageCommand.Name);
                        cmd.Parameters.AddWithValue("path", createNewImageCommand.Path);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException)
                {
                    commit = false;
                }
                
            }
            finally
            {
                _pgdbConn.Close();
            }
            return commit;
        }
    }
}
