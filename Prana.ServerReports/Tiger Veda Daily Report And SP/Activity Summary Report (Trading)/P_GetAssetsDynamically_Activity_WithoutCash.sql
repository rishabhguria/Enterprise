/*              
Author: Ankit Misra             
Date:   16 Jan. 2015            
Desc:  Return only the assets that are held by the client for Activity Summary Report.           
Exec   P_GetAssetsDynamically_Activity_new   

Modified By sachin mishra
Decs -http://jira.nirvanasolutions.com:8080/browse/PUC-730
Date: 04/09/2015
       
******/                            
Create PROCEDURE dbo.P_GetAssetsDynamically_Activity_WithoutCash         
AS                           
          
Create Table #TempAssets          
(          
AssetID Int,          
AssetName Varchar(100)          
)            
          
Insert Into #TempAssets                 
Select                 
Distinct               
A.AssetID,              
A.AssetName                          
From T_Asset A                           
Inner Join T_MW_Transactions Transactions on a.AssetName=Transactions.Asset    
where a.AssetName <> 'Cash'                       
Order by A.AssetName          
          
--------------------------------------------------------------------------          
-- IF NO DATA IN T_MW_Transactions Table FILL ALL ASSETS          
--------------------------------------------------------------------------          
          
If(Select Count(*) from #TempAssets) = 0          
Begin          
    Insert Into #TempAssets           
 Select           
 AssetID,          
 AssetName          
 From T_Asset          
End          
          
          
Select * from #TempAssets          
          
Drop Table #TempAssets    
    