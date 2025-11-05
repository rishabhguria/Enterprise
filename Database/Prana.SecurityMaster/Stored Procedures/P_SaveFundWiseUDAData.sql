 
CREATE proc [dbo].[P_SaveFundWiseUDAData]         
        
(                                                                                        
 @Xml varchar(max),                                                                                                                        
 @ErrorMessage varchar(500) output,                                                                                                                                 
 @ErrorNumber int output                                                                                 
)                                                                                        
As
SET @ErrorNumber = 0                                                                                                                
SET @ErrorMessage = 'Success'                                                                                              
                                                                                                        
BEGIN TRY                                                                                                 
                                                                                        
 BEGIN TRAN TRAN1         
                                                                   
DECLARE @handle int                                                                     
exec sp_xml_preparedocument @handle OUTPUT,@Xml           
        
Create TABLE #XmlItem                                                                                                                    
(
FundID int,        
Symbol_pk varchar(100),        
PrimarySymbol varchar(100),
UDAAssetClassID int,
UDASecurityTypeID int,
UDASectorID int,
UDASubSectorID int,
UDACountryID int, 
IsApproved bit,
CreatedBy varchar(100),
ModifiedBy varchar(100),
ApprovedBy varchar(100),
IsDeleted bit                                              
)  

INSERT INTO #XmlItem                                                    
( 
FundID ,        
Symbol_pk ,        
PrimarySymbol,
UDAAssetClassID ,
UDASecurityTypeID ,
UDASectorID ,
UDASubSectorID ,
UDACountryID , 
IsApproved ,
CreatedBy ,
ModifiedBy ,
ApprovedBy,
IsDeleted   
)
Select 
FundID ,        
Symbol_pk ,        
PrimarySymbol,
UDAAssetClassID ,
UDASecurityTypeID ,
UDASectorID ,
UDASubSectorID ,
UDACountryID , 
IsApproved ,
CreatedBy ,
ModifiedBy ,
ApprovedBy,
IsDeleted  

FROM  OPENXML(@handle, '//NewDataSet/Table',3)                                                            
WITH                                                                                    
(           
FundID int,        
Symbol_pk varchar(100),        
PrimarySymbol varchar(100),
UDAAssetClassID int,
UDASecurityTypeID int,
UDASectorID int,
UDASubSectorID int,
UDACountryID int, 
IsApproved bit,
CreatedBy varchar(100),
ModifiedBy varchar(100),
ApprovedBy varchar(100),
IsDeleted bit                           
)

update T_FundWiseUDAData
SET         
-- PrimarySymbol = #XmlItem.PrimarySymbol,            
UDAAssetClassID = #XmlItem.UDAAssetClassID ,
UDASecurityTypeID = #XmlItem.UDASecurityTypeID,
UDASectorID = #XmlItem.UDASectorID,
UDASubSectorID = #XmlItem.UDASubSectorID,
UDACountryID = #XmlItem.UDACountryID,      
ModifiedBy = #XmlItem.ModifiedBy,
ApprovedBy = #XmlItem.ApprovedBy,    
IsApproved = #XmlItem.IsApproved,
ModifiedDate = GETDATE()   
from #XmlItem  
  INNER JOIN T_FundWiseUDAData on T_FundWiseUDAData.PrimarySymbol =  #XmlItem.PrimarySymbol AND 
  T_FundWiseUDAData.FundID =  #XmlItem.FundID where #XmlItem.IsDeleted=0

INSERT INTO T_FundWiseUDAData
(FundID ,        
Symbol_pk ,        
PrimarySymbol,
UDAAssetClassID ,
UDASecurityTypeID ,
UDASectorID ,
UDASubSectorID ,
UDACountryID , 
IsApproved ,
ApprovedBy ,
CreatedBy ,
CreationDate 
)        
SELECT         
FundID ,        
Symbol_pk ,        
PrimarySymbol,
UDAAssetClassID ,
UDASecurityTypeID ,
UDASectorID ,
UDASubSectorID ,
UDACountryID , 
IsApproved ,
ApprovedBy ,
CreatedBy ,
GETDATE()      
 
from #XmlItem where #XmlItem.PrimarySymbol not in 
(Select TF.PrimarySymbol from T_FundWiseUDAData TF where #XmlItem.PrimarySymbol=TF.PrimarySymbol
and #XmlItem.FundID=TF.FundID) and #XmlItem.IsDeleted=0        

Delete FROM T_FundWiseUDAData where T_FundWiseUDAData.PrimarySymbol in 
(Select #XmlItem.PrimarySymbol from #XmlItem where #XmlItem.PrimarySymbol=T_FundWiseUDAData.PrimarySymbol
and #XmlItem.FundID=T_FundWiseUDAData.FundID AND #XmlItem.IsDeleted=1)


COMMIT TRANSACTION TRAN1                       
                                                                                                
 exec  sp_xml_removedocument @handle                       
END TRY                                                                                                
BEGIN CATCH                                                                                      
SET @ErrorMessage = ERROR_MESSAGE();                                                                                                
SET @ErrorNumber = Error_number();                                            
ROLLBACK TRANSACTION TRAN1                                    
END CATCH;

