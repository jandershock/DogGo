using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
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
        public List<Dog> GetDogsByOwnerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Dog.Id, Dog.[Name], Dog.OwnerId, Dog.Breed, Dog.Notes, Dog.ImageUrl, [Owner].[Name] AS OwnerName
                                        FROM Dog
                                        JOIN [Owner] ON [Owner].Id = Dog.OwnerId
                                        WHERE OwnerId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Dog> dogs = new List<Dog>();
                        while (reader.Read())
                        {
                            bool test = reader.IsDBNull(reader.GetOrdinal("Breed"));
                            dogs.Add(new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                                },
                                Breed = reader.IsDBNull(reader.GetOrdinal("Breed")) ? null : reader.GetString(reader.GetOrdinal("Breed")), //Checks if Breed column is null before assigning value
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")), //Checks if Notes column is null before assigning value
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")), //Checks if ImageUrl is null before assigning value
                            });
                        }
                        return dogs;
                    }
                }
            }
        }
    }
}
