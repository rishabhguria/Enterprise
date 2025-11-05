namespace Prana.AuditManager.Definitions.Enum
{
    public enum AuditAction
    {
        // Deault value
        NotDefined = 0,

        // For NavLock and NavUnlock
        NavLock = 1,
        NavUnlock = 2,

        // For Company user UI
        UserCreated = 3,
        UserUpdated = 4,
        UserDeleted = 5,
        UserApproved = 6,

        // For CounterParty Venue UI
        CounterPartyVenueCreated = 7,
        CounterPartyVenueUpdated = 8,
        CounterPartyVenueDeleted = 9,
        CounterPartyVenueApproved = 10,

        // For Master Fund UI
        MasterFundCreated = 11,
        MasterFundUpdated = 12,
        MasterFundDeleted = 13,
        MasterFundApproved = 14,

        // For Pricing UI
        PricingRuleCreated = 15,
        PricingRuleUpdated = 16,
        PricingRuleDeleted = 17,
        PricingRuleApproved = 18,

        // For Batch UI
        BatchCreated = 19,
        BatchUpdated = 20,
        BatchDeleted = 21,
        BatchApproved = 22,

        // For Account Group Setup UI
        AccountGroupCreated = 23,
        AccountGroupUpdated = 24,
        AccountGroupDeleted = 25,
        AccountGroupApproved = 26,

        // For Account Setup UI
        AccountCreated = 27,
        AccountUpdated = 28,
        AccountDeleted = 29,
        AccountApproved = 30,

        // For Client Details UI
        ClientCreated = 31,
        ClientUpdated = 32,
        ClientDeleted = 33,
        ClientApproved = 34,

        // For Strategy UI
        MasterStrategyCreated = 35,
        MasterStrategyUpdated = 36,
        MasterStrategyDeleted = 37,
        MasterStrategyApproved = 38,

        // For ThirdParty UI
        ThirdPartyCreated = 39,
        ThirdPartyUpdated = 40,
        ThirdPartyDeleted = 41,
        ThirdPartyApproved = 42,

        // For AUEC UI
        AUECCreated = 43,
        AUECUpdated = 44,
        AUECDeleted = 45,
        AUECApproved = 46,

        StrategyCreated = 47,
        StrategyUpdated = 48,
        StrategyDeleted = 49,
        StrategyApproved = 50
    }
}
