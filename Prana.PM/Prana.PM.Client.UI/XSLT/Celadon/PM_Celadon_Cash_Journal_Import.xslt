<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        
        <xsl:variable name="Amount">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:if test="number($Amount)">
          
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:value-of select="COL5"/>
            </xsl:variable>


            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME = ''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name = "amount" >
              <xsl:choose>
                <xsl:when test="$Amount &gt; 0">
                  <xsl:value-of select="$Amount"/>
                </xsl:when>
                <xsl:when test="$Amount &lt; 0">
                  <xsl:value-of select="$Amount*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Date>
              <xsl:value-of select="COL2"/>
            </Date>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$Amount &lt; 0">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount, '|Cash:', $amount )"/>
                </xsl:when>
                <xsl:when  test="$Amount &gt; 0">
                  <xsl:value-of select="concat( 'Cash:' , $amount, '|', $PRANA_ACRONYM_NAME,':' , $amount)"/>
                </xsl:when>

              </xsl:choose>
            </JournalEntries>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>