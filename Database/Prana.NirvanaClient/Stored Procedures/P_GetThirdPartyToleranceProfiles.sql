CREATE PROCEDURE [dbo].[P_GetThirdPartyToleranceProfiles]
AS
BEGIN
    SELECT TPTP.ThirdPartyToleranceProfileId,
           TPTP.ThirdPartyBatchId,
           TPB.Description,
           TCP.FullName,
           TP.ThirdPartyName,
           TPTP.LastModified,
           TPTP.MatchingField,
           TPTP.AvgPrice,
           TPTP.Commission,
           TPTP.MiscFees,
           TPTP.NetMoney
    FROM T_ThirdPartyToleranceProfile TPTP
        JOIN T_ThirdPartyBatch TPB
            ON TPTP.ThirdPartyBatchId = TPB.ThirdPartyBatchId
        JOIN T_ThirdParty TP
            ON TPB.ThirdPartyId = TP.ThirdPartyID
        JOIN T_CounterParty TCP
            ON TCP.CounterPartyID = TP.CounterPartyID
    WHERE TPB.ThirdPartyTypeId = 3
END
GO
