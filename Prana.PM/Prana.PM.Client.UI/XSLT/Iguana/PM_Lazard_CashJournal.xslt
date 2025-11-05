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

	<xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>

		<xsl:choose>
			<xsl:when test ="$varMonth='Jan'">
				<xsl:value-of select ="'01'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Feb'">
				<xsl:value-of select ="'02'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Mar'">
				<xsl:value-of select ="'03'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Apr'">
				<xsl:value-of select ="'04'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='May'">
				<xsl:value-of select ="'05'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Jun'">
				<xsl:value-of select ="'06'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Jul'">
				<xsl:value-of select ="'07'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Aug'">
				<xsl:value-of select ="'08'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Sep'">
				<xsl:value-of select ="'09'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Oct'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Nov'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Dec'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="normalize-space(COL10) = '0' and  normalize-space(COL16) != 'DIV'">
          <PositionMaster>

            <!--Start Of mandatory columns-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="substring(COL4,5,5)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Lazard']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <xsl:variable name ="varDescription">
				  <xsl:choose>
					  <xsl:when test ="COL16= '*'">
						  <xsl:value-of select ="COL2"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test ="COL16= '/WF'">
								  <xsl:value-of select ="'WIRED FUNDS FEE'"/>
							  </xsl:when>
							  <xsl:when test ="COL16= 'SFF'">
								  <xsl:value-of select ="'FEDERAL FUNDS SENT'"/>
							  </xsl:when>
							  <xsl:when test ="COL16= 'SWR'">
								  <xsl:value-of select ="'MONEY FUND REDEMPTION'"/>
							  </xsl:when>
							  <xsl:when test ="COL16= 'INM'">
								  <xsl:value-of select ="'INT. CHARGED ON DEBIT BALANCES'"/>
							  </xsl:when>
							  <xsl:when test ="COL16= 'FRE'">
								  <xsl:value-of select ="'FUNDS RECEIVED INTO YOUR  ACCOUNT'"/>
							  </xsl:when>
							  <xsl:when test ="COL16= 'SWP'">
								  <xsl:value-of select ="'MONEY FUND PURCHASE'"/>
							  </xsl:when>
							  <xsl:when test ="COL16= 'SVC'">
								  <xsl:value-of select ="'SERVICE CHARGE'"/>
							  </xsl:when>
						  </xsl:choose>
						  
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name = "PRANA_ACRONYM_NAME">
				  <xsl:choose>
					  <xsl:when test ="COL14 &gt; 0 and (normalize-space($varDescription) = 'WIR' or normalize-space($varDescription) = 'TFR' or normalize-space($varDescription) = 'MONEY FUND REDEMPTION' or normalize-space($varDescription) = 'FUNDS RECEIVED INTO YOUR  ACCOUNT' or normalize-space($varDescription) = 'MONEY FUND PURCHASE') ">
						  <xsl:value-of select="'CASH-WDL'"/>
					  </xsl:when>
					  <xsl:when test ="COL14 &lt; 0 and (normalize-space($varDescription) = 'WIR' or normalize-space($varDescription) = 'TFR' or normalize-space($varDescription) = 'MONEY FUND REDEMPTION' or normalize-space($varDescription) = 'FUNDS RECEIVED INTO YOUR  ACCOUNT' or normalize-space($varDescription) = 'MONEY FUND PURCHASE')">
						  <xsl:value-of select="'CASH-DEP'"/>
					  </xsl:when>
					  <xsl:when test ="COL14 &gt; 0 and (normalize-space($varDescription) = 'INT' or normalize-space($varDescription) = 'INT. CHARGED ON DEBIT BALANCES')">
						  <xsl:value-of select="'Interest_Expense'"/>
					  </xsl:when>
					  <xsl:when test ="COL14 &lt; 0 and (normalize-space($varDescription) = 'INT' or normalize-space($varDescription) = 'INT. CHARGED ON DEBIT BALANCES')">
						  <xsl:value-of select="'Interest_Income'"/>
					  </xsl:when>
					  <xsl:when test ="COL14 &gt; 0 and (normalize-space($varDescription) = 'FEE' or normalize-space($varDescription) = 'WIRED FUNDS FEE' or normalize-space($varDescription) = 'FEDERAL FUNDS SENT' or normalize-space($varDescription) = 'SERVICE CHARGE') ">
						  <xsl:value-of select="'MISC-EXP'"/>
					  </xsl:when>
					  <xsl:when test ="COL14 &lt; 0 and (normalize-space($varDescription) = 'FEE' or normalize-space($varDescription) = 'WIRED FUNDS FEE' or normalize-space($varDescription) = 'FEDERAL FUNDS SENT' or normalize-space($varDescription) = 'SERVICE CHARGE')">
						  <xsl:value-of select="'MISC-INC'"/>
					  </xsl:when>

				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name ="varMonthTrade">
				  <xsl:call-template name ="MonthNo">
					  <xsl:with-param name="varMonth" select="substring-before(COL5,' ')"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name="varPositionStartDate">
				  <xsl:value-of select ="concat($varMonthTrade,'/',substring(COL5,5,2),'/',substring(COL5,8,4))"/>
				  <!--<xsl:value-of select="COL5"/>-->
			  </xsl:variable>


			  <AccountName>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME = '*' or $PRANA_FUND_NAME = ''">
						  <xsl:value-of select="COL4"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$PRANA_FUND_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </AccountName>


			  <FundID>
				  <xsl:value-of select="0"/>
			  </FundID>

			  <xsl:variable name = "amount" >
				  <xsl:choose>
					  <xsl:when test="COL14 &gt; 0">
						  <xsl:value-of select="COL14"/>
					  </xsl:when>
					  <xsl:when test="COL14 &lt; 0">
						  <xsl:value-of select="COL14*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <Date>
              <xsl:value-of select="COL5"/>
            </Date>

            <Description>
				<xsl:value-of select ="$varDescription"/>
            </Description>

            <CurrencyName>
              <xsl:value-of select ="COL3"/>
            </CurrencyName>

            <CurrencyID>
              <xsl:value-of select ="0"/>
            </CurrencyID>

            <JournalEntries>
              <xsl:choose>
                <!-- Note
							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  -->
                <!-- Amount positive-->
				  <xsl:when test ="$PRANA_FUND_NAME = '*'">
					  <xsl:value-of select ="''"/>
				  </xsl:when>
                <xsl:when test="COL14 &lt; 0">
                  <xsl:value-of select="concat('Cash:' , $amount , '|', $PRANA_ACRONYM_NAME, ':' , $amount)"/>
                </xsl:when>
                <!-- Amount negative-->
                <xsl:when  test="COL14 &gt; 0">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount , '|Cash:' , $amount)"/>
                </xsl:when>
                <!--<xsl:otherwise>
							  <xsl:value-of select="concat('Interest_Expense:' , $amount , '|Cash:' , $amount)"/>
						  </xsl:otherwise>-->
              </xsl:choose>
            </JournalEntries>

            <!--End Of mandatory columns-->

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
