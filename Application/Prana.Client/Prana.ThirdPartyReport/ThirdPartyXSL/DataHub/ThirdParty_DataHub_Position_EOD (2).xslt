<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="FormatDate">
		<xsl:param name="Date"/>

		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="string-length(substring-before(substring-after($Date,'/'),'/'))=1">
					<xsl:value-of select="concat('0',substring-before(substring-after($Date,'/'),'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(substring-after($Date,'/'),'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="Month">
			<xsl:choose>
				<xsl:when test="string-length(substring-before($Date,'/'))=1">
					<xsl:value-of select="concat('0',substring-before($Date,'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before($Date,'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="Year">
			<xsl:value-of select="substring-after(substring-after($Date,'/'),'/')"/>
		</xsl:variable>
		<!--Null Check Removing "//" with blank-->
		<xsl:variable name="CompleteDate">
			<xsl:choose>
				<xsl:when test="$Month != '' and $Day != '' and $Year != ''">
					<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!--Nirvana Database is accepting americal datetime format, so changed to MM/dd/yyyy-->
		<xsl:value-of select="$CompleteDate"/>
	</xsl:template>

	<xsl:template match="/">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<ThirdPartyFlatFileDetail>

					<AssetClass>
						<xsl:value-of select="normalize-space(AssetClass)"/>
					</AssetClass>

					<eSignalTicker>
						<xsl:value-of select="normalize-space(Symbol)"/>
					</eSignalTicker>

					<BloombergCode>
						<xsl:value-of select="normalize-space(Bloomberg)"/>
					</BloombergCode>

					<SecurityDescription>
						<xsl:value-of select="normalize-space(SecurityDescription)"/>
					</SecurityDescription>

					<LocalCurreny>
						<xsl:value-of select="normalize-space(CurrencyName)"/>
					</LocalCurreny>

					<AccountName>
						<xsl:value-of select="normalize-space(FundName)"/>
					</AccountName>

					<xsl:variable name="varAUECLocalDate">
						<xsl:call-template name="FormatDate">
							<xsl:with-param name="Date" select="AUECLocalDate"/>
						</xsl:call-template>
					</xsl:variable>

					<TradeDate>
						<xsl:value-of select="$varAUECLocalDate"/>
					</TradeDate>

					<xsl:variable name="varPositionSettlementDate">
						<xsl:call-template name="FormatDate">
							<xsl:with-param name="Date" select="PositionSettlementDate"/>
						</xsl:call-template>
					</xsl:variable>

					<SettlementDate>
						<xsl:value-of select="$varPositionSettlementDate"/>
					</SettlementDate>

					<TradeSide>
						<xsl:value-of select="Side"/>
					</TradeSide>

					<Quantity>
						<xsl:value-of select="NetPosition"/>
					</Quantity>

					<TradePrice>
						<xsl:value-of select="CostBasis"/>
					</TradePrice>

					<NetAmount>
						<xsl:choose>
							<xsl:when test="format-number(Commission,'#.##') = ''">
								<xsl:value-of select="Commission"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(Commission,'#.##')" />
							</xsl:otherwise>
						</xsl:choose>
					</NetAmount>

					<OtherBrokerageFees>
						<xsl:value-of select="Fees"/>
					</OtherBrokerageFees>

					<MarketValueLocal>
						<xsl:value-of select="StampDuty"/>
					</MarketValueLocal>

					<MarketValueBase>
						<xsl:value-of select="SecFee"/>
					</MarketValueBase>

					<UnRealizedPNLLocal>
						<xsl:value-of select="TransactionLevy"/>
					</UnRealizedPNLLocal>

					<UnRealizedPNLBase>
						<xsl:value-of select="TaxOnCommissions"/>
					</UnRealizedPNLBase>

					<MarkPrice>
						<xsl:value-of select="OrfFee"/>
					</MarkPrice>

					<CashValueBase>
						<xsl:value-of select="OCCFee"/>
					</CashValueBase>

					<Multiplier>
						<xsl:value-of select="Multiplier"/>
					</Multiplier>

					<TradeFXRate>
						<xsl:value-of select="FXRate"/>
					</TradeFXRate>

					<AccruedInterest>
						<xsl:value-of select="AccruedInterest"/>
					</AccruedInterest>

					<BrokerName>
						<xsl:value-of select="normalize-space(ExecutingBroker)"/>
					</BrokerName>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<SEDOL>
						<xsl:value-of select="SEDOL"/>
					</SEDOL>

					<OSI-21>
						<xsl:value-of select="OSIOptionSymbol"/>
					</OSI-21>

					<UnderlyingSymbol>
						<xsl:value-of select="UnderlyingSymbol"/>
					</UnderlyingSymbol>

					<StrikePrice>
						<xsl:value-of select="StrikePrice"/>
					</StrikePrice>

					<xsl:variable name="varExpirationDate">
						<xsl:call-template name="FormatDate">
							<xsl:with-param name="Date" select="ExpirationDate"/>
						</xsl:call-template>
					</xsl:variable>
					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="STYPE = 'EQ' or STYPE = 'EF' or STYPE = 'PE' or STYPE = 'OA'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:when test="STYPE = 'CF'">
								<xsl:value-of select="'12/31/2220'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varExpirationDate"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExpirationDate>

					<LeadCurreny>
						<xsl:value-of select="normalize-space(LeadCurrencyName)"/>
					</LeadCurreny>

					<VsCurreny>
						<xsl:value-of select="normalize-space(VsCurrencyName)"/>
					</VsCurreny>

					<RootSymbol>
						<xsl:value-of select="normalize-space(DerivativeRootSymbol)"/>
					</RootSymbol>

					<NetAmountBase>
						<xsl:value-of select="Coupon"/>
					</NetAmountBase>

					<Strategy>
						<xsl:value-of select="Strategy"/>
					</Strategy>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

	<xsl:variable name="Small" select="'abcdefghijklmnopqrstuvwxyz'"/>

	<xsl:variable name="Caps" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>