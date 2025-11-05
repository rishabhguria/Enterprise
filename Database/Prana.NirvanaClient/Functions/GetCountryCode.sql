-- =============================================          
-- Author:  Sandeep Singh          
-- Create date: 08 March 2013          
-- Description: Returns Counrty Code      
--Select [dbo].[GetCountryCode] ('United States')      
-- =============================================          
CREATE FUNCTION [dbo].[GetCountryCode]       
(          
 @CountryName Varchar(50)      
)          
RETURNS  Varchar(10)          
AS          
BEGIN        
        
Declare @CountryCode Varchar(10)         
       
Set @CountryCode = Case @CountryName  --Find out Side multiplier          
       When 'United States'     
    THEN 'US'       
    
       When 'Australia'     
    THEN 'AU'        
    
       When 'Taiwan'     
    THEN 'TW'       
    
       When 'United Kingdom'     
    THEN  'GB'     
      
       When 'Singapore'     
       THEN  'SG'      
                       
       When 'HongKong'     
       THEN  'HK'     
       
       When 'India'     
       THEN 'IN'    
    
    When 'Canada'     
    THEN 'CA'     
      
       When 'France'     
    THEN 'FR'       
     
       When 'Germany'     
    THEN 'EU'      
     
       When 'Italy'     
    THEN  'IT'     
      
       When 'Spain'     
       THEN  'ES'     
                        
       When 'Sweden'     
       THEN  'SE'    
        
       When 'Russia'     
       THEN 'RU'     
        
    When 'Portugal'     
    THEN 'PT'     
      
       When 'Greece'     
    THEN 'GR'    
        
       When 'Mexico'     
    THEN 'MX'     
      
       When 'Norway'     
    THEN  'NO'     
      
       When 'Japan'     
       THEN  'JP'     
                        
       When 'China'     
       THEN  'CN'     
       
       When 'Ireland'     
       THEN 'US'    
    
    When 'South Africa'     
    THEN 'SA'       
    
       When 'Korea'     
    THEN 'KR'        
    
       When 'Netherlands'     
    THEN 'NL'    
       
       When 'Denmark'     
    THEN  'DK'       
    
       When 'Brazil'     
       THEN  'BR'    
                         
       When 'Switzerland'     
       THEN  'CH'      
    
       End      
        
RETURN (@CountryCode)          
          
END 