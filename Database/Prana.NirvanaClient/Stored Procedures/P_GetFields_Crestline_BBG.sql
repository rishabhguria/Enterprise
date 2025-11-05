/*
exec [P_GetOpenPos_Crestline_BBG] @thirdPartyID=86,@companyFundIDs=N'1257',
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

CREATE Procedure [dbo].[P_GetFields_Crestline_BBG]                        
(                                 
@ThirdPartyID int,                                            
@CompanyFundIDs varchar(max),                                                                                                                                                                          
@InputDate datetime,                                                                                                                                                                      
@CompanyID int,                                                                                                                                      
@AUECIDs varchar(max),                                                                            
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
@FileFormatID int                                  
)                                  
AS

Create Table #TempFields
(
Field Varchar(50)
)

Insert Into #TempFields (Field)
Values
--('ID_SEDOL1'),
('PX_MID_EOD'),
('PX_OFFICIAL_CLOSE'),
('PX_LAST_EOD'),
('PX_CLOSE_DT'),
('GICS_SECTOR_NAME'),
('DVD_EX_DT'),
('DVD_PAY_DT'),
('DVD_SH_LAST'),
('DVD_TYP_LAST'),
('DVD_CRNCY'),
('EQY_SPLIT_ADJUSTMENT_FACTOR'),
('EQY_SPLIT_DT'),
('SPINOFF_EX_DT'),
('SPINOFF_ADJ_FACTOR_CURR'),
('EQY_DVD_STK_EX_DT_CURR'),
('EQY_DVD_STK_PCT_CURR'),
('EQY_DVD_STK_ADJ_FCT_CURR'),
('EQY_INIT_PO_DT'),
('PX_BID_EOD'),
('PX_ASK_EOD')

Select * From #TempFields

Drop Table #TempFields
