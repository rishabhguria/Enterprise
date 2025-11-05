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
            <xsl:with-param name="Number" select="normalize-space(COL30)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash) and COL17!='MTM' and COL17!='TYP'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "VarMiscExpense">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>

                <xsl:when test="$VarMiscExpense ='INT. CHARGED ON DEBIT BALANCES' or $VarMiscExpense ='BOND INTEREST CHARGED' or $VarMiscExpense ='BOND INTEREST RECEIVED' or $VarMiscExpense ='BOND INTEREST ADJUSTMENT' or $VarMiscExpense ='SHORT SALE INTEREST BORROWCHARGE'">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL4"/>
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
              <xsl:value-of select="normalize-space(COL29)"/>
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
              <xsl:choose>
                <xsl:when test="$PRANA_ACRONYM_NAME!=''">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $AbsCash , '|Cash:' , $AbsCash)"/>
                </xsl:when>
                
                <xsl:when test="$Cash &lt; 0">
                  <xsl:value-of select="concat('CASH_WDL',':' , $AbsCash , '|Cash:' , $AbsCash)"/>
                </xsl:when>

                <xsl:when  test="$Cash &gt; 0">
                  <xsl:value-of select="concat('Cash:' , $AbsCash , '|' , 'CASH_DEP' , ':' , $AbsCash)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            <xsl:variable name="Description" select="normalize-space(COL7)"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>