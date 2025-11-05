/*          
Author: Ankit          
Date:   15 Jan. 2015        
Desc:  Return only the assets that are held by the client.       
Exec     P_GetAssetsDynamically      
******/                        
CREATE  PROCEDURE dbo.P_GetAssetsDynamically                    
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
Inner Join T_MW_GenericPNL PNL on a.AssetName=PNL.Asset       
Order by A.AssetName      
      
--------------------------------------------------------      
-- IF NO DATA IN MIDDLEWARE FILL ALL ASSETS      
--------------------------------------------------------      
      
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