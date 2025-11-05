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

            <!--<xsl:variable name="OptionUnderlyingSymbol">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='60'">
                  <xsl:variable name="varAfterQ" select="substring-after(COL5,'Q')"/>
                  <xsl:variable name="OpraCode" select="substring($varAfterQ,1,3)"/>
                  <xsl:value-of select="document('../ReconMappingXml/UnderlyingSymbolMapping.xml')/SymbolMapping/PB[@Name='BOFA']/SymbolData[@OPRASymbol=$OpraCode]/@UnderlyingSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="OptionMonth">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='60'">
                  <xsl:value-of select ="substring(COL5,string-length(COL5) - 1,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Strike">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='60'">
                  <xsl:variable name ="varStrikeDecimal" select ="substring-after(COL18,'.')"/>
                  <xsl:variable name ="varStrikeInt" select ="substring-before(COL18,'.')"/>
                  <xsl:choose>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
                      <xsl:value-of select ="concat(COL18,'0')"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
                      <xsl:value-of select ="COL18"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
                      <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="concat(COL18,'.00')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="ExpYear">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='60'">
                  <xsl:value-of select ="substring(COL16,3,2)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->

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
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test="$varInstrumentType='60'">
								<!-- QPXPHO-->
								<!--<xsl:variable name="varAfterQ" >
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
								</xsl:variable>-->
								<Symbol>
                  <!--<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>-->
                  <!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>-->
                  <xsl:value-of select="''"/>
								</Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="concat(COL5,'U')"/>
                </IDCOOptionSymbol>
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
                    <IDCOOptionSymbol>
                      <xsl:value-of select="''"/>
                    </IDCOOptionSymbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="$varAfterAstric"/>
										</Symbol>
                    <IDCOOptionSymbol>
                      <xsl:value-of select="''"/>
                    </IDCOOptionSymbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

            <PBSymbol>
              <xsl:value-of select="COL5"/>
            </PBSymbol>

            <xsl:choose>
              <xsl:when  test="boolean(number(COL24))">
                <MarkPrice>
                  <xsl:value-of select="COL24"/>
                </MarkPrice>
              </xsl:when >
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
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

						<IsForexRequired>
							<xsl:value-of select="'true'"/>
						</IsForexRequired>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
