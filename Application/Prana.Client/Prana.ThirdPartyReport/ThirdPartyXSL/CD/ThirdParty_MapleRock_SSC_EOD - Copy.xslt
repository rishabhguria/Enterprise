<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<!--for system internal use-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<FileHeader>
					<xsl:value-of select ="'true'"/>
				</FileHeader>

				<FileFooter>
					<xsl:value-of select ="'false'"/>
				</FileFooter>

				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<RecordAction>
					<xsl:value-of select="'RecordAction'"/>
				</RecordAction>

				<RecordType>
					<xsl:value-of select="'RecordType'"/>
				</RecordType>

				<Portfolio>
					<xsl:value-of select="'Portfolio'"/>
				</Portfolio>

				<Investment>
					<xsl:value-of select="'Investment'"/>
				</Investment>

				<LocationAccount>
					<xsl:value-of select="'LocationAccount'"/>
				</LocationAccount>

				<Strategy>
					<xsl:value-of select="'Strategy'"/>
				</Strategy>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<EventDate>
					<xsl:value-of select="'EventDate'"/>
				</EventDate>

				<SettleDate>
					<xsl:value-of select="'SettleDate'"/>
				</SettleDate>

				<ActualSettleDate>
					<xsl:value-of select="'ActualSettleDate'"/>
				</ActualSettleDate>

				<SecFeeAmount>
					<xsl:value-of select="'ORFfee'"/>
				</SecFeeAmount>

				<NetCounterAmount>
					<xsl:value-of select="'NetCounterAmount'"/>
				</NetCounterAmount>

				<NetInvestmentAmount>
					<xsl:value-of select="'NetInvestmentAmount'"/>
				</NetInvestmentAmount>

				<TotCommission>
					<xsl:value-of select="'TotCommission'"/>
				</TotCommission>

				<UserTranId1>
					<xsl:value-of select="'UserTranId1'"/>
				</UserTranId1>

				<PriceDenomination>
					<xsl:value-of select="'PriceDenomination'"/>
				</PriceDenomination>

				<CounterInvestment>
					<xsl:value-of select="'CounterInvestment'"/>
				</CounterInvestment>

				<CounterFXDenomination>
					<xsl:value-of select="'CounterFXDenomination'"/>
				</CounterFXDenomination>

				<TradeFX>
					<xsl:value-of select="'TradeFX'"/>
				</TradeFX>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SecurityType>
					<xsl:value-of select="'Security Type'"/>
				</SecurityType>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<!--for system internal use-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select ="'false'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select ="'false'"/>
					</FileFooter>

					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name ="PB_NAME">
						<xsl:value-of select="GS"/>
					</xsl:variable>

					<xsl:variable name="Prana_Fund_Name">
						<xsl:value-of select="FundName"/>
					</xsl:variable>

					<xsl:variable name="PRANA_MasterFund_Name">
						<xsl:value-of select="document('../ReconMappingXml/MasterFundMapping.xml')/MasterFundMapping/PB[@Name= 'GS']/MasterFundData[@FundName=$Prana_Fund_Name]/@MasterFundName"/>
					</xsl:variable>

					<xsl:variable name="PB_Fund_Name">
					<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$Prana_Fund_Name]/@PranaFund"/>
					</xsl:variable>

					<RecordAction>
						<xsl:value-of select="'New'"/>
					</RecordAction>

					<RecordType>
						<xsl:choose>
							<xsl:when test ="Side = 'Buy to Cover'">
								<xsl:value-of select ="'Buy to Close'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</RecordType>

					<Portfolio>
						<xsl:value-of select="'GS'"/>
					</Portfolio>

					<Investment>
						<xsl:choose>
							<xsl:when test ="contains(Symbol, '-') != false">
								<xsl:value-of select ="BBCode"/>
							</xsl:when>
							<xsl:when test ="Asset = 'EquityOption'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Investment>

					<xsl:variable name ="varFundName">
						<xsl:choose>
							<xsl:when test ="FundName = 'MS Main'">
								<xsl:value-of select ="'038CDFPK2'"/>
							</xsl:when>
							<xsl:when test ="FundName = 'MS Swap'">
								<xsl:value-of select ="'06178F8Q5'"/>
							</xsl:when>
							<xsl:when test ="FundName = 'MS FX Option'">
								<xsl:value-of select ="'058D17U04'"/>
							</xsl:when>
							<xsl:when test ="FundName = 'MS FX Spot'">
								<xsl:value-of select ="'0581CB0P7'"/>
							</xsl:when>
							<xsl:when test ="FundName = 'GS Main'">
								<xsl:value-of select ="'002506988'"/>
							</xsl:when>
							<xsl:when test ="FundName = 'GS FX Option'">
								<xsl:value-of select ="'044455921'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>

					<LocationAccount>
						<xsl:value-of select="concat($PRANA_MasterFund_Name, '_', $varFundName)"/>
					</LocationAccount>

					<Strategy>
						<xsl:value-of select="Strategy"/>
					</Strategy>

					<Quantity>
						<xsl:value-of select="format-number(AllocatedQty,'#.####')"/>
					</Quantity>

					<Price>
						<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
					</Price>

					<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>

					<EventDate>
						<xsl:value-of select="TradeDate"/>
					</EventDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<ActualSettleDate>
						<xsl:value-of select="SettlementDate"/>
					</ActualSettleDate>

					<SecFeeAmount>
							<xsl:value-of select ="OrfFee"/>
					</SecFeeAmount>

					<NetCounterAmount>
						<xsl:value-of select="''"/>
					</NetCounterAmount>

					<NetInvestmentAmount>
						<xsl:value-of select="format-number(NetAmount,'#.####')"/>
					</NetInvestmentAmount>

					<TotCommission>
						<xsl:value-of select="format-number(CommissionCharged,'#.####')"/>
					</TotCommission>

					<UserTranId1>
						<xsl:value-of select="TradeRefID"/>
					</UserTranId1>

					<PriceDenomination>
						<xsl:value-of select="CurrencySymbol"/>
					</PriceDenomination>

					<CounterInvestment>
						<xsl:value-of select="CurrencySymbol"/>
					</CounterInvestment>

					<CounterFXDenomination>
						<xsl:value-of select="CurrencySymbol"/>
					</CounterFXDenomination>

					<TradeFX>
						<xsl:value-of select="format-number(ForexRate,'#.####')"/>
					</TradeFX>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<SecurityType>
						<xsl:choose>
							<xsl:when test ="IsSwapped='true' and Asset='Equity'">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
