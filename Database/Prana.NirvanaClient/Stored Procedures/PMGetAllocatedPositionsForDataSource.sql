/*                                
Author : Abhilash Katiyar                           
Create Date : 19-Mar-2008                                
Descreption : TO get the Allocated Positions to Fund for a specific DataSource                                
                                
PMGetAllocatedPositionsForDataSource '09/14/2009 12:00:00 AM',224 ,1183                              
Select * from V_FundAllocation        
      
Modified By :Sandeep      
Modified Date : 15 Sept 2009      
Modification : Added one more Parameter (FundID)                            
*/                                
                                
Create Procedure [dbo].[PMGetAllocatedPositionsForDataSource]                              
(                                
 @inputDate datetime,                                          
 @thirdPartyID int,      
 @FundID int=0                                                
)                                
As       
       
If(@FundID=0)      
Begin      
                              
 Select                                 
 VT.OrderSideTagValue as SideTagValue,                                
 T_Side.Side as Side,                                
 VT.Symbol,                                
 VT.TaxLotQty as Quantity,                                
 VT.AvgPrice as AvgPX,                                
 T_CompanyFunds.FundName,                                
 VT.Commission as Commission,                                
 VT.OtherBrokerFees as Fees,                    
 V_SecMasterData.CompanyName,                
 VT.TaxLotQty* VT.AvgPrice*V_SecMasterData.Multiplier as GrossNotionalValue,               
 VT.TaxLotQty* VT.AvgPrice*V_SecMasterData.Multiplier + VT.SideMultiplier*VT.TotalExpenses as NetNotionalValue,                                
 VT.ClearingBrokerFee as ClearingBrokerFee,                                
 VT.SoftCommission as SoftCommission                        
 from V_taxlots VT                                                        
 Inner Join T_CompanyFunds  on T_CompanyFunds.CompanyFundID = VT.FundID                          
 Inner join T_Side on T_Side.SideTagValue=VT.OrderSideTagValue                            
 inner join V_SecMasterData on V_SecMasterData.TickerSymbol=VT.Symbol
 Where datediff(d,VT.AUECLocalDate,@inputDate) = 0                            
 And T_CompanyFunds.CompanyThirdPartyID=@thirdPartyID        
End      
      
Else      
Begin      
 Select                                 
 VT.OrderSideTagValue as SideTagValue,                                
 T_Side.Side as Side,                                
 VT.Symbol,                                
 VT.TaxLotQty as Quantity,                                
 VT.AvgPrice as AvgPX,                                
 T_CompanyFunds.FundName,                                
 VT.Commission as Commission,                                
 VT.OtherBrokerFees as Fees,                    
 V_SecMasterData.CompanyName,                
 VT.TaxLotQty* VT.AvgPrice*V_SecMasterData.Multiplier as GrossNotionalValue,               
 VT.TaxLotQty* VT.AvgPrice*V_SecMasterData.Multiplier + VT.SideMultiplier*VT.TotalExpenses as NetNotionalValue,                                
 VT.ClearingBrokerFee as ClearingBrokerFee,                                
 VT.SoftCommission as SoftCommission    
 from V_taxlots VT                                                         
 Inner Join T_CompanyFunds  on T_CompanyFunds.CompanyFundID = VT.FundID 
 Inner join T_Side on T_Side.SideTagValue=VT.OrderSideTagValue                            
 inner join V_SecMasterData on V_SecMasterData.TickerSymbol=VT.Symbol                  
             
 Where datediff(d,VT.AUECLocalDate,@inputDate) = 0                            
 And T_CompanyFunds.CompanyThirdPartyID=@thirdPartyID And VT.FundID=@FundID         
End                          
                        