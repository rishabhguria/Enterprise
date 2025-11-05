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
					<xsl:value-of select="translate(COL50,'&quot;','')"/>
				</xsl:variable>
				<xsl:variable name = "varInstrumentTypeDesc" >
					<xsl:value-of select="translate(COL51,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="($varInstrumentType='CASH' and $varInstrumentTypeDesc='CASH')">
					<PositionMaster>
						<FundName>
							<xsl:value-of select="'LP C/O'"/>
						</FundName>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="translate(COL44,'&quot;','')"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL32))">
								<CashValueLocal>
									<xsl:value-of select="COL32"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >
						
						<xsl:choose>
							<xsl:when test ="boolean(number(COL33))">
								<CashValueBase>
									<xsl:value-of select="COL33"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >						

						<Date>
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</Date>
						<PositionType>
							<xsl:value-of select="$varInstrumentType"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
