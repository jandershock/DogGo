using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
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
        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, OwnerId, Breed, Notes, ImageUrl
                                        FROM Dog";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Dog> dogs = new List<Dog>();
                        while (reader.Read())
                        {
                            dogs.Add(new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                                },
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            });
                        }
                        return dogs;
                    }
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, OwnerId, Breed, Notes, ImageUrl
                                        FROM Dog
                                        WHERE Dog.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Dog dog = null;
                        if (reader.Read())
                        {
                            dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                                },
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };
                        }
                        return dog;
                    }
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Dog (Name, OwnerId, Breed, Notes, ImageUrl)
                                        VALUES (@name, @ownerId, @breed, @notes, @imageUrl)";
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerId", dog.Owner.Id);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes == null ? DBNull.Value : dog.Notes);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl == null ? DBNull.Value : dog.ImageUrl);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Dog
                                        SET Name = @name, OwnerId = @ownerId, Breed = @breed, Notes = @notes, ImageUrl = @imageUrl
                                        WHERE Dog.Id = @dogId";
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerId", dog.Owner.Id);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes == null ? DBNull.Value : dog.Notes);
                    cmd.Parameters.AddWithValue("@imageURl", dog.ImageUrl == null ? DBNull.Value : dog.ImageUrl);
                    cmd.Parameters.AddWithValue("@dogId", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDogEntry(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Dog
                                        WHERE Dog.id = @dogId";
                    cmd.Parameters.AddWithValue("@dogId", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
