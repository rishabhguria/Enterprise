CREATE PROCEDURE [dbo].[P_SaveThirdPartyToleranceProfile]
(
    @thirdPartyBatchId INT,
    @lastModified DATETIME,
    @matchingField INT,
    @avgPrice FLOAT,
    @commission FLOAT,
    @miscFees FLOAT,
    @netMoney FLOAT
)
AS
BEGIN
    DECLARE @ThirdPartyBatchIdCount INT
    SELECT @ThirdPartyBatchIdCount = COUNT(*)
    FROM T_ThirdPartyToleranceProfile
    WHERE ThirdPartyBatchId = @thirdPartyBatchId;

    IF (@ThirdPartyBatchIdCount = 0)
    BEGIN
        INSERT INTO T_ThirdPartyToleranceProfile
        (
            ThirdPartyBatchId,
            LastModified,
            MatchingField,
            AvgPrice,
            Commission,
            MiscFees,
            NetMoney
        )
        VALUES
        (@thirdPartyBatchId, @lastModified, @matchingField, @avgPrice, @commission, @miscFees, @netMoney)
    END
    SELECT @ThirdPartyBatchIdCount
END
GO
