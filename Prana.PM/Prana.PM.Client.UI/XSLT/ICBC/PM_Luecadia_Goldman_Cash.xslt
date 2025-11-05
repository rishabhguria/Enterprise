<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name ="CurrencyCode">
		<xsl:param name="varcurrencyDes"/>
		<xsl:choose>
			
			<xsl:when test="$varcurrencyDes='SO AFRICAN RAND'">
				<xsl:value-of select="'ZAR'"/>
			</xsl:when>
			<xsl:when test="$varcurrencyDes='U S DOLLAR'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			
			<xsl:for-each select="PositionMaster">
				<xsl:if test ="number(COL6)">
					
				
					<PositionMaster>

						<xsl:variable name="varLocalCurrency">
							<xsl:call-template name="CurrencyCode">
								<xsl:with-param name="varcurrencyDes" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>
						
						<!--   Fund -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>				
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>						
								<xsl:value-of select="$varLocalCurrency"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL6)) and COL6 != 'Trade Date Quantity'">
								<CashValueLocal>
									<xsl:value-of select="COL6"/>
								</CashValueLocal>
								<CashValueBase>
									<xsl:value-of select="COL6"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >
						
						<Date>
							<xsl:value-of select="''"/>
						</Date>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	
</xsl:stylesheet>
