<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				
					<PositionMaster>

						
						
						
						
						
						<xsl:variable name ="varPBSymbol" >
							<xsl:choose>
								<xsl:when test ="COL34='Equity' and substring-after(COL61,' ')='US'">
									<xsl:value-of select ="substring-before(COL61,' ')"/>
								</xsl:when>
								<xsl:when test ="COL34='Equity' and substring-after(COL61,' ')!='US'">
									<xsl:value-of select ="COL61"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PB_COMPANY_NAME" select="$varPBSymbol"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SUNGARD']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
											
						
						
						<xsl:variable name="varOptionString" select="COL61"/>
						<xsl:variable name="varOptionString_length" select="string-length($varOptionString)"/>
						<xsl:variable name="varRoot" select="substring($varOptionString,1,$varOptionString_length -15)"/>
						<xsl:variable name="varRemainingString" select="substring($varOptionString,$varOptionString_length -14)"/>

						<xsl:variable name = "BlankCount_Root" >
							<xsl:call-template name="noofBlanks">
								<xsl:with-param name="count1" select="(6) - string-length($varRoot)" />
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:choose>
							
							<xsl:when test ="COL34='Option'">
								<IDCOOptionSymbol>
									<xsl:value-of select="concat($varRoot,$BlankCount_Root,$varRemainingString,'U')"/>
								</IDCOOptionSymbol>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>
							
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="$varPBSymbol"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
								<xsl:when  test="boolean(number(COL22))">
									<MarkPrice>
										<xsl:value-of select="COL22"/>
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
		
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


