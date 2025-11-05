<?xml version="1.0" encoding="utf-8" ?>

<!--Description: Seafearer , Alps
    Date       : 02-03-2012-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select="//PositionMaster">
                  <PositionMaster>


            <Symbol>
             
                <xsl:value-of select="COL1"/>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="COL1"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL3)">
                  <xsl:value-of select="COL3"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <Date>
         <xsl:value-of select="COL2"/>
            </Date>


          </PositionMaster>
    
      </xsl:for-each>

    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>

	