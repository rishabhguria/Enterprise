<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<!--TaxLotStateID=0 means Allocated i.e. Trade comes first time -->
	<!--TaxLotStateID=1 means Sent i.e. Trade sent to PB -->
	<!--TaxLotStateID=2 means Amended i.e. Trade amended after sending to PB -->
	<!--TaxLotStateID=3 means Deleted i.e. Trade unallocated or deleted after sending to PB -->
	<!--TaxLotStateID=4 means Ignore i.e.we can set a trade to ignore, it will come in the different tab-->
	<!--TranCode = ';'  means previous day buy transaction closes with today's sell transaction-->
	<xsl:template match="/">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="//ThirdPartyFlatFileDetail">
				<xsl:choose>
					<xsl:when test ="TaxLotStateID=2 and TranCode != ';' ">
						<xsl:choose>
							<xsl:when test ="(TranID != '' and TranID != '*' and InternalTranID != '' and InternalTranID != '*')">
								<ThirdPartyFlatFileDetail>
									<!-- system inetrnal use-->
									<TaxLotState>
										<xsl:value-of select ="'Deleted'"/>
									</TaxLotState>									
									<!--<TaxLotState>
										<xsl:choose>
											<xsl:when test ="TaxLotStateID=0">
												<xsl:value-of select ="'Allocated'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=1">
												<xsl:value-of select ="'Sent'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=2">
												<xsl:value-of select ="'Amemded'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=3">
												<xsl:value-of select ="'Deleted'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=4">
												<xsl:value-of select ="'Ignore'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select ="'Allocated'"/>
											</xsl:otherwise>
										</xsl:choose>
									</TaxLotState>-->
									
									<!-- system inetrnal use-->
									<RowHeader>
										<xsl:value-of select ="'false'"/>
									</RowHeader>

									<!--<TaxLot>
								<xsl:value-of select="Level1AllocationID"/>
							</TaxLot>-->

									<!-- COL1-->
									<PortfolioCode>
										<xsl:value-of select="PortfolioCode"/>
									</PortfolioCode>
									<!-- COL2-->
									<TranCode>
										<xsl:choose>
											<xsl:when test ="(TranID != '' and TranID != '*') or (InternalTranID != '' and InternalTranID != '*')">
												<xsl:value-of select="translate(TranCode,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="';'"/>
											</xsl:otherwise>
										</xsl:choose>
									</TranCode>
									<!-- COL3-->
									<Comment>
										<xsl:value-of select="''"/>
									</Comment>
									<!-- COL4-->
									<SecType>
										<xsl:value-of select="SecType"/>
									</SecType>
									<!-- COL5-->
									<SecuritySymbol>
										<xsl:value-of select="SecuritySymbol"/>
									</SecuritySymbol>
									<!-- COL6-->
									<TradeDate>
										<xsl:value-of select="''"/>
									</TradeDate>
									<!-- COL7-->
									<SettleDate>
										<xsl:value-of select="''"/>
									</SettleDate>
									<!-- COL8-->
									<OriginalCostDate>
										<xsl:value-of select="''"/>
									</OriginalCostDate>
									<!-- COL9-->
									<Quantity>
										<xsl:value-of select="''"/>
									</Quantity>
									<!-- COL10-->
									<CloseMath>
										<xsl:value-of select="''"/>
									</CloseMath>
									<!-- COL11-->
									<VersusDate>
										<xsl:value-of select="''"/>
									</VersusDate>
									<!-- COL12-->
									<SourceType>
										<xsl:value-of select="''"/>
									</SourceType>
									<!-- COL13-->
									<SourceSymbol>
										<xsl:value-of select="''"/>
									</SourceSymbol>
									<!-- COL14-->
									<TradeDateFXRate>
										<xsl:value-of select="''"/>
									</TradeDateFXRate>
									<!-- COL15-->
									<SettleDateFXRate>
										<xsl:value-of select="''"/>
									</SettleDateFXRate>
									<!-- COL16-->
									<OriginalFXRate>
										<xsl:value-of select="''"/>
									</OriginalFXRate>
									<!-- COL17-->
									<MarkToMarket>
										<xsl:value-of select="''"/>
									</MarkToMarket>
									<!-- COL18-->
									<TradeAmount>
										<xsl:value-of select="''"/>
									</TradeAmount>
									<!-- COL19-->
									<OriginalCost>
										<xsl:value-of select="''"/>
									</OriginalCost>
									<!-- COL20-->
									<Comment1>
										<xsl:value-of select="''"/>
									</Comment1>
									<!-- COL21-->
									<WithholdingTax>
										<xsl:value-of select="''"/>
									</WithholdingTax>
									<!-- COL22-->
									<Exchange>
										<xsl:value-of select="''"/>
									</Exchange>
									<!-- COL23-->
									<ExchangeFee>
										<xsl:value-of select="''"/>
									</ExchangeFee>
									<!-- COL24-->
									<commission>
										<xsl:value-of select="''"/>
									</commission>
									<!-- COL25-->
									<Broker>
										<xsl:value-of select="''"/>
									</Broker>
									<!-- COL26-->
									<ImpliedComm>
										<xsl:value-of select="''"/>
									</ImpliedComm>
									<!-- COL27-->
									<OtherFees>
										<xsl:value-of select="''"/>
									</OtherFees>
									<!-- COL28-->
									<CommPurpose>
										<xsl:value-of select="''"/>
									</CommPurpose>
									<!-- COL29-->
									<Pledge>
										<xsl:value-of select="''"/>
									</Pledge>
									<!-- COL30-->
									<LotLocation>
										<xsl:value-of select="''"/>
									</LotLocation>
									<!-- COL31-->
									<DestPledge>
										<xsl:value-of select="''"/>
									</DestPledge>
									<!-- COL32-->
									<DestLotLocation>
										<xsl:value-of select="''"/>
									</DestLotLocation>
									<!-- COL33-->
									<OriginalFace>
										<xsl:value-of select="''"/>
									</OriginalFace>
									<!-- COL34-->
									<YieldOnCost>
										<xsl:value-of select="''"/>
									</YieldOnCost>
									<!-- COL35-->
									<DurationOnCost>
										<xsl:value-of select="''"/>
									</DurationOnCost>
									<!-- COL36-->
									<UserDef1>
										<xsl:value-of select="''"/>
									</UserDef1>
									<!-- COL37-->
									<UserDef2>
										<xsl:value-of select="''"/>
									</UserDef2>
									<!-- COL38-->
									<UserDef3>
										<xsl:value-of select="''"/>
									</UserDef3>
									<!-- COL39-->
									<TranID>
										<xsl:choose>
											<xsl:when test ="TranID != '' and TranID != '*'">
												<xsl:value-of select="TranID"/>
											</xsl:when>
											<xsl:when test ="InternalTranID != '' and InternalTranID != '*'">
												<xsl:value-of select="InternalTranID"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</TranID>
									<!-- COL40-->
									<IPCounter>
										<xsl:value-of select="''"/>
									</IPCounter>
									<!-- COL41-->
									<!--<Repl>
								<xsl:value-of select="'y'"/>
							</Repl>-->
									<Repl>
										<xsl:value-of select="''"/>
									</Repl>
									<!-- COL42-->
									<Source>
										<xsl:value-of select="''"/>
									</Source>
									<!-- COL43-->
									<Comment2>
										<xsl:value-of select="''"/>
									</Comment2>
									<!-- COL44-->
									<OmniAcct>
										<xsl:value-of select="''"/>
									</OmniAcct>
									<!-- COL45-->
									<Recon>
										<xsl:value-of select="''"/>
									</Recon>
									<!-- COL46-->
									<Post>
										<xsl:value-of select="''"/>
									</Post>
									<!-- COL47-->
									<LabelName>
										<xsl:value-of select="''"/>
									</LabelName>
									<!-- COL48-->
									<LabelDefinition>
										<xsl:value-of select="''"/>
									</LabelDefinition>
									<!-- COL49-->
									<LabelDefinition_Date>
										<xsl:value-of select="''"/>
									</LabelDefinition_Date>
									<!-- COL50-->
									<LabelDefinition_String>
										<xsl:value-of select="''"/>
									</LabelDefinition_String>
									<!-- COL51-->
									<Comment3>
										<xsl:value-of select="''"/>
									</Comment3>
									<!-- COL52-->
									<RecordDate>
										<xsl:value-of select="''"/>
									</RecordDate>
									<!-- COL53-->
									<ReclaimAmount>
										<xsl:value-of select="''"/>
									</ReclaimAmount>
									<!-- COL54-->
									<Strategy>
										<xsl:value-of select="''"/>
									</Strategy>
									<!-- COL55-->
									<Comment4>
										<xsl:value-of select="''"/>
									</Comment4>
									<!-- COL56-->
									<IncomeAccount>
										<xsl:value-of select="''"/>
									</IncomeAccount>
									<!-- COL57-->
									<AccrualAccount>
										<xsl:value-of select="''"/>
									</AccrualAccount>
									<!-- COL58-->
									<DivAccrualMethod>
										<xsl:value-of select="''"/>
									</DivAccrualMethod>
									<!-- COL59-->
									<PerfContributionOrWithdrawal>
										<xsl:value-of select="''"/>
									</PerfContributionOrWithdrawal>

									<!-- system inetrnal use-->
									<!--<FromDeleted>
										<xsl:value-of select ="FromDeleted"/>
									</FromDeleted>-->
									<FromDeleted>
										<xsl:value-of select ="'Yes'"/>
									</FromDeleted>
									<EntityID>
										<xsl:value-of select="Level1AllocationID"/>
									</EntityID>
								</ThirdPartyFlatFileDetail>
							</xsl:when>
							<xsl:when test ="((TranID = '' or TranID = '*') and (InternalTranID != '' and InternalTranID != '*'))">
								<ThirdPartyFlatFileDetail>
									<!-- system inetrnal use-->
									<TaxLotState>
										<xsl:value-of select ="'Deleted'"/>
										<!--<xsl:choose>
									<xsl:when test ="TaxLotStateID=0">
										<xsl:value-of select ="'Allocated'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=1">
										<xsl:value-of select ="'Sent'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=2">
										<xsl:value-of select ="'Amemded'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=3">
										<xsl:value-of select ="'Deleted'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=4">
										<xsl:value-of select ="'Ignore'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'Allocated'"/>
									</xsl:otherwise>
								</xsl:choose>-->
									</TaxLotState>
									<!--<TaxLotState>
								<xsl:value-of select ="'Allocated'"/>
							</TaxLotState>-->
									<!-- system inetrnal use-->
									<RowHeader>
										<xsl:value-of select ="'false'"/>
									</RowHeader>

									<!--<TaxLot>
								<xsl:value-of select="Level1AllocationID"/>
							</TaxLot>-->

									<!-- COL1-->
									<PortfolioCode>
										<xsl:value-of select="PortfolioCode"/>
									</PortfolioCode>
									<!-- COL2-->
									<TranCode>
										<xsl:choose>
											<xsl:when test ="(TranID != '' and TranID != '*') or (InternalTranID != '' and InternalTranID != '*')">
												<xsl:value-of select="translate(TranCode,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="';'"/>
											</xsl:otherwise>
										</xsl:choose>
									</TranCode>
									<!-- COL3-->
									<Comment>
										<xsl:value-of select="''"/>
									</Comment>
									<!-- COL4-->
									<SecType>
										<xsl:value-of select="SecType"/>
									</SecType>
									<!-- COL5-->
									<SecuritySymbol>
										<xsl:value-of select="SecuritySymbol"/>
									</SecuritySymbol>
									<!-- COL6-->
									<TradeDate>
										<xsl:value-of select="''"/>
									</TradeDate>
									<!-- COL7-->
									<SettleDate>
										<xsl:value-of select="''"/>
									</SettleDate>
									<!-- COL8-->
									<OriginalCostDate>
										<xsl:value-of select="''"/>
									</OriginalCostDate>
									<!-- COL9-->
									<Quantity>
										<xsl:value-of select="''"/>
									</Quantity>
									<!-- COL10-->
									<CloseMath>
										<xsl:value-of select="''"/>
									</CloseMath>
									<!-- COL11-->
									<VersusDate>
										<xsl:value-of select="''"/>
									</VersusDate>
									<!-- COL12-->
									<SourceType>
										<xsl:value-of select="''"/>
									</SourceType>
									<!-- COL13-->
									<SourceSymbol>
										<xsl:value-of select="''"/>
									</SourceSymbol>
									<!-- COL14-->
									<TradeDateFXRate>
										<xsl:value-of select="''"/>
									</TradeDateFXRate>
									<!-- COL15-->
									<SettleDateFXRate>
										<xsl:value-of select="''"/>
									</SettleDateFXRate>
									<!-- COL16-->
									<OriginalFXRate>
										<xsl:value-of select="''"/>
									</OriginalFXRate>
									<!-- COL17-->
									<MarkToMarket>
										<xsl:value-of select="''"/>
									</MarkToMarket>
									<!-- COL18-->
									<TradeAmount>
										<xsl:value-of select="''"/>
									</TradeAmount>
									<!-- COL19-->
									<OriginalCost>
										<xsl:value-of select="''"/>
									</OriginalCost>
									<!-- COL20-->
									<Comment1>
										<xsl:value-of select="''"/>
									</Comment1>
									<!-- COL21-->
									<WithholdingTax>
										<xsl:value-of select="''"/>
									</WithholdingTax>
									<!-- COL22-->
									<Exchange>
										<xsl:value-of select="''"/>
									</Exchange>
									<!-- COL23-->
									<ExchangeFee>
										<xsl:value-of select="''"/>
									</ExchangeFee>
									<!-- COL24-->
									<commission>
										<xsl:value-of select="''"/>
									</commission>
									<!-- COL25-->
									<Broker>
										<xsl:value-of select="''"/>
									</Broker>
									<!-- COL26-->
									<ImpliedComm>
										<xsl:value-of select="''"/>
									</ImpliedComm>
									<!-- COL27-->
									<OtherFees>
										<xsl:value-of select="''"/>
									</OtherFees>
									<!-- COL28-->
									<CommPurpose>
										<xsl:value-of select="''"/>
									</CommPurpose>
									<!-- COL29-->
									<Pledge>
										<xsl:value-of select="''"/>
									</Pledge>
									<!-- COL30-->
									<LotLocation>
										<xsl:value-of select="''"/>
									</LotLocation>
									<!-- COL31-->
									<DestPledge>
										<xsl:value-of select="''"/>
									</DestPledge>
									<!-- COL32-->
									<DestLotLocation>
										<xsl:value-of select="''"/>
									</DestLotLocation>
									<!-- COL33-->
									<OriginalFace>
										<xsl:value-of select="''"/>
									</OriginalFace>
									<!-- COL34-->
									<YieldOnCost>
										<xsl:value-of select="''"/>
									</YieldOnCost>
									<!-- COL35-->
									<DurationOnCost>
										<xsl:value-of select="''"/>
									</DurationOnCost>
									<!-- COL36-->
									<UserDef1>
										<xsl:value-of select="''"/>
									</UserDef1>
									<!-- COL37-->
									<UserDef2>
										<xsl:value-of select="''"/>
									</UserDef2>
									<!-- COL38-->
									<UserDef3>
										<xsl:value-of select="''"/>
									</UserDef3>
									<!-- COL39-->
									<TranID>
										<xsl:choose>
											<xsl:when test ="TranID != '' and TranID != '*'">
												<xsl:value-of select="TranID"/>
											</xsl:when>
											<xsl:when test ="InternalTranID != '' and InternalTranID != '*'">
												<xsl:value-of select="InternalTranID"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</TranID>
									<!-- COL40-->
									<IPCounter>
										<xsl:value-of select="''"/>
									</IPCounter>
									<!-- COL41-->
									<!--<Repl>
								<xsl:value-of select="'y'"/>
							</Repl>-->
									<Repl>
										<xsl:value-of select="''"/>
									</Repl>
									<!-- COL42-->
									<Source>
										<xsl:value-of select="''"/>
									</Source>
									<!-- COL43-->
									<Comment2>
										<xsl:value-of select="''"/>
									</Comment2>
									<!-- COL44-->
									<OmniAcct>
										<xsl:value-of select="''"/>
									</OmniAcct>
									<!-- COL45-->
									<Recon>
										<xsl:value-of select="''"/>
									</Recon>
									<!-- COL46-->
									<Post>
										<xsl:value-of select="''"/>
									</Post>
									<!-- COL47-->
									<LabelName>
										<xsl:value-of select="''"/>
									</LabelName>
									<!-- COL48-->
									<LabelDefinition>
										<xsl:value-of select="''"/>
									</LabelDefinition>
									<!-- COL49-->
									<LabelDefinition_Date>
										<xsl:value-of select="''"/>
									</LabelDefinition_Date>
									<!-- COL50-->
									<LabelDefinition_String>
										<xsl:value-of select="''"/>
									</LabelDefinition_String>
									<!-- COL51-->
									<Comment3>
										<xsl:value-of select="''"/>
									</Comment3>
									<!-- COL52-->
									<RecordDate>
										<xsl:value-of select="''"/>
									</RecordDate>
									<!-- COL53-->
									<ReclaimAmount>
										<xsl:value-of select="''"/>
									</ReclaimAmount>
									<!-- COL54-->
									<Strategy>
										<xsl:value-of select="''"/>
									</Strategy>
									<!-- COL55-->
									<Comment4>
										<xsl:value-of select="''"/>
									</Comment4>
									<!-- COL56-->
									<IncomeAccount>
										<xsl:value-of select="''"/>
									</IncomeAccount>
									<!-- COL57-->
									<AccrualAccount>
										<xsl:value-of select="''"/>
									</AccrualAccount>
									<!-- COL58-->
									<DivAccrualMethod>
										<xsl:value-of select="''"/>
									</DivAccrualMethod>
									<!-- COL59-->
									<PerfContributionOrWithdrawal>
										<xsl:value-of select="''"/>
									</PerfContributionOrWithdrawal>

									<!-- system inetrnal use-->
									<!--<FromDeleted>
										<xsl:value-of select ="FromDeleted"/>
									</FromDeleted>-->
									<FromDeleted>
										<xsl:value-of select ="'Yes'"/>
									</FromDeleted>
									<EntityID>
										<xsl:value-of select="Level1AllocationID"/>
									</EntityID>
								</ThirdPartyFlatFileDetail>
							</xsl:when>
							<xsl:when test ="((TranID = '' or TranID = '*') and (InternalTranID = '' or InternalTranID = '*'))">
								<ThirdPartyFlatFileDetail>
									<!-- system inetrnal use-->
									<!--<TaxLotState>
										<xsl:value-of select ="'Deleted'"/>
									</TaxLotState>-->
									<TaxLotState>
										<xsl:choose>
											<xsl:when test ="TaxLotStateID=0">
												<xsl:value-of select ="'Allocated'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=1">
												<xsl:value-of select ="'Sent'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=2">
												<xsl:value-of select ="'Amemded'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=3">
												<xsl:value-of select ="'Deleted'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=4">
												<xsl:value-of select ="'Ignore'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select ="'Allocated'"/>
											</xsl:otherwise>
										</xsl:choose>
									</TaxLotState>
									
									<!-- system inetrnal use-->
									<RowHeader>
										<xsl:value-of select ="'false'"/>
									</RowHeader>

									<!-- COL1-->
									<PortfolioCode>
										<xsl:value-of select="PortfolioCode"/>
									</PortfolioCode>
									<!-- COL2-->
									<TranCode>
										<xsl:choose>
											<xsl:when test ="(TranID != '' and TranID != '*') or (InternalTranID != '' and InternalTranID != '*')">
												<xsl:value-of select="translate(TranCode,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="';'"/>
											</xsl:otherwise>
										</xsl:choose>
									</TranCode>
									<!-- COL3-->
									<Comment>
										<xsl:value-of select="''"/>
									</Comment>
									<!-- COL4-->
									<SecType>
										<xsl:value-of select="SecType"/>
									</SecType>
									<!-- COL5-->
									<SecuritySymbol>
										<xsl:value-of select="SecuritySymbol"/>
									</SecuritySymbol>
									<!-- COL6-->
									<TradeDate>
										<xsl:value-of select="''"/>
									</TradeDate>
									<!-- COL7-->
									<SettleDate>
										<xsl:value-of select="''"/>
									</SettleDate>
									<!-- COL8-->
									<OriginalCostDate>
										<xsl:value-of select="''"/>
									</OriginalCostDate>
									<!-- COL9-->
									<Quantity>
										<xsl:value-of select="''"/>
									</Quantity>
									<!-- COL10-->
									<CloseMath>
										<xsl:value-of select="''"/>
									</CloseMath>
									<!-- COL11-->
									<VersusDate>
										<xsl:value-of select="''"/>
									</VersusDate>
									<!-- COL12-->
									<SourceType>
										<xsl:value-of select="''"/>
									</SourceType>
									<!-- COL13-->
									<SourceSymbol>
										<xsl:value-of select="''"/>
									</SourceSymbol>
									<!-- COL14-->
									<TradeDateFXRate>
										<xsl:value-of select="''"/>
									</TradeDateFXRate>
									<!-- COL15-->
									<SettleDateFXRate>
										<xsl:value-of select="''"/>
									</SettleDateFXRate>
									<!-- COL16-->
									<OriginalFXRate>
										<xsl:value-of select="''"/>
									</OriginalFXRate>
									<!-- COL17-->
									<MarkToMarket>
										<xsl:value-of select="''"/>
									</MarkToMarket>
									<!-- COL18-->
									<TradeAmount>
										<xsl:value-of select="''"/>
									</TradeAmount>
									<!-- COL19-->
									<OriginalCost>
										<xsl:value-of select="''"/>
									</OriginalCost>
									<!-- COL20-->
									<Comment1>
										<xsl:value-of select="''"/>
									</Comment1>
									<!-- COL21-->
									<WithholdingTax>
										<xsl:value-of select="''"/>
									</WithholdingTax>
									<!-- COL22-->
									<Exchange>
										<xsl:value-of select="''"/>
									</Exchange>
									<!-- COL23-->
									<ExchangeFee>
										<xsl:value-of select="''"/>
									</ExchangeFee>
									<!-- COL24-->
									<commission>
										<xsl:value-of select="''"/>
									</commission>
									<!-- COL25-->
									<Broker>
										<xsl:value-of select="''"/>
									</Broker>
									<!-- COL26-->
									<ImpliedComm>
										<xsl:value-of select="''"/>
									</ImpliedComm>
									<!-- COL27-->
									<OtherFees>
										<xsl:value-of select="''"/>
									</OtherFees>
									<!-- COL28-->
									<CommPurpose>
										<xsl:value-of select="''"/>
									</CommPurpose>
									<!-- COL29-->
									<Pledge>
										<xsl:value-of select="''"/>
									</Pledge>
									<!-- COL30-->
									<LotLocation>
										<xsl:value-of select="''"/>
									</LotLocation>
									<!-- COL31-->
									<DestPledge>
										<xsl:value-of select="''"/>
									</DestPledge>
									<!-- COL32-->
									<DestLotLocation>
										<xsl:value-of select="''"/>
									</DestLotLocation>
									<!-- COL33-->
									<OriginalFace>
										<xsl:value-of select="''"/>
									</OriginalFace>
									<!-- COL34-->
									<YieldOnCost>
										<xsl:value-of select="''"/>
									</YieldOnCost>
									<!-- COL35-->
									<DurationOnCost>
										<xsl:value-of select="''"/>
									</DurationOnCost>
									<!-- COL36-->
									<UserDef1>
										<xsl:value-of select="''"/>
									</UserDef1>
									<!-- COL37-->
									<UserDef2>
										<xsl:value-of select="''"/>
									</UserDef2>
									<!-- COL38-->
									<UserDef3>
										<xsl:value-of select="''"/>
									</UserDef3>
									<!-- COL39-->
									<TranID>
										<xsl:choose>
											<xsl:when test ="TranID != '' and TranID != '*'">
												<xsl:value-of select="TranID"/>
											</xsl:when>
											<xsl:when test ="InternalTranID != '' and InternalTranID != '*'">
												<xsl:value-of select="InternalTranID"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</TranID>
									<!-- COL40-->
									<IPCounter>
										<xsl:value-of select="''"/>
									</IPCounter>
									<!-- COL41-->
									<Repl>
										<xsl:value-of select="'y'"/>
									</Repl>
									<!--<Repl>
										<xsl:value-of select="''"/>
									</Repl>-->
									<!-- COL42-->
									<Source>
										<xsl:value-of select="''"/>
									</Source>
									<!-- COL43-->
									<Comment2>
										<xsl:value-of select="''"/>
									</Comment2>
									<!-- COL44-->
									<OmniAcct>
										<xsl:value-of select="''"/>
									</OmniAcct>
									<!-- COL45-->
									<Recon>
										<xsl:value-of select="''"/>
									</Recon>
									<!-- COL46-->
									<Post>
										<xsl:value-of select="''"/>
									</Post>
									<!-- COL47-->
									<LabelName>
										<xsl:value-of select="''"/>
									</LabelName>
									<!-- COL48-->
									<LabelDefinition>
										<xsl:value-of select="''"/>
									</LabelDefinition>
									<!-- COL49-->
									<LabelDefinition_Date>
										<xsl:value-of select="''"/>
									</LabelDefinition_Date>
									<!-- COL50-->
									<LabelDefinition_String>
										<xsl:value-of select="''"/>
									</LabelDefinition_String>
									<!-- COL51-->
									<Comment3>
										<xsl:value-of select="''"/>
									</Comment3>
									<!-- COL52-->
									<RecordDate>
										<xsl:value-of select="''"/>
									</RecordDate>
									<!-- COL53-->
									<ReclaimAmount>
										<xsl:value-of select="''"/>
									</ReclaimAmount>
									<!-- COL54-->
									<Strategy>
										<xsl:value-of select="''"/>
									</Strategy>
									<!-- COL55-->
									<Comment4>
										<xsl:value-of select="''"/>
									</Comment4>
									<!-- COL56-->
									<IncomeAccount>
										<xsl:value-of select="''"/>
									</IncomeAccount>
									<!-- COL57-->
									<AccrualAccount>
										<xsl:value-of select="''"/>
									</AccrualAccount>
									<!-- COL58-->
									<DivAccrualMethod>
										<xsl:value-of select="''"/>
									</DivAccrualMethod>
									<!-- COL59-->
									<PerfContributionOrWithdrawal>
										<xsl:value-of select="''"/>
									</PerfContributionOrWithdrawal>

									<!-- system inetrnal use-->
									<!--<FromDeleted>
										<xsl:value-of select ="FromDeleted"/>
									</FromDeleted>-->
									<FromDeleted>
										<xsl:value-of select ="'Yes'"/>
									</FromDeleted>
									<EntityID>
										<xsl:value-of select="Level1AllocationID"/>
									</EntityID>
								</ThirdPartyFlatFileDetail>
							</xsl:when>
						</xsl:choose>					
						<ThirdPartyFlatFileDetail>
							<!-- system inetrnal use-->
							<!--<TaxLotState>
								<xsl:value-of select ="'Allocated'"/>
							</TaxLotState>-->
							<TaxLotState>
								<xsl:choose>
									<xsl:when test ="TaxLotStateID=0">
										<xsl:value-of select ="'Allocated'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=1">
										<xsl:value-of select ="'Sent'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=2">
										<xsl:value-of select ="'Amemded'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=3">
										<xsl:value-of select ="'Deleted'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=4">
										<xsl:value-of select ="'Ignore'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'Allocated'"/>
									</xsl:otherwise>
								</xsl:choose>
							</TaxLotState>
						
							<!-- system inetrnal use-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<!--<TaxLot>
								<xsl:value-of select="Level1AllocationID"/>
							</TaxLot>-->

							<!-- COL1-->
							<PortfolioCode>
								<xsl:value-of select="PortfolioCode"/>
							</PortfolioCode>
							<!-- COL2-->
							<TranCode>
								<xsl:value-of select="TranCode"/>
							</TranCode>
							<!-- COL3-->
							<Comment>
								<xsl:value-of select="Comment"/>
							</Comment>
							<!-- COL4-->
							<SecType>
								<xsl:value-of select="SecType"/>
							</SecType>
							<!-- COL5-->
							<SecuritySymbol>
								<xsl:value-of select="SecuritySymbol"/>
							</SecuritySymbol>
							<!-- COL6-->
							<TradeDate>
								<xsl:value-of select="TradeDate"/>
							</TradeDate>
							<!-- COL7-->
							<SettleDate>
								<xsl:value-of select="SettleDate"/>
							</SettleDate>
							<!-- COL8-->
							<OriginalCostDate>
								<xsl:value-of select="OriginalCostDate"/>
							</OriginalCostDate>
							<!-- COL9-->
							<Quantity>
								<xsl:choose>
									<xsl:when test ="Quantity != 0">
										<xsl:value-of select="Quantity"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>								
							</Quantity>
							<!-- COL10-->
							<CloseMath>
								<xsl:value-of select="CloseMath"/>
							</CloseMath>
							<!-- COL11-->
							<VersusDate>
								<xsl:value-of select="VersusDate"/>
							</VersusDate>
							<!-- COL12-->
							<SourceType>
								<xsl:value-of select="SourceType"/>
							</SourceType>
							<!-- COL13-->
							<SourceSymbol>
								<xsl:value-of select="SourceSymbol"/>
							</SourceSymbol>
							<!-- COL14-->
							<TradeDateFXRate>
								<xsl:value-of select="TradeDateFXRate"/>
							</TradeDateFXRate>
							<!-- COL15-->
							<SettleDateFXRate>
								<xsl:value-of select="SettleDateFXRate"/>
							</SettleDateFXRate>
							<!-- COL16-->
							<OriginalFXRate>
								<xsl:value-of select="OriginalFXRate"/>
							</OriginalFXRate>
							<!-- COL17-->
							<MarkToMarket>
								<xsl:value-of select="MarkToMarket"/>
							</MarkToMarket>
							<!-- COL18-->
							<TradeAmount>
								<xsl:value-of select="TradeAmount"/>
							</TradeAmount>
							<!-- COL19-->
							<OriginalCost>
								<xsl:value-of select="OriginalCost"/>
							</OriginalCost>
							<!-- COL20-->
							<Comment1>
								<xsl:value-of select="Comment1"/>
							</Comment1>
							<!-- COL21-->
							<WithholdingTax>
								<xsl:value-of select="WithholdingTax"/>
							</WithholdingTax>
							<!-- COL22-->
							<Exchange>
								<xsl:value-of select="Exchange"/>
							</Exchange>
							<!-- COL23-->
							<ExchangeFee>
								<xsl:value-of select="ExchangeFee"/>
							</ExchangeFee>
							<!-- COL24-->
							<commission>
								<xsl:value-of select="commission"/>
							</commission>
							<!-- COL25-->
							<Broker>
								<xsl:value-of select="Broker"/>
							</Broker>
							<!-- COL26-->
							<ImpliedComm>
								<xsl:value-of select="ImpliedComm"/>
							</ImpliedComm>
							<!-- COL27-->
							<OtherFees>
								<xsl:value-of select="OtherFees"/>
							</OtherFees>
							<!-- COL28-->
							<CommPurpose>
								<xsl:value-of select="CommPurpose"/>
							</CommPurpose>
							<!-- COL29-->
							<Pledge>
								<xsl:value-of select="Pledge"/>
							</Pledge>
							<!-- COL30-->
							<LotLocation>
								<xsl:value-of select="LotLocation"/>
							</LotLocation>
							<!-- COL31-->
							<DestPledge>
								<xsl:value-of select="DestPledge"/>
							</DestPledge>
							<!-- COL32-->
							<DestLotLocation>
								<xsl:value-of select="DestLotLocation"/>
							</DestLotLocation>
							<!-- COL33-->
							<OriginalFace>
								<xsl:value-of select="OriginalFace"/>
							</OriginalFace>
							<!-- COL34-->
							<YieldOnCost>
								<xsl:value-of select="YieldOnCost"/>
							</YieldOnCost>
							<!-- COL35-->
							<DurationOnCost>
								<xsl:value-of select="DurationOnCost"/>
							</DurationOnCost>
							<!-- COL36-->
							<UserDef1>
								<xsl:value-of select="UserDef1"/>
							</UserDef1>
							<!-- COL37-->
							<UserDef2>
								<xsl:value-of select="UserDef2"/>
							</UserDef2>
							<!-- COL38-->
							<UserDef3>
								<xsl:value-of select="UserDef3"/>
							</UserDef3>
							<!-- COL39-->
							<!--<TranID>
								<xsl:value-of select="TranID"/>
							</TranID>-->
							<TranID>
								<xsl:value-of select="''"/>
							</TranID>
							<!-- COL40-->
							<IPCounter>
								<xsl:value-of select="IPCounter"/>
							</IPCounter>
							<!-- COL41-->
							<!-- 'y' means replace-->
							<Repl>
								<xsl:value-of select="'y'"/>
							</Repl>
							<!--<Repl>
								<xsl:value-of select="''"/>
							</Repl>-->
							<!-- COL42-->
							<Source>
								<xsl:value-of select="Source"/>
							</Source>
							<!-- COL43-->
							<Comment2>
								<xsl:value-of select="Comment2"/>
							</Comment2>
							<!-- COL44-->
							<OmniAcct>
								<xsl:value-of select="OmniAcct"/>
							</OmniAcct>
							<!-- COL45-->
							<Recon>
								<xsl:value-of select="Recon"/>
							</Recon>
							<!-- COL46-->
							<Post>
								<xsl:value-of select="Post"/>
							</Post>
							<!-- COL47-->
							<LabelName>
								<xsl:value-of select="LabelName"/>
							</LabelName>
							<!-- COL48-->
							<LabelDefinition>
								<xsl:value-of select="LabelDefinition"/>
							</LabelDefinition>
							<!-- COL49-->
							<LabelDefinition_Date>
								<xsl:value-of select="LabelDefinition_Date"/>
							</LabelDefinition_Date>
							<!-- COL50-->
							<LabelDefinition_String>
								<xsl:value-of select="LabelDefinition_String"/>
							</LabelDefinition_String>
							<!-- COL51-->
							<Comment3>
								<xsl:value-of select="Comment3"/>
							</Comment3>
							<!-- COL52-->
							<RecordDate>
								<xsl:value-of select="RecordDate"/>
							</RecordDate>
							<!-- COL53-->
							<ReclaimAmount>
								<xsl:value-of select="ReclaimAmount"/>
							</ReclaimAmount>
							<!-- COL54-->
							<Strategy>
								<xsl:value-of select="Strategy"/>
							</Strategy>
							<!-- COL55-->
							<Comment4>
								<xsl:value-of select="Comment4"/>
							</Comment4>
							<!-- COL56-->
							<IncomeAccount>
								<xsl:value-of select="IncomeAccount"/>
							</IncomeAccount>
							<!-- COL57-->
							<AccrualAccount>
								<xsl:value-of select="AccrualAccount"/>
							</AccrualAccount>
							<!-- COL58-->
							<DivAccrualMethod>
								<xsl:value-of select="DivAccrualMethod"/>
							</DivAccrualMethod>
							<!-- COL59-->
							<PerfContributionOrWithdrawal>
								<xsl:value-of select="PerfContributionOrWithdrawal"/>
							</PerfContributionOrWithdrawal>

							<!-- system inetrnal use-->
							<FromDeleted>
								<xsl:value-of select ="FromDeleted"/>
							</FromDeleted>
							<EntityID>
								<xsl:value-of select="Level1AllocationID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<xsl:when test ="TaxLotStateID=3">
						<ThirdPartyFlatFileDetail>
							<!-- system inetrnal use-->
							<TaxLotState>
								<xsl:choose>
									<xsl:when test ="TaxLotStateID=0">
										<xsl:value-of select ="'Allocated'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=1">
										<xsl:value-of select ="'Sent'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=2">
										<xsl:value-of select ="'Amemded'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=3">
										<xsl:value-of select ="'Deleted'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=4">
										<xsl:value-of select ="'Ignore'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'Allocated'"/>
									</xsl:otherwise>
								</xsl:choose>
							</TaxLotState>
							<!--<TaxLotState>
								<xsl:value-of select ="'Allocated'"/>
							</TaxLotState>-->
							<!-- system inetrnal use-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<!--<TaxLot>
								<xsl:value-of select="Level1AllocationID"/>
							</TaxLot>-->

							<!-- COL1-->
							<PortfolioCode>
								<xsl:value-of select="PortfolioCode"/>
							</PortfolioCode>
							<!-- COL2-->
							<!--<TranCode>
								--><!--<xsl:value-of select="translate(TranCode,vLowercaseChars_CONST,vUppercaseChars_CONST)"/>--><!--
								<xsl:value-of select="translate(TranCode,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
							</TranCode>-->
							<TranCode>
								<xsl:choose>
									<xsl:when test ="TranID != '' and TranID != '*'">
										<xsl:value-of select="translate(TranCode,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="';'"/>
									</xsl:otherwise>
								</xsl:choose>
							</TranCode>
							<!-- COL3-->
							<Comment>
								<xsl:value-of select="''"/>
							</Comment>
							<!-- COL4-->
							<SecType>
								<xsl:value-of select="SecType"/>
							</SecType>
							<!-- COL5-->
							<SecuritySymbol>
								<xsl:value-of select="SecuritySymbol"/>
							</SecuritySymbol>
							<!-- COL6-->
							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>
							<!-- COL7-->
							<SettleDate>
								<xsl:value-of select="''"/>
							</SettleDate>
							<!-- COL8-->
							<OriginalCostDate>
								<xsl:value-of select="''"/>
							</OriginalCostDate>
							<!-- COL9-->
							<Quantity>
								<xsl:value-of select="''"/>
							</Quantity>
							<!-- COL10-->
							<CloseMath>
								<xsl:value-of select="''"/>
							</CloseMath>
							<!-- COL11-->
							<VersusDate>
								<xsl:value-of select="''"/>
							</VersusDate>
							<!-- COL12-->
							<SourceType>
								<xsl:value-of select="''"/>
							</SourceType>
							<!-- COL13-->
							<SourceSymbol>
								<xsl:value-of select="''"/>
							</SourceSymbol>
							<!-- COL14-->
							<TradeDateFXRate>
								<xsl:value-of select="''"/>
							</TradeDateFXRate>
							<!-- COL15-->
							<SettleDateFXRate>
								<xsl:value-of select="''"/>
							</SettleDateFXRate>
							<!-- COL16-->
							<OriginalFXRate>
								<xsl:value-of select="''"/>
							</OriginalFXRate>
							<!-- COL17-->
							<MarkToMarket>
								<xsl:value-of select="''"/>
							</MarkToMarket>
							<!-- COL18-->
							<TradeAmount>
								<xsl:value-of select="''"/>
							</TradeAmount>
							<!-- COL19-->
							<OriginalCost>
								<xsl:value-of select="''"/>
							</OriginalCost>
							<!-- COL20-->
							<Comment1>
								<xsl:value-of select="''"/>
							</Comment1>
							<!-- COL21-->
							<WithholdingTax>
								<xsl:value-of select="''"/>
							</WithholdingTax>
							<!-- COL22-->
							<Exchange>
								<xsl:value-of select="''"/>
							</Exchange>
							<!-- COL23-->
							<ExchangeFee>
								<xsl:value-of select="''"/>
							</ExchangeFee>
							<!-- COL24-->
							<commission>
								<xsl:value-of select="''"/>
							</commission>
							<!-- COL25-->
							<Broker>
								<xsl:value-of select="''"/>
							</Broker>
							<!-- COL26-->
							<ImpliedComm>
								<xsl:value-of select="''"/>
							</ImpliedComm>
							<!-- COL27-->
							<OtherFees>
								<xsl:value-of select="''"/>
							</OtherFees>
							<!-- COL28-->
							<CommPurpose>
								<xsl:value-of select="''"/>
							</CommPurpose>
							<!-- COL29-->
							<Pledge>
								<xsl:value-of select="''"/>
							</Pledge>
							<!-- COL30-->
							<LotLocation>
								<xsl:value-of select="''"/>
							</LotLocation>
							<!-- COL31-->
							<DestPledge>
								<xsl:value-of select="''"/>
							</DestPledge>
							<!-- COL32-->
							<DestLotLocation>
								<xsl:value-of select="''"/>
							</DestLotLocation>
							<!-- COL33-->
							<OriginalFace>
								<xsl:value-of select="''"/>
							</OriginalFace>
							<!-- COL34-->
							<YieldOnCost>
								<xsl:value-of select="''"/>
							</YieldOnCost>
							<!-- COL35-->
							<DurationOnCost>
								<xsl:value-of select="''"/>
							</DurationOnCost>
							<!-- COL36-->
							<UserDef1>
								<xsl:value-of select="''"/>
							</UserDef1>
							<!-- COL37-->
							<UserDef2>
								<xsl:value-of select="''"/>
							</UserDef2>
							<!-- COL38-->
							<UserDef3>
								<xsl:value-of select="''"/>
							</UserDef3>
							<!-- COL39-->
							<TranID>
								<xsl:value-of select="TranID"/>
							</TranID>
							<!-- COL40-->
							<IPCounter>
								<xsl:value-of select="''"/>
							</IPCounter>
							<!-- COL41-->
							<Repl>
								<xsl:value-of select="''"/>
							</Repl>
							<!-- COL42-->
							<Source>
								<xsl:value-of select="''"/>
							</Source>
							<!-- COL43-->
							<Comment2>
								<xsl:value-of select="''"/>
							</Comment2>
							<!-- COL44-->
							<OmniAcct>
								<xsl:value-of select="''"/>
							</OmniAcct>
							<!-- COL45-->
							<Recon>
								<xsl:value-of select="''"/>
							</Recon>
							<!-- COL46-->
							<Post>
								<xsl:value-of select="''"/>
							</Post>
							<!-- COL47-->
							<LabelName>
								<xsl:value-of select="''"/>
							</LabelName>
							<!-- COL48-->
							<LabelDefinition>
								<xsl:value-of select="''"/>
							</LabelDefinition>
							<!-- COL49-->
							<LabelDefinition_Date>
								<xsl:value-of select="''"/>
							</LabelDefinition_Date>
							<!-- COL50-->
							<LabelDefinition_String>
								<xsl:value-of select="''"/>
							</LabelDefinition_String>
							<!-- COL51-->
							<Comment3>
								<xsl:value-of select="''"/>
							</Comment3>
							<!-- COL52-->
							<RecordDate>
								<xsl:value-of select="''"/>
							</RecordDate>
							<!-- COL53-->
							<ReclaimAmount>
								<xsl:value-of select="''"/>
							</ReclaimAmount>
							<!-- COL54-->
							<Strategy>
								<xsl:value-of select="''"/>
							</Strategy>
							<!-- COL55-->
							<Comment4>
								<xsl:value-of select="''"/>
							</Comment4>
							<!-- COL56-->
							<IncomeAccount>
								<xsl:value-of select="''"/>
							</IncomeAccount>
							<!-- COL57-->
							<AccrualAccount>
								<xsl:value-of select="''"/>
							</AccrualAccount>
							<!-- COL58-->
							<DivAccrualMethod>
								<xsl:value-of select="''"/>
							</DivAccrualMethod>
							<!-- COL59-->
							<PerfContributionOrWithdrawal>
								<xsl:value-of select="''"/>
							</PerfContributionOrWithdrawal>

							<!-- system inetrnal use-->
							<FromDeleted>
								<xsl:value-of select ="FromDeleted"/>
							</FromDeleted>
							<EntityID>
								<xsl:value-of select="Level1AllocationID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<xsl:when test ="(TaxLotStateID=0 or TaxLotStateID = 1) or (TaxLotStateID = 2 and TranCode = ';')">
						<ThirdPartyFlatFileDetail>
							<!-- system inetrnal use-->
							<!--<TaxLotState>
								<xsl:choose>
									<xsl:when test ="(TaxLotStateID = 2 and TranCode = ';')">
										<xsl:value-of select ="'Allocated'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test ="TaxLotStateID=0">
												<xsl:value-of select ="'Allocated'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=1">
												<xsl:value-of select ="'Sent'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=2">
												<xsl:value-of select ="'Amemded'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=3">
												<xsl:value-of select ="'Deleted'"/>
											</xsl:when>
											<xsl:when test ="TaxLotStateID=4">
												<xsl:value-of select ="'Ignore'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select ="'Allocated'"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</TaxLotState>-->
							<TaxLotState>
								<xsl:choose>
									<xsl:when test ="TaxLotStateID=0">
										<xsl:value-of select ="'Allocated'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=1">
										<xsl:value-of select ="'Sent'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=2">
										<xsl:value-of select ="'Amemded'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=3">
										<xsl:value-of select ="'Deleted'"/>
									</xsl:when>
									<xsl:when test ="TaxLotStateID=4">
										<xsl:value-of select ="'Ignore'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'Allocated'"/>
									</xsl:otherwise>
								</xsl:choose>
							</TaxLotState>
							<!-- system inetrnal use-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<!--<TaxLot>
						<xsl:value-of select="Level1AllocationID"/>
					</TaxLot>-->

							<!-- COL1-->
							<PortfolioCode>
								<xsl:value-of select="PortfolioCode"/>
							</PortfolioCode>
							<!-- COL2-->
							<TranCode>
								<xsl:value-of select="TranCode"/>
							</TranCode>
							<!-- COL3-->
							<Comment>
								<xsl:value-of select="Comment"/>
							</Comment>
							<!-- COL4-->
							<SecType>
								<xsl:value-of select="SecType"/>
							</SecType>
							<!-- COL5-->
							<SecuritySymbol>
								<xsl:value-of select="SecuritySymbol"/>
							</SecuritySymbol>
							<!-- COL6-->
							<TradeDate>
								<xsl:value-of select="TradeDate"/>
							</TradeDate>
							<!-- COL7-->
							<SettleDate>
								<xsl:value-of select="SettleDate"/>
							</SettleDate>
							<!-- COL8-->
							<OriginalCostDate>
								<xsl:value-of select="OriginalCostDate"/>
							</OriginalCostDate>
							<!-- COL9-->
							<Quantity>
								<xsl:choose>
									<xsl:when test ="Quantity != 0">
										<xsl:value-of select="Quantity"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>
							<!-- COL10-->
							<CloseMath>
								<xsl:value-of select="CloseMath"/>
							</CloseMath>
							<!-- COL11-->
							<VersusDate>
								<xsl:value-of select="VersusDate"/>
							</VersusDate>
							<!-- COL12-->
							<SourceType>
								<xsl:value-of select="SourceType"/>
							</SourceType>
							<!-- COL13-->
							<SourceSymbol>
								<xsl:value-of select="SourceSymbol"/>
							</SourceSymbol>
							<!-- COL14-->
							<TradeDateFXRate>
								<xsl:value-of select="TradeDateFXRate"/>
							</TradeDateFXRate>
							<!-- COL15-->
							<SettleDateFXRate>
								<xsl:value-of select="SettleDateFXRate"/>
							</SettleDateFXRate>
							<!-- COL16-->
							<OriginalFXRate>
								<xsl:value-of select="OriginalFXRate"/>
							</OriginalFXRate>
							<!-- COL17-->
							<MarkToMarket>
								<xsl:value-of select="MarkToMarket"/>
							</MarkToMarket>
							<!-- COL18-->
							<TradeAmount>
								<xsl:value-of select="TradeAmount"/>
							</TradeAmount>
							<!-- COL19-->
							<OriginalCost>
								<xsl:value-of select="OriginalCost"/>
							</OriginalCost>
							<!-- COL20-->
							<Comment1>
								<xsl:value-of select="Comment1"/>
							</Comment1>
							<!-- COL21-->
							<WithholdingTax>
								<xsl:value-of select="WithholdingTax"/>
							</WithholdingTax>
							<!-- COL22-->
							<Exchange>
								<xsl:value-of select="Exchange"/>
							</Exchange>
							<!-- COL23-->
							<ExchangeFee>
								<xsl:value-of select="ExchangeFee"/>
							</ExchangeFee>
							<!-- COL24-->
							<commission>
								<xsl:value-of select="commission"/>
							</commission>
							<!-- COL25-->
							<Broker>
								<xsl:value-of select="Broker"/>
							</Broker>
							<!-- COL26-->
							<ImpliedComm>
								<xsl:value-of select="ImpliedComm"/>
							</ImpliedComm>
							<!-- COL27-->
							<OtherFees>
								<xsl:value-of select="OtherFees"/>
							</OtherFees>
							<!-- COL28-->
							<CommPurpose>
								<xsl:value-of select="CommPurpose"/>
							</CommPurpose>
							<!-- COL29-->
							<Pledge>
								<xsl:value-of select="Pledge"/>
							</Pledge>
							<!-- COL30-->
							<LotLocation>
								<xsl:value-of select="LotLocation"/>
							</LotLocation>
							<!-- COL31-->
							<DestPledge>
								<xsl:value-of select="DestPledge"/>
							</DestPledge>
							<!-- COL32-->
							<DestLotLocation>
								<xsl:value-of select="DestLotLocation"/>
							</DestLotLocation>
							<!-- COL33-->
							<OriginalFace>
								<xsl:value-of select="OriginalFace"/>
							</OriginalFace>
							<!-- COL34-->
							<YieldOnCost>
								<xsl:value-of select="YieldOnCost"/>
							</YieldOnCost>
							<!-- COL35-->
							<DurationOnCost>
								<xsl:value-of select="DurationOnCost"/>
							</DurationOnCost>
							<!-- COL36-->
							<UserDef1>
								<xsl:value-of select="UserDef1"/>
							</UserDef1>
							<!-- COL37-->
							<UserDef2>
								<xsl:value-of select="UserDef2"/>
							</UserDef2>
							<!-- COL38-->
							<UserDef3>
								<xsl:value-of select="UserDef3"/>
							</UserDef3>
							<!-- COL39-->
							<TranID>
								<xsl:value-of select="TranID"/>
							</TranID>
							<!-- COL40-->
							<IPCounter>
								<xsl:value-of select="IPCounter"/>
							</IPCounter>
							<!-- COL41-->
							<Repl>
								<xsl:value-of select="Repl"/>
							</Repl>
							<!-- COL42-->
							<Source>
								<xsl:value-of select="Source"/>
							</Source>
							<!-- COL43-->
							<Comment2>
								<xsl:value-of select="Comment2"/>
							</Comment2>
							<!-- COL44-->
							<OmniAcct>
								<xsl:value-of select="OmniAcct"/>
							</OmniAcct>
							<!-- COL45-->
							<Recon>
								<xsl:value-of select="Recon"/>
							</Recon>
							<!-- COL46-->
							<Post>
								<xsl:value-of select="Post"/>
							</Post>
							<!-- COL47-->
							<LabelName>
								<xsl:value-of select="LabelName"/>
							</LabelName>
							<!-- COL48-->
							<LabelDefinition>
								<xsl:value-of select="LabelDefinition"/>
							</LabelDefinition>
							<!-- COL49-->
							<LabelDefinition_Date>
								<xsl:value-of select="LabelDefinition_Date"/>
							</LabelDefinition_Date>
							<!-- COL50-->
							<LabelDefinition_String>
								<xsl:value-of select="LabelDefinition_String"/>
							</LabelDefinition_String>
							<!-- COL51-->
							<Comment3>
								<xsl:value-of select="Comment3"/>
							</Comment3>
							<!-- COL52-->
							<RecordDate>
								<xsl:value-of select="RecordDate"/>
							</RecordDate>
							<!-- COL53-->
							<ReclaimAmount>
								<xsl:value-of select="ReclaimAmount"/>
							</ReclaimAmount>
							<!-- COL54-->
							<Strategy>
								<xsl:value-of select="Strategy"/>
							</Strategy>
							<!-- COL55-->
							<Comment4>
								<xsl:value-of select="Comment4"/>
							</Comment4>
							<!-- COL56-->
							<IncomeAccount>
								<xsl:value-of select="IncomeAccount"/>
							</IncomeAccount>
							<!-- COL57-->
							<AccrualAccount>
								<xsl:value-of select="AccrualAccount"/>
							</AccrualAccount>
							<!-- COL58-->
							<DivAccrualMethod>
								<xsl:value-of select="DivAccrualMethod"/>
							</DivAccrualMethod>
							<!-- COL59-->
							<PerfContributionOrWithdrawal>
								<xsl:value-of select="PerfContributionOrWithdrawal"/>
							</PerfContributionOrWithdrawal>

							<!-- system inetrnal use-->
							<FromDeleted>
								<xsl:value-of select ="FromDeleted"/>
							</FromDeleted>
							<EntityID>
								<xsl:value-of select="Level1AllocationID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<!--<xsl:otherwise>
					
					</xsl:otherwise>-->
				</xsl:choose>
			</xsl:for-each>			
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	
	<!-- variable declaration for lower to upper case -->
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<!-- variable declaration for lower to upper case ENDs -->
	
</xsl:stylesheet>
