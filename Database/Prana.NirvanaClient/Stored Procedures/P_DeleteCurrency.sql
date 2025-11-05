
/****** Object:  Stored Procedure dbo.P_DeleteCurrency    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteCurrency

	(
		@currencyID int		
	)
	AS
		Declare @total int

		--Select @total = Count(1) 
		--	From T_AUECCompliance
		--	Where BaseCurrencyID = @currencyID
		--if (@total = 0)
		--begin
		
			--Select @total = Count(1) 
			--From T_AUECCompliance
			--Where OtherCurrencyID = @currencyID
			--if (@total = 0)
			--begin
			
				Select @total = Count(1) 
				From T_CVCurrency
				Where CurrencyID = @currencyID
				if (@total = 0)
				begin
				
				Select @total = Count(1) 
				From T_Company
				Where BaseCurrencyID = @currencyID
					if (@total = 0)
					begin
				
					Select @total = Count(1) 
					From T_CompanyAllCurrencies
					Where CurrencyID = @currencyID
						if (@total = 0)
						begin
			
							DELETE T_Currency
							Where CurrencyID = @currencyID
						end
					end
				end
--			end
--		end
		
		


