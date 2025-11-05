

CREATE PROCEDURE dbo.P_Test
(
		@tab varchar(50),
		@col varchar(50)
)
AS
--declare @tab1 varchar(50), @col1 varchar(50)
--set @tab1=@tab
--set @col1=@col

--declare @inte int
DECLARE @paramID int, @paramName varchar(50)
--set @inte=5

DECLARE c CURSOR
FOR SELECT ParameterID,Name  FROM T_RoutingLogicParameter -- WHERE title_id=@title_id
OPEN c
FETCH c INTO @paramID, @paramName
WHILE (@@FETCH_STATUS=0) 
	BEGIN	-- Here's a WHILE loop
--while(@inte>0)
--begin


select null, AssetID from T_Asset

FETCH c INTO @paramID, @paramName
--if(@@FETCH_STATUS<>0)-
--begin
--UNION 
--end

--@inte = @inte -1
--FETCH c INTO @paramID, @paramName
	END
CLOSE c
DEALLOCATE c



	RETURN 


