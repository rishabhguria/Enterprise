<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL7)">
					<PositionMaster>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='LCM']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME=''">
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						
						<Date>
							<xsl:value-of select="''"/>
						</Date>

						

						<xsl:variable name="varMarkPrice">
							<xsl:choose>
								<xsl:when test="number(COL7)!=0">
									<xsl:value-of select="COL11 div COL7"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>
						
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($varMarkPrice) &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="number($varMarkPrice) &lt; 0">
									<xsl:value-of select="$varMarkPrice*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						

						<PBSymbol>
							<xsl:value-of select="COL5"/>
						</PBSymbol>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
