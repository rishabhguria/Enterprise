

CREATE PROCEDURE [dbo].[P_GetAlgoSyntheticReplaceOrderRequest] (  
@startdate varchar(50),  
@userID int  
)  
as  
  
Begin  
(  
select  
AlgoSyntheticReplaceOrders.AlgoSyntheticRPLParent,  
AlgoSyntheticReplaceOrders.ClOrderID,  
AlgoSyntheticReplaceOrders.TransactionTime,  
AlgoSyntheticReplaceOrders.DiscretionOffset,  
AlgoSyntheticReplaceOrders.ExecInst,  
AlgoSyntheticReplaceOrders.HandlingInst,  
AlgoSyntheticReplaceOrders.MsgType,  
AlgoSyntheticReplaceOrders.OrderID,  
AlgoSyntheticReplaceOrders.Side,  
AlgoSyntheticReplaceOrders.OrderStatus,  
AlgoSyntheticReplaceOrders.OrderType,  
AlgoSyntheticReplaceOrders.OrigClOrderID,  
AlgoSyntheticReplaceOrders.PegDiff,  
AlgoSyntheticReplaceOrders.PNP,  
AlgoSyntheticReplaceOrders.Price,  
AlgoSyntheticReplaceOrders.Quantity,  
AlgoSyntheticReplaceOrders.StopPrice,  
AlgoSyntheticReplaceOrders.Symbol,  
AlgoSyntheticReplaceOrders.TargetCompID,  
AlgoSyntheticReplaceOrders.TargetSubID,  
AlgoSyntheticReplaceOrders.TimeInForce,  
AlgoSyntheticReplaceOrders.Venue,  
AlgoSyntheticReplaceOrders.ParentClOrderID,  
AlgoSyntheticReplaceOrders.LocateReqd,  
AlgoSyntheticReplaceOrders.BorrowerID,  
AlgoSyntheticReplaceOrders.ShortRebate,  
AlgoSyntheticReplaceOrders.TradingAccountID,  
AlgoSyntheticReplaceOrders.CompanyUserID,  
AlgoSyntheticReplaceOrders.CounterPartyID,  
AlgoSyntheticReplaceOrders.AuecID,  
AlgoSyntheticReplaceOrders.StagedID,  
AlgoSyntheticReplaceOrders.NirvanaMsgType,  
AlgoSyntheticReplaceOrders.OrderText,  
AlgoSyntheticReplaceOrders.StrategyID,  
AlgoSyntheticReplaceOrders.FundID,  
AlgoSyntheticReplaceOrders.SecurityType,  
AlgoSyntheticReplaceOrders.PutOrCall,  
AlgoSyntheticReplaceOrders.MaturityMonthYear,  
AlgoSyntheticReplaceOrders.StrikePrice,  
AlgoSyntheticReplaceOrders.OpenClose,  
AlgoSyntheticReplaceOrders.ParentClientOrderID,  
AlgoSyntheticReplaceOrders.ClientOrderID,  
AlgoSyntheticReplaceOrders.Cmta,  
AlgoSyntheticReplaceOrders.GiveUp,  
AlgoSyntheticReplaceOrders.UnderlyingSymbol,  
AlgoSyntheticReplaceOrders.ExpirationDate,  
AlgoSyntheticReplaceOrders.SettlementDate,  
AlgoSyntheticReplaceOrders.AlgoStrategyID,  
AlgoSyntheticReplaceOrders.AlgoStrategyParameters  
,  
V_OrderAUECDetails.AssetID  
,  
V_OrderAUECDetails.UnderLyingID,  
T_CountryFlag.CountryFlagImage  
  
 from AlgoSyntheticReplaceOrders  
  
  
left join V_OrderAUECDetails on AlgoSyntheticReplaceOrders.AuecID = V_OrderAUECDetails.AUECID  
left join T_CountryFlag on V_OrderAUECDetails.CountryFlagID = T_CountryFlag.CountryFlagID  
  
left join [T_CompanyAUECClearanceTimeBlotter] as clearance ON AlgoSyntheticReplaceOrders.AuecID = clearance.[CompanyAUECID]   
 -- orders corresponding to the UserID given  
where AlgoSyntheticReplaceOrders.CompanyUserID = @userID   
and   
dbo.GetFormattedDatePart(InsertionTime)= dbo.GetFormattedDatePart(@startdate)  

 --pick orders for this date  
--and  
--(  
-- (  
--  datediff(ms,   
--    GetUTCDate(),  
--    dateadd(dd, datediff(dd , ISNULL(ClearanceTime, GetUTCDATE()), GetUTCDate()),ISNULL(ClearanceTime, GetUTCDate())) ) >= 0  
-- )  
-- OR(  
--  datediff(ms,   
--    GetUTCDate(),  
--  dateadd(dd, datediff(dd , ISNULL(ClearanceTime, GetUTCDATE()), GetUTCDate()),ISNULL(ClearanceTime, GetUTCDate())) ) <= 0  
--  AND  
--  insertiontime  > dateadd(dd, datediff(dd , ISNULL(ClearanceTime,convert(datetime, (substring(convert(varchar, getutcdate(), 121), 0, 11) + ' 00:00:00 AM'), 121) ), GetUTCDate()),ISNULL(ClearanceTime, convert(datetime, (substring(convert(varchar, getut
--cdate(), 121), 0, 11) + ' 00:00:00 AM'), 121) ) )  
--   )  
--)  
--  
----order by   
---- Clorderid,  
---- parentclorderid  
)  
End  
--select * from AlgoSyntheticReplaceOrders
