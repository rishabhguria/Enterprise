<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <PositionMaster>
          <!--   Fund -->
          <AccountName>
            <xsl:value-of select="''"/>
          </AccountName>


          <!--<xsl:choose>
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
          </xsl:choose >-->

			<Symbol>
				<xsl:value-of select="COL1"/>
			</Symbol>

          <PBSymbol>
            <xsl:value-of select="COL1"/>
          </PBSymbol>

			<xsl:choose>
				<xsl:when test ="COL2 &lt; 0 ">
					<NetPosition>
						<xsl:value-of select="COL2*(-1)"/>
					</NetPosition>
					<SideTagValue>
						<xsl:value-of select="'5'"/>
					</SideTagValue>
				</xsl:when >
				<xsl:when test ="COL2 &gt; 0 ">
					<NetPosition>
						<xsl:value-of select="COL2"/>
					</NetPosition>
					<SideTagValue>
						<xsl:value-of select="'B'"/>
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
            <xsl:when test ="boolean(number(COL3))">
              <CostBasis>
                <xsl:value-of select="COL3"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>

          <CounterPartyID>
            <xsl:value-of select="'67'"/>
          </CounterPartyID>


          <PositionStartDate>
            <xsl:value-of select="''"/>
          </PositionStartDate>

        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
