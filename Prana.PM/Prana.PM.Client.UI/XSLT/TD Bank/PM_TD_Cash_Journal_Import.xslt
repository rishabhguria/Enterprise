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
        <xsl:if test="number(COL17)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'TD Securities'"/>
            </xsl:variable>

            <xsl:variable name = "PB_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL19)"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
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

            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="COL6"/>
            </xsl:variable>
            
            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>

            
            <xsl:variable name="varCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name = "varAmount" >
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

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="contains($PB_ACRONYM_NAME,'STOCK LOAN') and ($varCash &gt; 0)">
                  <xsl:value-of select="concat('Cash',':', $varAmount , '|','Stock Loan Fees Income', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'DEBIT INTEREST') and ($varCash &lt; 0)">
                  <xsl:value-of select="concat('Interest_Expense',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'CREDIT INTEREST') and ($varCash &gt; 0)">
                  <xsl:value-of select="concat('Cash',':', $varAmount , '|','Interest_Income', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'WIRE') and ($varCash &gt; 0)">
                  <xsl:value-of select="concat('Cash',':', $varAmount , '|','CASH_DEP', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'WIRE') and ($varCash &lt; 0)">
                  <xsl:value-of select="concat('CASH_WDL',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            <Description>
              <xsl:choose>
                <xsl:when test="contains($PB_ACRONYM_NAME,'STOCK LOAN') and ($varCash &gt; 0)">
                  <xsl:value-of select="'Stock Loan'"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'DEBIT INTEREST') and ($varCash &lt; 0)">
                  <xsl:value-of select="'Interest Expense'"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'CREDIT INTEREST') and ($varCash &gt; 0)">
                  <xsl:value-of select="'Interest Income'"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'WIRE')">
                  <xsl:value-of select="'Wire'"/>
                </xsl:when>
              </xsl:choose>
            </Description>

            <Symbol>
              <xsl:choose>
                <xsl:when test="contains($PB_ACRONYM_NAME,'STOCK LOAN') and ($varCash &gt; 0)">
                  <xsl:value-of select="'STOCK LOAN'"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'DEBIT INTEREST') and ($varCash &lt; 0)">
                  <xsl:value-of select="'DEBIT INTEREST'"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'CREDIT INTEREST') and ($varCash &gt; 0)">
                  <xsl:value-of select="'CREDIT INTEREST'"/>
                </xsl:when>
              </xsl:choose>
            </Symbol>
            
           <!-- <xsl:variable name="varFXRate"> -->
              <!-- <xsl:value-of select="number(COL18) div number(COL17)"/> -->
            <!-- </xsl:variable> -->
            <!-- <FxRate> -->
              <!-- <xsl:choose> -->
                <!-- <xsl:when test="COL2='USD'"> -->
                  <!-- <xsl:value-of select="'1'"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:otherwise> -->
                  <!-- <xsl:value-of select="0"/> -->
                <!-- </xsl:otherwise> -->
              <!-- </xsl:choose> -->
            <!-- </FxRate> -->
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>