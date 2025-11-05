<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[OpenPositions !=0]">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<PORTFOLIONAME>
						<xsl:choose>
							<xsl:when test="LocalCurrency ='USD'">
								<xsl:choose>
									<xsl:when test="Asset='Equity' or Asset='Cash'">
										<xsl:value-of select="'BBG_US_Equity_Portfolio'"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="'BBG_US_EquityOption_Portfolio'"/>
									</xsl:when>
								</xsl:choose>								
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Asset='Equity' or Asset='Cash'">
										<xsl:value-of select="'BBG_INTL_Equity_Portfolio'"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="'BBG_INTL_EquityOption_Portfolio'"/>
									</xsl:when>
								</xsl:choose>								
							</xsl:otherwise>
						</xsl:choose>

					</PORTFOLIONAME>

					<MasterFund>
						<xsl:value-of select="MasterFundName"/>
					</MasterFund>

					<xsl:variable name="varSecurity">
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="concat(Symbol,' US')"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="substring-before(BloombergSymbol,' EQUITY')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SECURITYID>
						<xsl:value-of select="$varSecurity"/>
					</SECURITYID>

					<SECURITY_ID>
						<xsl:choose>
							<xsl:when test="LocalCurrency ='USD'">
								<xsl:value-of select="CUSIPSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</SECURITY_ID>

					<SecurityName>
						<xsl:value-of select="SecurityDescription"/>
					</SecurityName>

					<QUANTITYWEIGHT>
						<xsl:value-of select="OpenPositions"/>
					</QUANTITYWEIGHT>


					<Date>
						<xsl:value-of select="TradeDate"/>
					</Date>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>