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

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL24)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL11)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>
				  <xsl:when test="$varSubAccountDesc ='WFS Credit Interest'">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>

                <xsl:when test="$varSubAccountDesc ='WFS Debit Interest'">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>

				  <xsl:when test="$varSubAccountDesc ='WFS Security Lending Revenue' and $Cash &lt; 0">
					  <xsl:value-of select="'StockLoanExpense'"/>
				  </xsl:when>
					<xsl:when test="$varSubAccountDesc ='WFS Security Lending Revenue' and $Cash &gt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='WFS Custody Fee Expense'">
					  <xsl:value-of select="'ClearingExpenses'"/>
				  </xsl:when>
                <xsl:when test="$varSubAccountDesc ='Deposit'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Withdrawal'">
                  <xsl:value-of select="'CASH_WDL'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>
				<xsl:when test="$varSubAccountDesc ='WFS Credit Interest'">
					  <xsl:value-of select="'Interest_Income'"/>
				  </xsl:when>
                <xsl:when test="$varSubAccountDesc ='WFS Debit Interest'
                          or $varSubAccountDesc ='WFS Custody Fee Expense'
                          or $varSubAccountDesc ='WFS Ticket Charge'
                          or $varSubAccountDesc ='Withdrawal' or $varSubAccountDesc ='WFS Custody Fee Expense'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				<xsl:when test="$varSubAccountDesc ='WFS Security Lending Revenue' and $Cash &lt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
                <xsl:when test="$varSubAccountDesc ='Deposit'">
                  <xsl:value-of select="'CASH_DEP'"/>
                </xsl:when>
				<xsl:when test="$varSubAccountDesc ='WFS Security Lending Revenue' and $Cash &gt; 0">
					  <xsl:value-of select="'RebateIncome'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
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

            <xsl:variable name="varCurrency" select="normalize-space(COL5)"/>
            <CurrencyName>
              <xsl:value-of select ="$varCurrency"/>
            </CurrencyName>

            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="$Cash &gt; 0">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:when test="$Cash &lt; 0">
                  <xsl:value-of select="$Cash*-1"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:value-of select="concat($PRANA_ACRONYM_NAME_PRE, ':' , $AbsCash , '|' , $PRANA_ACRONYM_NAME_POST, ':' , $AbsCash)"/>
            </JournalEntries>

            <xsl:variable name="Day" select="substring-before(substring-after(COL26,'-'),'-')"/>
            <xsl:variable name="Month" select="substring-before(COL26,'-')"/>
            <xsl:variable name="Year" select="substring-after(substring-after(COL26,'-'),'-')"/>
            <Date>
              <xsl:value-of select="COL14"/>
            </Date>

         
            <xsl:variable name="Description" select="normalize-space(COL11)"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
			  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>