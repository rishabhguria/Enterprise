<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template name="tempMetalSymbolCode">
    <xsl:param name="paramMetalSymbol"/>
    <!-- 1 characters for metal code -->
    <!--  e.g. A represents A = aluminium-->
    <xsl:choose>
      <xsl:when test ="$paramMetalSymbol='U S DOLLAR'">
        <xsl:value-of select ="'USD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='UK POUND STERLING'">
        <xsl:value-of select ="'GBP'"/>
      </xsl:when>

      <xsl:when test ="$paramMetalSymbol='EURO'">
        <xsl:value-of select ="'EUR'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='AUSTRALIAN DOLLAR'">
        <xsl:value-of select ="'AUD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='HONG KONG DOLLAR'">
        <xsl:value-of select ="'HKD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='INDONESIAN RUPIAH'">
        <xsl:value-of select ="'IDR'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='JAPANESE YEN'">
        <xsl:value-of select ="'JPY'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='MALAYSIAN RINGGIT'">
        <xsl:value-of select ="'MYR'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='NEW ZEALAND DOLLAR'">
        <xsl:value-of select ="'NZD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='PHILIPPINO PESO'">
        <xsl:value-of select ="'PBP'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='SINGAPORE DOLLAR'">
        <xsl:value-of select ="'SGD'"/>
      </xsl:when>

      <xsl:when test ="$paramMetalSymbol='THAI BAHT'">
        <xsl:value-of select ="'THB'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
		  <xsl:variable name = "PB_FUND_NAME">
			  <xsl:value-of select="COL6"/>
		  </xsl:variable>

		  <xsl:variable name="PRANA_FUND_NAME">
			  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
		  </xsl:variable>

		  <!--<xsl:if test="normalize-space(normalize-space(COL5)) = 'CONTRIBUTIONS' or normalize-space(normalize-space(COL5)) = 'WITHDRAWALS'or normalize-space(normalize-space(COL5)) = 'OTHER INC/EXP' or normalize-space(normalize-space(COL3)) = 'INTR' or normalize-space(normalize-space(COL3)) = 'DEPOSIT WITHHOLDING IRS' or normalize-space(normalize-space(COL4)) = 'CINTR' or normalize-space(normalize-space(COL4)) = 'JNL' or  normalize-space(normalize-space(COL4)) = 'FEDWT' and  (normalize-space(normalize-space(COL4))!='DIV' or  normalize-space(normalize-space(COL4))!='JRL')">-->
		  <xsl:if test="number(COL58) and normalize-space(normalize-space(COL14))= 'Interest'/>
			  <PositionMaster>

				  <!--Start Of mandatory columns-->


				  <xsl:variable name="PRANA_ACRONYM_NAME">
					  <xsl:choose>
					
						  <!--<xsl:when test="contains(COL3,'SHORT INT') or contains(COL3,'SHORT INTEREST')and (COL6 &lt; 0)">
							  <xsl:value-of select="'Short_Rebate'"/>
						  </xsl:when>

						  <xsl:when test="contains(COL4,'STAX') and (COL6 &lt; 0)">
							  <xsl:value-of select="'TAX_EXP'"/>
						 </xsl:when>-->

						  <xsl:when test="(normalize-space(COL14) = 'Interest' and (COL58 &gt; 0)">
							  <xsl:value-of select="'Interest_Income'"/>
						  </xsl:when>
						  <xsl:when test="(normalize-space(COL14) = 'Interest' and (COL58 &lt; 0)">
							  <xsl:value-of select="'Interest_Expense'"/>
						  </xsl:when>
						  
						  <!--<xsl:when test="(contains(COL3,'CIP') or contains(COL3,'FEE') or contains(COL3,'/15') or normalize-space(COL5) = 'OTHER INC/EXP') and (COL6 &gt; 0)">
							  <xsl:value-of select="'MISC_INC'"/>
						  </xsl:when>
						  <xsl:when test="(contains(COL3,'CIP') or contains(COL3,'/15')or contains(COL3,'STERLING') or contains(COL3,'NIRVANA') or normalize-space(COL5) = 'OTHER INC/EXP') and (COL6 &lt; 0)">
							  <xsl:value-of select="'MISC_EXP'"/>
						  </xsl:when>
						  						 					  
						  <xsl:when test="((normalize-space(COL4) = 'FEDWT') or (normalize-space(COL4) = 'JNL' or normalize-space(COL5) = 'WITHDRAWALS')) and (COL6 &lt; 0)  ">
							  <xsl:value-of select="'CASH_WDL'"/>
						  </xsl:when>

						  <xsl:when test="((normalize-space(COL4) = 'FEDWT' ) or (normalize-space(COL4) = 'JRL' or normalize-space(COL5) = 'CONTRIBUTIONS')) and (COL6 &gt; 0) ">
							  <xsl:value-of select="'CASH_DEP'"/>
						  </xsl:when>-->
					  </xsl:choose>
				  </xsl:variable>

				  <AccountName>
					  <xsl:choose>
						  <xsl:when test="$PRANA_FUND_NAME!=''">
							  <xsl:value-of select="$PRANA_FUND_NAME"/>
						  </xsl:when>
					  </xsl:choose>
				  </AccountName>

				  <xsl:variable name = "amount" >
					  <xsl:choose>
						  <xsl:when test="number(COL58) &gt; 0">
							  <xsl:value-of select="number(COL58)"/>
						  </xsl:when>
						  <xsl:when test="number(COL58) &lt; 0">
							  <xsl:value-of select="number(COL58)*-1"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="0"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

				  <Date>
					  <xsl:value-of select="COL35"/>
				  </Date>

				  <Description>

					  <xsl:value-of select ="normalize-space(COL15)"/>
					  <!--<xsl:value-of select ="normalize-space(concat(COL3, 'For ', COL7))"/>-->

				  </Description>

				  <CurrencyName>
					  <xsl:value-of select ="'USD'"/>
				  </CurrencyName>

				  <CurrencyID>
					  <xsl:value-of select ="'1'"/>
				  </CurrencyID>

				  <JournalEntries>
					  <!-- Note							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  -->
					  <!-- Amount Negative-->
					  <xsl:choose>
						  <xsl:when test="COL58 &gt; 0">
							  <xsl:value-of select="concat('Cash:', $amount , '|', $PRANA_ACRONYM_NAME, ':' , $amount)"/>
						  </xsl:when>
						  <!--Amount positive-->
						  <xsl:when  test="COL58 &lt; 0">
							  <xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $amount , '|Cash:' , $amount)"/>
						  </xsl:when>
					  </xsl:choose>
				  </JournalEntries>

				  <!--End Of mandatory columns-->

			  </PositionMaster>
		  </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
