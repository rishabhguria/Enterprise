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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL17"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="PB_NAME">
          <xsl:value-of select="'ClearStreet'"/>
        </xsl:variable>
        
        <xsl:variable name = "PB_FUND_NAME" >
          <xsl:value-of select="concat(COL6,' ',COL4)"/>
        </xsl:variable>

        <xsl:variable name ="PRANA_FUND_NAME">
          <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
        </xsl:variable>

        <xsl:if test="(number($varCash)) and $PRANA_FUND_NAME!=''">

          <xsl:if test="COL17!=0 and COL16=0">
            <PositionMaster>

              <xsl:variable name = "PB_ACRONYM_NAME">
                <xsl:choose>
                  <xsl:when test="$varCash &gt; 0">
                    <xsl:value-of select="normalize-space(COL15)"/>
                  </xsl:when>

                  <xsl:when test="$varCash &lt; 0">
                    <xsl:value-of select="normalize-space(COL15)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
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

              <xsl:variable name="varCurrencyName">
                <xsl:value-of select="'USD'"/>
              </xsl:variable>

              <CurrencyName>
                <xsl:value-of select="$varCurrencyName"/>
              </CurrencyName>

              <xsl:variable name = "varAmount" >
                <xsl:choose>
                  <xsl:when test="$varCash &gt; 0">
                    <xsl:value-of select="$varCash"/>
                  </xsl:when>
                  <xsl:when test="$varCash &lt; 0">
                    <xsl:value-of select="$varCash * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="PRANA_ACRONYM_NAME">
                <xsl:choose>
                  <xsl:when test="$varCash &gt; 0">
                    <xsl:value-of select="'MISC_INC'"/>
                  </xsl:when>

                  <xsl:when  test="$varCash &lt; 0">
                    <xsl:value-of select="'MISC_EXP'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:variable>

              <JournalEntries>
                <xsl:choose>
                  <xsl:when test="$varCash &gt; 0">
                    <xsl:value-of select="concat('Cash:', $varAmount , '|' , $PRANA_ACRONYM_NAME, ':' , $varAmount)"/>
                  </xsl:when>

                  <xsl:when  test="$varCash &lt; 0">
                    <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $varAmount , '|Cash:' , $varAmount)"/>
                  </xsl:when>
                </xsl:choose>
              </JournalEntries>
              
              <xsl:variable name="varDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL3"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varMonthName">
                <xsl:choose>
                  <xsl:when test="string-length(substring-before(COL36,'/'))=1">
                    <xsl:value-of select="concat('0',substring-before(COL36,'/'))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-before(COL36,'/')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varDayName">
                <xsl:choose>
                  <xsl:when test="string-length(substring-before(substring-after(COL36,'/'),'/'))=1">
                    <xsl:value-of select="concat('0',substring-before(substring-after(COL36,'/'),'/'))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-before(substring-after(COL36,'/'),'/')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varYearName">
                <xsl:value-of select="substring-after(substring-after(COL36,'/'),'/')"/>
              </xsl:variable>

              <xsl:variable name="varDateName">
                <xsl:value-of select="concat($varMonthName,'/',$varDayName,'/',$varYearName)"/>
              </xsl:variable>

              <xsl:variable name="Date">
                <xsl:value-of select ="concat(substring(COL1,5,2),'/',substring(COL1,7,2),'/',substring(COL1,1,4))"/>
              </xsl:variable>

              <Date>
                <xsl:value-of select="$Date"/>
              </Date>

              <xsl:variable name="varDescriptionName">
                <xsl:value-of select="translate(normalize-space(COL15),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
              </xsl:variable>

              <Description>
                <xsl:choose>
                  <xsl:when test="$varDescriptionName!=''">
                    <xsl:value-of select="$varDescriptionName"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Description>

            </PositionMaster>
          </xsl:if>
          
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>