    
/*      
      
Author : Sumit Kakra    
Created: 04 March 2008    
Usage       
declare @AllAUECDatesString varchar(max)    
set @AllAUECDatesString = '1^ 3/5/2008 12:00:00 AM~11^3/6/2008 12:00:00 AM~12^3/7/2008 12:00:00 AM~15^3/8/2008 12:00:00 AM~16^3/9/2008 12:00:00 AM~17^3/10/2008 12:00:00 AM~18^3/11/2008 12:00:00 AM~19^3/12/2008 12:00:00 AM~20^3/13/2008 12:00:00 AM~21^3/14/2008 12:00:00 AM~22^3/15/2008 12:00:00 AM~23^3/16/2008 12:00:00 AM~24^3/17/2008 12:00:00 AM~25^3/18/2008 12:00:00 AM~26^3/19/2008 12:00:00 AM~27^3/20/2008 12:00:00 AM~28^3/21/2008 12:00:00 AM~'    
--Select Items from dbo.Split(@AllAUECDatesString,'~')    
select * from dbo.GetAllAUECDatesFromString(@AllAUECDatesString)     
     
*/      
CREATE FUNCTION [dbo].[GetAllAUECDatesFromString] (    
 @AllAUECDatesString varchar(max)    
)      
RETURNS @AllAUECDates TABLE     
 (    
  AUECID int,    
  CurrentAUECDate DateTime    
 )     
AS      
      
BEGIN      
 DECLARE @AUECSeparator varchar(1)    
 DECLARE @DateSeparator varchar(1)    
    
 SET @AUECSeparator = '~'    
 SET @DateSeparator = '^'    
    
    DECLARE @INDEX INT    
    DECLARE @DATEINDEX INT    
    DECLARE @SLICE varchar(max)    
    DECLARE @AUECIDSLICE varchar(max)    
    DECLARE @DATESLICE varchar(max)    
    -- HAVE TO SET TO 1 SO IT DOESNT EQUAL ZERO FIRST TIME IN LOOP    
    --SELECT @INDEX = 1    
    -- following line added 10/06/04 as null values cause issues    
    --IF @AllAUECDatesString IS NULL RETURN    
    WHILE LEN(@AllAUECDatesString)!=0    
        BEGIN     
         -- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER    
         SELECT @INDEX = CHARINDEX(@AUECSeparator,@AllAUECDatesString)    
         -- NOW PUSH EVERYTHING TO THE LEFT OF IT INTO THE SLICE VARIABLE    
         IF @INDEX !=0    
          SELECT @SLICE = LEFT(@AllAUECDatesString,@INDEX - 1)    
         ELSE    
          SELECT @SLICE = @AllAUECDatesString    
    
   -- With in each AUEC Info separate AUECID and Date and then insert that into resultant table    
         -- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER    
         SELECT @DATEINDEX = CHARINDEX(@DateSeparator,@SLICE)    
         -- NOW PUSH EVERYTHING TO THE LEFT OF IT INTO THE SLICE VARIABLE    
         IF @DATEINDEX !=0    
          SELECT @AUECIDSLICE = LEFT(@SLICE,@DATEINDEX - 1)    
         ELSE    
          SELECT @AUECIDSLICE = @SLICE    
         SELECT @DATESLICE = RIGHT(@SLICE,LEN(@SLICE) - @DATEINDEX)    
         -- PUT THE AUECID AND DATE INTO THE RESULTS SET    
   IF @AUECIDSLICE IS NOT NULL    
          INSERT INTO @AllAUECDates(AUECID, CurrentAUECDate) VALUES(@AUECIDSLICE,@DATESLICE)    
    
         -- CHOP THE ITEM REMOVED OFF THE MAIN STRING    
         SELECT @AllAUECDatesString = RIGHT(@AllAUECDatesString,LEN(@AllAUECDatesString) - @INDEX)    
         -- BREAK OUT IF WE ARE DONE    
         --IF LEN(@AllAUECDatesString) = 0 BREAK    
    END    
    RETURN    
END 