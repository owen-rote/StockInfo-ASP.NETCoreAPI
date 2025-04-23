CREATE PROCEDURE spGetStockById
  @Id INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT Id, Symbol, CompanyName, Purchase, LastDiv, Industry, MarketCap
  FROM Stocks
  WHERE Id = @Id;
END
