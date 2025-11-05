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
    <xsl:param name="DateTime"/>
    <!--  converts date time double number to 18/12/2009  -->
    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019"/>
    </xsl:variable>
    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))"/>
    </xsl:variable>
    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
    </xsl:variable>
    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
    </xsl:variable>
    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
    </xsl:variable>
    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))"/>
    </xsl:variable>
    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
    </xsl:variable>
    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))"/>
    </xsl:variable>
    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
    </xsl:variable>
    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
    </xsl:variable>
    <xsl:variable name="varMonthUpdated">
      <xsl:choose>
        <xsl:when test="string-length($nMonth) = 1">
          <xsl:value-of select="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="nDayUpdated">
      <xsl:choose>
        <xsl:when test="string-length($nDay) = 1">
          <xsl:value-of select="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$nDay"/>
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

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL21)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

             <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDes">
              <xsl:value-of select="normalize-space(COL24)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>
			   <xsl:when test="contains($varSubAccountDes,'WIRE TRANSFER CHARGE') or contains($varSubAccountDes,'MARGIN INTEREST') and $Cash &lt; 0">
					  <xsl:value-of select="'MISC_EXP'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'SHORT STOCK REBATES') and $Cash &gt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'MARGIN INTEREST') and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'FUNDS WIRED IN') and $Cash &gt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'FUNDS WIRED OUT') and $Cash &lt; 0">
					  <xsl:value-of select="'CASH_WDL'"/>
				  </xsl:when>
				  <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>

				  <xsl:when test="contains($varSubAccountDes,'WIRE TRANSFER CHARGE') and $Cash &lt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'SHORT STOCK REBATES') and $Cash &gt; 0">
					  <xsl:value-of select="'MISC_INC'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'MARGIN INTEREST') and $Cash &gt; 0">
					  <xsl:value-of select="'MISC_INC'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'MARGIN INTEREST') and $Cash &lt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'FUNDS WIRED IN') and $Cash &gt; 0">
					  <xsl:value-of select="'CASH_DEP'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDes,'FUNDS WIRED OUT') and $Cash &lt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL14"/>
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

       
            <CurrencyName>
              <xsl:value-of select ="COL39"/>
            </CurrencyName>

            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="$Cash &gt; 0">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:when test="$Cash &lt; 0">
                  <xsl:value-of select="$Cash * -1"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>          
			        <xsl:value-of select="concat($PRANA_ACRONYM_NAME_PRE, ':' , $AbsCash , '|' , $PRANA_ACRONYM_NAME_POST, ':' , $AbsCash)"/>
            </JournalEntries>

			
			 <xsl:variable name="varDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL1"/>
              </xsl:call-template>
            </xsl:variable>
            <Date>
              <!-- <xsl:value-of select="concat($Month,'/',$DD ,'/', $YYYY)"/> -->
			  <xsl:value-of select="COL3"/>
            </Date>

            <Description>
              <xsl:value-of select="$varSubAccountDes"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>