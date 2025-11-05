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
					<xsl:value-of select="translate(COL5,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$varInstrumentType='EQUITY' or $varInstrumentType='OPTION'">
					<PositionMaster>
						<!--  Symbol Region -->

						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varInstrumentType='EQUITY' and $PRANA_SYMBOL_NAME != ''">								
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="$varInstrumentType='OPTION'">
								<!-- $PXPHO-->
								<xsl:variable name="varAfterDollar" >
									<xsl:value-of select="substring-after(COL8,'$')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterDollar)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterDollar,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterDollar,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
							</xsl:when >
							<xsl:otherwise>
								<xsl:variable name="varAfterAstric" >
									<xsl:value-of select="substring-after(COL8,'*')"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test ="$varAfterAstric =''">
										<Symbol>
											<xsl:value-of select="COL8"/>
										</Symbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="$varAfterAstric"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>
								<!--<Symbol>
									<xsl:value-of select="COL8"/>
								</Symbol>-->
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL12))">
								<MarkPrice>
									<xsl:value-of select="COL12"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="''"/>
						</Date>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
