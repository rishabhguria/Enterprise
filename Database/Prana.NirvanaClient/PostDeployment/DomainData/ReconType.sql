-----------------------------------------------------------------------------------
--Created By:	Pranay
--Date:			2015-09-11
--Purpose:		Insert data into Table T_ReconType
-----------------------------------------------------------------------------------

IF (SELECT COUNT(ReconTypeID) FROM T_ReconType) = 0
BEGIN
INSERT INTO T_ReconType(ReconTypeID,ReconTypeName) VALUES(0,'Transaction');
INSERT INTO T_ReconType(ReconTypeID,ReconTypeName) VALUES(1,'Position');
INSERT INTO T_ReconType(ReconTypeID,ReconTypeName) VALUES(2,'PNL');
INSERT INTO T_ReconType(ReconTypeID,ReconTypeName) VALUES(3,'TaxLot');
END

IF (SELECT COUNT(ReconTypeID_FK) FROM T_ReconGroupingCriteria) = 0
BEGIN
INSERT INTO T_ReconGroupingCriteria(ID,ReconTypeID_FK,GroupingColumns) VALUES(1,0,'Fund,Symbol,Side,MasterFund,Broker,TradeDate');
INSERT INTO T_ReconGroupingCriteria(ID,ReconTypeID_FK,GroupingColumns) VALUES(2,1,'Fund,Symbol,Side,MasterFund,Broker,TradeDate');
INSERT INTO T_ReconGroupingCriteria(ID,ReconTypeID_FK,GroupingColumns) VALUES(3,2,'Fund,Symbol,Side,MasterFund,Broker,TradeDate');
INSERT INTO T_ReconGroupingCriteria(ID,ReconTypeID_FK,GroupingColumns) VALUES(4,3,'Fund,Symbol,Side,MasterFund,Broker,TradeDate');
END
 
