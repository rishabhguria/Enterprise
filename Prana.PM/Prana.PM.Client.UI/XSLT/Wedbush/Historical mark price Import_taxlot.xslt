<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL10)">
				
					<PositionMaster>

			  <xsl:variable name="varSingleQuote">'</xsl:variable>
			  <xsl:variable name="varSymbol2" select="substring-after(COL2,$varSingleQuote)"/>
			  <xsl:variable name="varSymbol1" select="substring-before(COL2,$varSingleQuote)"/>
			  
			           <xsl:variable name="PB_NAME">
							<xsl:value-of select="'WedbushP'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="contains(COL2,$varSingleQuote)">
									<xsl:value-of select="concat($varSymbol1,'/','P',$varSymbol2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>
							</xsl:choose>
														
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="Costbasis" select = "number(COL10)"/>
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$Costbasis &gt; 0">
									<xsl:value-of select="$Costbasis"/>
								</xsl:when>
								<xsl:when test="$Costbasis &lt; 0">
									<xsl:value-of select="$Costbasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						
						<!--Date Field does not come in the Position file, so user will select from the UI -->
						<Date>
							<xsl:value-of select="''"/>
						</Date>

					</PositionMaster>
				</xsl:if>
				
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
