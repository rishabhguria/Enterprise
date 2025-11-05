/************************************************    
AUTHOR : RAHUL GUPTA        CREATION DATE : 2012-03-19    
    
It will delete the data for particular Id according to the given options:    
0  -  auecID from T_AUEC    
1  -  exchangeID from T_Exchange    
2  -  currencyID from T_Curency    
3  -  countryID from T_Country    
4  -  stateID from T_State    
    
Since Deletion of such data in our application does not take place too much,    
hence making it simple and consolidated in one procedure only.    
    
    
************************************************/    
CREATE PROCEDURE P_DELETESM_AECCS(    
@ID int,    
@option int    
)      
AS      
      
IF(@option = 0)    
BEGIN     
DELETE FROM T_AUEC WHERE AUECID = @ID    
END    
-----------------------------------------------------------------    
IF(@option = 1)    
BEGIN     
DELETE FROM T_Exchange WHERE ExchangeID = @ID    
END    
-----------------------------------------------------------------    
IF(@option = 2)    
BEGIN     
DELETE FROM T_Currency WHERE CurrencyID = @ID    
END    
    
------------------------------------------------------------------    
IF(@option = 3)    
BEGIN     
DELETE FROM T_Country WHERE CountryID = @ID    
END    
-----------------------------------------------------------------    
IF(@option = 4)    
BEGIN     
DELETE FROM T_State WHERE StateID = @ID    
END 