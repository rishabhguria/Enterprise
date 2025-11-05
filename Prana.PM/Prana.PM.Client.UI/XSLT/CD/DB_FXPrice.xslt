<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->


                                        <!--DB Cash Import
											Date-30-11-2011
										-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
		  <xsl:if test ="COL1 !='ProcessDate' ">
        <PositionMaster>

          <BaseCurrency>
            <xsl:value-of select="COL10"/>
          </BaseCurrency>

          <SettlementCurrency>
            <xsl:value-of select="COL15"/>
          </SettlementCurrency>


          <xsl:choose>
            <xsl:when test="boolean(number(COL20))">
              <ForexPrice>
                <xsl:value-of select="COL20"/>
              </ForexPrice>
            </xsl:when>
            <xsl:otherwise>
              <ForexPrice>
                <xsl:value-of select="0"/>
              </ForexPrice>
            </xsl:otherwise>
          </xsl:choose>
			
			<Date>
				<xsl:value-of select="concat(substring(COL2,5,2),'/',substring(COL2,7,2),'/',substring(COL2,1,4))"/>
			</Date>
			
        </PositionMaster>
			  </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>