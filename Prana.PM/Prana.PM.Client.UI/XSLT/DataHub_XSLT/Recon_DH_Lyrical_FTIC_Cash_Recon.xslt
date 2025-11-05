<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

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

  <xsl:template name="varMonth">
    <xsl:param name="MonthName"/>
    <xsl:choose>
      <xsl:when test="$MonthName='January'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$MonthName='Febuary'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$MonthName='March'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$MonthName='April'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$MonthName='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$MonthName='June'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$MonthName='July'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$MonthName='August'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$MonthName='September'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$MonthName='October'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$MonthName='November'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$MonthName='December'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:variable name="varDate" select="(//Comparision[COL1][4]/COL1[child::node()[1]])"/>
      
      <xsl:variable name="varFund" select="(//Comparision[COL1][1]/COL1[child::node()[1]])"/>
      
      <xsl:variable name="SymbolName" select="normalize-space(substring-after(//Comparision[contains(COL1,'Base Currency')]/COL1, ':'))"/>
      
      <xsl:for-each select ="//Comparision">
        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="translate(COL6,'$','')"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test ="number($Cash) and normalize-space(COL1)='Cash' ">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'FTIC'"/>
              </xsl:variable>

              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="$varFund"/>
              </xsl:variable>

              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>

                </xsl:choose>
              </PortfolioAccount>
              <xsl:variable name="varCurrency">
                <xsl:value-of select="'USD'"/>
              </xsl:variable>
              <Currency>
                <xsl:value-of select ="$varCurrency"/>
              </Currency>


              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($Cash)">
                    <xsl:value-of select="$Cash"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>

              <xsl:variable name="OpeningBalanceCR">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <OpeningBalanceCR>
                <xsl:choose>
                  <xsl:when test="number($OpeningBalanceCR)">
                    <xsl:value-of select="$OpeningBalanceCR"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceCR>

              <xsl:variable name="MonthNo">
                <xsl:call-template name="varMonth">
                  <xsl:with-param name="MonthName" select="substring-before($varDate,' ')"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="vardateCode" select="substring-before(substring-after($varDate,' '),',')"/>

              <xsl:variable name="varYearCode" select="number(substring-after(substring-after($varDate,','),''))"/>

              <TradeDate>
                <xsl:value-of select="concat($MonthNo,'/',$vardateCode,'/',$varYearCode)"/>
              </TradeDate>

            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <PortfolioAccount>
                <xsl:value-of select="''"/>
              </PortfolioAccount>


              <Currency>
                <xsl:value-of select="''"/>
              </Currency>


              <OpeningBalanceDR>
                <xsl:value-of select="0"/>
              </OpeningBalanceDR>

              <OpeningBalanceCR>
                <xsl:value-of select="0"/>
              </OpeningBalanceCR>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>


            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>