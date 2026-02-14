using Dapper;
using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Data;


namespace InventorySystem.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DapperContext _context;

    // 🔥 THIS CONSTRUCTOR IS MANDATORY
    public ProductRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> CreateAsync(Product product)
    {
        var sql = @"
    INSERT INTO dbo.Products
    (
        Name,
        CategoryId,
        SubCategoryId,
        Quantity,
        Price,
        CreatedAt,
        IsActive
    )
    OUTPUT INSERTED.Id
    VALUES
    (
        @Name,
        @CategoryId,
        @SubCategoryId,
        @Quantity,
        @Price,
        GETUTCDATE(),
        1
    );";

        using var connection = _context.CreateConnection();
        connection.Open();

        return await connection.ExecuteScalarAsync<int>(sql, product);
    }


    public async Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pageSize)
    {
        var sql = @"SELECT * FROM Products
                    ORDER BY Id
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Product>(sql,
            new { Skip = (pageNumber - 1) * pageSize, Take = pageSize });
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Product>(
            "SELECT * FROM Products WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var sql = @"UPDATE Products
                    SET Name=@Name, Quantity=@Quantity, Price=@Price
                    WHERE Id=@Id";

        using var connection = _context.CreateConnection();
        return await connection.ExecuteAsync(sql, product) > 0;
    }
}
