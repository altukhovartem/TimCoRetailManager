CREATE PROCEDURE [dbo].[spSale_SaleReport]
AS
BEGIN
	SET nocount on;

	select [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], [u].[FirstName], [u].[LastName], [u].[EmailAddress] 
	from Sale s
	join [User] u on  s.CashierId = u.Id


END
