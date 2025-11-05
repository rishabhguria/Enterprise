

CREATE PROCEDURE dbo.P_GetLayoutID
	(
		@LayoutName varchar(50)
	)

AS
	
Declare @LayoutID int 
Set @LayoutID = -1

Select @LayoutID = LayoutID From T_Layout
Where LayoutName = @LayoutName 

if @LayoutID != -1
begin



	Select @LayoutID
			end
		
			

--select @result

