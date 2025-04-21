using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite; // Use the Microsoft.Data.Sqlite namespace
using POSLib.Models;         // Assuming Product model is here

namespace POSLib.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly string _connStr;
        public ProductRepository(string connStr) => _connStr = connStr;

        public List<Product> GetAll()
        {
            var products = new List<Product>();
            // Use SqliteConnection from Microsoft.Data.Sqlite
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            // Create command, explicitly list columns
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, name, price, barcode FROM Products";

            // Use SqliteDataReader
            using var reader = command.ExecuteReader();
            while (reader.Read()) // Loop through all resulting rows
            {
                products.Add(new Product
                {
                    // Use GetOrdinal and typed getters
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Price = reader.GetDouble(reader.GetOrdinal("price")), // SQLite REAL affinity maps well to double
                    Barcode = reader.GetString(reader.GetOrdinal("barcode"))
                });
            }
            return products;
        }

        public Product GetByBarcode(string barcode)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            // Use $ parameter prefix, explicitly list columns
            command.CommandText = "SELECT id, name, price, barcode FROM Products WHERE barcode = $barcode";
            command.Parameters.AddWithValue("$barcode", barcode);

            using var reader = command.ExecuteReader();
            if (reader.Read()) // Check if a product was found
            {
                return new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Price = reader.GetDouble(reader.GetOrdinal("price")),
                    Barcode = reader.GetString(reader.GetOrdinal("barcode"))
                };
            }
            // No product found with that barcode
            return null;
        }

        public void Add(Product product)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            // Use $ parameter prefixes
            command.CommandText = "INSERT INTO Products (name, price, barcode) VALUES ($name, $price, $barcode)";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$price", product.Price);
            command.Parameters.AddWithValue("$barcode", product.Barcode);

            command.ExecuteNonQuery(); // Execute the insert command
        }

        public void Update(Product product)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            // Use $ parameter prefixes
            command.CommandText = "UPDATE Products SET name = $name, price = $price, barcode = $barcode WHERE id = $id";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$price", product.Price);
            command.Parameters.AddWithValue("$barcode", product.Barcode);
            command.Parameters.AddWithValue("$id", product.Id);

            command.ExecuteNonQuery(); // Execute the update command
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            // Use $ parameter prefix
            command.CommandText = "DELETE FROM Products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery(); // Execute the delete command
        }
    }
}