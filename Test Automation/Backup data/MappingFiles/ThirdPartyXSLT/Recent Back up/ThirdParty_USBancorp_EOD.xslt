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

			<xsl:for-each select="ThirdPartyFlatFileDetail[contains(AccountName,'US')='true' or contains(AccountName,'LoCorr - Shorts - Morgan Stanley')='true']">

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
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='USBANKEOD']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
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
								<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CustomUDA4"/>
									</xsl:otherwise>
								</xsl:choose>
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

					<EXECPRICE>
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(SettlCurrAmt,'0.####')"/>
							</xsl:otherwise>
							<!--<xsl:when test="SettlCurrAmt=0">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(SettlCurrAmt,'0.####')"/>
							</xsl:otherwise>-->
						</xsl:choose>
					</EXECPRICE>

					<xsl:variable name="Commission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="COMMAMNT">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$Commission"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$Commission * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$Commission div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<COMMISSIONAMT>
						<xsl:value-of select="format-number($COMMAMNT,'0.##')"/>
					</COMMISSIONAMT>

					<xsl:variable name = "OthFees">
						<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee)"/>
						<!--<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy)"/>-->
					</xsl:variable>

					<xsl:variable name = "OTH">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$OthFees"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$OthFees * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$OthFees div SettlCurrFxRate"/>
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
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="SecFee"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="SecFee * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="SecFee div SettlCurrFxRate"/>
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

					<NETSETTLEMENTAMOUNT>
						<xsl:value-of select="format-number($NETAMNT,'0.##')"/>
					</NETSETTLEMENTAMOUNT>

					<xsl:variable name="Principal">
						<xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
					</xsl:variable>

					<xsl:variable name="PRINCIPAL">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$Principal"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$Principal * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$Principal div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PRINCIPAL>
						<xsl:value-of select="format-number($PRINCIPAL,'0.##')"/>
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
						<xsl:choose>

							<xsl:when test="CounterParty='MSCO'">
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

						</xsl:choose>

					</EXECUTINGBROKER>

					<EXECUTINGBROKERNAME>
						<xsl:value-of select="CounterParty"/>
					</EXECUTINGBROKERNAME>

					<EXECUTINGBROKERAC>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRIA'">
								<xsl:value-of select="'010124600 / 00'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'1280000'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='FRANCE'">
								<xsl:value-of select="'46741Y'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'120023710002'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='DENMARK'">
								<xsl:value-of select="'05295145376'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='ITALY'">
								<xsl:value-of select="'1154649'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRALIA'">
								<xsl:value-of select="'011-797750-066'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWEDEN'">
								<xsl:value-of select="'01-000-828-107'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SPAIN'">
								<xsl:value-of select="'71000'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWITZERLAND'">
								<xsl:value-of select="'US100201'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</EXECUTINGBROKERAC>

					<CLEARINGBROKER>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="$THIRDPARTY_BROKER"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRIA'">
								<xsl:value-of select="'BKAUATWW'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'7171'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'MSTKJPJX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='UNITED KINGDOM'">
								<xsl:value-of select="'50708'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='FRANCE'">
								<xsl:value-of select="'PARBFRPPXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'RBCT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='DENMARK'">
								<xsl:value-of select="'ESSEDKKK'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='ITALY'">
								<xsl:value-of select="'CITIITM1022'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='BRAZIL'">
								<xsl:value-of select="'ITAUBRSPINT '"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRALIA'">
								<xsl:value-of select="'HKBAAU2S'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWEDEN'">
								<xsl:value-of select="'ESSESESS'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SPAIN'">
								<xsl:value-of select="'PARBESMX '"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWITZERLAND'">
								<xsl:value-of select="'MSNYUS33ISL '"/>
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

								</xsl:choose>

						

					</CLEARINGBROKER>

					<CLEARINGBROKERAC>
						<xsl:value-of select="''"/>
					</CLEARINGBROKERAC>

					<PLACEOFSETTLEMENT>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="'USA'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRIA'">
								<xsl:value-of select="'OCSDATWW'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'DAKVDEFF'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'JJSDJPJT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='UNITED KINGDOM'">
								<xsl:value-of select="'CRSTGB22'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='FRANCE'">
								<xsl:value-of select="'SICVFRPP'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'CDSLCATT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='DENMARK'">
								<xsl:value-of select="'VPDKDKKK'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='ITALY'">
								<xsl:value-of select="'XTAEILIT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='BRAZIL'">
								<xsl:value-of select="'SSCSBRR1'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRALIA'">
								<xsl:value-of select="'CAETAU21'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWEDEN'">
								<xsl:value-of select="'VPCSSESS'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SPAIN'">
								<xsl:value-of select="'IBRCESMM'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWITZERLAND'">
								<xsl:value-of select="'INSECHZZ'"/>
							</xsl:when>
							

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
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
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</COMMENTS>

					<SECURITYTYPE>
						<xsl:choose>
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


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>