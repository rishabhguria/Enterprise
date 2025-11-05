<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL27"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='Stock' or $varInstrumentType ='Equity ADR' or $varInstrumentType ='Future' or $varInstrumentType ='Option' or $varInstrumentType ='Future Option'">
					<PositionMaster>
						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="translate(COL19,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Eleven11']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL3='USD' and ($varInstrumentType ='Stock' or $varInstrumentType ='Equity ADR')">
										<xsl:variable name ="strBeforeRIC">
											<xsl:value-of select="substring-before(COL9,'.')"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test ="$strBeforeRIC=''">
												<Symbol>
													<xsl:value-of select="COL9"/>
												</Symbol>
												<RIC>
													<xsl:value-of select="''"/>
												</RIC>
											</xsl:when>
											<xsl:otherwise>
												<Symbol>
													<xsl:value-of select="$strBeforeRIC"/>
												</Symbol>
												<RIC>
													<xsl:value-of select="''"/>
												</RIC>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test ="$varInstrumentType ='Future'">
										<xsl:variable name = "varLength" >
											<xsl:value-of select="string-length(COL9)"/>
										</xsl:variable>
										<xsl:variable name = "varAfter" >
											<xsl:value-of select="substring(COL9,($varLength)-1,2)"/>
										</xsl:variable>
										<xsl:variable name = "varBefore" >
											<xsl:value-of select="substring(COL9,1,($varLength)-2)"/>
										</xsl:variable>
										<Symbol>
											<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
										</Symbol>
										<RIC>
											<xsl:value-of select="''"/>
										</RIC>
									</xsl:when >
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<RIC>
											<xsl:value-of select="COL9"/>
										</RIC>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select="COL9"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL10))">
								<MarkPrice>
									<xsl:value-of select="COL10"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
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
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
