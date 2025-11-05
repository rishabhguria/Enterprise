/********************************************  
  
AUTHOR : RAHUL GUPTA     CREATED ON : 2012-03-28  

MODIFIED BY : RAHUL GUPTA	
MODIFIED DATE : 2013-02-07 
DESCRIPTION : 
Checking T_Exchange table has identity column or not and if it exists then setting IDENTITY_INSERT ON
********************************************/  
          
CREATE PROCEDURE [dbo].[P_SaveSMExchangeDetails]        
 (        
  @ExchangeID int,        
  @FullName varchar(50),        
  @DisplayName varchar(50),         
  @TimeZone varchar(100),         
  @LunchTimeStartTime datetime,         
  @LunchTimeEndTime datetime,         
  @RegularTradingStartTime datetime,         
  @RegularTradingEndTime datetime,             
  @RegularTime int,        
  @LunchTime int,        
  @Country varchar(50),         
  @StateID int,        
  @CountryFlagID int,        
  @LogoID int,        
  @ExchangeIdentifier varchar(10),        
  @TimeZoneOffSet float,        
  @result int        
 )        
AS        
 Declare @total int        
 set @total = 0        
 declare @count int        
 set @count = 0   
 declare @Identity bit
 set @Identity = 0       
         
 Select @total = Count(*)        
 From T_Exchange        
 Where ExchangeID = @ExchangeID        
         
 if(@total > 0)        
 Begin        
  select @count = count(*)        
  from T_Exchange         
  Where (FullName = @FullName OR DisplayName = @DisplayName) AND ExchangeID <> @ExchangeID        
  if(@count = 0)        
  begin        
      
     --Update Data        
     Update T_Exchange        
     Set FullName = @FullName,         
      DisplayName = @DisplayName,         
      TimeZone = @TimeZone,         
      RegularTime = @RegularTime,        
      RegularTradingStartTime = @RegularTradingStartTime,        
      RegularTradingEndTime = @RegularTradingEndTime,         
      LunchTime = @LunchTime,        
      LunchTimeStartTime = @LunchTimeStartTime,         
      LunchTimeEndTime = @LunchTimeEndTime,         
      Country = @Country,         
      StateID = @StateID,        
      CountryFlagID = @CountryFlagID,        
      LogoID = @LogoID,        
      ExchangeIdentifier = @ExchangeIdentifier,        
      TimeZoneOffSet = @TimeZoneOffSet        
     Where ExchangeID = @ExchangeID        
     Set @result = @ExchangeID         
   end             
  end        
  else        
  begin        
     select @count = count(*)        
     from T_Exchange         
     Where FullName = @FullName OR DisplayName = @DisplayName        
             
     if(@count = 0)            
     begin  
      SELECT @Identity = dbo.F_IsIdentityExists('T_Exchange')
	  IF(@Identity = 1)
	  SET IDENTITY_INSERT T_Exchange ON       
      --Insert Data        
      INSERT INTO T_Exchange(ExchangeID,FullName, DisplayName, TimeZone, RegularTime, RegularTradingStartTime,         
       RegularTradingEndTime, LunchTime, LunchTimeStartTime, LunchTimeEndTime, Country, StateID, CountryFlagID,        
       LogoID, ExchangeIdentifier, TimeZoneOffSet)        
      Values(@ExchangeID,@FullName, @DisplayName, @TimeZone, @RegularTime, @RegularTradingStartTime,         
       @RegularTradingEndTime, @LunchTime, @LunchTimeStartTime, @LunchTimeEndTime, @Country, @StateID,        
       @CountryFlagID, @LogoID, @ExchangeIdentifier, @TimeZoneOffSet)        
  end    
end      
       
