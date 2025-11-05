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

  <xsl:template match="/ThirdPartyFlatFileHeader">

    <ThirdPartyFlatFileHeader>


      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>


      <xsl:variable name = "varDataMarker" >
        <xsl:value-of select="'FV01'"/>
      </xsl:variable>
      
      <xsl:variable name = "varAddBlanks_FileName" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="4" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varFileType" >
        <xsl:value-of select="'031'"/>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_TimeStamp" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="8" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_Filler" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="4" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_DateStamp" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="8" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_Filler1" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="4" />
        </xsl:call-template>
      </xsl:variable>
      
      <xsl:variable name = "varAddBlanks_RecordLength" >
        <xsl:value-of select="'291'"/>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_Filler2" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="6" />
        </xsl:call-template>
      </xsl:variable>
      
      

      <xsl:variable name = "varAddBlanks_PricingNumber" >
        <xsl:value-of select="'3376 '"/>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_TBA" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="2" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_Transaction" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varAddBlanks_MinorVersion" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>


      <RecordType>
        <xsl:value-of select="concat($varDataMarker,$varAddBlanks_FileName,$varFileType,$varAddBlanks_TimeStamp,$varAddBlanks_Filler,$varAddBlanks_DateStamp,$varAddBlanks_Filler1,
                      $varAddBlanks_RecordLength,$varAddBlanks_Filler2,$varAddBlanks_PricingNumber,$varAddBlanks_TBA,$varAddBlanks_Transaction,$varAddBlanks_MinorVersion)"/>
      </RecordType>


    </ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>