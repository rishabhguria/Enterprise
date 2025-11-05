  
/****** Object:  Stored Procedure dbo.P_DeleteAUECExchange    Script Date: 01/18/2006 8:35:23 PM ******/  
CREATE PROCEDURE [dbo].[P_DeleteAUEC]  
 (  
  @auecID int,  
  @assetID int,  
  @underLyingID int  
 )  
AS  
   
 Declare @total int  
 Declare @ExchangeId int  
 set @ExchangeId=0  
  
    
 -- Select @total = Count(1)  
 --FROM T_Venue V inner join T_CounterPartyVenue CPV on V.VenueID = CPV.VenueID inner join T_AUECExchange AUEC on  V.ExchangeID = AUEC.ExchangeID Where AUEC.AUECExchangeID = @auecExchangeID  
    
 -- if ( @total = 0)  
 -- begin   
    
   Select @total = Count(1)  
   FROM T_CVAUEC Where AUECID = @auecID  
     
   if ( @total = 0)  
   begin  
     
    Select @total = Count(1)  
    FROM T_AUECCommissionRules Where AUECID_FK = @auecID  
      
     if ( @total = 0)  
     begin  
     
      Select @total = Count(1)  
      FROM T_CompanyAUEC Where AUECID = @auecID  
        
       if ( @total = 0)  
       begin  
       
       -- If AUECExchange & AUEC is not referenced anywhere.  
       --Delete AUECExchange & AUEC related details.  
         
           
        --DELETE T_AUECCompliance  
        --Where AUECID = @auecID  
          
        DELETE T_AUECSide  
        Where AUECID = @auecID  
          
        DELETE T_AUECOrderTypes  
        Where AUECID = @auecID  
          
        DELETE T_AUECHolidays  
        Where AUECID = @auecID  

		DELETE T_OtherFeeRules  
        Where AUECID = @auecID

		DELETE T_AUECWeeklyHolidays  
        Where AUECID = @auecID
  
        Select @ExchangeId=ExchangeId from T_AUEC   
        Where AUECID=@auecID  
        if (@ExchangeId>0)  
        Begin  
         Delete T_Venue Where ExchangeId=@ExchangeId  
        End   
          
        DELETE T_AUEC  
        Where AUECID = @auecID                    
       end  
      end  
    end   
--   end  
   