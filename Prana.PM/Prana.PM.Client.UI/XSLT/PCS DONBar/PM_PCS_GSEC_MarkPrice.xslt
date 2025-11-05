<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name = "PB_ASSET_NAME" >
          <xsl:value-of select="translate(COL2,'&quot;','')"/>
        </xsl:variable>
        <!--<xsl:variable name="MARKET_VALUE" select="translate(translate(COL12,'&quot;',''),' ','')"/>-->

        <xsl:if test="$PB_ASSET_NAME != 'CURRENCY' and COL8 != 'QUANTITY' and COL8 != 0">
          <PositionMaster>
            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL4)"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name = "varInstrumentType" >
              <xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
            </xsl:variable>


            <xsl:variable name ="varFutureSymbol">
							<xsl:choose>
								<xsl:when test ="$varInstrumentType='FUTURE'">
									<xsl:variable name = "varLength" >
										<xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
									</xsl:variable>
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring(COL6,($varLength)-1,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL6,1,($varLength)-2)"/>
									</xsl:variable>

									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
                <PBSymbol>
                  <xsl:value-of select="translate(COL3,'&quot;','')"/>
                </PBSymbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrumentType='EQUITY'">
                <Symbol>
                  <xsl:value-of select="translate(COL3,'&quot;','')"/>
                </Symbol>
                <PBSymbol>
                  <xsl:value-of select="translate(COL3,'&quot;','')"/>
                </PBSymbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrumentType='OPTION'">
                <Symbol>
                  <!--<xsl:value-of select="translate(COL5,'&quot;','')"/>-->
                  <!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>-->
                  <xsl:value-of select="''"/>
                </Symbol>
                <PBSymbol>
                  <xsl:value-of select="COL17"/>
                </PBSymbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="concat(COL17,'U')"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrumentType='FUTURE'">
                <!--<xsl:variable name = "varLength" >
                  <xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test ="$varLength &gt; 0 ">
                    <xsl:variable name = "varAfter" >
                      <xsl:value-of select="substring(COL6,($varLength)-1,2)"/>
                    </xsl:variable>
                    <xsl:variable name = "varBefore" >
                      <xsl:value-of select="substring(COL6,1,($varLength)-2)"/>
                    </xsl:variable>
                    <Symbol>
                      <xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
                    </Symbol>
                    <PBSymbol>
                      <xsl:value-of select="translate(COL6,'&quot;','')"/>
                    </PBSymbol>
                    <IDCOOptionSymbol>
                      <xsl:value-of select="''"/>
                    </IDCOOptionSymbol>
                  </xsl:when>
                  <xsl:otherwise>
                    <Symbol>
                      <xsl:value-of select="''"/>
                    </Symbol>
                    <PBSymbol>
                      <xsl:value-of select="''"/>
                    </PBSymbol>
                    <IDCOOptionSymbol>
                      <xsl:value-of select="''"/>
                    </IDCOOptionSymbol>
                  </xsl:otherwise>
                </xsl:choose>-->
                <Symbol>
									<xsl:value-of select="$varFutureSymbol"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL6,'&quot;','')"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <PBSymbol>
                  <xsl:value-of select="''"/>
                </PBSymbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:choose>
              <xsl:when test ="boolean(number(COL15))">
                <xsl:choose>
                  <xsl:when  test="starts-with($varFutureSymbol,'JY') or starts-with($varFutureSymbol,'6J') or starts-with($varFutureSymbol,'HG') or 
                                        starts-with($varFutureSymbol,'ZS') or starts-with($varFutureSymbol,'ZC') or starts-with($varFutureSymbol,'ZW') or 
                                        starts-with($varFutureSymbol,'YO') or starts-with($varFutureSymbol,'SB') or starts-with($varFutureSymbol,'CT') or 
                                        starts-with($varFutureSymbol,'HE') or $varFutureSymbol= 'Z Z9'">
                    <MarkPrice>
                      <xsl:value-of select="COL15 * 100"/>
                    </MarkPrice>
                  </xsl:when >
                  <xsl:otherwise>
                    <MarkPrice>
                      <xsl:value-of select="COL15"/>
                    </MarkPrice>
                  </xsl:otherwise>
                </xsl:choose >                
              </xsl:when>
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="COL7 !='Trade/Settlement/Value Date' and COL7 != '*'">
                <Date>
                  <xsl:value-of select="COL7"/>
                </Date>
              </xsl:when>
              <xsl:otherwise>
                <Date>
                  <xsl:value-of select="''"/>
                </Date>
              </xsl:otherwise>
            </xsl:choose>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
