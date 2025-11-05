


/****** Object:  Stored Procedure dbo.P_AddUser    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_AddUser
	(
		@F_name varchar(50)
	)
AS 
INSERT T_User (FirstName)
  Values(@F_name)	


