<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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

    <xsl:variable name="varDay">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'/'),'/'))=1">
              <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'/'),'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before(substring-after($DateTime,'/'),'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'-'),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'-'),'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after($DateTime,'-'),'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(normalize-space($DateTime),'/'))=1">
              <xsl:value-of select="concat(0,substring-before($DateTime,'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before($DateTime,'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(normalize-space($DateTime),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before($DateTime,'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before($DateTime,'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:value-of select="substring-after(substring-after($DateTime,'/'),'/')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:value-of select="substring-after(substring-after($DateTime,'-'),'-')"/>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>


    <xsl:value-of select="$varMonth"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varDay"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varYear"/>
  </xsl:template>
  
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
        
      <xsl:variable name="Cash">
        <xsl:call-template name="Translate">
          <xsl:with-param name="Number" select="normalize-space(COL6)"/>
        </xsl:call-template>
      </xsl:variable>


		  <!--<xsl:if test="normalize-space(normalize-space(COL5)) = 'CONTRIBUTIONS' or normalize-space(normalize-space(COL5)) = 'WITHDRAWALS'or normalize-space(normalize-space(COL5)) = 'OTHER INC/EXP' or normalize-space(normalize-space(COL3)) = 'INTR' or normalize-space(normalize-space(COL3)) = 'DEPOSIT WITHHOLDING IRS' or normalize-space(normalize-space(COL4)) = 'CINTR' or normalize-space(normalize-space(COL4)) = 'JNL' or  normalize-space(normalize-space(COL4)) = 'FEDWT' and  (normalize-space(normalize-space(COL4))!='DIV' or  normalize-space(normalize-space(COL4))!='JRL')">-->
		  <xsl:if test="number($Cash) and COL4!='DIVPAY'">
			  <PositionMaster>

				  <!--Start Of mandatory columns-->

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="COL1"/>
          </xsl:variable>

          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          
				  <xsl:variable name="PRANA_ACRONYM_NAME">
					  <xsl:choose>
					
						  <xsl:when test="contains(COL4,'CSHFEE') and ($Cash &lt; 0)">
							  <xsl:value-of select="'Misc_Fees'"/>
						  </xsl:when>
						  
						  <xsl:when test="contains(COL4,'CSHMVE') and ($Cash &lt; 0)">
							  <xsl:value-of select="'Misc_Fees'"/>
						  </xsl:when>


						  <xsl:when test="contains(COL4,'MGNADV') and ($Cash &lt; 0)">
							  <xsl:value-of select="'M2MExpense'"/>
						  </xsl:when>
						  <xsl:when test="contains(COL4,'MGNADV') and (COL6 &gt; 0)">
							  <xsl:value-of select="'M2MIncome'"/>
						  </xsl:when>

						  <xsl:when test="contains(COL4,'MGNMTM') and (COL6 &lt; 0)">
							  <xsl:value-of select="'M2MExpense'"/>
						  </xsl:when>
						  <xsl:when test="contains(COL4,'MGNMTM') and (COL6 &gt; 0)">
							  <xsl:value-of select="'M2MIncome'"/>
						  </xsl:when>


						  <xsl:when test="contains(COL4,'ACTMVT') and ($Cash &lt; 0)">
							  <xsl:value-of select="'CASH_WDL'"/>
						  </xsl:when>
						  
						  <xsl:when test="contains(COL4,'CRIPAY') and ($Cash &lt; 0)">
							  <xsl:value-of select="'Interest_Expense'"/>
						  </xsl:when>
						  
						  <xsl:when test="contains(COL4,'ACTMVT') and ($Cash &gt; 0)">
							  <xsl:value-of select="'CASH_DEP'"/>
						  </xsl:when>
						  
						  <xsl:when test="contains(COL4,'CSHJNL') and ($Cash &lt; 0)">
							  <xsl:value-of select="'MISC_EXP'"/>
						  </xsl:when>
						  
						  <xsl:when test="contains(COL4,'CSHJNL') and ($Cash &gt; 0)">
							  <xsl:value-of select="'MISC_INC'"/>
						  </xsl:when>

						  <xsl:when test="contains(COL4,'CSHGEN') and ($Cash &lt; 0)">
							  <xsl:value-of select="'MISC_EXP'"/>
						  </xsl:when>

						  <xsl:when test="contains(COL4,'CSHGEN') and ($Cash &gt; 0)">
							  <xsl:value-of select="'MISC_INC'"/>
						  </xsl:when>
						  
					  </xsl:choose>
				  </xsl:variable>

				  <AccountName>
					  <xsl:choose>
						  <xsl:when test="$PRANA_FUND_NAME!=''">
							  <xsl:value-of select="$PRANA_FUND_NAME"/>
						  </xsl:when>

						  <xsl:otherwise>
							  <xsl:value-of select="$PB_FUND_NAME"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </AccountName>

				  <xsl:variable name = "amount" >
					  <xsl:choose>
						  <xsl:when test="number($Cash) &gt; 0">
							  <xsl:value-of select="number(COL6)"/>
						  </xsl:when>
						  <xsl:when test="number($Cash) &lt; 0">
							  <xsl:value-of select="number(COL6)*-1"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="0"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

          <xsl:variable name="varTradeDate">
            <xsl:choose>
              <xsl:when test="COL2!='' and COL2!='*'">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="normalize-space(COL2)"/>
                </xsl:call-template>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
				  <Date>
            <xsl:value-of select="$varTradeDate"/>
				  </Date>

				  <Description>

					  <xsl:value-of select ="normalize-space(COL3)"/>
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
              <xsl:when test="$Cash &gt; 0 and (normalize-space(COL3)='DOM WIRE' or contains(normalize-space(COL3),'ONE TIME ACH') 
                        or normalize-space(COL3)='WIRE' or contains(normalize-space(COL3),'Tranfer') or contains(normalize-space(COL3),'WIRE TRF/C'))">
                <xsl:value-of select="concat('Cash:', $amount , '|', 'CashTransferIn', ':' , $amount)"/>
              </xsl:when>
              
              <xsl:when test="$Cash &lt; 0 and (normalize-space(COL3)='DOM WIRE' or contains(normalize-space(COL3),'ONE TIME ACH') 
                        or normalize-space(COL3)='WIRE' or contains(normalize-space(COL3),'Tranfer') or contains(normalize-space(COL3),'WIRE TRF/C'))">
                <xsl:value-of select="concat('CashTransferIn', ':', $amount , '|', 'Cash', ':' , $amount)"/>
              </xsl:when>
              
						  <xsl:when test="$Cash &gt; 0">
							  <xsl:value-of select="concat('Cash:', $amount , '|', $PRANA_ACRONYM_NAME, ':' , $amount)"/>
						  </xsl:when>
						  <!--Amount positive-->
						  <xsl:when  test="$Cash &lt; 0">
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
