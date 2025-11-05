  
-- =============================================  
-- Author:  <Alan Hau>  
-- Create date: <3/28/2011>  
-- Description: <function to query various Black Scholes outputs>  
-- sample:  Select dbo.F_BS_Greeks('OptionValue', 'Call', 214.08, 165, 19, .0023, .40, .23)  
-- sample:  Select dbo.F_BS_Greeks('Delta', 'Call', 214.08, 165, 19, .0023, .40, .23)  
-- sample:  Select dbo.F_BS_Greeks('Theta', 'Call', 214.08, 165, 19, .0023, .40, .23)  
-- sample:  Select dbo.F_BS_Greeks('Rho', 'Call', 214.08, 165, 19, .0023, .40, .23)  
-- sample:  Select dbo.F_BS_Greeks('Vega', 'Call', 214.08, 165, 19, .0023, .40, .23)  
-- sample:  Select dbo.F_BS_Greeks('Gamma', 'Call', 214.08, 165, 19, .0023, .40, .23)  
-- =============================================  
CREATE FUNCTION [dbo].[F_BS_Greeks_Black76]  
(  
 @Query varchar(20),  
 @PutOrCall varchar(4),  
 @UnderlyingPrice float,  
 @ExercisePrice float,  
 @time float,  
 @Interest float,  
 @Volatility float,  
 @Dividend float  
)  
RETURNS float  
AS  
BEGIN  
 -- Declare the return variable here  
 DECLARE @Result as float  
   
 --Nirvana assumes a calendar with 365 days.  
 select @time = (case when @time < 0.00 then 0.00 else @time end)  / 365.00  
  
 --Proceed with if...Else if on @Query first and then use specific equations for Put/Call cases  
 If @Query = 'OptionValue'  
 Begin  
  Select @result =   
   Case UPPER(@PutOrCall)  
    when 'PUT' then Exp(-@Interest * @time) * (@ExercisePrice * dbo.NormSDist(-dbo.F_BS_dTwo_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)) -  @UnderlyingPrice * dbo.NormSDist(-dbo.F_BS_dOne_Black76(@UnderlyingPrice, 
@ExercisePrice, @time, @Interest, @Volatility, @Dividend)))  
    When 'CALL' then Exp(-@Interest * @time) * (@UnderlyingPrice * dbo.NormSDist(dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)) - @ExercisePrice *
  dbo.NormSDist(dbo.F_BS_dTwo_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)))  
    else NULL  
   End  
 End  
  
 Else if @Query = 'Delta'  
 Begin  
   Select @result =   
   Case UPPER(@PutOrCall)  
    when 'PUT' then  Exp(-@Interest * @time) * (dbo.NormSDist(dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend))-1)  
    When 'CALL' then Exp(-@Interest * @time) * dbo.NormSDist(dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend))  
    else 1  
   End  
 End   
 Else if @Query = 'Theta'  
 Begin  
   Select @result =   
   Case UPPER(@PutOrCall)  
    when 'PUT' then  Exp(-@Interest * @time) * ((-@UnderlyingPrice * @Volatility * dbo.F_BS_NdOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)) / (2.00 * Sqrt(@time)) - @Interest *@UnderlyingPrice 
*  dbo.NormSDist(-dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)) + @Interest* @ExercisePrice 
* dbo.NormSDist(-dbo.F_BS_dTwo_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)))  
    When 'CALL' then Exp(-@Interest * @time) * ((-@UnderlyingPrice * @Volatility * dbo.F_BS_NdOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)) / (2.00 * Sqrt(@time)) + @Interest *@UnderlyingPrice 
*  dbo.NormSDist(dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)) - @Interest* @ExercisePrice 
* dbo.NormSDist(dbo.F_BS_dTwo_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)))  
    else NULL  
   End  
   Select @result = @result / 365.00  
 End  
  
 Else if @Query = 'Rho'  
 Begin  
   Select @result =   
   Case UPPER(@PutOrCall)  
    when 'PUT' then  @time * Exp(-@Interest * @time) * (@UnderlyingPrice*dbo.NormSDist(-dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)-@ExercisePrice*dbo.NormSDist(-dbo.F_BS_dTwo_Black76(@UnderlyingPrice,
 @ExercisePrice, @time, @Interest, @Volatility, @Dividend)))) / 100.00  
    When 'CALL' then -@time * Exp(-@Interest * @time) * (@UnderlyingPrice*dbo.NormSDist(dbo.F_BS_dOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend)-@ExercisePrice*dbo.NormSDist(dbo.F_BS_dTwo_Black76(@UnderlyingPrice, 
@ExercisePrice, @time, @Interest, @Volatility, @Dividend)))) / 100.00  
    else NULL  
   End  
 End  
  
 Else if @Query = 'Vega'  
 Begin  
  Select @result =   
  Case   
   when UPPER(@PutOrCall) = 'PUT' or UPPER(@PutOrCall) = 'CALL' then @UnderlyingPrice * Exp(-@Interest * @time) * Sqrt(@time) * dbo.F_BS_NdOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend) / 100.00  
  Else NULL  
  End  
 End  
  
 Else if @Query = 'Gamma'  
 Begin  
  Select @result =  
  Case   
   when UPPER(@PutOrCall) = 'PUT' or UPPER(@PutOrCall) = 'CALL' then Exp(-@Interest * @time) * (dbo.F_BS_NdOne_Black76(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend) / (@UnderlyingPrice * @Volatility * Sqrt(@time)))  
  Else NULL  
  End  
 End  
   
 Else  
 Select @result = NULL  
   
   
  
 -- Return the result of the function  
 RETURN @result  
  
END  
  
  
  
  