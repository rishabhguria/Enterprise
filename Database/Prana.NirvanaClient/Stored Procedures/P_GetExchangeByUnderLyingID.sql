
CREATE procedure [dbo].[P_GetExchangeByUnderLyingID]
(@underlyingID int )
as

select ExchangeID,DisplayName from T_AUEC
join T_UnderLying on T_Underlying.UnderLyingID=T_AUEC.UnderLyingID
where T_UnderLying.underlyingID=@underlyingID
