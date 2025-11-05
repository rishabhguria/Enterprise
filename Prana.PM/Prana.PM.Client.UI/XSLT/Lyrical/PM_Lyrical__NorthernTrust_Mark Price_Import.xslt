<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL4)">
					<PositionMaster>

						<xsl:variable name = "PB_NAME" >
							<xsl:value-of select ="'Test'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL18"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="COL20!='*'">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<CUSIP>
							<xsl:choose>
								<xsl:when test ="COL20!='*'">
									<xsl:value-of select ="COL20"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>


						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL4)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<Date>
							<xsl:value-of select ="COL3"/>
						</Date>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
