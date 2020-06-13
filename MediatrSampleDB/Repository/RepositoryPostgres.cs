using MediatrSampleDB.Commands;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MediatrSampleDB.Repository
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
            }
            return commit;
        }

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
