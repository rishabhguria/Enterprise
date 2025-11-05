<?xml version="1.0" encoding="utf-8"?>
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

        <PositionMaster>

          <xsl:variable name ="varLocalCurr">
            <xsl:value-of select ="substring(COL1,294,3)"/>
          </xsl:variable>
          <BaseCurrency>
            <xsl:value-of select="$varLocalCurr"/>
          </BaseCurrency>

          <SettlementCurrency>
            <xsl:value-of select="'USD'"/>
          </SettlementCurrency>

          <xsl:variable name ="varFXRate">
            <xsl:value-of select ="substring(COL1,334,15)"/>
          </xsl:variable>

          <xsl:variable name ="varFXInt">
            <xsl:value-of select ="substring($varFXRate,1,7)"/>
          </xsl:variable>

          <xsl:variable name ="varFXFrac">
            <xsl:value-of select ="substring($varFXRate,8,8)"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="boolean(number($varFXRate))">
              <ForexPrice>
                <xsl:value-of select="concat($varFXInt,'.',$varFXFrac)"/>
              </ForexPrice>
            </xsl:when>
            <xsl:otherwise>
              <ForexPrice>
                <xsl:value-of select="0"/>
              </ForexPrice>
            </xsl:otherwise>
          </xsl:choose>

          <Date>
            <xsl:value-of select="''"/>
          </Date>

          <!--<xsl:choose>
						<xsl:when test ="COL1 = 'Date/Time'">
							<Date>
								<xsl:value-of select="''"/>
							</Date>
						</xsl:when >
						<xsl:otherwise>
							<Date>
								<xsl:value-of select="translate(COL1,'&quot;','')"/>
							</Date>
						</xsl:otherwise>
					</xsl:choose>-->

          <!--<FXConversionMethodOperator>
							<xsl:value-of select ="COL47"/>
						</FXConversionMethodOperator>-->
        </PositionMaster>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>