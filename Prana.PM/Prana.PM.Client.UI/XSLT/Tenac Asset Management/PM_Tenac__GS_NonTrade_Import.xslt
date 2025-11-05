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

<xsl:template name="MonthCode1">
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
                                <xsl:value-of select="'10'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Nov'">
                                <xsl:value-of select="'11'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Dec'">
                                <xsl:value-of select="'12'"/>
                        </xsl:when>
                        <xsl:otherwise>
                                <xsl:value-of select="''"/>
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

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL7)"/>
          </xsl:call-template>
        </xsl:variable>
			<xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>
        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

			  <xsl:variable name="PB_FUND_NAME" select="COL2"/>
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

			  <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
				  <xsl:choose>
					  <xsl:when test="$varSubAccountDesc = 'PAI' and $Cash &lt; 0">
						  <xsl:value-of select="'MISC_EXP'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'PAI' and $Cash &gt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'ADJ' and $Cash &gt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'GS Transaction Commission' and $Cash &lt; 0">
						  <xsl:value-of select="'Commission'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'LCH Clearing Fee' and $Cash &lt; 0">
						  <xsl:value-of select="'Commission'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'CME Clearing Fee' and $Cash &lt; 0">
						  <xsl:value-of select="'Commission'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Initial Margin Maintenance Commission' and $Cash &lt; 0">
						  <xsl:value-of select="'Commission'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'INTEREST CHARGED' and $Cash &lt; 0">
						  <xsl:value-of select="'Interest_Expense'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'INTEREST PAID' and $Cash &gt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'ICE CLEARING FEE' and $Cash &lt; 0">
						  <xsl:value-of select="'Commission'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Coupon Payment' and $Cash &lt; 0">
						  <xsl:value-of select="'Interest Expense'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Coupon Payment' and $Cash &gt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Funding Fee' and $Cash &lt; 0">
						  <xsl:value-of select="'Commission'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'DCS SWEEP USD' and $Cash &gt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="PRANA_ACRONYM_NAME_POST">
				  <xsl:choose>
					  <xsl:when test="$varSubAccountDesc = 'PAI' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'PAI' and $Cash &gt; 0">
						  <xsl:value-of select="'MISC_INC'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'ADJ' and $Cash &gt; 0">
						  <xsl:value-of select="'CASH_DEP'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'GS Transaction Commission' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'LCH Clearing Fee' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'CME Clearing Fee' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Initial Margin Maintenance Commission' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'INTEREST CHARGED' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'INTEREST PAID' and $Cash &gt; 0">
						  <xsl:value-of select="'Interest_Income'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'ICE CLEARING FEE' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Coupon Payment' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Coupon Payment' and $Cash &gt; 0">
						  <xsl:value-of select="'Interest_Income'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'Funding Fee' and $Cash &lt; 0">
						  <xsl:value-of select="'Cash'"/>
					  </xsl:when>
					  <xsl:when test="$varSubAccountDesc = 'DCS SWEEP USD' and $Cash &gt; 0">
						  <xsl:value-of select="'CASH_DEP'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

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
			
			   <xsl:variable name="varMM">
				  <xsl:call-template name="MonthCode1">
					  <xsl:with-param name="Month" select="substring-before(COL5,' ')"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name="varDD">
				  <xsl:value-of select="substring-before(substring-after(COL5, ' '), ',')" />
			  </xsl:variable>
			  <xsl:variable name="varYYYY">
				  <xsl:value-of select="substring-after(COL5, ', ')" />
			  </xsl:variable>
			  
			  <xsl:variable name="varTradeDate">
				  <xsl:call-template name="FormatDate">
					  <xsl:with-param name="DateTime" select="COL5"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <Date>
				  <xsl:value-of select ="$varTradeDate"/>
			  </Date>

			  <FXRate>
			 <xsl:choose>
                <xsl:when test="COL6 ='USD'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
               <xsl:otherwise>
                  <xsl:value-of select ="'0'"/>
                </xsl:otherwise>
              </xsl:choose>
			  </FXRate>
			  
            <Description>
				<xsl:value-of select="$varSubAccountDesc"/>
            </Description>
			  
			<CurrencyName>
					<xsl:value-of select="COL6"/>			
			  </CurrencyName>  
			
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>