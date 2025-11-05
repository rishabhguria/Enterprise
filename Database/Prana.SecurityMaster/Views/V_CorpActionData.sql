
--Author : Rajat -- Description : It gives information about all of the corporate actions in the systems saved till now 
CREATE View [dbo].[V_CorpActionData] 
As
SELECT  CaXml.value('(CorpActionID)[1]','uniqueidentifier') AS CorpActionID,   CaXml.value('(CorporateActionType)[1]','int') AS CorpActionTypeId,   
CaXml.value('(EffectiveDate)[1]','datetime') AS EffectiveDate,   CaXml.value('(OrigSymbol)[1]','varchar(100)') AS Symbol,  
 CaXml.value('(NewSymbol)[1]','varchar(100)') AS NewSymbol,   CaXml.value('(CompanyName)[1]','varchar(100)') AS CompanyName,   
CaXml.value('(NewCompanyName)[1]','varchar(100)') AS NewCompanyName,   CaXml.value('(SymbologyID)[1]','int') AS 
SymbologyID,   CaXml.value('(OrigSecQtyRatio)[1]','float') AS OrigSplitQty,   CaXml.value('(NewSecQtyRatio)[1]','float') AS NewSplitQty,  
 CaXml.value('(Factor)[1]','float') AS SplitFactor,   CaXml.value('(DivDeclarationDate)[1]','datetime') AS DivDeclarationDate,   
CaXml.value('(ExDivDate)[1]','datetime') AS ExDivDate,   CaXml.value('(RecordDate)[1]','datetime') AS RecordDate,   
CaXml.value('(DivPayoutDate)[1]','datetime') AS DivPayoutDate,   CaXml.value('(DivRate)[1]','float') AS DivRate,   SM.IsApplied,SM.UTCInsertiontime
 FROM T_SMCorporateactions SM Outer Apply CorporateAction.nodes('/CaFullTable') as T(CaXml)
