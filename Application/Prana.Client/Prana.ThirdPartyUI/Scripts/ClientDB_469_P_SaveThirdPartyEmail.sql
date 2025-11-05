/****** Object:  StoredProcedure [dbo].[P_SaveThirdPartyEmail]    Script Date: 05/03/2013 11:03:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_SaveThirdPartyEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_SaveThirdPartyEmail]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[P_SaveThirdPartyEmail]
(
	@EmailId int,
	@EmailName nvarchar(50),
    @MailFrom nvarchar(50),
    @MailTo nvarchar(255),
    @CcTo nvarchar(255),
    @Smtp nvarchar(255),
    @Port int,
    @UserName nvarchar(50),
    @Password nvarchar(50),
    @Enabled bit,
    @Subject nvarchar(50),
    @Body nvarchar(255),
    @Priority int,
	@MailType int,
	@SSLEnabled bit
)

AS

Declare @result int
if @EmailId > 0
begin
	UPDATE T_ThirdPartyEmail
	SET 
		[EmailName] = @EmailName,
		[MailFrom]	= @MailFrom,
		[MailTo]	= @MailTo,
		[CcTo]		= @CcTo,
		[SMTP]		= @Smtp,
		[Port]		= @Port,
		[UserName]	= @UserName,
		[Password]	= @Password,
		[Enabled]	= @Enabled,
		[Subject]	= @Subject,
		[Body]		= @Body, 
		[Priority]	= @Priority,
		[MailType]  = @MailType,
		[SSLEnabled]= @SSLEnabled

	WHERE EmailId = @EmailId
	
	Set @result = @EmailId
end
else
begin
	INSERT INTO T_ThirdPartyEmail
    (
		[EmailName],
        [MailFrom],
        [MailTo],
        [CcTo],
        [SMTP],
        [Port],
        [UserName],
        [Password],
        [Enabled],
        [Subject],
        [Body],
        [Priority],
		[MailType],
		[SSLEnabled]
     )
     VALUES
     (
		@EmailName,
        @MailFrom,
        @MailTo,
        @CcTo,
        @Smtp,
        @Port,
        @UserName,
        @Password,
        @Enabled,
        @Subject,
        @Body,
        @Priority,
		@MailType,
		@SSLEnabled
    )
	Set @result = @@Identity
end

Select @result
