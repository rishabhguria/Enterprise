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

  <!--<xsl:template match="First|Last">
    <xsl:value-of select="concat(translate(substring(.,1,1), $vLower, $vUpper),substring(., 2),substring(' ', 1 div not(position()=last())))"/>
  </xsl:template>-->

  <!--<xsl:template match="month">
    <xsl:value-of select="concat(substring(@name, 1, 1), translate(substring(@name, 2), $uppercase, $lowercase))"/>
  </xsl:template>-->


  <xsl:template match="month">
    <xsl:value-of select="concat(substring(@name, 1, 1), translate(substring(@name, 2), $uppercase, $lowercase))"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL58"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varCash) and COL4 ='THINK INDIA OPPORTUNITIES MASTER FUND LP' and COL26='Journal'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL43"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="COL55"/>
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
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "PB_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL9)"/>
            </xsl:variable>



            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>               

                <xsl:when test="$PB_ACRONYM_NAME ='FUNDS PAID OR RECEIVED' and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
                <xsl:when test="$PB_ACRONYM_NAME ='FUNDS PAID OR RECEIVED' and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferOut'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            
            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$PB_ACRONYM_NAME ='Cash Collateral Movement' and $varCash &gt; 0">
                  <xsl:value-of select="concat('Swap Collateral',':', $varAmount , '|', 'CashTransferIn', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="$PB_ACRONYM_NAME ='Cash Collateral Movement' and $varCash &lt; 0">
                  <xsl:value-of select="concat('CashTransferOut',':', $varAmount , '|', 'Swap Collateral', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varCash &gt; 0">
                      <xsl:value-of select="concat('Cash:', $varAmount , '|', $PRANA_ACRONYM_NAME, ':' , $varAmount)"/>
                    </xsl:when>

                    <xsl:when  test="$varCash &lt; 0">
                      <xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $varAmount , '|Cash:' , $varAmount)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>            
           
            </JournalEntries>


            
            
            <Date>
              <xsl:value-of select="concat(substring-before(COL36,'-'),'/',substring-before(substring-after(COL36,'-'),'-'),'/',substring-after(substring-after(COL36,'-'),'-'))"/>
            </Date>

           

            <xsl:variable name="varDescriptionName">
              <xsl:value-of select="COL21"/>
            </xsl:variable>

            <xsl:variable name="varName">
              <xsl:value-of select="concat(substring($varDescriptionName, 1, 1), translate(substring($varDescriptionName, 2), $uppercase, $lowercase))"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select="$PB_ACRONYM_NAME"/>
            </Description>



          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>