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


      <xsl:variable name = "varRecodType" >
        <xsl:value-of select="'HD'"/>
      </xsl:variable>
      
      <xsl:variable name = "varEmpty" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varFA" >
        <xsl:value-of select="'FA'"/>
      </xsl:variable>

      <xsl:variable name = "varEmpty2" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varM" >
        <xsl:value-of select="'M'"/>
      </xsl:variable>

      <xsl:variable name = "varEmpty3" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varMasterAccount" >
        <xsl:value-of select="'08047386'"/>
      </xsl:variable>

      <xsl:variable name = "varEmpty4" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>
  
      <xsl:variable name = "varFA30" >
        <xsl:value-of select="'FA30'"/>
      </xsl:variable>

      <xsl:variable name = "varEmpty5" >
        <xsl:call-template name="noofBlanks">
          <xsl:with-param name="count1" select="1" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:variable name = "varMasterAccountNo" >
        <xsl:value-of select="'M8047386'"/>
      </xsl:variable>

      <RecordType>
        <xsl:value-of select="concat($varRecodType,$varEmpty,$varFA,$varEmpty2,$varM,$varEmpty3,$varMasterAccount,$varEmpty4,$varFA30,$varEmpty5,$varMasterAccountNo)"/>
      </RecordType>


    </ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>