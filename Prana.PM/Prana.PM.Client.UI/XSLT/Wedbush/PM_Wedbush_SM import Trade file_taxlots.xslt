<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:if test="number(COL1)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Demo'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="varSingleQuote">'</xsl:variable>
               <xsl:variable name="varSymbol2" select="substring-after(COL2,$varSingleQuote)"/>
			  <xsl:variable name="varSymbol1" select="substring-before(COL2,$varSingleQuote)"/>
						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL2,$varSingleQuote)">
									<xsl:value-of select="concat($varSymbol1,'/','P',$varSymbol2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>


						<TickerSymbol>
							<xsl:value-of select="$varSymbol"/>
						</TickerSymbol>


						<Multiplier>
							<xsl:value-of select="'1'"/>
						</Multiplier>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="contains(COL2,$varSingleQuote)">
									<xsl:value-of select="substring-before(COL2,$varSingleQuote)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<UnderLyingSymbol>
							<xsl:value-of select="$varUnderlying"/>
						</UnderLyingSymbol>


						<AUECID>
							<xsl:value-of select="15"/>
						</AUECID>

						<LongName>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</LongName>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
