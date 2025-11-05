<?xml version="1.0" encoding="utf-8" ?>

<!--Description: Seafearer , Alps
    Date       :02-03-2012-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL6)">
          <PositionMaster>


            <Symbol>
              <xsl:if test="COL1 !=''">
                <xsl:value-of select="COL1"/>
              </xsl:if>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="''"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL6)">
                  <xsl:value-of select="COL6"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <!--<Date>
              <xsl:if test="COL2 != ''">
                <xsl:value-of select="concat(substring(COL2,5,2),'/',substring(COL2,7,2),'/',substring(COL2,1,4))"/>
              </xsl:if>
            </Date>-->

			  <Date>
				  <xsl:if test="COL2 != ''">
					  <xsl:value-of select="COL2"/>
				  </xsl:if>
			  </Date>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>

    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>

	