<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL27"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='$$' and COL8 != 0">
					<PositionMaster>
						<!--   Fund -->
						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</LocalCurrency>

						<!--<xsl:choose>
							<xsl:when test ="boolean(number(COL5))">
								<CashValueBase>
									<xsl:value-of select="COL5"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >-->
						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL8))">
								<CashValueLocal>
									<xsl:value-of select="COL8"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:choose>
							<xsl:when test ="COL1='Date' or COL1='*' or COL1='ok'">
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
								<Date>
									<xsl:value-of select="concat(substring(COL1,5,2),'/',substring(COL1,7,8),'/',substring(COL1,1,4))"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
