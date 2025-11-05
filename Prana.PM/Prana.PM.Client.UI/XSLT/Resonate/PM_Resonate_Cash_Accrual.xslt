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
        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL58"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and COL26='Journal' and COL13='Financing'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'RiverEdge'"/>
            </xsl:variable>

            <xsl:variable name = "PB_DR_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_DR_ACRONYM_NAME">
              <xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@PBAcronymName=$PB_DR_ACRONYM_NAME]/@PranaAcronym"/>
            </xsl:variable>

            <xsl:variable name = "PB_CR_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_CR_ACRONYM_NAME">
              <xsl:value-of select="document('../ReconMappingXml/Cash_Journal_AcronymMapping.xml')/AcronymMapping/PB[@Name=$PB_NAME]/AcronymData[@PBAcronymName=$PB_CR_ACRONYM_NAME]/@PranaAcronym"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="CO3"/>
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

            <xsl:variable name="Date" select="COL36"/>
            <Date>
              <xsl:value-of select="$Date"/>
            </Date>

            <CurrencyName>
              <xsl:value-of select="COL55"/>
            </CurrencyName>

            <!--<CurrencyID>
              <xsl:value-of select="1"/>
            </CurrencyID>-->


            <xsl:variable name="varCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL58"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name = "Dramount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "Cramount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="DR_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL14 ='Interest' and COL12 ='SBR' and $varCash &gt; 0">
                  <xsl:value-of select="'Interest_Receivable'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Interest' and COL12 ='SBR' and $varCash &lt; 0">
                  <xsl:value-of select="'StockLoanExpense'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Interest' and $varCash &lt; 0">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Interest' and $varCash &gt; 0">
                  <xsl:value-of select="'Interest_Receivable'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Custody Fee'">
                  <xsl:value-of select="'Ticker FeesExpense'"/>
                </xsl:when>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="CR_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL14 ='Interest' and COL12 ='SBR' and $varCash &gt; 0">
                  <xsl:value-of select="'Interest_Income'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Interest' and COL12 ='SBR' and $varCash &lt; 0">
                  <xsl:value-of select="'StockLoanPayable'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Interest' and $varCash &lt; 0">
                  <xsl:value-of select="'Interest_Payable'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Interest' and $varCash &gt; 0">
                  <xsl:value-of select="'Interest_Income'"/>
                </xsl:when>
                <xsl:when test="COL14 ='Custody Fee'">
                  <xsl:value-of select="'TicketFeesPayable'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="number($Dramount)">
                  <xsl:value-of select="concat($DR_ACRONYM_NAME,':', $Dramount , '|',$CR_ACRONYM_NAME, ':' , $Cramount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            <xsl:variable name="Description" select="COL9"/>
            <Description>
              <xsl:value-of select="'Accrual'"/>
            </Description>
            <FXRate>
              <xsl:choose>
                <xsl:when test="COL55 !='USD'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>