CREATE PROCEDURE spGetAllStocks
  @Symbol NVARCHAR(50) = NULL,
  @CompanyName NVARCHAR(100) = NULL,
  @SortBy NVARCHAR(50) = NULL,
  @IsDescending BIT = 0,
  @PageNumber INT = 1,
  @PageSize INT = 10
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
  DECLARE @SortColumn NVARCHAR(50) = ISNULL(@SortBy, 'Id');
  DECLARE @OrderDirection NVARCHAR(4) = CASE WHEN @IsDescending = 1 THEN 'DESC' ELSE 'ASC' END;

  DECLARE @Sql NVARCHAR(MAX) = '
    SELECT Id, Symbol, CompanyName, Purchase, LastDiv, Industry, MarketCap
    FROM Stocks
    WHERE (@Symbol IS NULL OR Symbol LIKE ''%'' + @Symbol + ''%'')
      AND (@CompanyName IS NULL OR CompanyName LIKE ''%'' + @CompanyName + ''%'')
    ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @OrderDirection + '
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
  ';

  EXEC sp_executesql
    @Sql,
    N'@Symbol NVARCHAR(50), @CompanyName NVARCHAR(100), @Offset INT, @PageSize INT',
    @Symbol, @CompanyName, @Offset, @PageSize;
END
