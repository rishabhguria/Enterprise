CREATE PROCEDURE [dbo].[P_UpdateThirdPartyToleranceProfile]
(
    @thirdPartyToleranceProfileId INT,
    @lastModified DATETIME,
    @matchingField INT,
    @avgPrice FLOAT,
    @commission FLOAT,
    @miscFees FLOAT,
    @netMoney FLOAT
)
AS
BEGIN
    UPDATE T_ThirdPartyToleranceProfile
    SET LastModified = @lastModified,
        MatchingField = @matchingField,
        AvgPrice = @avgPrice,
        Commission = @commission,
        MiscFees = @miscFees,
        NetMoney = @netMoney
    WHERE ThirdPartyToleranceProfileId = @thirdPartyToleranceProfileId
END
GO
