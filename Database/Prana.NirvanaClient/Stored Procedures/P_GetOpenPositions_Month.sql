      
CREATE Procedure [dbo].[P_GetOpenPositions_Month]                                
(                                         
@ThirdPartyID int,                                                    
@CompanyFundIDs varchar(max),                                                                                                                                                                                  
@InputDate DateTime,                                                                                                                                                                              
@CompanyID Int,                                                                                                                                              
@AUECIDs varchar(max),                                                                                    
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                    
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                    
@FileFormatID int                                          
)                                          
AS        
      
--Declare @ThirdPartyID int                                                    
--Declare @CompanyFundIDs varchar(max)      
--Declare @InputDate DateTime                                                                                                                                                                         
--Declare @CompanyID int      
--Declare @AUECIDs varchar(max)      
--Declare @TypeID int      
--Declare @DateType int      
--Declare @FileFormatID int        
      
      

--Set @thirdPartyID=37
--Set @companyFundIDs=N'9,13,12,11,3,8,2,10,1,14,4,5,6'
--Set @inputDate='2020-05-01 00:00:00'
--Set @companyID=7
--Set @auecIDs=N'20,30,63,182,44,34,43,56,53,59,31,54,45,21,60,18,180,1,15,62,73,32,81'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=104     
      
Declare @StartDate DateTime                
Set @StartDate = (dbo.AdjustBusinessDays(DateAdd(d,1,@inputDate),-30,1))     
      
Select distinct Symbol  into #Temp
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                            
(                                                                                                   
   Select max(Taxlot_PK) from PM_Taxlots
   where DateDiff(d,PM_Taxlots.AUECModifiedDate,@Startdate) >= 0                                                                      
   group by taxlotid                                                   
 )  


 select  VSCD.BloombergSYmbol as BBCode  from #Temp temp  inner join  V_SecMasterData_WithUnderlying  VSCD on temp.symbol= VSCD.tickersymbol
      

Drop Table #Temp