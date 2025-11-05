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
					<xsl:value-of select="translate(COL11,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$varInstrumentType='50' or $varInstrumentType='60' or $varInstrumentType='70'">
					<PositionMaster>
						
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL13,'&quot;','')"/>
						<!--CompanyName and Symbol Section-->
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BOFA']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="($varInstrumentType='50' or $varInstrumentType='70') and $PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="$varInstrumentType='60'">
								<!-- QPXPHO-->
								<xsl:variable name="varAfterQ" >
									<xsl:value-of select="substring-after(COL5,'Q')"/>
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
							</xsl:when >
							<xsl:otherwise>
								<xsl:variable name="varAfterAstric" >
									<xsl:value-of select="substring-after(COL5,'*')"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test ="$varAfterAstric =''">
										<Symbol>
											<xsl:value-of select="COL5"/>
										</Symbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="$varAfterAstric"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="COL21 != 0 and COL29 != 0 ">
								<MarkPrice>
									<xsl:value-of select="COL21 * COL29"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="COL21"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:variable name = "varYR" >
							<xsl:value-of select="translate(substring(COL4,1,4),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varMth" >
							<xsl:value-of select="translate(substring(COL4,5,2),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varDt" >
							<xsl:value-of select="translate(substring(COL4,7,2),'&quot;','')"/>
						</xsl:variable>
						<Date>
							<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
						</Date>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
