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

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[(TaxLotState != 'Sent' and TaxLotState = 'Allocated') and (UDASecurityTypeName!='cfeu' and UDASecurityTypeName!='cfus')]">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<OrderNumber>
						<xsl:value-of select="concat(EntityID,'A')"/>
					</OrderNumber>

					<xsl:variable name ="varAllocationState">
						<xsl:choose>
							<xsl:when test ="TaxLotState = 'Allocated'">
								<xsl:value-of  select="'N'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState = 'Amended'">
								<xsl:value-of  select="'A'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState = 'Deleted'">
								<xsl:value-of  select="'C'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of  select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Cancelcorrectindicator>
						<xsl:value-of select="$varAllocationState"/>
					</Cancelcorrectindicator>

					<xsl:variable name="PB_NAME" select="'GS_swap'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>


					<xsl:variable name="AccountName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<AccountNumber>
						<xsl:value-of select="$AccountName"/>
					</AccountNumber>

					<xsl:variable name="varSymbol">
						<xsl:choose>
							<xsl:when test="SEDOL != '' and SEDOL != '*'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="ISIN != '*' and ISIN != ''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:when test="CUSIP != '*' and CUSIP != ''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>

							<xsl:when test="BBCode != '*' and BBCode != ''">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SecurityIdentifier>
						<xsl:value-of select="$varSymbol"/>
					</SecurityIdentifier>


					

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Broker>
						<xsl:value-of select="$Broker"/>
					</Broker>

					<Custodian>
						<xsl:value-of select="'GSCO'"/>
					</Custodian>

					<xsl:variable name="varTransactionType">
						<xsl:choose>
							<xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side = 'Sell' or Side = 'Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side = 'Buy to Close' or Side = 'Buy to Cover'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<TransactionType>
						<xsl:value-of select="$varTransactionType"/>
					</TransactionType>

					<xsl:variable name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="contains(Asset,'FX')">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlCurrency"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<CurrencyCode>
						<xsl:value-of select="'USD'"/>
					</CurrencyCode>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<xsl:variable name="SettleFx">
						<xsl:choose>
							<xsl:when test="number(SettlCurrFxRate)">
								<xsl:value-of select="SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="CommissionFX">

						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) * $SettleFx"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) div $SettleFx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="(CommissionCharged + SoftCommissionCharged)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="Commission" select="CommissionCharged + SoftCommissionCharged + OtherBrokerFee + ClearingFee + TaxOnCommissions + MiscFees + OrfFee + OccFee + ClearingBrokerFee + SecFees + StampDuty + TransactionLevy"/>

					<Commission>
						<xsl:value-of select="$Commission"/>
					</Commission>

					<xsl:variable name ="varAvgPrice">
						<xsl:choose>
							<xsl:when test ="CounterParty = 'GWEP'">
								<xsl:value-of  select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of  select="AveragePrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name ="AvgPriceFX">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varAvgPrice * $SettleFx"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varAvgPrice div $SettleFx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$varAvgPrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<AccruedInterest>
						<xsl:value-of select="AccruedInterest"/>
					</AccruedInterest>

					<xsl:variable name="TradeTax" select="TransactionLevy "/>

					<xsl:variable name ="varTradeTax">
						<xsl:choose>
							<xsl:when test="$TradeTax &gt; 0">
								<xsl:value-of select="$TradeTax"/>
							</xsl:when>
							<xsl:when test="$TradeTax &lt; 0">
								<xsl:value-of select="$TradeTax * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="TradeTaxFX">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varTradeTax * $SettleFx"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varTradeTax div $SettleFx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$varTradeTax"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<TradeTax>
						<xsl:value-of select="''"/>
					</TradeTax>


					<xsl:variable name="MiscMoney">
						<xsl:value-of select="''"/>
					</xsl:variable>

					<xsl:variable name="varMiscMoney">
						<xsl:choose>
							<xsl:when test="$MiscMoney &gt; 0">
								<xsl:value-of select="$MiscMoney"/>
							</xsl:when>
							<xsl:when test="$MiscMoney &lt; 0">
								<xsl:value-of select="$MiscMoney * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="MiscMoneyFX">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varMiscMoney * $SettleFx"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varMiscMoney div $SettleFx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$varMiscMoney"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<MiscMoney>
						<xsl:value-of select="$varMiscMoney"/>
					</MiscMoney>

					<xsl:variable name="NetAmountFX">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="NetAmount * $SettleFx"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="NetAmount div $SettleFx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NetAmount>
						<xsl:value-of select="NetAmount"/>
					</NetAmount>

					<xsl:variable name="GrossAmountFX">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="GrossAmount * $SettleFx"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="GrossAmount div $SettleFx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="GrossAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Principal>
						<xsl:value-of select="GrossAmount"/>
					</Principal>

					<Description>
						<xsl:value-of select="FullSecurityName"/>
					</Description>

					<xsl:variable name="varSecurityName">


						<xsl:choose>
							<xsl:when test="Asset='Equity' ">
								<xsl:value-of select="'CFD'"/>
							</xsl:when>
							<xsl:when test="SEDOL != '' and SEDOL != '*'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="ISIN != '*' and ISIN != ''">
								<xsl:value-of select="'I'"/>
							</xsl:when>
							<xsl:when test="CUSIP != '*' and CUSIP != ''">
								<xsl:value-of select="'C'"/>
							</xsl:when>

							<xsl:when test="BBCode != '*' and BBCode != ''">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SecurityType>
						<xsl:value-of select="$varSecurityName"/>
					</SecurityType>

					<CountrySettlementCode>
						<xsl:value-of select="'USD'"/>
					</CountrySettlementCode>

					<ClearingAgent>
						<xsl:value-of select="''"/>
					</ClearingAgent>

					<SECFee>
						<xsl:value-of select="''"/>
					</SECFee>
					<RepoOpenSettleDate>
						<xsl:value-of select="''"/>
					</RepoOpenSettleDate>
					<RepoMaturityDate>
						<xsl:value-of select="''"/>
					</RepoMaturityDate>

					<RepoRate>
						<xsl:value-of select="''"/>
					</RepoRate>

					<RepoInterest>
						<xsl:value-of select="''"/>
					</RepoInterest>

					<xsl:variable name ="varUnderlyingSymbol">
						<xsl:choose>
							<xsl:when test ="contains(Asset, 'Option')!= false">
								<xsl:value-of select ="UnderlyingSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OptionUnderlyer>
						<xsl:value-of select="$varUnderlyingSymbol"/>
					</OptionUnderlyer>

					<xsl:variable name="ExpiryDate">
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="concat(substring(substring-after(substring-after(ExpirationDate,'/'),'/'),3,2),substring-before(ExpirationDate,'/'),substring-after(substring-before(ExpirationDate,'/'),'/'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OptionExpiryDate>
						<xsl:value-of select="$ExpiryDate"/>
					</OptionExpiryDate>

					<OptionCallPutIndicator>
						<xsl:value-of select="substring(PutOrCall,1,1)"/>
					</OptionCallPutIndicator>

					<xsl:variable name ="varStrikePrice">
						<xsl:choose>
							<xsl:when test ="contains(Asset, 'Option')!= false">
								<xsl:value-of select ="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OptionStrikePrice>
						<xsl:value-of select="$varStrikePrice"/>
					</OptionStrikePrice>

					<Trailer>
						<xsl:value-of select="'TEXT'"/>
					</Trailer>

					<GenevaLotNumber1>
						<xsl:value-of select="''"/>
					</GenevaLotNumber1>

					<GainsKeeperLotNumber1>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber1>

					<LotDate1>
						<xsl:value-of select="''"/>
					</LotDate1>

					<LotQty1>
						<xsl:value-of select="''"/>
					</LotQty1>

					<LotPrice1>
						<xsl:value-of select="''"/>
					</LotPrice1>

					<GenevaLotNumber2>
						<xsl:value-of select="''"/>
					</GenevaLotNumber2>

					<GainsKeeperLotNumber2>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber2>

					<LotDate2>
						<xsl:value-of select="''"/>
					</LotDate2>

					<LotQty2>
						<xsl:value-of select="''"/>
					</LotQty2>

					<LotPrice2>
						<xsl:value-of select="''"/>
					</LotPrice2>

					<GenevaLotNumber3>
						<xsl:value-of select="''"/>
					</GenevaLotNumber3>

					<GainsKeeperLotNumber3>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber3>

					<LotDate3>
						<xsl:value-of select="''"/>
					</LotDate3>

					<LotQty3>
						<xsl:value-of select="''"/>
					</LotQty3>

					<LotPrice3>
						<xsl:value-of select="''"/>
					</LotPrice3>

					<GenevaLotNumber4>
						<xsl:value-of select="''"/>
					</GenevaLotNumber4>

					<GainsKeeperLotNumber4>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber4>

					<LotDate4>
						<xsl:value-of select="''"/>
					</LotDate4>

					<LotQty4>
						<xsl:value-of select="''"/>
					</LotQty4>

					<LotPrice4>
						<xsl:value-of select="''"/>
					</LotPrice4>

					<GenevaLotNumber5>
						<xsl:value-of select="''"/>
					</GenevaLotNumber5>

					<GainsKeeperLotNumber5>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber5>

					<LotDate5>
						<xsl:value-of select="''"/>
					</LotDate5>

					<LotQty5>
						<xsl:value-of select="''"/>
					</LotQty5>

					<LotPrice5>
						<xsl:value-of select="''"/>
					</LotPrice5>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>