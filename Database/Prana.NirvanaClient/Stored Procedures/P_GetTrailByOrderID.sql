CREATE PROC [dbo].[P_GetTrailByOrderID]    
(    
 @Root varchar(50)    
)    
AS    
BEGIN    
    
    
Select     
--sub1.isstaged    
sub2.ClOrderID,    
T_Order.Symbol ,     
sub2.OrigClOrderID ,    
sub2.Quantity,    
sub2.Price,    
T_Order.OrderSideTagValue,    
sub2.Text,    
sub2.AUECLocalDate,    
sub2.OrderTypeTagValue,    
sub2.UserID,    
T_Order.AUECID,
sub2.NirvanaMsgType,
sub2.BorrowBroker,
sub2.BorrowerID,
sub2.ShortRebate
From dbo.T_Sub as sub2    
--join dbo.T_Sub as sub2 on sub1.ClOrderID = sub2.ParentClOrderID     
join T_Order on sub2.ParentClOrderID = T_Order.ParentClOrderID    
 where T_Order.ParentClorderid =   @Root    
    
--Final Query could be sth like the one given below where we will get the fills along with. The problem here is to decide how to shows them as bands.    
/*    
Select sub2.SubClOrderID, sub2.Symbol , sub2.OrigClOrderID , sub2.Quantity, sub2.Price, fills.LastShares , fills.Text , fills.Price,  fills.TransactTime    
From dbo.T_Sub as sub1  join dbo.T_Sub as sub2 on sub1.SubClOrderID = sub2.ParentClOrderID    
 join dbo.T_Fills as fills on sub2.SubClOrderID = fills.ClOrderID     
 where sub1.SubClOrderID = @Root    
    
    
    
*/    
/* SET NOCOUNT ON    
 DECLARE @Sub bigint    
    
     
 SELECT  Symbol ,  Price, Quantity, Side    
   FROM dbo.T_Sub WHERE SubClOrderID = @Root    
 --Select * From dbo.T_Fills where ClOrderID = @Root    
 --PRINT REPLICATE('-', @@NESTLEVEL * 4) + @EmpName    
    
 SET @Sub = (SELECT MIN(SubClOrderID) FROM dbo.T_Sub WHERE OrigClOrderID = @Root)    
    
 WHILE @Sub IS NOT NULL    
 BEGIN    
  EXEC dbo.P_GetTrailByOrderID @Sub    
  SET @Sub = (    
  SELECT MIN(SubClOrderID) FROM dbo.T_Sub WHERE OrigClOrderID = @Root AND SubClOrderID > @Sub)    
 END*/    
END    