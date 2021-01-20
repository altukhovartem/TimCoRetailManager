CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId nvarchar(128), 
	@SaleDate datetime2
AS
Begin
	Set nocount on;
	select Id from dbo.Sale
	where CashierId = @CashierId and SaleDate = @SaleDate;
End

