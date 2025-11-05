CREATE PROCEDURE [dbo].[P_GetThirdPartyToleranceProfilesCommon]
AS
BEGIN
    SELECT TTPB.thirdpartybatchid,
           TTPB.description,
           TCP.fullname
    FROM t_thirdpartybatch TTPB
        JOIN t_thirdparty TTP
            ON TTPB.thirdpartyid = TTP.thirdpartyid
        JOIN t_counterparty TCP
            ON TCP.counterpartyid = TTP.counterpartyid
    WHERE TTPB.thirdpartytypeid = 3
END
GO
