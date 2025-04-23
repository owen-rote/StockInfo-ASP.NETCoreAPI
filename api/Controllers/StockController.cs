using System.Data;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Helpers;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly string _connectionString;

        public StockController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("Invalid Connection String");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            // Instantiate connection
            // using: ensures connection is automatically disposed when the code block ends
            using var connection = CreateConnection();
            var stocks = await connection.QueryAsync<StockDto>(
                "spGetAllStocks",
                query,
                commandType: CommandType.StoredProcedure
                ).ConfigureAwait(false);

            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = CreateConnection();
            var stock = await connection.QueryFirstOrDefaultAsync<StockDto>(
              "spGetStockById",
              new { Id = id },
              commandType: CommandType.StoredProcedure
            ).ConfigureAwait(false);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var connection = CreateConnection();
            var newStock = await connection.QuerySingleAsync<StockDto>(
              "spCreateStock",
              stockDto,
              commandType: CommandType.StoredProcedure
            ).ConfigureAwait(false);

            return CreatedAtAction(nameof(GetById), new { id = newStock.Id }, newStock);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var connection = CreateConnection();
            var updatedStock = await connection.QueryFirstOrDefaultAsync<StockDto>(
              "spUpdateStock",
              new { Id = id, updateDto.Symbol, updateDto.CompanyName, updateDto.Purchase, updateDto.LastDiv, updateDto.Industry, updateDto.MarketCap },
              commandType: CommandType.StoredProcedure
            ).ConfigureAwait(false);

            if (updatedStock == null)
            {
                return NotFound();
            }

            return Ok(updatedStock);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteAsync(
              "spDeleteStock",
              new { Id = id },
              commandType: CommandType.StoredProcedure)
             .ConfigureAwait(false);

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
