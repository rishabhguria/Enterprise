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
              <xsl:value-of select="normalize-space(COL27)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>

                <xsl:when test="$varSubAccountDesc ='01/29-02/25 DEBIT INTEREST' 
                          or $varSubAccountDesc ='01/31-02/25 DB EUR INTEREST' or $varSubAccountDesc ='01/31-02/25 DB GBP INTEREST' 
                          or $varSubAccountDesc ='INTEREST - MTH ENDING 02/28/21'">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='0884709 CLEARING FEE' or $varSubAccountDesc ='INTL SHORTS PRM B2R9XL5 AMMM' 
                          or $varSubAccountDesc ='MKT DATA/LICENSE FEE FOR 03/21' or $varSubAccountDesc ='PREM. ON SHORTS 01/21 AMMM' 
                          or $varSubAccountDesc ='ROUNDING ON ECM121 RESIDUAL CREDIT' or $varSubAccountDesc ='Settlement tarde of 251427' or $varSubAccountDesc ='SHORT PRM. EXP LOGI 03/21'">
                  <xsl:value-of select="'CASH_WDL'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='CDM121 RESIDUAL CREDIT' or $varSubAccountDesc ='CUST INT 02/26-03/31 FROM AE18' or $varSubAccountDesc ='WIRE FROM DAVID CAPITAL PARTNERS FUND, L'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>

                <xsl:when test="$varSubAccountDesc ='01/29-02/25 DEBIT INTEREST' 
                          or $varSubAccountDesc ='01/31-02/25 DB EUR INTEREST' or $varSubAccountDesc ='01/31-02/25 DB GBP INTEREST' 
                          or $varSubAccountDesc ='INTEREST - MTH ENDING 02/28/21' or $varSubAccountDesc ='0884709 CLEARING FEE' or $varSubAccountDesc ='INTL SHORTS PRM B2R9XL5 AMMM' 
                          or $varSubAccountDesc ='MKT DATA/LICENSE FEE FOR 03/21' or $varSubAccountDesc ='PREM. ON SHORTS 01/21 AMMM' 
                          or $varSubAccountDesc ='ROUNDING ON ECM121 RESIDUAL CREDIT' or $varSubAccountDesc ='Settlement tarde of 251427' or $varSubAccountDesc ='SHORT PRM. EXP LOGI 03/21'
                          or $varSubAccountDesc ='CDM121 RESIDUAL CREDIT'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='CUST INT 02/26-03/31 FROM AE18'">
                  <xsl:value-of select="'Interest_Income'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='WIRE FROM DAVID CAPITAL PARTNERS FUND, L'">
                  <xsl:value-of select="'CASH_DEP'"/>
                </xsl:when>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>
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
              <xsl:choose>
                <xsl:when test ="contains(normalize-space(COL8),'_')">
                  <xsl:value-of select ="substring-before(normalize-space(COL8),'_')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="normalize-space(COL8)"/>
                </xsl:otherwise>
              </xsl:choose>
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

            <Date>
              <xsl:value-of select="normalize-space(COL2)"/>
            </Date>

            <xsl:variable name="Description" select="normalize-space(COL27)"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>