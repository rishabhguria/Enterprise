
Create PROCEDURE [dbo].[P_SaveThirdPartyEmail]
(
	@EmailId int,
	@EmailName nvarchar(50),
    @MailFrom nvarchar(50),
    @MailTo nvarchar(255),
    @CcTo nvarchar(255),
	@BccTo nvarchar(255), 
    @Smtp nvarchar(255),
    @Port int,
    @UserName nvarchar(50),
    @Password nvarchar(200),
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
  [BccTo] = @BccTo,  
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
		[BccTo], 
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
		@BccTo,
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
