<?xml version="1.0" encoding="utf-8"?>
									<!--Description- Sansato markprice Impoert
									    Date -07-12-2011
									-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">                    
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
   <xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="COL1 != 'Account GLI' and COL5='PortfolioSwap'">
				
					<PositionMaster>

						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL14,'&quot;','')"/>

						<PBSymbol>
							<xsl:value-of select="COL14"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SENSATO']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<CompanyName>
							<xsl:value-of select='COL12'/>
						</CompanyName>-->

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<SEDOL>
									<xsl:value-of select="COL37"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name="varDatePart">
							<xsl:value-of select="substring(COL3,1,10)"/>
						</xsl:variable>
												
						<Date>
							<xsl:value-of select="concat(substring-before(substring-after($varDatePart,'/'),'/'),'/',substring-before($varDatePart,'/'),'/',substring-after(substring-after($varDatePart,'/'),'/'))"/>
						</Date>
						
																	
						<xsl:choose>
							<xsl:when test ="number(COL64) and number(COL64) &lt; 0 ">
								<MarkPrice>
									<xsl:value-of select="COL64 * (-1)"/>
								</MarkPrice>		
							</xsl:when>
							<xsl:when test ="number(COL64) and number(COL64) &gt; 0 ">
								<MarkPrice>
									<xsl:value-of select="COL64"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>															
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>