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

      <xsl:for-each select ="//PositionMaster">

        <xsl:if test="number(COL8)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@PBAcronymName=$PB_ACRONYM_NAME]/@PranaAcronym"/>
            </xsl:variable>

            <xsl:variable name="ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test ="$PRANA_ACRONYM_NAME!=''">
                  <xsl:value-of select ="$PRANA_ACRONYM_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_ACRONYM_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varDay">
              <xsl:value-of select ="substring(COL5,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select ="substring(COL5,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select ="substring(COL5,1,4)"/>
            </xsl:variable>

            <xsl:variable name="Date">
              <xsl:value-of select ="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="$Date"/>
            </Date>

            <xsl:variable name="varAmount">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name = "Amount" >
              <xsl:choose>
                <xsl:when test="$varAmount &gt; 0">
                  <xsl:value-of select="$varAmount"/>
                </xsl:when>
                <xsl:when test="$varAmount &lt; 0">
                  <xsl:value-of select="$varAmount*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varAmount &gt; 0">
                  <xsl:value-of select="concat('IntrestAccural',':', $Amount , '|','IntrestIncome', ':' , $Amount)"/>
                </xsl:when>
                <xsl:when test="$varAmount &lt; 0">
                  <xsl:value-of select="concat('IntrestIncome',':', $Amount , '|','IntrestAccural', ':' , $Amount)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </JournalEntries>

            <Description>
              <xsl:value-of select="'Broker Interest Received'"/>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>