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
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
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
						<xsl:with-param name="Number" select="COL31"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash) and COL1 = 'DTPXX'">
				<!-- <xsl:if test="number($Cash) and ((contains(COL32, 'WFS Custody Fee Expense')) or (contains(COL32, 'Deposit')) or (contains(COL32, 'Buy')) or (contains(COL32, 'Sell')) or (contains(COL32, 'WFS Debit Interest')) or (contains(COL32, 'Withdrawal')) or (contains(COL32, 'WFS Securities Lending Income')) or (contains(COL32, 'Transfer To')) or (contains(COL32, 'WFS Security Lending Revenue')) or (contains(COL32, 'WFS Ticket Charge')) or (contains(COL32, 'WF Credit Interest'))or (contains(COL32, 'Transfer From')))">	 -->
					
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JEFF'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="varSymbol" select="normalize-space(COL1)"/>
					<xsl:variable name="CurrencyName">
							<xsl:value-of select="'USD'"/>
						</xsl:variable>
						<CurrencyName>
							<xsl:value-of select="$CurrencyName"/>
						</CurrencyName>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL16)"/>
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


						
			<xsl:variable name="Date">
              <xsl:call-template name="FormatDate">
               <xsl:with-param name="DateTime" select="COL2"/>
              </xsl:call-template>
            </xsl:variable>
						
						<Date>
							<xsl:value-of select="$Date"/>
						</Date>

						


						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Cash * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="(contains(COL6, 'Dividend Income'))">
									<xsl:value-of select="'Money_Fund_Dividend_Income'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Dividend Expense'))">
									<xsl:value-of select="'Money_Fund_Dividend_Income'"/>
								</xsl:when>
								<!-- <xsl:when test="(contains(COL6, 'Dividend Income')) and contains(COL16, '43000456')"> -->
									<!-- <xsl:value-of select="'DTPXX Money Fund'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'WFS Security Lending Revenue'))"> -->
									<!-- <xsl:value-of select="'MISC_INC'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'Withdrawal'))"> -->
									<!-- <xsl:value-of select="'CASH_WDL'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'WFS Ticket Charge'))"> -->
									<!-- <xsl:value-of select="'MISC_EXP'"/> -->
								<!-- </xsl:when>	 -->
								<!-- <xsl:when test="(contains(COL32, 'Transfer To'))"> -->
									<!-- <xsl:value-of select="'CashTransfer'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'Transfer From'))"> -->
									<!-- <xsl:value-of select="'CashTransfer'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'WFS Securities Lending Income'))"> -->
									<!-- <xsl:value-of select="'MISC_INC'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'WFS Credit Interest'))"> -->
									<!-- <xsl:value-of select="'Interest_Income'"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="(contains(COL32, 'Deposit'))"> -->
									<!-- <xsl:value-of select="'CASH_DEP'"/> -->
								<!-- </xsl:when> -->
								<xsl:when test="(contains(COL6, 'Buy'))">
									<xsl:value-of select="'DTPXX Money Fund'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Sell'))">
									<xsl:value-of select="'DTPXX Money Fund'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Withdraw'))">
									<xsl:value-of select="'CASH_WDL'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Deposit'))">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						
						
						<JournalEntries>
							<xsl:choose>
							<xsl:when test="(contains(COL6, 'Dividend Income')) and contains(COL16, '43000456')">
									<xsl:value-of select="concat( 'DTPXX Money Fund:',$AbsCash,'|', 'Money_Fund_Dividend_Income',':' , $AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
								
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="Description" select="COL6"/>
						<Description>
							<xsl:choose>
								<xsl:when test="(contains(COL6, 'Buy'))">
									<xsl:value-of select="'Money Market Trades'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Sell'))">
									<xsl:value-of select="'Money Market Trades'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Dividend Income'))">
									<xsl:value-of select="'Dividend Income'"/>
								</xsl:when>
								<xsl:when test="(contains(COL6, 'Dividend Expense'))">
									<xsl:value-of select="'Dividend Expense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Description"/>
								</xsl:otherwise>
							</xsl:choose>

						</Description>
						
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>