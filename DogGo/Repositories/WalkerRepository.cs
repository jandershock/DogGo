using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Walker.Id, Walker.[Name], ImageUrl, Neighborhood.Id as NeighborhoodId, Neighborhood.[Name] as NeighborhoodName
                        FROM Walker
                        JOIN Neighborhood ON Neighborhood.Id = Walker.NeighborhoodId
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                                }
                            };

                            walkers.Add(walker);
                        }

                        return walkers;
                    }
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Walker.Id, Walker.[Name], ImageUrl, NeighborhoodId, Neighborhood.[Name] AS NeighborhoodName 
                        FROM Walker
                        JOIN Neighborhood ON Neighborhood.Id = Walker.NeighborhoodId
                        WHERE Walker.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                                }
                            };

                            return walker;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public List<Walker> GetWalkersByNeighborhood(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Walker.Id AS WalkerId, Walker.[Name], ImageUrl, NeighborhoodId, Neighborhood.[Name] AS NeighborhoodName
                                        FROM Walker
                                        JOIN Neighborhood ON Neighborhood.Id = NeighborhoodId
                                        WHERE NeighborhoodId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            walkers.Add(new Walker()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                                }
                            });
                        }
                        return walkers;
                    }
                }
            }
        }
    }
}