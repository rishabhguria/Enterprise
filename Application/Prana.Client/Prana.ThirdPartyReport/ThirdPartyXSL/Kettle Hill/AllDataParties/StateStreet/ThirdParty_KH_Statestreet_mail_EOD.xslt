<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='PACE - Longs - State Street' or AccountName='PACE - Shorts - Morgan Stanley']">

				 
										
			<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>
					
					<xsl:variable name="PB_NAME" select="'StateStreetMa'"/>
					
					<TradeReferenceID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="substring(PBUniqueID,3)"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="concat('COR',substring(PBUniqueID,3))"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="concat('CAN',substring(PBUniqueID,3))"/>
							</xsl:when>
							
						</xsl:choose>
						<!--<xsl:value-of select="substring(PBUniqueID,3)"/>-->
					</TradeReferenceID>

				<xsl:variable name = "PRANA_FUND_NAME">
					<xsl:value-of select="'CFFW'"/>
				</xsl:variable>

				<xsl:variable name = "PRANA_FUND_NAME1">
					<xsl:value-of select="AccountName"/>
				</xsl:variable>

				<xsl:variable name ="THIRDPARTY_FUND_CODE">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME1]/@PBFundCode"/>
				</xsl:variable>
				<ExecutionAccountNo>
					<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
				
				</ExecutionAccountNo>

				
				
					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>
						
					<SettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</SettlementDate>
						
					
					<TradeType>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeType>
				<Cusip>

					<xsl:value-of select="CUSIP"/>


				</Cusip>

				<Sedol>

					<xsl:value-of select="SEDOL"/>


				</Sedol>
					<SecurityID>
						<xsl:choose>
							
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>
				
					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>
					<SecurityType>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'STOCK'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'BOND'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SecurityType>
					<SubAccountQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</SubAccountQuantity>
					<Price>
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(SettlCurrAmt,'####.0000')"/>
							</xsl:otherwise>
							<!--<xsl:when test="SettlCurrAmt=0">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(SettlCurrAmt,'####.0000')"/>
								
							</xsl:otherwise>-->
						</xsl:choose>
					</Price>
				
				<xsl:variable name="Commission">
					<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
				</xsl:variable>
					<CommissionAmt>
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="format-number($Commission,'##.00')"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="format-number($Commission * SettlCurrFxRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="format-number($Commission div SettlCurrFxRate,'##.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionAmt>
				
					<CommissionType>
						<xsl:value-of select="''"/>
					</CommissionType>

				<xsl:variable name = "NETAMNT">
					<xsl:choose>
						<xsl:when test="SettlCurrFxRate=0">
							<xsl:value-of select="NetAmount"/>
						</xsl:when>
						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
							<xsl:value-of select="NetAmount * SettlCurrFxRate"/>
						</xsl:when>

						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
							<xsl:value-of select="NetAmount div SettlCurrFxRate"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
					<NetAmount>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='JPY'">
								<xsl:value-of select="format-number($NETAMNT,'0.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($NETAMNT,'0.##')"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetAmount>

					<SettlementCurrency>
						<xsl:choose>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'FOP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlCurrency"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SettlementCurrency>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
					</xsl:variable>
					<ExecutingBroker>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBroker>
					
					<Interest>
						<xsl:value-of select="format-number(AccruedInterest,'##.00')"/>
					</Interest>
					<Custodian>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME1"/>
							</xsl:otherwise>
						</xsl:choose>
					</Custodian>
				
				<xsl:variable name="PRANA_EXCHANGE_CODE" select="Exchange"/>

				<xsl:variable name="PB_EXCHANGE_CODE">
					<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE_CODE]/@PBExchangeName"/>
				</xsl:variable>


				<ExchangeRate>
					<xsl:choose>
						<xsl:when test="number(FXRate_Taxlot)">
							<xsl:value-of select="format-number(FXRate_Taxlot,'##.00')"/>
						</xsl:when>
						<xsl:when test="number(ForexRate)">
							<xsl:value-of select="format-number(ForexRate,'##.00')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="1"/>
						</xsl:otherwise>
					</xsl:choose>
					</ExchangeRate>
					<SubAccountTradeID>
						<xsl:value-of select="''"/>
					</SubAccountTradeID>
					<SubAccountNo>
						<xsl:value-of select="'CFFW'"/>
					</SubAccountNo>
				<xsl:variable name = "OthFees">
					<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee)"/>
					<!--<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy)"/>-->
				</xsl:variable>
					<OtherFees>
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="format-number($OthFees,'##.00')"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="format-number($OthFees * SettlCurrFxRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="format-number($OthFees div SettlCurrFxRate,'##.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</OtherFees>
					<IssueCountry>
						<xsl:choose>
						<xsl:when test="CurrencySymbol='USD'">
							<xsl:value-of select="'USA'"/>
						</xsl:when>
							<xsl:when test="CurrencySymbol='EUR'">
								<xsl:value-of select="'EU'"/>
							</xsl:when>
							<xsl:when test="CurrencySymbol='DKK'">
								<xsl:value-of select="'DK'"/>
							</xsl:when>
							<xsl:when test="CurrencySymbol='JPY'">
								<xsl:value-of select="'JP3'"/>
							</xsl:when>
							<xsl:when test="CurrencySymbol='GBP'">
								<xsl:value-of select="'LN'"/>
							</xsl:when>
							<xsl:when test="CurrencySymbol='AUD'">
								<xsl:value-of select="'AU'"/>
							</xsl:when>
							<xsl:when test="CurrencySymbol='CAD'">
								<xsl:value-of select="'CA'"/>
							</xsl:when>
							
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
						</xsl:choose>
						
					</IssueCountry>



					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
