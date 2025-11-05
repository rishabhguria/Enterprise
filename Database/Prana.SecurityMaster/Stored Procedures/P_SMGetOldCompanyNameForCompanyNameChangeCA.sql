CREATE Procedure [dbo].[P_SMGetOldCompanyNameForCompanyNameChangeCA]        
(        
 @corporateActionID uniqueidentifier              
)        
As        
 
declare @symbol_PK varchar(50)
declare @date datetime
 
set @symbol_PK = (select Symbol_PK from T_SMEquityNonHistoryData where CorpActionID = @corporateActionID)
set @date = (select ModifiedAt from T_SMEquityNonHistoryData where CorpActionID = @corporateActionID)

select CompanyName,CorpActionID from T_SMEquityNonHistoryData where Symbol_PK = @symbol_PK and ModifiedAt < @date ORDER BY ModifiedAt DESC