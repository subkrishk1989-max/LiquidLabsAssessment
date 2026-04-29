using LiquidLabsAssessment.Application.Interfaces;
using LiquidLabsAssessment.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LiquidLabsAssessment.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IDbConnection Conn => new SqlConnection(_configuration.GetConnectionString("Default"));
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        const string query = @"
        SELECT
            p.Id,
            p.Name,
            pa.Id AS AttributeId,
            pa.ProductId,
            pa.AttributeName,
            pa.AttributeValue
        FROM Products p
        LEFT JOIN ProductAttributes pa
            ON p.Id = pa.ProductId
        ORDER BY p.Id";

        await using var conn = (SqlConnection)Conn;
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(query, conn);

        await using var reader = await cmd.ExecuteReaderAsync();

        var products = new Dictionary<int, Product>();

        while (await reader.ReadAsync())
        {
            var productId = reader.GetInt32(reader.GetOrdinal("Id"));

            // Create product only once
            if (!products.TryGetValue(productId, out var product))
            {
                product = new Product
                {
                    Id = productId,
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Attributes = new List<ProductAttribute>()
                };

                products.Add(productId, product);
            }

            // Product may not have attributes
            if (!reader.IsDBNull(reader.GetOrdinal("AttributeId")))
            {
                product.Attributes.Add(new ProductAttribute
                {
                    Id = reader.GetInt32(reader.GetOrdinal("AttributeId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    AttributeName = reader.GetString(reader.GetOrdinal("AttributeName")),
                    AttributeValue = reader.IsDBNull(reader.GetOrdinal("AttributeValue"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("AttributeValue"))
                });
            }
        }

        return products.Values;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        const string query = @"
        SELECT 
            p.Id,
            p.Name,
            pa.Id AS AttributeId,
            pa.ProductId,
            pa.AttributeName,
            pa.AttributeValue
        FROM Products p
        LEFT JOIN ProductAttributes pa 
            ON p.Id = pa.ProductId
        WHERE p.Id = @Id";

        await using var conn = (SqlConnection)Conn;
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(query, conn);

        cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = id;

        await using var reader = await cmd.ExecuteReaderAsync();

        Product? product = null;

        while (await reader.ReadAsync())
        {
            if (product == null)
            {
                product = new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Attributes = new List<ProductAttribute>()
                };
            }

            // Product may not have attributes
            if (!reader.IsDBNull(reader.GetOrdinal("AttributeId")))
            {
                product.Attributes.Add(new ProductAttribute
                {
                    Id = reader.GetInt32(reader.GetOrdinal("AttributeId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    AttributeName = reader.GetString(reader.GetOrdinal("AttributeName")),
                    AttributeValue = reader.IsDBNull(reader.GetOrdinal("AttributeValue"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("AttributeValue"))
                });
            }
        }

        return product;
    }

    public async Task InsertAsync(Product product)
    {
        using var conn = Conn;
        conn.Open();

        var cmd = new SqlCommand(
            "INSERT INTO Products(Id,Name) VALUES(@Id,@Name)",
            (SqlConnection)conn);

        cmd.Parameters.AddWithValue("@Id", product.Id);
        cmd.Parameters.AddWithValue("@Name", product.Name);

        await cmd.ExecuteNonQueryAsync();

        foreach (var a in product.Attributes)
        {
            var attrCmd = new SqlCommand(@"
                INSERT INTO ProductAttributes
                (ProductId,AttributeName,AttributeValue)
                VALUES(@ProductId,@Name,@Value)",
                (SqlConnection)conn);

            attrCmd.Parameters.AddWithValue("@ProductId", product.Id);
            attrCmd.Parameters.AddWithValue("@Name", a.AttributeName);
            attrCmd.Parameters.AddWithValue("@Value", a.AttributeValue ?? "");

            await attrCmd.ExecuteNonQueryAsync();
        }
    }
}
