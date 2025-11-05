<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>



			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset!='FX' and Asset!='FXForward' and (AccountName='NT - IAMG' or AccountName='GS - D9MA' or AccountName='GS SWAP - D9MA') and (CurrencySymbol='USD' or CurrencySymbol='CAD' or CurrencySymbol='BRL'  or CurrencySymbol='CLP'  or CurrencySymbol='MXN') and (Symbol !='J37-SES']">

				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name ="PB_NAME">
						<xsl:value-of select="'NT'"/>
					</xsl:variable>

					<SIDE>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SIDE>

					<EXTERNALREFERENCEID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select="concat(EntityID,'A')"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="concat(EntityID,'D')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="EntityID"/>
							</xsl:otherwise>
						</xsl:choose>
					</EXTERNALREFERENCEID>

					<TRADEDATE>
						<xsl:value-of select="TradeDate"/>
					</TRADEDATE>

					<FINANCIALTYPE>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'COMMON'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="'FUTURE'"/>
							</xsl:when>
							<xsl:when test="Asset='FutureOption'">
								<xsl:value-of select="'FUTUREOPT'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'BOND'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</FINANCIALTYPE>

					<CURRENCY>
						<xsl:value-of select="CurrencySymbol"/>
					</CURRENCY>

					<INSTRUMENTIDENTIFIER>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</INSTRUMENTIDENTIFIER>

					<INSTRUMENTIDENTIFIERTYPE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="'OCC SYMBOL'"/>
							</xsl:when>
							<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="'SEDOL'"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="'ISIN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</INSTRUMENTIDENTIFIERTYPE>

					<xsl:variable name="varCounterPartyAndTradeCurrency">
						<xsl:choose>
							<xsl:when test ="CurrencySymbol='USD' or CurrencySymbol='CAD' or CurrencySymbol='BRL' or CurrencySymbol='CLP' or CurrencySymbol='MXN'">
								<xsl:value-of select="concat(CounterParty,'US')"/>
							</xsl:when>
							<xsl:when test ="CurrencySymbol='EUR' or CurrencySymbol='GBP' or CurrencySymbol='MXN' or CurrencySymbol='DKK' or CurrencySymbol='CHF' or CurrencySymbol='SEK' or CurrencySymbol='NOK' or CurrencySymbol='ISK' or CurrencySymbol='GBX' or CurrencySymbol='HUF' or CurrencySymbol='RON'">
								<xsl:value-of select="concat(CounterParty,'UK')"/>
							</xsl:when>
							<xsl:when test ="CurrencySymbol='JPY' or CurrencySymbol='PHP' or CurrencySymbol='HKD' or CurrencySymbol='INR' or CurrencySymbol='KRW' or CurrencySymbol='THB' or CurrencySymbol='SGD' or CurrencySymbol='CNY' or CurrencySymbol='AUD' or CurrencySymbol='NZD' or CurrencySymbol='ILS' or CurrencySymbol='EGP' or CurrencySymbol='IDR' or CurrencySymbol='MYR' or CurrencySymbol='VND'">
								<xsl:value-of select="concat(CounterParty,'HK')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<EXECUTINGCOUNTERPARTY>
						<xsl:choose>
							<!--<xsl:when test ="CounterParty ='CSFB' and (CurrencySymbol='EUR' or CurrencySymbol='GBP' or CurrencySymbol='MXN' or CurrencySymbol='DKK' or CurrencySymbol='CHF' or CurrencySymbol='SEK' or CurrencySymbol='NOK' or CurrencySymbol='ISK' or CurrencySymbol='GBX' or CurrencySymbol='HUF' or CurrencySymbol='RON')">
								<xsl:value-of select="concat('CSFL','UK')"/>
							</xsl:when>-->

							<xsl:when test="CounterParty ='CSFB' or CounterParty ='BERN'">
								<xsl:value-of select="$varCounterPartyAndTradeCurrency"/>
							</xsl:when>
							<xsl:when test="CounterParty ='JPMS' ">
								<xsl:value-of select="'JPMSUS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>

					</EXECUTINGCOUNTERPARTY>



					<QUANTITY>
						<xsl:value-of select="AllocatedQty"/>
					</QUANTITY>

					<PRICE>
						<xsl:value-of select="AveragePrice"/>
					</PRICE>

					<xsl:variable name ="varAllocationStateMSG">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'C'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'X'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<MSGTYPE>
						<xsl:value-of select="$varAllocationStateMSG"/>
					</MSGTYPE>

					<SETTLEDATE>
						<xsl:value-of select="SettlementDate"/>
					</SETTLEDATE>

					<PREVEXTERNALREFERENCEID>
						<xsl:choose>
							<xsl:when test="TaxLotState ='Amended' or TaxLotState ='Deleted'">
								<xsl:value-of select="EntityID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PREVEXTERNALREFERENCEID>

					<FEE_OTHER_FEE>

						<!--<xsl:choose>
							<xsl:when test="number(TaxOnCommissions + TransactionLevy + StampDuty) &gt; 0">
								<xsl:value-of select="number(TaxOnCommissions + TransactionLevy + StampDuty)"/>
							</xsl:when>
							<xsl:when test="number(TaxOnCommissions + TransactionLevy + StampDuty) &lt; 0">
								<xsl:value-of select="number(TaxOnCommissions + TransactionLevy + StampDuty)*(-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</FEE_OTHER_FEE>

					<NOTES>
						<xsl:value-of select="''"/>
					</NOTES>

					<CASHSUBACCOUNT>
						<!--<xsl:value-of select="'MARGIN'"/>-->
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'FUTURE'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SHORT'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="'MARGIN'"/>
							</xsl:otherwise>
						</xsl:choose>
					</CASHSUBACCOUNT>

					<CLEARINGACCOUNT>
						<xsl:choose>
							<xsl:when test="AccountName ='NT - IAMG' and IsSwapped!='true'">
								<xsl:value-of select="'NTRC-CUS-IAMG'"/>
							</xsl:when>
							<xsl:when test="AccountName ='GS - D9MA' and IsSwapped!='true'">
								<xsl:value-of select="'GSCO-DPB-D9MA'"/>
							</xsl:when>
							<xsl:when test="AccountName ='GS - D9MA' and IsSwapped='true'">
								<xsl:value-of select="'GSCO-ISD-D9MA'"/>
							</xsl:when>
							<xsl:when test="AccountName ='GS SWAP - D9MA' and IsSwapped='true'">
								<xsl:value-of select="'GSCO-ISD-D9MA'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>

					</CLEARINGACCOUNT>


					<COMMISSIONRATE>
						<xsl:value-of select="CommissionCharged"/>
					</COMMISSIONRATE>

					<COMMISSIONTYPE>
						<xsl:value-of select="'EXPLICIT'"/>
					</COMMISSIONTYPE>

					<DESK>
						<xsl:value-of select="'IAML'"/>
					</DESK>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name='NT']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<FUND>
						<xsl:choose>
							<xsl:when test="$PB_FUND_CODE!=''">
								<xsl:value-of select="$PB_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</FUND>

					<PB>
						<xsl:choose>
							<xsl:when test="AccountName='NT - IAMG'">
								<xsl:value-of select="'CASH:XXIAMGIAML0001'"/>
							</xsl:when>
							<xsl:when test="AccountName='GS - D9MA'">
								<xsl:value-of select="'CASH:XXD9MAIAML0001'"/>
							</xsl:when>
							<xsl:when test="AccountName='GS SWAP - D9MA'">
								<xsl:value-of select="'CASH:XXD9MAIAML0001'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</PB>

					<ISTRS>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</ISTRS>

					<OPTIONTYPE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="PutOrCall"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</OPTIONTYPE>

					<EXPIRYDATE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</EXPIRYDATE>
					
					<STRIKEPRICE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</STRIKEPRICE>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
