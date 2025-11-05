<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileHeader">
    <ThirdPartyFlatFileHeader>
      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

      <xsl:variable name ="varClientCode">
        <xsl:value-of select ="'CCOD'"/>
      </xsl:variable>

      <xsl:variable name ="varSystemCode">
        <xsl:value-of select ="'SCOD'"/>
      </xsl:variable>

      <xsl:variable name ="varFileCount">
        <xsl:value-of select ="'0001'"/>
      </xsl:variable>

      <xsl:variable name = "varMth" >
        <xsl:value-of select="substring(DateAndTime,1,2)"/>
      </xsl:variable>
      <xsl:variable name = "varDt" >
        <xsl:value-of select="substring(DateAndTime,4,2)"/>
      </xsl:variable>
      <xsl:variable name = "varYR" >
        <xsl:value-of select="substring(DateAndTime,7,4)"/>
      </xsl:variable>

      <xsl:variable name ="varhrs">
        <xsl:value-of select ="substring-after(DateAndTime,':')"/>
      </xsl:variable>


      <HeaderTag>
        <xsl:value-of select="concat('H',$varClientCode,$varSystemCode,$varFileCount,$varYR,$varMth,$varDt,$varhrs)"/>
      </HeaderTag>

      <!--<xsl:choose>
        <xsl:when test ="string-length($varMth) &lt; 2 and string-length($varDt) &lt; 2">
          <HeaderTag>
            <xsl:value-of select="concat('H',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,concat('0',$varMth),$varDt))"/>
          </HeaderTag>
        </xsl:when>
        <xsl:when test ="string-length($varMth) = 2 and string-length($varDt) &lt; 2">
          <HeaderTag>
            <xsl:value-of select="concat('H',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,$varMth,concat('0',$varDt)))"/>
          </HeaderTag>
        </xsl:when>
        <xsl:when test ="string-length($varMth) &lt; 2 and string-length($varDt) = 2">
          <HeaderTag>
            <xsl:value-of select="concat('H',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,concat('0',$varMth),$varDt))"/>
          </HeaderTag>
        </xsl:when>
        <xsl:otherwise>
          <HeaderTag>
            <xsl:value-of select="concat('H',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,$varMth,$varDt))"/>
          </HeaderTag>
        </xsl:otherwise>
      </xsl:choose>-->
    </ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>
