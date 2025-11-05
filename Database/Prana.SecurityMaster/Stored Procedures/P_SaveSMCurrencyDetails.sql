/********************************************  
  
AUTHOR : RAHUL GUPTA     CREATED ON : 2012-03-28  
  
MODIFIED BY : RAHUL GUPTA	
MODIFIED DATE : 2013-02-07 
DESCRIPTION : 
Checking T_Currency table has identity column or not and if it exists then setting IDENTITY_INSERT ON
********************************************/  
      
CREATE PROCEDURE P_SaveSMCurrencyDetails        
 (        
  @currencyID int,        
  @currencyName varchar(50),        
  @currencySymbol varchar(50)        
 )        
AS        
        
 declare @Identity bit
 set @Identity = 0         
declare @total int           
select @total = count(*)        
 from T_Currency          
 Where CurrencyID = @currencyID        
         
 if(@total > 0)            
 begin        
  UPDATE T_Currency         
  Set CurrencyName = @currencyName, CurrencySymbol = @currencySymbol        
  Where CurrencyID = @currencyID        
 end        
else    
begin    
if(@total = 0)        
begin     
  SELECT @Identity = dbo.F_IsIdentityExists('T_Currency')
  IF(@Identity = 1)
  SET IDENTITY_INSERT T_Currency ON       
      
  INSERT INTO T_Currency(CurrencyID, CurrencyName, CurrencySymbol)        
  Values (@currencyID, @currencyName, @currencySymbol)          
end        
end    


