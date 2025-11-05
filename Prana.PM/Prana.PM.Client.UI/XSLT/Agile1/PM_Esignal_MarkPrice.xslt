<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>

		<!-- Future month Codes e.g. 01 represents ,January -->
      <xsl:choose>
        <xsl:when test ="$varMonth=01">
          <xsl:value-of select ="'F'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=02">
          <xsl:value-of select ="'G'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=03">
          <xsl:value-of select ="'H'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=04">
          <xsl:value-of select ="'J'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=05">
          <xsl:value-of select ="'K'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=06">
          <xsl:value-of select ="'M'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=07">
          <xsl:value-of select ="'N'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=08">
          <xsl:value-of select ="'Q'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=09">
          <xsl:value-of select ="'U'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=10">
          <xsl:value-of select ="'V'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=11">
          <xsl:value-of select ="'X'"/>
        </xsl:when>
        <xsl:when test ="$varMonth=12">
          <xsl:value-of select ="'Z'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
		  <xsl:if test="number(COL6)">

        <PositionMaster>
		
            <Symbol>
                  <xsl:value-of select="COL1"/>
            </Symbol>

            <xsl:choose>
              <xsl:when test="boolean(number(COL6))">
                <MarkPrice>
                  <xsl:value-of select="COL6"/>
                </MarkPrice>
              </xsl:when>
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose>

            <Date>
                  <xsl:value-of select="COL2"/>
            </Date>

					</PositionMaster>
		  </xsl:if>

	  </xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
