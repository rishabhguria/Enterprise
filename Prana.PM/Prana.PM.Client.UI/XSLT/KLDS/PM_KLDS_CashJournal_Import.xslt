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

  <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>

  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <xsl:template name="FDate">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL37 + COL38 + COL39 + COL40 + COL41 + COL42 + COL43 + COL44"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:variable name="varNetAmount">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL46"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varFlags">
          <xsl:choose>
            <xsl:when test="COL1='C'">
              <xsl:value-of select="$varNetAmount"/>              
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varCash"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        
        <!--<xsl:if test="number($varFlags) and COL1 !='T' and COL1 !='P' and COL1 !='R' and COL1 !='B' and not(contains(substring-before(COL19,' '),'FX'))">-->
        <xsl:if test="number($varFlags) and (COL1 ='C' or COL1 ='X' or COL1 ='E' or COL1 ='A') and not(contains(substring-before(COL19,' '),'FX'))">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Shawspring'"/>
            </xsl:variable>

           
            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>
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

            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="COL29"/>
            </xsl:variable>

            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>



            <xsl:variable name = "varAmount" >
              <xsl:choose>
                <xsl:when test="$varFlags &gt; 0">
                  <xsl:value-of select="$varFlags"/>
                </xsl:when>
                <xsl:when test="$varFlags &lt; 0">
                  <xsl:value-of select="$varFlags*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL1 ='X'  or COL1 ='E'">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                <xsl:when test="COL1 ='A' and $varFlags &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
                <xsl:when test="COL1 ='A' and $varFlags &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>

                <xsl:when test="COL1 ='C' and $varFlags &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
                <xsl:when test="COL1 ='C' and $varFlags &lt; 0">
                  <xsl:value-of select="'CashTransferOut'"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varFlags &gt; 0">
                  <xsl:value-of select="concat('Cash:', $varAmount , '|', $PRANA_ACRONYM_NAME, ':' , $varAmount)"/>
                </xsl:when>

                <xsl:when  test="$varFlags &lt; 0">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $varAmount , '|Cash:' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            <xsl:variable name="varDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="''"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varDayName">            
                <xsl:value-of select="substring(COL12,7,2)"/>             
            </xsl:variable>

            <xsl:variable name="varMonthName">
              <xsl:value-of select="substring(COL12,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varYearName">
              <xsl:value-of select="substring(COL12,1,4)"/>
            </xsl:variable>

            <xsl:variable name="varDateName">
              <xsl:value-of select="concat($varMonthName,'/',$varDayName,'/',$varYearName)"/>
            </xsl:variable>

            <Date>
              <xsl:value-of select="''"/>
            </Date>

            <xsl:variable name="varDescriptionName">

              <xsl:choose>
                <xsl:when test="COL1='X'">
                  <xsl:value-of select="'Option Expiration Fee'"/>
                </xsl:when>
                <xsl:when test="COL1='E'">
                  <xsl:value-of select="'Option Assignment Fee'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL48"/>
                </xsl:otherwise>
              </xsl:choose>             
            </xsl:variable>

            <!--<xsl:variable name="varName">
              <xsl:value-of select="concat(substring($varDescriptionName, 1, 1), translate(substring($varDescriptionName, 2), $uppercase, $lowercase))"/>
            </xsl:variable>-->

            <Description>
              <xsl:value-of select="$varDescriptionName"/>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>