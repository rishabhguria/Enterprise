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

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountMappedName = 'CPB10740']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>
					
					<Transaction_Type_Indicator>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="'ST'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'BS'"/>
							</xsl:otherwise>
						</xsl:choose>
					</Transaction_Type_Indicator>
					
					<Client_Ref>
						<xsl:value-of select="EntityID"/>
					</Client_Ref>
					
					<Shaped_Trade_Ref>
						<xsl:value-of select="''"/>
					</Shaped_Trade_Ref>

					<xsl:variable name="PB_NAME" select="'SSC'"/>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

					<xsl:variable name="THIRDPARTY_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					
					<Account_Number>
						<!--<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="AccountMappedName"/>
					</Account_Number>
					
					<Trade_Version>
						<xsl:choose>
							<xsl:when test="TaxLotState = 'Allocated'">
								<xsl:value-of select="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState = 'Amended'">
								<xsl:value-of select="'CORRECT'"/>
							</xsl:when>
							<xsl:when test="TaxLotState = 'Deleted'">
								<xsl:value-of select="'CANCEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Trade_Version>

					<xsl:variable name="TYear" select="substring-after(substring-after(TradeDate,'/'),'/')"/>

					<xsl:variable name="TMonth" select="substring-before(TradeDate,'/')"/>

					<xsl:variable name="TDay" select="substring-before(substring-after(TradeDate,'/'),'/')"/>

					<Trade_Date>
						<!--<xsl:value-of select="concat($TDay,'/',$TMonth,'/',$TYear)"/>-->
						<xsl:value-of select="concat($TYear,$TMonth,$TDay)"/>
					</Trade_Date>

					<xsl:variable name="SYear" select="substring-after(substring-after(SettlementDate,'/'),'/')"/>

					<xsl:variable name="SMonth" select="substring-before(SettlementDate,'/')"/>

					<xsl:variable name="SDay" select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
					
					<Settlement_Date>
						<xsl:value-of select="concat($SYear,$SMonth,$SDay)"/>
					</Settlement_Date>
					
					<BS_Indicator>
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
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BS_Indicator>
					
					<Security_Indicator_Type>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OSI'"/>
							</xsl:when>

							<xsl:when test="ISIN!=''">
								<xsl:value-of select="'IS'"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="'SD'"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="'CU'"/>
							</xsl:when>
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="'SY'"/>
							</xsl:when>
							<xsl:when test="BBCode!=''">
								<xsl:value-of select="'BT'"/>
							</xsl:when>
							<xsl:when test="RIC!=''">
								<xsl:value-of select="'RIC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'SY'"/>
							</xsl:otherwise>
						</xsl:choose>
					</Security_Indicator_Type>
					
					<Security_Val>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="BBCode!=''">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="RIC!=''">
								<xsl:value-of select="RIC"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Security_Val>
					
					<Security_Description>
						<xsl:value-of select="FullSecurityName"/>
					</Security_Description>
					
					<Issue_Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Issue_Currency>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="PB_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>
					
					<Broker>
						<!--<xsl:choose>
							<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="'IPL'"/>
					</Broker>
					
					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>
					
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>
					
					<Commission_Type>
						<xsl:value-of select="''"/>
					</Commission_Type>
					
					<Commission_Value>
						<xsl:value-of select="CommissionCharged"/>
					</Commission_Value>

					<xsl:variable name="Tax" select="OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
					
					<Tax>
						<xsl:value-of select="$Tax"/>
					</Tax>
					
					<Proceeds>
						<xsl:value-of select="NetAmount"/>
					</Proceeds>

					<Proceeds_Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Proceeds_Currency>
					
					<Interest>
						<xsl:value-of select="AccruedInterest"/>
					</Interest>
					
					<Prefigured_Indicator>
						<xsl:value-of select="'YES'"/>
					</Prefigured_Indicator>

					<Settlement_Location>
						<xsl:value-of select="''"/>
					</Settlement_Location>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>