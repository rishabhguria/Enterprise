CREATE proc P_GetInterestRate  
as  
select Period, Rate from T_InterestRate order by Period