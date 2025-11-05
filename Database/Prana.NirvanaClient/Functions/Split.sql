
CREATE FUNCTION [dbo].[Split] (@String varchar(max), @Delimiter char(1))
RETURNS @Results TABLE (Items varchar(max))
AS


    BEGIN
    DECLARE @INDEX INT
    DECLARE @SLICE varchar(max)
    -- HAVE TO SET TO 1 SO IT DOESNT EQUAL ZERO FIRST TIME IN LOOP
    SELECT @INDEX = 1
    -- following line added 10/06/04 as null values cause issues
    IF @String IS NULL RETURN
    WHILE @INDEX !=0


        BEGIN	
        	-- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER
        	SELECT @INDEX = CHARINDEX(@Delimiter,@STRING)
        	-- NOW PUSH EVERYTHING TO THE LEFT OF IT INTO THE SLICE VARIABLE
        	IF @INDEX !=0
        		SELECT @SLICE = LEFT(@STRING,@INDEX - 1)
        	ELSE
        		SELECT @SLICE = @STRING
        	-- PUT THE ITEM INTO THE RESULTS SET
        	INSERT INTO @Results(Items) VALUES(@SLICE)
        	-- CHOP THE ITEM REMOVED OFF THE MAIN STRING
        	SELECT @STRING = RIGHT(@STRING,LEN(@STRING) - @INDEX)
        	-- BREAK OUT IF WE ARE DONE
        	IF LEN(@STRING) = 0 BREAK
    END
    RETURN
END
