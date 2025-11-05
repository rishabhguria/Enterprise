<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileHeader">
    <ThirdPartyFlatFileHeader>

      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

      <RedordType>
        <xsl:value-of select="'HDR'"/>
      </RedordType>

      <xsl:variable name="varDate">
        <xsl:value-of select ="substring-before(DateAndTime,':')"/>
      </xsl:variable>

      <xsl:variable name="varDay">
        <xsl:choose>
          <xsl:when test="string-length(substring-before(substring-after($varDate,'/'),'/'))=1">
            <xsl:value-of select="concat('0',substring-before(substring-after($varDate,'/'),'/'))"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="substring-before(substring-after($varDate,'/'),'/')"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="varMonth">
        <xsl:choose>
          <xsl:when test="string-length(substring-before($varDate,'/'))=1">
            <xsl:value-of select="concat('0',substring-before($varDate,'/'))"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of  select="substring-before($varDate,'/')"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <Date>
        <xsl:value-of select="concat($varMonth,$varDay,substring(substring-after(substring-after($varDate,'/'),'/'),3,2))"/>
      </Date>


    </ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>
