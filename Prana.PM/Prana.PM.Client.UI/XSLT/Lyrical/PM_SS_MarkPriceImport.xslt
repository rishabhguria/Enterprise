<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="number(COL39)">
          <PositionMaster>
			  <xsl:variable name="varOSISymbol">
				  <xsl:value-of select="COL10"/>
			  </xsl:variable>

			  <xsl:variable name="varOptionSymbol">
				  <xsl:value-of select="''"/>
			  </xsl:variable>

			  <xsl:variable name="varEquitySymbol">
				  <xsl:value-of select="COL6"/>
			  </xsl:variable>

			  <xsl:choose>
				  <xsl:when test="$varOSISymbol = '*' or $varOSISymbol = ''">
					  <Symbol>
						  <xsl:value-of select="$varEquitySymbol"/>
					  </Symbol>

					  <IDCOOptionSymbol>
						  <xsl:value-of select="''"/>
					  </IDCOOptionSymbol>

				  </xsl:when>
				  <xsl:otherwise>
					  <Symbol>
						  <xsl:value-of select="''"/>
					  </Symbol>
					  <IDCOOptionSymbol>
						  <xsl:value-of select="concat($varOSISymbol, 'U')"/>
					  </IDCOOptionSymbol>
				  </xsl:otherwise>
			  </xsl:choose>


            <PBSymbol>
              <xsl:value-of select="COL5"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL39)">
                  <xsl:value-of select="COL39"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <Date>
              <xsl:value-of select="''"/>
            </Date>


          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
