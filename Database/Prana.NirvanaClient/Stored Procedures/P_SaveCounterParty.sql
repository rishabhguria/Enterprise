

CREATE PROCEDURE [dbo].[P_SaveCounterParty]
(
		@counterPartyID int,
		@counterPartyFullName varchar(50),
		@shortName varchar(50),
		@address varchar(50),
		@phone varchar(50),
		@fax varchar(50),
		@contactName1 varchar(50),
		@title1 varchar(50),
		@email1 varchar(50),
		@contactName2 varchar(50),
		@title2 varchar(20),
		@email2 varchar(20),
		@counterPartyType int,
		
		@address2 varchar(50),
		@countryID int,
		@stateID int,
		@zip varchar(50),
		@contactName1_LastName varchar(50),
		@contactName1_WorkPhone varchar(50),
		@contactName1_Cell varchar(50),
		@contactName2_LastName varchar(50),
		@contactName2_WorkPhone varchar(50),
		@contactName2_Cell varchar(50),
		
		@city varchar(50),
		@isAlgoBroker bit,
		@result int, 
		@isOTDorEMS bit
	)
AS 
	Declare @total int
	set @total = 0
	declare @count int
	set @count = 0
	Select @total = Count(*)
	From T_CounterParty
	Where CounterPartyID = @counterPartyID
    
if(@total > 0)
begin	
	select @count = count(*)
		from T_CounterParty 
		Where (FullName = @counterPartyFullName OR ShortName = @shortName) AND CounterPartyID <> @counterPartyID
		if(@count = 0)
		begin
			Update T_CounterParty 
			Set FullName = @counterPartyFullName, 
			ShortName = @ShortName, 
			Address = @address, 
			Phone = @phone, 
			Fax = @fax, 
			ContactName1 = @contactName1, 
			Title1 = @title1,
			Email1 = @email1, 
			ContactName2 = @contactName2, 
			Title2 = @title2,
			Email2 = @email2,
			CounterPartyTypeID = @counterPartyType,
			Address2 = @address2,
			CountryID = @countryID,
			StateID = @stateID,
			Zip = @zip,
			ContactName1_LastName = @contactName1_LastName,
			ContactName1_WorkPhone = @contactName1_WorkPhone,
			ContactName1_Cell = @contactName1_Cell,
			ContactName2_LastName = @contactName2_LastName,
			ContactName2_WorkPhone = @contactName2_WorkPhone,
			ContactName2_Cell = @contactName2_Cell,
			City = @city,
			IsAlgoBroker = @isAlgoBroker,
			IsOTDorEMS = @isOTDorEMS
			Where CounterPartyID = @counterPartyID
			Set @result = @counterPartyID
			end
			else
			begin
				Set @result = -1
			end
	end
	else
	begin
		select @count = count(*)
		from T_CounterParty 
		Where FullName = @counterPartyFullName OR ShortName = @shortName
		
		if(@count > 0)
		begin
			
			Set @result = -1
		end
		else
		begin
		INSERT T_CounterParty(FullName, ShortName, Address, Phone, Fax, ContactName1, Title1, Email1,
               ContactName2, Title2, Email2, CounterPartyTypeID, Address2, CountryID, StateID, Zip, 
               ContactName1_LastName, ContactName1_WorkPhone, ContactName1_Cell, ContactName2_LastName, 
               ContactName2_WorkPhone, ContactName2_Cell, City, IsAlgoBroker, IsOTDorEMS)
		Values(@counterPartyFullName, @shortName, @address, @phone, @fax, @contactName1, @title1, @email1,
               @contactName2, @title2, @email2, @counterPartyType, @address2, @countryID, @stateID, @zip, 
               @contactName1_LastName, @contactName1_WorkPhone, @contactName1_Cell, @contactName2_LastName, 
               @contactName2_WorkPhone, @contactName2_Cell, @city, @isAlgoBroker, @isOTDorEMS)  
			
		Set @result = scope_identity()
  
      IF 	@result > 0 -- here @result is new counterparty id
       BEGIN
		   Declare @ReleaseType int
		   Select @Releasetype = CONVERT(int,Preferencevalue) from T_Pranakeyvaluepreferences where preferencekey = 'releaseviewtype'
			IF 	 @Releasetype = 1
			BEGIN
				IF (select  count(*)      
				from T_CompanyCounterParties   
				Where CompanyID = -1 AND counterPartyID = @result) = 0
				BEGIN
					Insert into T_CompanyCounterParties(CompanyID,counterPartyID)
					VALUES(-1,@result)
				END
		END
      END 	
	end
end
select @result
