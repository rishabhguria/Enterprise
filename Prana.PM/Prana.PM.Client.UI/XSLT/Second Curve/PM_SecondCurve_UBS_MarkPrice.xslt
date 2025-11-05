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
				<xsl:variable name = "varAssetType" >
					<xsl:value-of select="translate(COL2,' ','')"/>
				</xsl:variable>
				
				<xsl:if test="$varAssetType='Equity' or $varAssetType='Option'">
					<PositionMaster>
						<!--  Symbol Region -->
						<xsl:choose>
							<xsl:when test ="$varAssetType='Equity'">
								<Symbol>
									<xsl:value-of select="translate(COL5, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="$varAssetType='Option'">
								<xsl:variable name="varAfterQ" >
									<xsl:value-of select="translate(substring-after(COL5,'Q'),' ','')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterQ)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="translate(COL5, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL8))">
								<MarkPrice>
									<xsl:value-of select="COL8"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						<!-- Position Date mapped with the column 10 -->
						<Date>
							<xsl:value-of select="''"/>
						</Date>
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
