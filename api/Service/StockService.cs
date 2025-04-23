using api.Models;
using Dapper;
using System.Data;

public class StockService
{
    private readonly IDbConnection _dbConnection;

    public StockService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    // Executes sql for checking if stock exists (Used in CreateComment)
    public async Task<bool> StockExists(int stockId)
    {
        var sql = "SELECT 1 FROM Stocks WHERE Id = @StockId";

        var result = await _dbConnection.QueryFirstOrDefaultAsync<int?>(sql, new { StockId = stockId });

        return result.HasValue;
    }


    // Executes sql for getting stock by symbol (Used in adding to portfolio)
    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        var sql = "SELECT Id, Symbol, Name FROM Stocks WHERE LOWER(Symbol) = LOWER(@Symbol)";
        return await _dbConnection.QueryFirstOrDefaultAsync<Stock>(sql, new { Symbol = symbol });
    }
}
