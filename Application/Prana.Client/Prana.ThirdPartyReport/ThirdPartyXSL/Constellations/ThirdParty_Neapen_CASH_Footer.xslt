<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:variable name="GreaterThan" select="'&gt;'"/>
  <xsl:variable name="LessThan" select="'&lt;'"/>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileFooter">
    <ThirdPartyFlatFileFooter>
      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>


      <xsl:variable name = "varAddBlanks_DataMarker" >
        <xsl:value-of select="'09END'"/>
      </xsl:variable>
      
      <xsl:variable name = "varAddBlanks_RecordType" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="3" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varTradeCount" >
        <xsl:value-of select="RecordCount"/>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_TradeCountRec" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="(5 - string-length(RecordCount))" />
        </xsl:call-template>
      </xsl:variable>

      <RecordType>
        <xsl:value-of select="concat($varAddBlanks_DataMarker,$varAddBlanks_RecordType,$varAddBlanks_TradeCountRec,$varTradeCount)"/>
      </RecordType>

  
    </ThirdPartyFlatFileFooter>
  </xsl:template>
</xsl:stylesheet>