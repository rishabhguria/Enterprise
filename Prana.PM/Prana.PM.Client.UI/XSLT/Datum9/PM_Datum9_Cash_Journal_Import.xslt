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

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL17"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varCash) and not(contains(COL7,'NO ACTIVITY')) and not(contains(COL18,'DIVIDEND'))">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name ="varDate" select="COL9"/>
            <Date>
              <xsl:value-of select="$varDate"/>
            </Date>


            <xsl:variable name ="varCurrencyName" select="COL8"/>
            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>




            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
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
            

            
            <xsl:variable name ="Description" select="COL18"/>
            <Description>
              <xsl:choose>
                <xsl:when test="$Description!=''">
                  <xsl:value-of select="$Description" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''" />
                </xsl:otherwise>
              </xsl:choose>
            </Description>

            <xsl:variable name ="varAmount">
            <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>

                <xsl:when  test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash * (-1)"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
           
         

            <xsl:variable name ="varActivity" select="COL11"/>
            <xsl:variable name ="varBrokerDescription" select="COL18"/>
            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varActivity='DEPOSIT' and contains($varBrokerDescription,'INTEREST')">
                  <xsl:value-of select="concat('Cash:', $varAmount, '|', 'Interest_Income', ':' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
              <xsl:choose>
                <xsl:when test="$varActivity='DEPOSIT' and not(contains($varBrokerDescription,'INTEREST'))">
                  <xsl:value-of select="concat('Cash:', $varAmount, '|', 'CASH_DEP', ':' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
              <xsl:choose>
                <xsl:when test="$varActivity='WITHDRAWAL' and contains($varBrokerDescription,'INTEREST')">
                  <xsl:value-of select="concat('Interest_Expense:', $varAmount, '|', 'Cash', ':' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
              <xsl:choose>
                <xsl:when test="$varActivity='WITHDRAWAL' and not(contains($varBrokerDescription,'INTEREST'))">
                  <xsl:value-of select="concat('CASH_WDL:', $varAmount, '|', 'Cash', ':' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>