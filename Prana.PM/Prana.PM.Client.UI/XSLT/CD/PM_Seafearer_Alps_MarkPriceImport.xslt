<?xml version="1.0" encoding="utf-8" ?>

<!--Description: Seafearer , Alps
    Date       : 02-03-2012-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>

        <xsl:for-each select="//PositionMaster">
          <xsl:if test="COL1 != 'FUNDID'">
          <PositionMaster>

            <xsl:choose>
              <xsl:when test="COL47 = 'USD'">
                <CUSIP>
                  <xsl:value-of select="COL17"/>
                </CUSIP>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <CUSIP>
                  <xsl:value-of select="''"/>
                </CUSIP>
                <SEDOL>
                  <xsl:value-of select="COL17"/>
                </SEDOL>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="''"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL25)">
                  <xsl:value-of select="COL25"/>
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
        </xsl:if>
      </xsl:for-each>

    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>

	