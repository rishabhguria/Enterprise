CREATE PROCEDURE [dbo].[P_GetNonExistingCurrencyPairs]
AS
Begin

Declare @BaseCurrencyID int
SET @BaseCurrencyID = (Select top 1 BaseCurrencyID from T_Company where CompanyID > 0 )

Select 
Case
When (LeadCurrencyID IS NOT NULL And VsCurrencyID IS NOT NULL)
Then
LeadCurrency 
End As LeadCurrency,
Case
When (LeadCurrencyID IS NOT NULL And VsCurrencyID IS NOT NULL)
Then
VsCurrency 
End As VsCurrency
Into #Temp
From V_Secmasterdata

Delete From #Temp Where LeadCurrency = '' and VsCurrency = '' 

Insert Into #Temp (LeadCurrency, VsCurrency)
Select CurrencyID,@BaseCurrencyID
From V_Secmasterdata
Where ((LeadCurrencyID = 0 Or VsCurrencyID = 0) And CurrencyID != @BaseCurrencyID )

Delete From #Temp Where (LeadCurrency = VsCurrency)

Select CurrencyID,CurrencySymbol Into #C
From T_Currency

Update #Temp
Set #Temp.LeadCurrency = #C.CurrencySymbol
From #Temp,#C
Where  #Temp.LeadCurrency = cast(#C.CurrencyID as varchar)
Update #Temp
Set #Temp.VsCurrency = #C.CurrencySymbol
From #Temp,#C
Where #Temp.VsCurrency = cast(#C.CurrencyID as varchar)

Insert Into #Temp
Select LeadCurrency,'USD'
From #Temp Where LeadCurrency <> 'USD' and VsCurrency <> 'USD'
Insert Into #Temp
Select 'USD',VsCurrency
From #Temp where LeadCurrency <> 'USD' and VsCurrency <> 'USD'

Select Distinct LeadCurrency,VsCurrency Into #TempNew From #Temp

Select FromCurrencyID,ToCurrencyID Into #CSP 
From T_CurrencyStandardPairs
Union
Select ToCurrencyID,FromCurrencyID
From T_CurrencyStandardPairs

Update #TempNew
Set #TempNew.LeadCurrency = #C.CurrencyID
From #C,#TempNew
Where  #C.CurrencySymbol = #TempNew.LeadCurrency
Update #TempNew
Set #TempNew.VsCurrency = #C.CurrencyID
From #C,#TempNew
Where  #C.CurrencySymbol = #TempNew.VsCurrency

Delete t
From #TempNew t
INNER JOIN #CSP
ON t.LeadCurrency = #CSP.FromCurrencyID
AND t.VsCurrency = #CSP.ToCurrencyID

Update #TempNew
Set LeadCurrency=VsCurrency,
VsCurrency=LeadCurrency
Where LeadCurrency < VsCurrency

Update #TempNew
Set #TempNew.LeadCurrency = #C.CurrencySymbol
From #C,#TempNew
Where  #C.CurrencyID = #TempNew.LeadCurrency
Update #TempNew
Set #TempNew.VsCurrency = #C.CurrencySymbol
From #C,#TempNew
Where  #C.CurrencyID = #TempNew.VsCurrency

Select Distinct LeadCurrency,VsCurrency from #TempNew

DROP TABLE #Temp
DROP TABLE #TempNew
DROP TABLE #CSP
DROP TABLE #C

End