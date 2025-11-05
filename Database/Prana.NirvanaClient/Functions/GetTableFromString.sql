  
CREATE FUNCTION [dbo].[GetTableFromString] (    
 @AllAUECDatesString varchar(max),    
    @seperator1 char(1),    
    @seperator2 char(1)    
    
)      
RETURNS @AllAUECDates TABLE     
 (    
  Column1 varchar(500),    
  Column2 varchar(500)    
 )     
AS      
      
BEGIN       
    DECLARE @INDEX INT    
    DECLARE @DATEINDEX INT    
    DECLARE @SLICE varchar(max)    
    DECLARE @AUECIDSLICE varchar(max)    
    DECLARE @DATESLICE varchar(max)    
    -- HAVE TO SET TO 1 SO IT DOESNT EQUAL ZERO FIRST TIME IN LOOP    
    SELECT @INDEX = 1    
    -- following line added 10/06/04 as null values cause issues    
    IF @AllAUECDatesString IS NULL RETURN    
    WHILE @INDEX !=0    
        BEGIN     
         -- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER    
         SELECT @INDEX = CHARINDEX(@seperator1,@AllAUECDatesString)    
         -- NOW PUSH EVERYTHING TO THE LEFT OF IT INTO THE SLICE VARIABLE    
         IF @INDEX !=0    
          SELECT @SLICE = LEFT(@AllAUECDatesString,@INDEX - 1)    
         ELSE    
          SELECT @SLICE = @AllAUECDatesString    
    
   -- With in each AUEC Info separate AUECID and Date and then insert that into resultant table    
         -- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER    
         SELECT @DATEINDEX = CHARINDEX(@seperator2,@SLICE)    
         -- NOW PUSH EVERYTHING TO THE LEFT OF IT INTO THE SLICE VARIABLE    
         IF @DATEINDEX !=0    
          SELECT @AUECIDSLICE = LEFT(@SLICE,@DATEINDEX - 1)    
         ELSE    
          SELECT @AUECIDSLICE = @SLICE    
         SELECT @DATESLICE = RIGHT(@SLICE,LEN(@SLICE) - @DATEINDEX)    
         -- PUT THE AUECID AND DATE INTO THE RESULTS SET    
   IF @AUECIDSLICE IS NOT NULL    
          INSERT INTO @AllAUECDates(Column1, Column2) VALUES(@AUECIDSLICE,@DATESLICE)    
    
         -- CHOP THE ITEM REMOVED OFF THE MAIN STRING    
         SELECT @AllAUECDatesString = RIGHT(@AllAUECDatesString,LEN(@AllAUECDatesString) - @INDEX)    
         -- BREAK OUT IF WE ARE DONE    
         IF LEN(@AllAUECDatesString) = 0 BREAK    
    END    
    RETURN    
END  