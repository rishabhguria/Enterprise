





CREATE    procedure [dbo].[P_DeleteFundDefault]
(

@defaultID varchar(200)

)
as

delete from  T_FundDefault

where DefaultID=@defaultID







