




CREATE procedure [dbo].[P_GetMaxSeqNumber]
as
select max(cast(NirvanaSeqNumber as numeric )) from T_Fills
--11217430003
--select Fills_PK,OrderSeqNumber from T_Fills order by OrderSeqNumber
--11215470554



