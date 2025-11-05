<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <PositionMaster>
          <!--   Fund -->
          <FundName>
            <xsl:value-of select="''"/>
          </FundName>


          <xsl:choose>
            <xsl:when test ="COL3 = 'STK'">
              <Symbol>
                <xsl:value-of select="COL1"/>
              </Symbol>
            </xsl:when>
            <xsl:when test ="COL3 = 'SO'">
              <xsl:variable name = "varLength" >
                <xsl:value-of select="string-length(COL1)"/>
              </xsl:variable>
              <xsl:variable name = "varAfter" >
                <xsl:value-of select="substring(COL1,($varLength)-1,2)"/>
              </xsl:variable>
              <xsl:variable name = "varBefore" >
                <xsl:value-of select="substring(COL1,1,($varLength)-2)"/>
              </xsl:variable>
              <Symbol>
                <xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
              </Symbol>
            </xsl:when>
            <xsl:when test ="COL3 = 'FUT'">
              <xsl:variable name ="varFut">
                <xsl:value-of select ="translate(COL1,'/','')"/>
              </xsl:variable>
              <xsl:variable name ="varFutLen">
                <xsl:value-of select ="string-length($varFut)"/>
              </xsl:variable>
              <xsl:variable name ="varFutSymbol">
                <xsl:value-of select ="concat(substring($varFut,1,2),' ',substring($varFut,3,1),substring($varFut,$varFutLen,1))"/>
              </xsl:variable>
              <Symbol>
                <xsl:value-of select="$varFutSymbol"/>
              </Symbol>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="COL1"/>
              </Symbol>
            </xsl:otherwise>
          </xsl:choose >

          <PBSymbol>
            <xsl:value-of select="COL1"/>
          </PBSymbol>

          <xsl:choose>
            <xsl:when test ="COL7 &lt; 0 and COL3 = 'STK'">
              <NetPosition>
                <xsl:value-of select="COL7*(-1)"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'5'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:when test ="COL7 &gt; 0 and COL3 = 'STK'">
              <NetPosition>
                <xsl:value-of select="COL7"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'1'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:when test ="COL7 &lt; 0 and (COL3 = 'SO' or COL3 = 'FUT')">
              <NetPosition>
                <xsl:value-of select="COL7*(-1)"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'C'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:when test ="COL7 &gt; 0 and (COL3 = 'SO' or COL3 = 'FUT')">
              <NetPosition>
                <xsl:value-of select="COL7"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'A'"/>
              </SideTagValue>
            </xsl:when >

            <xsl:otherwise>
              <NetPosition>
                <xsl:value-of select="0"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="''"/>
              </SideTagValue>
            </xsl:otherwise>
          </xsl:choose >


          <xsl:choose>
            <xsl:when test ="boolean(number(COL8))">
              <CostBasis>
                <xsl:value-of select="COL8"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>

          <CounterPartyID>
            <xsl:value-of select="'13'"/>
          </CounterPartyID>


          <PositionStartDate>
            <xsl:value-of select="''"/>
          </PositionStartDate>

        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
