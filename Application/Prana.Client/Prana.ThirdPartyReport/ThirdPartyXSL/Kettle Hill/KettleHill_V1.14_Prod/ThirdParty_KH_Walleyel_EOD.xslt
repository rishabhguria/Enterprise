<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>



	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName ='Walleye - GS']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<xsl:variable name="PB_NAME" select="'GS'"/>
					<Status>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'New'"/>
							</xsl:when>

							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'Can'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'Cor'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'Sent'"/>
							</xsl:otherwise>
						</xsl:choose>
					</Status>

					<TradeReferenceID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="PBUniqueID"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="PBUniqueID"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="PBUniqueID"/>
							</xsl:when>

						</xsl:choose>
					</TradeReferenceID>



					<xsl:variable name = "PRANA_FUND_NAME1">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME1]/@PBFundCode"/>
					</xsl:variable>
					<ExecutionAccountNo>
						<xsl:value-of select="AccountNo"/>
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

					<xsl:variable name="VarSymbol111">

						<xsl:choose>
							<xsl:when test="BBCode='H-IWM US EQUITY'">
								<xsl:value-of select="'IWM US EQUITY'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BBCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Ticker>

						<xsl:value-of select="$VarSymbol111"/>


					</Ticker>
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


					<xsl:variable name="varSettFxAmt">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AveragePrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Price>
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varSettFxAmt,'####.0000')"/>
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

					<xsl:variable name="varFXRate">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<CommissionAmt>
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number($Commission,'##.00')"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="format-number($Commission * $varFXRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="format-number($Commission div $varFXRate,'##.00')"/>
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
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="NetAmount * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="NetAmount div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
								<!--<xsl:value-of select="NetAmount * $varFXRate"/>-->
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

								<xsl:value-of select="SettlCurrency"/>


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
						<xsl:value-of select="AccountNo"/>
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
						<xsl:value-of select="'GS'"/>
					</SubAccountNo>
					<xsl:variable name = "varSecFee">
						<xsl:value-of select="SecFee"/>
						<!--<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy)"/>-->
					</xsl:variable>
					<SecFee>
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number($varSecFee,'##.00')"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="format-number($varSecFee * $varFXRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="format-number($varSecFee div $varFXRate,'##.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</SecFee>
					<xsl:variable name = "OthFees">
						<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee)"/>
						<!--<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy)"/>-->
					</xsl:variable>
					<OtherFees>
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number($OthFees,'##.00')"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="format-number($OthFees * $varFXRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="format-number($OthFees div $varFXRate,'##.00')"/>
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
