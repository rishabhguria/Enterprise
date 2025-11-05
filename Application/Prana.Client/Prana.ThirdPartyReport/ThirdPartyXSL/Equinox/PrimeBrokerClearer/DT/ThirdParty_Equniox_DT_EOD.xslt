<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
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

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<Process>
						<xsl:value-of select="'Y'"/>
					</Process>
					<xsl:variable name="PB_NAME" select="'GS'"/>
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

					<Account_Type>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Sell'">
								<xsl:value-of select="'Margin'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Sell Short'">
								<xsl:value-of select="'Short'"/>
							</xsl:when>
						</xsl:choose>
					</Account_Type>

					<Security>
						<xsl:value-of select="FullSecurityName"/>
					</Security>

					<Trade_Type>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Cover'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Trade_Type>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Trade>
						<xsl:value-of select="AveragePrice"/>
					</Trade>

					<CCY>
						<xsl:value-of select="CurrencySymbol"/>
					</CCY>

					<Commission_Type>
						<xsl:value-of select="'3'"/>
					</Commission_Type>

					<Commission_Value>
						<xsl:value-of select="CommissionCharged"/>
					</Commission_Value>

					<!--<Front_Exec>
						<xsl:value-of select="''"/>
					</Front_Exec>-->


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
