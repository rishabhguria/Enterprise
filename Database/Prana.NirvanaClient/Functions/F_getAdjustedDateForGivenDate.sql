

CREATE Function [dbo].[F_getAdjustedDateForGivenDate]                            
(                  
@Date datetime,              
@Choice int                 
)
RETURNS datetime            
AS            
BEGIN               
         
            
Declare @AdjustedDate datetime               
              
DECLARE @companyID int                    
SET @companyID = (select top 1 CompanyID from T_Company)                    
                    
DECLARE @auecID int                    
SET @auecID = (SELECT DefaultAUECID FROM T_Company WHERE CompanyID = @companyID)                  
            
SELECT @AdjustedDate =             
CASE         
When @Choice = 10 Then      
DATEADD(DAY,-1,dateadd(yy, datediff(yy,0,@Date) ,0))      
When @Choice = 9 Then      
--DATEADD(mm, DATEDIFF(mm,0,getdate()), 0)-1    
DATEADD(MONTH, DATEDIFF(MONTH, -1, @Date)-1, -1)    
When @Choice = 8 Then      
--DATEADD(mm, DATEDIFF(mm,0,getdate())-1, 0)    
DATEADD(m,-1,DATEADD(mm, DATEDIFF(m,0,@Date), 0))    
When @Choice = 7 Then             
dateadd(yy, datediff(yy,0,@Date ),0)            
When @Choice = 6 Then            
dateadd(yy, -1, @Date)              
When @Choice = 5 Then              
dateadd(qq, datediff(qq,0,@Date ),0)              
When @Choice = 4 Then            
dateadd(qq, -1, @Date)              
When @Choice = 3 Then              
dateadd(mm, datediff(mm,0,@Date ),0)              
When @Choice = 2 Then            
dateadd(mm, -1, @Date)            
When @Choice = 1 Then            
dateadd(dd, -1, @Date)              
Else          
@Date          
End      
        
SELECT @AdjustedDate =             
CASE when @Choice = 1 or @Choice = 9 Then            
CONVERT(VARCHAR(10),dbo.AdjustBusinessDays(dateAdd(dd,+1,@AdjustedDate),-1,@auecID) ,110 )  
when @Choice = 2 or @Choice = 3 or @Choice = 4 or @Choice = 5 or @Choice = 6 or @Choice = 7 or @Choice = 8 or @Choice = 10 Then            
CONVERT(VARCHAR(10),dbo.AdjustBusinessDays(dateAdd(dd,-1,@AdjustedDate),+1,@auecID) ,110 )               
Else  
@Date  
End  
  
Return @AdjustedDate          
--Return CONVERT(int,@AdjustedDate)      
End

