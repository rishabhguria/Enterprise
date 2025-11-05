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

			  <xsl:variable name="varDrCash">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL6"/>
				  </xsl:call-template>
			  </xsl:variable>

        <xsl:if test="number($varDrCash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MapelRock'"/>
            </xsl:variable>

			  <xsl:variable name = "PB_DR_ACRONYM_NAME">
				  <xsl:value-of select="normalize-space(COL5)"/>
			  </xsl:variable>

			  <xsl:variable name="varPRANA_DR_ACRONYM_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@AccountName=$PB_DR_ACRONYM_NAME]/@Acronym"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_DR_ACRONYM_NAME">
				  <xsl:choose>
					  <xsl:when test ="$varPRANA_DR_ACRONYM_NAME !=''">
						  <xsl:value-of select="$varPRANA_DR_ACRONYM_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varPRANA_DR_ACRONYM_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name = "PB_CR_ACRONYM_NAME">
				  <xsl:value-of select="normalize-space(COL6)"/>
			  </xsl:variable>

			  <xsl:variable name="varPRANA_CR_ACRONYM_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@AccountName=$PB_CR_ACRONYM_NAME]/@Acronym"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_CR_ACRONYM_NAME">
				  <xsl:choose>
					  <xsl:when test ="$varPRANA_CR_ACRONYM_NAME !=''">
						  <xsl:value-of select="$varPRANA_CR_ACRONYM_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varPRANA_CR_ACRONYM_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
			
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
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>

			  <xsl:variable name="varDrCash">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL6"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name = "Dramount" >
				  <xsl:choose>
					  <xsl:when test="$varDrCash &gt; 0">
						  <xsl:value-of select="$varDrCash"/>
					  </xsl:when>
					  <xsl:when test="$varDrCash &lt; 0">
						  <xsl:value-of select="$varDrCash*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <xsl:variable name="varCrCash">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL10"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name = "Cramount" >
				  <xsl:choose>
					  <xsl:when test="$varCrCash &gt; 0">
						  <xsl:value-of select="$varCrCash"/>
					  </xsl:when>
					  <xsl:when test="$varCrCash &lt; 0">
						  <xsl:value-of select="$varCrCash*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <JournalEntries>
				  <xsl:choose>
					  <xsl:when test="number($Dramount) and number($Cramount) and $PRANA_DR_ACRONYM_NAME!='' and $PRANA_CR_ACRONYM_NAME!=''">
						  <xsl:value-of select="concat($PRANA_DR_ACRONYM_NAME,':', $Dramount , '|',$PRANA_CR_ACRONYM_NAME, ':' , $Cramount)"/>
					  </xsl:when>
				  </xsl:choose>
			  </JournalEntries>

            <xsl:variable name="varDescription">
              <xsl:value-of select="COL8"/>
            </xsl:variable>
            <Description>
              <xsl:value-of select="$varDescription"/>
            </Description>

            <Date>
              <xsl:call-template name="FormatDate">
            <xsl:with-param name="DateTime" select="normalize-space(COL1)"/>
          </xsl:call-template>
            </Date>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>