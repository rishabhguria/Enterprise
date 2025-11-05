<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileFooter">
    <ThirdPartyFlatFileFooter>
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

      <xsl:variable name = "recordCount" >
        <xsl:call-template name="noofzeros">
          <xsl:with-param name="count" select="(6) - string-length(RecordCount)" />
        </xsl:call-template>
      </xsl:variable>

      <Trailer>
        <xsl:value-of select="concat('T',$varClientCode,$varSystemCode,$varFileCount,$varYR,$varMth,$varDt,$varhrs,$recordCount,RecordCount)"/>
      </Trailer>

      <!--<xsl:choose>
        <xsl:when test ="string-length($varMth) &lt; 2 and string-length($varDt) &lt; 2">
          <Trailer>
            <xsl:value-of select="concat('T',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,concat('0',$varMth),$varDt),$recordCount,RecordCount)"/>
          </Trailer>
        </xsl:when>
        <xsl:when test ="string-length($varMth) = 2 and string-length($varDt) &lt; 2">
          <Trailer>
            <xsl:value-of select="concat('T',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,$varMth,concat('0',$varDt)),$recordCount,RecordCount)"/>
          </Trailer>
        </xsl:when>
        <xsl:when test ="string-length($varMth) &lt; 2 and string-length($varDt) = 2">
          <Trailer>
            <xsl:value-of select="concat('T',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,concat('0',$varMth),$varDt),$recordCount,RecordCount)"/>
          </Trailer>
        </xsl:when>
        <xsl:otherwise>
          <Trailer>
            <xsl:value-of select="concat('T',$varClientCode,$varSystemCode,$varFileCount,concat($varYR,$varMth,$varDt),$recordCount,RecordCount)"/>
          </Trailer>
        </xsl:otherwise>
      </xsl:choose>-->

    </ThirdPartyFlatFileFooter>
  </xsl:template>
</xsl:stylesheet>
