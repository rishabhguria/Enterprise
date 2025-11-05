<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL9)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<!--<xsl:variable name="varSymbol">
							<xsl:value-of select="substring-before(COL22,'Index')"/>
						</xsl:variable>-->

						<xsl:variable name ="varBBCode">
							<xsl:value-of select ="normalize-space(substring(COL22, 1,2))"/>
						</xsl:variable>

						<xsl:variable name ="varBBKey">
							<xsl:value-of select ="normalize-space(substring(COL22, 6))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeName"/>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME=''">
									<xsl:value-of select="''"/>
								</xsl:when>
														
								<xsl:when test="string-length(substring-before(COL22,' '))=5">
									<xsl:value-of select="concat($PRANA_ROOT_NAME, ' ', substring(COL22,4,2), $PRANA_EXCHANGE_NAME)"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="concat($PRANA_ROOT_NAME, ' ', substring(COL22,3,2), $PRANA_EXCHANGE_NAME)"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>

						<!--<xsl:variable name="Root">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL22,' '))=4">
									<xsl:value-of select="concat($PRANA_ROOT_NAME, ' ', substring(COL22,3,2), $PRANA_EXCHANGE_NAME)"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(COL22,' '))=5">
									<xsl:value-of select="concat($PRANA_ROOT_NAME, ' ', substring(COL22,4,2), $PRANA_EXCHANGE_NAME)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>-->

						<!--<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>-->

						<MarkPrice>
							<xsl:value-of select="number(COL20)"/>
						</MarkPrice>

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
	

</xsl:stylesheet> 
