<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>

			<xsl:for-each select="ThirdPartyFlatFileDetail[contains(AccountName,'Venbio')='true']">

				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<TransactionType>
						<xsl:value-of select="'BS'"/>
					</TransactionType>

					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'BNP'"/>
					</xsl:variable>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<xsl:variable name="FundCode">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<UBSAccount>
						<xsl:value-of select="$FundCode"/>
					</UBSAccount>

					<OriginalTradeID>
						<xsl:value-of select="''"/>
					</OriginalTradeID>

					<TradeID>
						<xsl:value-of select="concat('A',EntityID)"/>
					</TradeID>

					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'))"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'))"/>
					</SettlementDate>

					<ActionCode>
						<xsl:choose>
							<xsl:when test="Side = 'Buy to Close'">
								<xsl:value-of select="'Buy Cover'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</ActionCode>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<NetAmount>
						<xsl:value-of select="NetAmount"/>
					</NetAmount>

					<SettlementCurrency>
						<xsl:value-of select="SettlCurrency"/>
					</SettlementCurrency>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="SEDOL!='' and SEDOL!='*'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP!='' and CUSIP!='*'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="ISIN!='' and ISIN!='*'">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:when test="OSIOptionSymbol!='' and OSIOptionSymbol!='*'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<SecurityIDType>
						<xsl:choose>
							<xsl:when test="SEDOL!='' and SEDOL!='*'">
								<xsl:value-of select="'SEDOL'"/>
							</xsl:when>
							<xsl:when test="CUSIP!='' and CUSIP!='*'">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="ISIN!='' and ISIN!='*'">
								<xsl:value-of select="'ISIN'"/>
							</xsl:when>
							<xsl:when test="OSIOptionSymbol!='' and OSIOptionSymbol!='*'">
								<xsl:value-of select="'OCC Ticker'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Ticker'"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityIDType>

					<CommissionCode>
						<xsl:value-of select="'G'"/>
					</CommissionCode>

					<CommissionAmount>
						<xsl:value-of select="CommissionCharged"/>
					</CommissionAmount>


					<xsl:variable name = "Prana_CounterParty" >
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>

					<xsl:variable name="PB_CounterParty">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerName=$Prana_CounterParty]/@PBBrokerName"/>
					</xsl:variable>

					<ExecutingBroker>
						<xsl:choose>
							<xsl:when test ="$PB_CounterParty != ''">
								<xsl:value-of select ="$PB_CounterParty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$Prana_CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBroker>

					<BrokerName>
						<xsl:choose>
							<xsl:when test ="$PB_CounterParty != ''">
								<xsl:value-of select ="$PB_CounterParty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$Prana_CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</BrokerName>

					<Remarks>
						<xsl:value-of select="''"/>
					</Remarks>

					<Instructions>
						<xsl:value-of select="''"/>
					</Instructions>

					<Blank1>
						<xsl:value-of select="''"/>
					</Blank1>

					<Blank2>
						<xsl:value-of select="''"/>
					</Blank2>

					<Blank3>
						<xsl:value-of select="''"/>
					</Blank3>

					<Blank4>
						<xsl:value-of select="''"/>
					</Blank4>

					<Blank5>
						<xsl:value-of select="''"/>
					</Blank5>

					<TradeCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</TradeCurrency>

					<ExchangeRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExchangeRate>

					<LiquidMethod>
						<xsl:value-of select="''"/>
					</LiquidMethod>

					<StampTax>
						<xsl:value-of select="StampDuty"/>
					</StampTax>

					<Interest>
						<xsl:value-of select="''"/>
					</Interest>

					<SettlementLocation>
						<xsl:value-of select="''"/>
					</SettlementLocation>

					<Blank6>
						<xsl:value-of select="''"/>
					</Blank6>

					<Security>
						<xsl:value-of select="Symbol"/>
					</Security>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>