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
            <xsl:with-param name="Number" select="normalize-space(COL15)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>

                <xsl:when test="$varSubAccountDesc ='WIRED FUNDS FEE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='FEDERAL FUNDS SENT'">
                  <xsl:value-of select="'Capital Redemption'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='AGENT SERVICING FEE'">
                  <xsl:value-of select="'ClearingExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='FEE ON FOREIGN DIVIDEND WITHHELD AT THE SOURCE'">
                  <xsl:value-of select="'ForeignTaxWithholding'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='FOREIGN CUSTODY FEE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='INT. CHARGED ON DEBIT BALANCES'">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>

              
                <xsl:when test="$varSubAccountDesc ='SERVICE CHARGE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='INTRA - ACCT JNL'">
                  <xsl:value-of select="'MiscFeesPayable'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Fund - Operating Expenses'">
                  <xsl:value-of select="'ADMINISTRATIONFEEEXPENSE'"/>
                </xsl:when>

				  <xsl:when test="$varSubAccountDesc ='INTEREST CHARGED ON DEBIT'">
					  <xsl:value-of select="'Interest_Expense'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>

                <xsl:when test="$varSubAccountDesc ='WIRED FUNDS FEE' or $varSubAccountDesc ='FEDERAL FUNDS SENT'
                          or $varSubAccountDesc ='AGENT SERVICING FEE'
                          or $varSubAccountDesc ='FEE ON FOREIGN DIVIDEND WITHHELD AT THE SOURCE' or $varSubAccountDesc ='FOREIGN CUSTODY FEE' 
                          or $varSubAccountDesc ='INT. CHARGED ON DEBIT BALANCES'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Fund - Operating Expenses'">
                  <xsl:value-of select="'ADMINISTRATIONFEESPAYABLE'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="''"/>
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

            <xsl:variable name="varCurrency" select="normalize-space(COL19)"/>
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

            <xsl:variable name="varDay" select="COL1"/>
           
            <Date>
              <xsl:value-of select="$varDay"/>
            </Date>

            <xsl:variable name="Description" select="normalize-space(COL32)"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>