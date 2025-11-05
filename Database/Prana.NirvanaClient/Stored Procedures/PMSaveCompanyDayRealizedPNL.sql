


CREATE PROCEDURE [dbo].[PMSaveCompanyDayRealizedPNL] (                                                
  @CompanyID int,                                                
  @Date Datetime, 
  @RealizedPNL float
 )  
AS
	SELECT 
		EquityValue 
	FROM 
		PM_CompanyDailyEquityValue
	WHERE 
			companyID = @companyID
			AND 
			DATEADD(day, DATEDIFF(day, 0, Date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0);
			
	IF(@@rowcount = 0)		
	BEGIN
		Declare @yesterdaysEquityValue float; 
		SET @yesterdaysEquityValue  = 
										(SELECT 
													EquityValue 
										FROM 	
													PM_CompanyDailyEquityValue 
											WHERE 
											companyID = @CompanyID 
											AND 
											DATEADD(day, DateDiff(Day, 0, date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0)
										)
								
		IF 	(@yesterdaysEquityValue is null) 
			BEGIN 
				set @yesterdaysEquityValue = (select baseequityvalue from 
															pm_companybaseequityvalue 
														where companyid = @companyID)
			END

		INSERT INTO 
				PM_CompanyDailyEquityValue (Date, ApplicationRealizedPL, CompanyID, EquityValue) 
		VALUES 
				(DATEADD(day, DATEDIFF(day, 0, @date), 0), @RealizedPNL, @CompanyID, ISNULL(@yesterdaysEquityValue, 0) + @RealizedPNL)
	END
	ELSE
		BEGIN
			
			UPDATE PM_CompanyDailyEquityValue 
					SET	ApplicationRealizedPL = ISNULL(ApplicationRealizedPL, 0) + @RealizedPNL , EquityValue = EquityValue+ @RealizedPNL
			WHERE
				companyID = @companyID
			AND 
			DATEADD(day, DATEDIFF(day, 0, Date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0);
		END



