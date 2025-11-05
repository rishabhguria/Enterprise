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

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					

					<xsl:variable name="PB_NAME" select="'JP Morgan EOD'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<TRADETYPE>
						<xsl:value-of select="'BTS'"/>
					</TRADETYPE>

					<TRADINGACCT>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRADINGACCT>

					<TRADEDATE>
						<xsl:value-of select="TradeDateTime"/>
					</TRADEDATE>

					<SETTLEDATE>
						<xsl:value-of select="SettlementDate"/>
					</SETTLEDATE>

					<BUYSELLIND>
						<!--<xsl:choose>
							<xsl:when test="(Side='Buy' or Side='Buy to Open') and TaxLotState='Deleted'">
								<xsl:value-of select="'CANCELBL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell' or Side='Sell to Close') and TaxLotState='Deleted'">
								<xsl:value-of select="'CANCELSL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell short' or Side='Sell to Open') and TaxLotState='Deleted'">
								<xsl:value-of select="'CANCELSS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted' ">
								<xsl:value-of select="'CANCELBS'"/>
							</xsl:when>

							<xsl:when test="(Side='Buy' or Side='Buy to Open') and TaxLotState='Amended'">
								<xsl:value-of select="'UPDATEBL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell' or Side='Sell to Close') and TaxLotState='Amended'">
								<xsl:value-of select="'UPDATESL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell short' or Side='Sell to Open') and TaxLotState='Amended'">
								<xsl:value-of select="'UPDATESS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted' ">
								<xsl:value-of select="'UPDATEBS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' ">
										<xsl:value-of select="'BS'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>-->
				<xsl:choose>
					<xsl:when test="Side='Buy' or Side='Buy to Open'">
						<xsl:value-of select="'BUY'"/>
					</xsl:when>
					<xsl:when test="Side='Sell' or Side='Sell to Close'">
						<xsl:value-of select="'SELL'"/>
					</xsl:when>
					<xsl:when test="Side='Sell short' or Side='Sell to Open'">
						<xsl:value-of select="'SELL'"/>
					</xsl:when>
					<xsl:when test="Side='Buy to Close' ">
						<xsl:value-of select="'BUY'"/>
					</xsl:when>
				</xsl:choose>
				</BUYSELLIND>

					<!--<xsl:choose>
						<xsl:when test="TaxLotState='Allocated'">
							<xsl:value-of select ="'N'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Amemded'">
							<xsl:value-of select ="'A'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:value-of select ="'D'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>-->
					<!--<NewCancelCorrect>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'UPDATE'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'CANCEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NewCancelCorrect>-->

					<!--<PosType>
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
							<xsl:when test="Asset='PrivateEquity'">
								<xsl:value-of select="'PrivateEquity'"/>
							</xsl:when>
						</xsl:choose>
					</PosType>-->

					<!--<ID1>
						<xsl:choose>
							<xsl:when test="Asset='Equity'and CurrencySymbol!='USD'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='PrivateEquity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ID1>-->

					<!--<Blank2>
						<xsl:value-of select="''"/>
					</Blank2>-->

					<!--<Exchange>
						<xsl:value-of select="Exchange"/>
					</Exchange>-->

					<QUANTITY>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</QUANTITY>
					
					

					<SYMBOL>
						<xsl:choose>
							<xsl:when test="Asset='Equity'and CurrencySymbol!='USD'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='PrivateEquity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SYMBOL>

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
					
					<Price>
						<xsl:value-of select="format-number($Price,'0.####')"/>
						
					</Price>

					<xsl:variable name="Commission">
						<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) div AllocatedQty"/>
					</xsl:variable>

					<xsl:variable name="COMM">
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

					<COMM>
						<xsl:value-of select="format-number($COMM,'0.####')"/>

					</COMM>

					<RR>
						<xsl:value-of select="'DT7'"/>
					</RR>


					<BLOTTER>
						<xsl:value-of select="'18'"/>
					</BLOTTER>


					<TCODE1>
						<xsl:value-of select="'un'"/>
					</TCODE1>

					<xsl:variable name="i" select="position()" />				

					<TAG>
						<xsl:choose>
							<xsl:when test="$i &lt; 10">
								<xsl:value-of select="concat('A000',$i)"/>
							</xsl:when>
							<xsl:when test="$i &lt; 100">
								<xsl:value-of select="concat('A00',$i)"/>
							</xsl:when>
							<xsl:when test="$i &lt; 1000">
								<xsl:value-of select="concat('A0',$i)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat('A',$i)"/>
							</xsl:otherwise>
						</xsl:choose>
					</TAG>


					

					<FileHeader>
						<xsl:value-of select="'false'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'false'"/>
					</FileFooter>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>