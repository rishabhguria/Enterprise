<?xml version="1.0" encoding="UTF-8"?>
										<!-- Object -Trade Recon for GS, Date -01-11-2012(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL12 = 'OP' and COL1 != 'DATA : Custody Positions with Aggregated CFDs (Exp Price)' and COL1 != 'Advisor:' and COL1 != 'Fund:' and COL1 != 'Business Date:' and COL1 != 'Run Date:' and COL1 != 'Report Code:' and COL1 != 'Custody Group Mnemonic' ">
					<PositionMaster>

						<Date>
							<xsl:value-of select ="''"/>
						</Date>

						<!--SYMBOL-->
						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL11)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SENSATO']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="concat('SEDOL - ',COL41, ', Description - ',COL11)"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:when test="COL41 != ''">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="concat('SEDOL - ',COL41, ', Description - ',COL13)"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="COL41"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="concat('SEDOL - ',COL41, ', Description - ',COL13)"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>

						<!--Mark Price-->


						<xsl:choose>
							<xsl:when  test="number(COL23)">
								<MarkPrice>
									<xsl:value-of select="COL23"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
