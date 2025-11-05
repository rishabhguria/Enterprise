<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public int Now(string date)
		{
		DateTime d1 = DateTime.Parse(date);
		DateTime d2 = DateTime.Today;

		int result = DateTime.Compare(d1,d2);
		return result;
		}

	</msxsl:script>

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
		
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Columbus LP' or AccountName='Columbus QP']">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'BTIG'"/>
					</xsl:variable>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>				
					<AccountID>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountID>

					<xsl:variable name="varMonth">
						<xsl:value-of select="substring-before(TradeDate,'/')"/>
					</xsl:variable>
					<xsl:variable name="varDay">
						<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
					</xsl:variable>
					<xsl:variable name="varYear">
						<xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
					</xsl:variable>	
					<TradeDate>
						<xsl:value-of select="concat($varYear,$varMonth,$varDay)"/>
					</TradeDate>
					
					<xsl:variable name="varSMonth">
						<xsl:value-of select="substring-before(SettlementDate,'/')"/>
					</xsl:variable>
					<xsl:variable name="varSDay">
						<xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
					</xsl:variable>
					<xsl:variable name="varSYear">
						<xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
					</xsl:variable>
					<SettlementDate>
						<xsl:value-of select="concat($varSYear,$varSMonth,$varSDay)"/>
					</SettlementDate>

					<ActionCode>
						<xsl:choose>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ActionCode>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<SettlementCcy>
						<xsl:value-of select="SettlCurrency"/>
					</SettlementCcy>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="BBCode!=''">
								<xsl:value-of select="BBCode"/>
							</xsl:when>				
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<TICKER>
						<xsl:choose>
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</TICKER>
					
					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<ExecBrokerCode>
						<xsl:value-of select="$Broker"/>
					</ExecBrokerCode>
					
					<xsl:variable name="varHardAndSoftCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					
					<CommissionPERSHARE>
						
							<xsl:value-of  select="format-number((CommissionCharged div AllocatedQty ),'0.######')"/>
						
						<!--<xsl:value-of select="$varHardAndSoftCommission"/>-->
					</CommissionPERSHARE>		
			
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>