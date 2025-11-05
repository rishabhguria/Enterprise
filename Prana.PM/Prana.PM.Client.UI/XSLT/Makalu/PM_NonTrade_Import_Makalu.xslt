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
            <xsl:with-param name="Number" select="normalize-space(COL10)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash) and normalize-space(COL2)!='MARK TO MARKET'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "varPreSubAccountDesc">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>

                <xsl:when test="$varPreSubAccountDesc ='AGENT SERVICING FEE'"> 
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='CASH DIVIDEND CHARGED'">
                  <xsl:value-of select="'MiscFeesPayable'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='FEDERAL FUNDS RECEIVED'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='FEDERAL FUNDS SENT'">
                  <xsl:value-of select="'ADMINISTRATIONFEESPAYABLE'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='FEE ON FOREIGN DIVIDEND WITHHELD AT THE SOURCE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='FOREIGN CUSTODY FEE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='FREE CREDIT BALANCE INTEREST CHARGE'">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='INT. CHARGED ON DEBIT BALANCES'">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='SERVICE CHARGE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='SHORT SALE INTEREST BORROWCHARGE'">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>

                <xsl:when test="$varPreSubAccountDesc ='WIRED FUNDS FEE'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>


            <xsl:variable name = "varPostSubAccountDesc">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>

                <xsl:when test="$varPostSubAccountDesc ='AGENT SERVICING FEE' 
                          or $varPostSubAccountDesc ='CASH DIVIDEND CHARGED' or $varPostSubAccountDesc ='FEDERAL FUNDS SENT' 
                          or $varPostSubAccountDesc ='FEE ON FOREIGN DIVIDEND WITHHELD AT THE SOURCE' or $varPostSubAccountDesc ='FOREIGN CUSTODY FEE' 
                          or $varPostSubAccountDesc ='FREE CREDIT BALANCE INTEREST CHARGE' or $varPostSubAccountDesc ='INT. CHARGED ON DEBIT BALANCES'
                          or $varPostSubAccountDesc ='SERVICE CHARGE' or $varPostSubAccountDesc ='SHORT SALE INTEREST BORROWCHARGE' or $varPostSubAccountDesc ='WIRED FUNDS FEE'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varPostSubAccountDesc ='FEDERAL FUNDS RECEIVED'">
                  <xsl:value-of select="'CONTRIBUTEDCAPITAL'"/>
                </xsl:when>
                
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="''"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
              <xsl:value-of select="normalize-space(COL19)"/>
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
              <xsl:value-of select="concat($PRANA_ACRONYM_NAME_PRE, ':' , $AbsCash , '|', $PRANA_ACRONYM_NAME_POST, ':' , $AbsCash)"/>
            </JournalEntries>

            <xsl:variable name="Description" select="normalize-space(COL2)"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>