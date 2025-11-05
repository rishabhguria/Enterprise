<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>		
			
			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset!='FX' and Asset!='FXForward']">
				
				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>					

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<ProductCode>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:when test="Asset = 'Equity' or Asset = 'EquityOption'" >
								<xsl:value-of select="'BS'"/>
							</xsl:when>						
							<xsl:when test="Asset='FX' or Asset='FXForward'">
								<xsl:value-of select="'FX'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ProductCode>

					<xsl:variable name="PB_NAME" select="'UBS'"/>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountNo"/>

					<xsl:variable name="PB_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund = $PRANA_FUND_NAME]/@PBFundName"/>
					</xsl:variable>

					<AccountID>
						<xsl:choose>
							<xsl:when test="$PB_FUND_NAME!=''">
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountID>

					<TradeRefID>
						<xsl:value-of select="TradeRefID"/>
					</TradeRefID>

					<TradeDate>
						<xsl:value-of select="concat(substring(TradeDate,7,4), substring(TradeDate,1,2), substring(TradeDate,4,2))"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="concat(substring(SettlementDate,7,4), substring(SettlementDate,1,2), substring(SettlementDate,4,2))"/>
					</SettlementDate>

					<ActionCode>
						<xsl:choose>
							<xsl:when test ="TaxLotState='Amended'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'AB'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'ABC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'AS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'ASHS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test ="TaxLotState='Deleted'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'XB'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'XBC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'XS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'XSS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'BC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</ActionCode>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:choose>
							<xsl:when test ="number(AveragePrice)">
								<xsl:value-of select="AveragePrice"/>		
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Price>

					<NetAmount>
						<xsl:choose>
							<xsl:when test ="number(NetAmount)">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetAmount>

					<SettlementCcy>
						<xsl:value-of select="CurrencySymbol"/>
					</SettlementCcy>

					<SecurityID>
						<xsl:choose>
							<xsl:when test ="Asset = 'EquityOption'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="SEDOL != ''">
								<xsl:value-of select ="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP != ''">
								<xsl:value-of select ="CUSIP"/>
							</xsl:when>
							<xsl:when test="ISIN != ''">
								<xsl:value-of select ="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</SecurityID>

					<xsl:variable name = "PRANA_Broker" >
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>

					<xsl:variable name="PB_Broker">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker = $PRANA_Broker]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<ExecBrokerCode>
						<xsl:choose>
							<xsl:when test="$PB_Broker!=''">
								<xsl:value-of select="$PB_Broker"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_Broker"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecBrokerCode>

					<CommissionAmount>
						<xsl:choose>
							<xsl:when test ="number(CommissionCharged)">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionAmount>

					<Principal>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome' or Asset='ConvertibleBond'">
								<xsl:choose>
									<xsl:when test ="number(GrossAmount)">
										<xsl:value-of select="GrossAmount"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Principal>

					<AccruedInterest>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome' or Asset='ConvertibleBond'">
								<xsl:choose>
									<xsl:when test ="number(AccruedInterest)">
										<xsl:value-of select="AccruedInterest"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccruedInterest>

					<Factor>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select ="0.01"/>
							</xsl:when>
							<xsl:otherwise>
								<!--<xsl:choose>
									<xsl:when test ="number(AssetMultiplier)">
										<xsl:value-of select="AssetMultiplier"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>
								</xsl:choose>-->
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</Factor>

					<!-- system use only-->
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
					
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
			
		</ThirdPartyFlatFileDetailCollection>
		
	</xsl:template>

</xsl:stylesheet>