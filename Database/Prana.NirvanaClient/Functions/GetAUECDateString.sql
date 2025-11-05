--Purpose: Returns AUEC Date String depending upon the date passed.
--Parameter: Date
--Bhupesh Bareja

CREATE function [dbo].[GetAUECDateString] (@date datetime)      
returns varchar(max) as      
      
BEGIN 
	DECLARE @auecID int    
	set @auecID=0    
	    
	-- DECLARE @AllAUECDatesString VARCHAR(MAX)    
	-- Set @AllAUECDatesString = ' '    
	 DECLARE @AUECSeparator varchar(1)      
	 DECLARE @DateSeparator varchar(1)    
	 declare @AllAUECDatesStringFinal VARCHAR(MAX)      
	      
	 SET @AUECSeparator = '~'      
	 SET @DateSeparator = '^'    
	 Set @AllAUECDatesStringFinal = ' '      
	    
	    
	DECLARE AUECString_Cursor CURSOR FAST_FORWARD FOR                                                        
	Select                         
	 AUECID    
	 FROM T_AUEC    
	    
	   Open AUECString_Cursor    
	    
	FETCH NEXT FROM AUECString_Cursor INTO    
	@auecID;    
	     
	WHILE @@fetch_status = 0                                                        
	  BEGIN     
	    
	-- select    
	-- @auecID    
	    
	--print '1'    
	--print @auecID      
	 Set @AllAUECDatesStringFinal = @AllAUECDatesStringFinal + CONVERT(varchar, @auecID) + @DateSeparator + CONVERT(varchar, @date, 101) + @AUECSeparator    
	    
	 FETCH NEXT FROM AUECString_Cursor INTO     
	 @auecID ;                                                    
	  END       
	               
	         
	                                   
	CLOSE AUECString_Cursor    
	DEALLOCATE AUECString_Cursor 
	return @AllAUECDatesStringFinal

END
