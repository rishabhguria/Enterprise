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

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxlotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxlotState>




				<Account>
					<xsl:value-of select="'Account'"/>
				</Account>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<TRADEDATE>
					<xsl:value-of select="'TRADEDATE'"/>
				</TRADEDATE>

				<SETTLEDATE>
					<xsl:value-of select="'SETTLEDATE'"/>
				</SETTLEDATE>

				<QTYPARVALUE>
					<xsl:value-of select="'QTYPARVALUE'"/>
				</QTYPARVALUE>

				<ORIGFACEAMT>
					<xsl:value-of select="'ORIGFACEAMT'"/>
				</ORIGFACEAMT>

				<EXECPRICE>
					<xsl:value-of select="'EXECPRICE'"/>
				</EXECPRICE>			

				<COMMISSIONAMT>
					<xsl:value-of select="'COMMISSIONAMT'"/>
				</COMMISSIONAMT>
				
				<OTHERFEES>
					<xsl:value-of select="'OTHERFEES'"/>
				</OTHERFEES>

				<EXCHANGEFEES>
					<xsl:value-of select="'EXCHANGEFEES'"/>
				</EXCHANGEFEES>

				<SECFEES>
					<xsl:value-of select="'SECFEES'"/>
				</SECFEES>

				<NETSETTLEMENTAMOUNT>
					<xsl:value-of select="'NETSETTLEMENTAMOUNT'"/>
				</NETSETTLEMENTAMOUNT>			

				<PRINCIPAL>
					<xsl:value-of select="'PRINCIPAL'"/>
				</PRINCIPAL>

				<ACCRUEDINTEREST>
					<xsl:value-of select="'ACCRUEDINTEREST'"/>
				</ACCRUEDINTEREST>

				<TRANCODE>
					<xsl:value-of select="'TRANCODE'"/>
				</TRANCODE>				

				<EXECUTINGBROKER>
					<xsl:value-of select="'EXECUTINGBROKER'"/>
				</EXECUTINGBROKER>

				<EXECUTINGBROKERNAME>
					<xsl:value-of select="'EXECUTINGBROKERNAME'"/>
				</EXECUTINGBROKERNAME>

				<EXECUTINGBROKERAC>
					<xsl:value-of select="'EXECUTINGBROKERAC'"/>
				</EXECUTINGBROKERAC>

				<CLEARINGBROKER>
					<xsl:value-of select="'CLEARINGBROKER'"/>
				</CLEARINGBROKER>

				<CLEARINGBROKERAC>
					<xsl:value-of select="'CLEARINGBROKERAC'"/>
				</CLEARINGBROKERAC>

				<PLACEOFSETTLEMENT>
					<xsl:value-of select="'PLACEOFSETTLEMENT'"/>
				</PLACEOFSETTLEMENT>

				<EXCHRATEIND>
					<xsl:value-of select="'EXCHRATEIND'"/>
				</EXCHRATEIND>

				<EXECUTEFX>
					<xsl:value-of select="'EXECUTEFX'"/>
				</EXECUTEFX>

				<FOREIGNCCYISOOFTRADE>
					<xsl:value-of select="'FOREIGNCCYISOOFTRADE'"/>
				</FOREIGNCCYISOOFTRADE>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITYDESCRIPTION'"/>
				</SECURITYDESCRIPTION>

				<COMMENTS>
					<xsl:value-of select="'COMMENTS'"/>
				</COMMENTS>

				<SECURITYTYPE>
					<xsl:value-of select="'SECURITYTYPE'"/>
				</SECURITYTYPE>

				<TRANSACTIONFXRATE>
					<xsl:value-of select="'TRANSACTIONFXRATE'"/>
				</TRANSACTIONFXRATE>

				<FXBUYCCYISO>
					<xsl:value-of select="'FXBUYCCYISO'"/>
				</FXBUYCCYISO>

				<FXBUYCCYAMOUNT>
					<xsl:value-of select="'FXBUYCCYAMOUNT'"/>
				</FXBUYCCYAMOUNT>

				<FXBUYCCYDELIVERINGBIC>
					<xsl:value-of select="'FXBUYCCYDELIVERINGBIC'"/>
				</FXBUYCCYDELIVERINGBIC>

				<FXSELLCCYISO>
					<xsl:value-of select="'FXSELLCCYISO'"/>
				</FXSELLCCYISO>

				<FXSELLCCYAMOUNT>
					<xsl:value-of select="'FXSELLCCYAMOUNT'"/>
				</FXSELLCCYAMOUNT>

				<FXSELLCCYACCOUNTWITHINSTITUTIONBIC>
					<xsl:value-of select="'FXSELLCCYACCOUNTWITHINSTITUTIONBIC'"/>
				</FXSELLCCYACCOUNTWITHINSTITUTIONBIC>

				<FXSELLCCYBENEFICIARYACCT>
					<xsl:value-of select="'FXSELLCCYBENEFICIARYACCT'"/>
				</FXSELLCCYBENEFICIARYACCT>

				<FXSELLCCYBENEFICIARYBIC>
					<xsl:value-of select="'FXSELLCCYBENEFICIARYBIC'"/>
				</FXSELLCCYBENEFICIARYBIC>

				<FXSELLCCYFFCINFORMATION>
					<xsl:value-of select="'FXSELLCCYFFCINFORMATION'"/>
				</FXSELLCCYFFCINFORMATION>


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<!--<xsl:for-each select="ThirdPartyFlatFileDetail">-->

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='TTAI']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>


					<xsl:variable name="PB_NAME" select="'USBank'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountMappedName"/>
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

					<CUSIP>
						<xsl:choose>
							<xsl:when test="CurrencySymbol = 'USD' and SettlCurrency = 'USD'">
								<xsl:choose>
									<xsl:when test="CUSIP =''">
										<xsl:value-of select="CustomUDA4"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CUSIP"/>
									</xsl:otherwise>
								</xsl:choose>								
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</CUSIP>

					<SEDOL>
						<xsl:choose>
							<xsl:when test="CurrencySymbol != 'USD' and SettlCurrency != 'USD'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
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


					<xsl:variable name="Price">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlCurrAmt"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>
					<EXECPRICE>
						<xsl:value-of select="format-number($Price,'0.####')"/>				
						
					</EXECPRICE>

					<xsl:variable name="Commission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="varCOMM">
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
						<xsl:value-of select="$varCOMM"/>
					</COMMISSIONAMT>

					<xsl:variable name="Fees">
						<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee)"/>
					</xsl:variable>

					<xsl:variable name="varFEE1">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$Fees"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$Fees * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$Fees div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OTHERFEES>
						<xsl:value-of select="$varFEE1"/>
					</OTHERFEES>

					<EXCHANGEFEES>
						<xsl:value-of select="''"/>
					</EXCHANGEFEES>

					<xsl:variable name="varFEE2">
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
					  <xsl:value-of select="format-number($varFEE2,'#.##')"/>							
					</SECFEES>

					<xsl:variable name="varNetAmount">
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
								<xsl:value-of select="format-number($varNetAmount,'#.##')"/>							
					</NETSETTLEMENTAMOUNT>

					<xsl:variable name="varPrincipal">
						<xsl:value-of select="AllocatedQty * AveragePrice"/>
					</xsl:variable>

					<xsl:variable name="varPrincipalAmount">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$varPrincipal"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varPrincipal * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varPrincipal div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<PRINCIPAL>						
							<xsl:value-of select="$varPrincipalAmount"/>						
					</PRINCIPAL>

					<ACCRUEDINTEREST>
						<xsl:value-of select="AccruedInterest"/>
					</ACCRUEDINTEREST>

					<TRANCODE>
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
					</TRANCODE>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
					</xsl:variable>
					
					<EXECUTINGBROKER>
						<xsl:choose>
							<xsl:when test="contains(CounterParty,'CANT')">
								<xsl:value-of select="'596'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
										<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_BROKER_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</EXECUTINGBROKER>

					<EXECUTINGBROKERNAME>

						<xsl:choose>
							<xsl:when test="contains(CounterParty,'CANT')">
								<xsl:value-of select="'Cantor'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</EXECUTINGBROKERNAME>

					<EXECUTINGBROKERAC>
						<xsl:value-of select="''"/>
					</EXECUTINGBROKERAC>

					<CLEARINGBROKER>
						<xsl:choose>
							<xsl:when test="contains(CounterParty,'CANT')">
								<xsl:value-of select="'596'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
										<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
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
								<xsl:value-of select="UDACountryName"/>						
					</PLACEOFSETTLEMENT>

					<EXCHRATEIND>
						<xsl:value-of select="''"/>
					</EXCHRATEIND>

					<EXECUTEFX>
						<xsl:choose>
							<xsl:when test="CurrencySymbol = SettlCurrency">
								<xsl:value-of select="'Y'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'N'"/>
							</xsl:otherwise>
						</xsl:choose>
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

						<xsl:choose>
							<xsl:when test="CurrencySymbol != SettlCurrency">
								<xsl:value-of select="SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

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