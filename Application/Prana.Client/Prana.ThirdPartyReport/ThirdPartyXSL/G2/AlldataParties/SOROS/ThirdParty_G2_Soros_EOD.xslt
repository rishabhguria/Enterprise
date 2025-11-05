<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Quantum Partners LP']">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<TradeNumber>
						<xsl:value-of select="EntityID"/>
					</TradeNumber>

					<Ticker>
						<xsl:choose>
							<xsl:when test="contains(Asset,'EquityOption')">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Equity')and CurrencySymbol = 'USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Equity')and CurrencySymbol != 'USD'">
								<xsl:value-of select="substring-before(BBCode,' EQUITY')"/>
							</xsl:when>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="BBCode!='*'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="ISIN!='*'">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:when test="Symbol!='*'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Ticker>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<SecurityType>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'equityswap'"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Forward')">
								<xsl:value-of select="'fxforward'"/>
							</xsl:when>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'fxspot'"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="'equityoption'"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'equity'"/>
							</xsl:when>
							<!--<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="'FUT'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'CORP'"/>
							</xsl:when>-->
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<Side>
						<xsl:choose>
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="'buy'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="'sell'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<OpenClose>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:choose>
									<xsl:when test="contains(Side,'Open')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Close')">
										<xsl:value-of select="'close'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'short')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="contains(Asset,'EquityOption')">
								<xsl:choose>
									<xsl:when test="contains(Side,'Close')">
										<xsl:value-of select="'close'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Open')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'short')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
								
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						
							<xsl:when test="contains(Asset,'Equity')">
								<xsl:choose>
									<xsl:when test="contains(Side,'Close')">
										<xsl:value-of select="'close'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'short')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="'close'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="contains(Side,'Close')">
										<xsl:value-of select="'close'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'short')">
										<xsl:value-of select="'open'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="'close'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</OpenClose>

					<TransactionType>
						<xsl:choose>
							<xsl:when test ="TaxLotState='Allocated'">
								<xsl:value-of select ="'new'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState='Amended'">
								<xsl:value-of select ="'amend'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState='Deleted'">
								<xsl:value-of select ="'cancel'"/>
							</xsl:when>
							</xsl:choose>
					</TransactionType>

					<Quantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<Price>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Price>

					<Commission>
						<xsl:value-of select ="SoftCommissionCharged + CommissionCharged"/>
					</Commission>

					<Fees>
						<xsl:value-of select="OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
					</Fees>

					<NotionalAmount>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="GrossAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</NotionalAmount>

					<SettlementAmount>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettlementAmount>

					<Currency>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="LeadCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</Currency>

					<TradeDate>
						<xsl:value-of select ="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select ="SettlementDate"/>
					</SettleDate>

					<xsl:variable name="PB_NAME" select="'SOROS'"/> 
					
					<xsl:variable name = "PRANA_EXCHANGE_NAME">
						<xsl:value-of select="Exchange"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_EXCHANGE_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE_NAME]/@PBExchangeName"/>
					</xsl:variable>

					
					<Exchange>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_EXCHANGE_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_EXCHANGE_CODE"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Exchange>

					<Cusip>
						<xsl:value-of select="CUSIP"/>
					</Cusip>

					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>

					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<OCC>
						<xsl:value-of select="OSIOptionSymbol"/>
					</OCC>

					<OPRA>
						<xsl:value-of select="OpraOptionSymbol"/>
					</OPRA>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="'G2iP'"/>
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

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBrokerCode"/>
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
					
					<Counterparty>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'MSCO'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$Broker"/>
							</xsl:otherwise>
						</xsl:choose>
						
					
					</Counterparty>
					

					<Custodian>
						<xsl:value-of select="'MSPB'"/>
					</Custodian>

					<ExpiryDate>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>	
					</ExpiryDate>

					<MaturityDate>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlementDate"/>
							</xsl:otherwise>
						</xsl:choose>
					</MaturityDate>

					<Hedge>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'CASH'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Hedge>

					<QuoteConvention>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'Ccy1ToCcy2'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</QuoteConvention>

					<Currency2>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Currency2>

					<SpotRate>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select ="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SpotRate>

					<FxRate>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select ="AveragePrice"/>
							</xsl:when>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</FxRate>

					<Currency2Amount>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select ="NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					
					</Currency2Amount>




					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
