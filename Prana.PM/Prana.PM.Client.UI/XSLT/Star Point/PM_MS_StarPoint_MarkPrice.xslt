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
				<xsl:if test="($varInstrumentType='EQTY' and $varInstrumentTypeDesc='Equity') or ($varInstrumentType='CALLL' and $varInstrumentTypeDesc='Call - Listed') or ($varInstrumentType='PUTL' and $varInstrumentTypeDesc='Put - Listed') or ($varInstrumentType='FUTUR' and $varInstrumentTypeDesc='FUTURES') or ($varInstrumentType='FORWA' and $varInstrumentTypeDesc='FUTURES') or ($varInstrumentType='STOCK' and $varInstrumentTypeDesc='EQUITY SWAP') or ($varInstrumentType='STOCK' and $varInstrumentTypeDesc='EQUITY SWAP FINANCING')">
					<PositionMaster>
						<!--  Symbol Region -->
						<Symbol>
							<xsl:value-of select="translate(COL8, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
						</Symbol>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL30))">
								<MarkPrice>
									<xsl:value-of select="COL30"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						<!-- Position Date mapped with the column 1 -->
						<xsl:choose>
							<xsl:when test ="COL1='*' or COL1='MAC001RX - Normalized Rollup Extract' or COL1='038C54502'">
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
								<Date>
									<xsl:value-of select="translate(COL1,'&quot;','')"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
