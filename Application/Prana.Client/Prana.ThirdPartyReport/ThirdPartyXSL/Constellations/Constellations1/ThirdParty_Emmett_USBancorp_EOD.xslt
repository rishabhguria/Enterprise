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

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Emmett Investment Management' and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>



					<xsl:variable name="PB_NAME" select="'US Bank'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
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

					<CUSIP>
						<xsl:choose>
							<xsl:when test="CurrencySymbol!='USD'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<!--<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CustomUDA4"/>
									</xsl:otherwise>
								</xsl:choose>-->
								<xsl:value-of select="CUSIP"/>
							</xsl:otherwise>
						</xsl:choose>

					</CUSIP>

					<SEDOL>
						<xsl:choose>
							<xsl:when test="SettlCurrency!='USD'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SEDOL"/>
							</xsl:otherwise>
						</xsl:choose>

					</SEDOL>
					<TICKER>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</TICKER>

					<TRADEDATE>
						<xsl:choose>
							<xsl:when test="substring(substring-before(TradeDate,'/'),1,1)='0'">
								<xsl:value-of select="concat(substring(substring-before(TradeDate,'/'),2,1),'/',substring-before(substring-after(TradeDate,'/'),'/'),'/',substring-after(substring-after(TradeDate,'/'),'/'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="TradeDate"/>
							</xsl:otherwise>
						</xsl:choose>

					</TRADEDATE>

					<SETTLEDATE>
						<xsl:choose>
							<xsl:when test="substring(substring-before(SettlementDate,'/'),1,1)='0'">
								<xsl:value-of select="concat(substring(substring-before(SettlementDate,'/'),2,1),'/',substring-before(substring-after(SettlementDate,'/'),'/'),'/',substring-after(substring-after(SettlementDate,'/'),'/'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlementDate"/>
							</xsl:otherwise>
						</xsl:choose>

					</SETTLEDATE>

					<QTYPARVALUE>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</QTYPARVALUE>

					<ORIGFACEAMT>
						<xsl:value-of select="''"/>
					</ORIGFACEAMT>

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

					<EXECPRICE>
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varSettFxAmt,'0.####')"/>
							</xsl:otherwise>
							<!--<xsl:when test="SettlCurrAmt=0">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(SettlCurrAmt,'0.####')"/>
							</xsl:otherwise>-->
						</xsl:choose>
					</EXECPRICE>


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
					
					<xsl:variable name="Commission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="COMMAMNT">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="$Commission"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="$Commission * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="$Commission div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<COMMISSIONAMT>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='JPY'">
								<xsl:value-of select="format-number($COMMAMNT,'0.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($COMMAMNT,'0.##')"/>
							</xsl:otherwise>
						</xsl:choose>
					</COMMISSIONAMT>

					<xsl:variable name = "OthFees">
						<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee)"/>
						<!--<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy)"/>-->
					</xsl:variable>

					<xsl:variable name = "OTH">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="$OthFees"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="$OthFees * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="$OthFees div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<OTHERFEES>
						<xsl:value-of select="format-number($OTH,'0.##')"/>
					</OTHERFEES>

					<EXCHANGEFEES>
						<xsl:value-of select="''"/>
					</EXCHANGEFEES>


					<xsl:variable name = "SECF">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="SecFee"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="SecFee * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="SecFee div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<SECFEES>
						<xsl:value-of select="format-number($SECF,'0.##')"/>
					</SECFEES>

			

					<xsl:variable name = "NETAMNT">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>      								
							            <xsl:when test="contains(Asset,'FixedIncome') and (Side='Buy' or Side='Buy to Close')">
											<xsl:value-of select="NetAmount + AccruedInterest"/>
										</xsl:when>

										<xsl:when test="contains(Asset,'FixedIncome') and (Side='Sell' or Side='Sell short')">
											<xsl:value-of select="NetAmount - AccruedInterest"/>
										</xsl:when>				
							
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="NetAmount * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="NetAmount div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NETSETTLEMENTAMOUNT>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='JPY'">
								<xsl:value-of select="format-number($NETAMNT,'0.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($NETAMNT,'0.###')"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</NETSETTLEMENTAMOUNT>

					<xsl:variable name="Principal">
						<xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
					</xsl:variable>

					<xsl:variable name="PRINCIPAL">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="$Principal"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="$Principal * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="$Principal div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PRINCIPAL>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='JPY'">
								<xsl:value-of select="format-number($PRINCIPAL,'0.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($PRINCIPAL,'0.##')"/>
							</xsl:otherwise>
						</xsl:choose>
					</PRINCIPAL>



					<ACCRUEDINTEREST>
						<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
					</ACCRUEDINTEREST>

					<TRANCODE>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRANCODE>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:variable name="THIRDPARTY_BROKER">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@DTCCode"/>
					</xsl:variable>

					<EXECUTINGBROKER>
						<!--<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="$THIRDPARTY_BROKER"/>
							</xsl:when>
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'RBCT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='BRITAIN'">
								<xsl:value-of select="'50706'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'MSNYUS33'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'MSNYUS33'"/>
							</xsl:when>
						
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER != ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_BROKER_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>

						</xsl:choose>-->
						<xsl:value-of select="''"/>

					</EXECUTINGBROKER>

					<EXECUTINGBROKERNAME>
						<xsl:value-of select="CounterParty"/>
					</EXECUTINGBROKERNAME>

					<EXECUTINGBROKERAC>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="''"/>
							</xsl:when>
						
							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'64-92561'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</EXECUTINGBROKERAC>

					<CLEARINGBROKER>
						<!--<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="$THIRDPARTY_BROKER"/>
							</xsl:when>

							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'MSFFDEFX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'MSTKJPJX'"/>
							</xsl:when>
							
							<xsl:when test="UDACountryName='BRITAIN'">
								<xsl:value-of select="'50706'"/>
							</xsl:when>
							
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'RBCT'"/>
							</xsl:when>
							

									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$THIRDPARTY_BROKER != ''">
												<xsl:value-of select="$THIRDPARTY_BROKER"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$PRANA_BROKER_NAME"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>

								</xsl:choose>-->

						<xsl:value-of select="''"/>

					</CLEARINGBROKER>

					<CLEARINGBROKERAC>
						<xsl:value-of select="''"/>
					</CLEARINGBROKERAC>

					<PLACEOFSETTLEMENT>
						<xsl:value-of select="SettlCurrency"/>
					</PLACEOFSETTLEMENT>

					<EXCHRATEIND>
						<xsl:value-of select="''"/>
					</EXCHRATEIND>

					<EXECUTEFX>
						<xsl:value-of select="'N'"/>
					</EXECUTEFX>

					<FOREIGNCCYISOOFTRADE>
						<xsl:value-of select="SettlCurrency"/>
					</FOREIGNCCYISOOFTRADE>

					<SECURITYDESCRIPTION>
						<xsl:value-of select="FullSecurityName"/>
					</SECURITYDESCRIPTION>

					<COMMENTS>
						<!--xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
							<xsl:value-of select="''"/>
					</COMMENTS>

					<SECURITYTYPE>
						<xsl:choose>
							<xsl:when test="contains(Symbol,'SWAP')">
								<xsl:value-of select="'ES'"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'E'"/>
							</xsl:when>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'F'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'O'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SECURITYTYPE>

					<TRANSACTIONFXRATE>
						<xsl:value-of select="''"/>
					</TRANSACTIONFXRATE>

					<FXBUYCCYISO>
						<xsl:value-of select="''"/>
					</FXBUYCCYISO>

					<FXBUYCCYAMOUNT>
						<xsl:value-of select="''"/>
					</FXBUYCCYAMOUNT>

					<FXBUYCCYDELIVERINGBIC>
						<xsl:value-of select="''"/>
					</FXBUYCCYDELIVERINGBIC>

					<FXSELLCCYISO>
						<xsl:value-of select="''"/>
					</FXSELLCCYISO>

					<FXSELLCCYAMOUNT>
						<xsl:value-of select="''"/>
					</FXSELLCCYAMOUNT>

					<FXSELLCCYACCOUNTWITHINSTITUTIONBIC>
						<xsl:value-of select="''"/>
					</FXSELLCCYACCOUNTWITHINSTITUTIONBIC>

					<FXSELLCCYBENEFICIARYACCT>
						<xsl:value-of select="''"/>
					</FXSELLCCYBENEFICIARYACCT>

					<FXSELLCCYBENEFICIARYBIC>
						<xsl:value-of select="''"/>
					</FXSELLCCYBENEFICIARYBIC>

					<FXSELLCCYFFCINFORMATION>
						<xsl:value-of select="''"/>
					</FXSELLCCYFFCINFORMATION>


					<xsl:variable name="varTaxlotStateTx">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'COR'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'CAN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<TRANSACTIONSTATUS>
						
						<xsl:value-of select="$varTaxlotStateTx"/>
					</TRANSACTIONSTATUS>

					<TRANSACTIONSID>
						<xsl:value-of select="PBUniqueID"/>
					</TRANSACTIONSID>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>