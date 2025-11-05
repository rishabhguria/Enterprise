<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name = "varDate">
				<xsl:value-of select="PositionMaster[COL1='Business Date:']/COL2"/>
			</xsl:variable>
			<xsl:variable name = "varPortfolioID">
				<xsl:value-of select="PositionMaster[COL1='Fund:']/COL2"/>
			</xsl:variable>

			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL1,'&quot;','')"/>
				</xsl:variable>
				<xsl:variable name = "varCurrLen" >
					<xsl:value-of select="string-length(translate(COL12,'&quot;',''))"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType = 'Cash' and  $varCurrLen=3">
					<PositionMaster>
						<!--   Fund -->						
						<xsl:choose>
							<xsl:when test="$varPortfolioID='LUCAS TOP'">
								<FundName>
									<xsl:value-of select="'LP C/O'"/>
								</FundName>
							</xsl:when>
							<!--<xsl:when test="$varPortfolioID='30502'">
								<FundName>
									<xsl:value-of select="'OFFSHORE'"/>
								</FundName>
							</xsl:when>-->
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="' '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >					
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="translate(COL12,'&quot;','')"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="COL6 &lt; 0 or COL6 &gt; 0 or COL6 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL6"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:choose>
							<xsl:when test ="COL8 &lt; 0 or COL8 &gt; 0 or COL8 = 0">
								<CashValueBase>
									<xsl:value-of select="COL8"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >
						
						<Date>
							<xsl:value-of select="$varDate"/>
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
