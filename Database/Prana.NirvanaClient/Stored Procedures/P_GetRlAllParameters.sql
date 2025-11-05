
CREATE PROCEDURE [dbo].[P_GetRlAllParameters]
	(
		@CompanyID int
	)
AS
	
	
create table #tempTableRLParameters (ParamID int, ParamName varchar(50),  AUEC int, ID int, Name varchar(50), OperatorID int, OperatorName varchar(5))

DECLARE @paramID int, @paramName varchar(50)

DECLARE c CURSOR
FOR SELECT ParameterID, Name  FROM T_RoutingLogicParameter ORDER BY Name

OPEN c
FETCH c INTO @paramID, @paramName

WHILE (@@FETCH_STATUS=0) 
	BEGIN	
		IF(@paramName = 'Exchange')
		BEGIN
			insert into #tempTableRLParameters SELECT     @paramID AS ParamID, @paramName AS ParamName, NULL AS AUEC, AUECID AS ID, DisplayName AS Name, NULL AS operatorID, NULL 
			                                             AS OperatorName
			                       FROM         T_AUEC
			                       ORDER BY DisplayName, AUECID
		
		END ELSE IF(@paramName = 'ExecutionInstructions')
		BEGIN
		insert into #tempTableRLParameters SELECT     T_RoutingLogicParameter.ParameterID, T_RoutingLogicParameter.Name, T_CompanyAUEC.AUECID AS AUEC, 
		                                                         T_ExecutionInstructions.ExecutionInstructionsID AS ID, T_ExecutionInstructions.ExecutionInstructions AS Name, NULL AS operatorID, NULL 
		                                                         AS OperatorName
		                                   FROM         T_AUEC INNER JOIN
		                                                         T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID INNER JOIN
		                                                         T_AUECExecutionInstruction ON T_AUEC.AUECID = T_AUECExecutionInstruction.AUECID INNER JOIN
		                                                         T_ExecutionInstructions ON T_AUECExecutionInstruction.ExecutionInstructionID = T_ExecutionInstructions.ExecutionInstructionsID CROSS JOIN
		                                                         T_RoutingLogicParameter
		                                   WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_RoutingLogicParameter.ParameterID = @paramID)
		                                   ORDER BY T_ExecutionInstructions.ExecutionInstructions, T_CompanyAUEC.AUECID, T_ExecutionInstructions.ExecutionInstructionsID
		
		END /* ELSE IF(@paramName = 'HandlingInstructions')
		BEGIN
		
		END */ELSE IF(@paramName = 'OrderType')
		BEGIN
			insert into #tempTableRLParameters SELECT     T_RoutingLogicParameter.ParameterID, T_RoutingLogicParameter.Name, T_CompanyAUEC.AUECID AS AUEC, T_OrderType.OrderTypesID AS ID, 
			                                                         T_OrderType.OrderTypes AS Name, NULL AS operatorID, NULL AS OperatorName
			                                   FROM         T_AUEC INNER JOIN
			                                                         T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID INNER JOIN
			                                                         T_AUECOrderTypes ON T_AUEC.AUECID = T_AUECOrderTypes.AUECID INNER JOIN
			                                                         T_OrderType ON T_AUECOrderTypes.OrderTypeID = T_OrderType.OrderTypesID CROSS JOIN
			                                                         T_RoutingLogicParameter
			                                   WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_RoutingLogicParameter.ParameterID = @paramID)
			                                   ORDER BY T_OrderType.OrderTypes, T_CompanyAUEC.AUECID, T_OrderType.OrderTypesID

		END  ELSE IF(@paramName = 'Quantity')
		BEGIN
			insert into #tempTableRLParameters SELECT     T_RoutingLogicParameter.ParameterID, T_RoutingLogicParameter.Name, NULL AS Expr1, NULL AS Expr2, NULL AS Expr3, 
			                                             T_Operator.OperatorID AS operatorID, T_Operator.Name AS OperatorName
			                       FROM         T_RoutingLogicParameter CROSS JOIN
			                                             T_Operator
			           WHERE     (T_RoutingLogicParameter.ParameterID = @paramID)
		
		END	 ELSE IF(@paramName = 'Side')
		BEGIN
			insert into #tempTableRLParameters SELECT     T_RoutingLogicParameter.ParameterID, T_RoutingLogicParameter.Name, T_CompanyAUEC.AUECID AS AUEC, T_Side.SideID AS ID, 
			                                                         T_Side.Side AS Name, NULL AS operatorID, NULL AS OperatorName
			                                   FROM         T_AUEC INNER JOIN
			                                                         T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID INNER JOIN
			                                                         T_AUECSide ON T_AUEC.AUECID = T_AUECSide.AUECID INNER JOIN
			                                                         T_Side ON T_AUECSide.SideID = T_Side.SideID CROSS JOIN
			                                                         T_RoutingLogicParameter
			                                   WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_RoutingLogicParameter.ParameterID = @paramID)
			                                   ORDER BY T_Side.Side, T_CompanyAUEC.AUECID, T_Side.SideID
		
		END ELSE IF(@paramName = 'Symbol')
		BEGIN
			insert into #tempTableRLParameters values (@paramID, @paramName, null, null,null,null,null)
			
		END					

		FETCH c INTO @paramID, @paramName
	END
	
CLOSE c
DEALLOCATE c

SELECT * From #tempTableRLParameters

Drop Table #tempTableRLParameters
	/*
	Sandip Vimal: create table #tempTableName (col names here)
Sandip Vimal: while loop
Sandip Vimal: insert into #tempTableName values (ith select statement)
Sandip Vimal: end while lopp
Sandip Vimal: loop*
*/
	
	
	RETURN 

