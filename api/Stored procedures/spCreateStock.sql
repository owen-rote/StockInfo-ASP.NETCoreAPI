CREATE PROCEDURE spCreateStock
  @Symbol NVARCHAR(50),
  @CompanyName NVARCHAR(100),
  @Purchase DECIMAL(18,2),
  @LastDiv DECIMAL(18,2),
  @Industry NVARCHAR(100),
  @MarketCap BIGINT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO Stocks (Symbol, CompanyName, Purchase, LastDiv, Industry, MarketCap)
  VALUES (@Symbol, @CompanyName, @Purchase, @LastDiv, @Industry, @MarketCap);

  SELECT TOP 1 Id, Symbol, CompanyName, Purchase, LastDiv, Industry, MarketCap
  FROM Stocks
  ORDER BY Id DESC;
END
