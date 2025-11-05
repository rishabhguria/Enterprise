<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='WNDR']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>-->

					<TaxLotState>
						<xsl:choose>
							<xsl:when test="TaxlotState = 0 ">
								<xsl:value-of select="'Allocated'"/>
							</xsl:when>
							<xsl:when test="TaxlotState = 1 ">
								<xsl:value-of select="'Sent'"/>
							</xsl:when>
							<xsl:when test="TaxlotState = 2 ">
								<xsl:value-of select="'Amended'"/>
							</xsl:when>
							<xsl:when test="TaxlotState = 3 ">
								<xsl:value-of select="'Deleted'"/>
							</xsl:when>
							<xsl:when test="TaxlotState = 4 ">
								<xsl:value-of select="'Ignore'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Allocated'"/>
							</xsl:otherwise>
						</xsl:choose>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="'Wunderlich'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<Account>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
											
					</Account>

					<Side>
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
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<xsl:variable name="VarSymbol" select="Symbol"/>
					<Ticker>
						<xsl:choose>
							<xsl:when test="$VarSymbol='H-IWM'">
								<xsl:value-of select="'IWM'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="$VarSymbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Ticker>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>	
					</CUSIP>

					<RIC>
						<xsl:value-of select="RIC"/>
					</RIC>

				<BBCode>
						<xsl:choose>
							<xsl:when test="BBCode='H-IWM US EQUITY'">
								<xsl:value-of select="'IWM US EQUITY'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BBCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</BBCode>

					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>

					<OrderID>
						<xsl:value-of select="Level1AllocationID"/>
					</OrderID>

					<OrderQuantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</OrderQuantity>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>						
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>						
					</SettlementDate>

					<xsl:variable name="varSettFxAmt">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator ='M'">
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

					<xsl:variable name="Price">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varSettFxAmt"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<ExecutionPrice>
						<xsl:value-of select="format-number($Price,'0.####')"/>
					</ExecutionPrice>

					<ExecutingBrokerCode>
						<xsl:value-of select="CounterParty"/>
					</ExecutingBrokerCode>

					<Custodian>
						<xsl:value-of select="'Wunderlich'"/>
					</Custodian>

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

					<xsl:variable name="VarComm">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number($Commission,'##.00')"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
								<xsl:value-of select="format-number($Commission * $varFXRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
								<xsl:value-of select="format-number($Commission div $varFXRate,'##.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<TradeCommission>
						<xsl:value-of select="format-number($VarComm,'##.00')"/>
					</TradeCommission>

					<xsl:variable name="SecFees">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number(SecFee,'##.00')"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
								<xsl:value-of select="format-number(SecFee * $varFXRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
								<xsl:value-of select="format-number(SecFee div $varFXRate,'##.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SecFees>
						<xsl:value-of select="format-number($SecFees,'##.00')"/>
					</SecFees>

					<xsl:variable name="OtherFees">
						<xsl:value-of select="StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee"/>
					</xsl:variable>

					<xsl:variable name="OtherFees1">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number($OtherFees,'##.00')"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
								<xsl:value-of select="format-number($OtherFees * $varFXRate,'##.00')"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
								<xsl:value-of select="format-number($OtherFees div $varFXRate,'##.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OtherFees>
						<xsl:value-of select="format-number($OtherFees1,'##.00')"/>
					</OtherFees>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test ="number(StrikePrice)">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'), substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExpirationDate>

					<PutOrCall>
						<xsl:choose>
							<xsl:when test="PutOrCall='CALL'">
								<xsl:value-of select="'CL'"/>
							</xsl:when>
							<xsl:when test="PutOrCall='PUT'">
								<xsl:value-of select="'PT'"/>
							</xsl:when>
						</xsl:choose>
					</PutOrCall>

					<UnderlyingSymbol>
						<xsl:value-of select="UnderlyingSymbol"/>
					</UnderlyingSymbol>

					<xsl:variable name="PRANA_EXCHANGE_CODE" select="Exchange"/>

					<xsl:variable name="PB_EXCHANGE_CODE">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE_CODE]/@PBExchangeName"/>
					</xsl:variable>

					<Exchange>
						<xsl:choose>
							<xsl:when test="$PB_EXCHANGE_CODE!=''">
								<xsl:value-of select="$PB_EXCHANGE_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_EXCHANGE_CODE"/>
							</xsl:otherwise>
						</xsl:choose>
					</Exchange>

					<TradedCurrency>
						<xsl:value-of select="CurrencySymbol"/>						
					</TradedCurrency>



					<EntityID>
						<xsl:value-of select="Level1AllocationID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>