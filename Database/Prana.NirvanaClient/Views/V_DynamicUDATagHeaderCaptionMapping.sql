/******************************************************* 
Created By : Ankit Misra
Description : Extract Header Information for Dynamic UDA Tags
Creation Date : 13 Oct 2015
*******************************************************/   
CREATE VIEW [dbo].[V_DynamicUDATagHeaderCaptionMapping]  
AS
  
Select 
*
From 
[$(SecurityMaster)].dbo.T_UDA_DynamicUDA

