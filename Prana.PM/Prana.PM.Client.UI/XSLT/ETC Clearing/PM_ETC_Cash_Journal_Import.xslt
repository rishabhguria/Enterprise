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

        <xsl:if test="number($varCash) and not ((COL3 ='NON-TRADE ACTIVITY') and (COL10 = 0) and (contains(COL4,'DIV') or contains(COL4,'Option Expiration') or contains(COL4,'Expired rights') or contains(COL4,'Rights distribution') or contains(COL4,'Spin off')))">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>
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
              <xsl:value-of select="COL7"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="$Date"/>
            </Date>

            <CurrencyName>
              <xsl:value-of select="COL2"/>
            </CurrencyName>

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

            <xsl:variable name="varJournalDes">
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="(COL3 ='NON-TRADE ACTIVITY') and (COL10 = 0) and (contains(COL4,'Transfer') or contains(COL4,'Wire In') or contains(COL4,'JNL Request Per Ops') or contains(COL4,'Spin off'))">
                  <xsl:choose>
                    <xsl:when test="$varCash &gt; 0">
                      <xsl:value-of select="concat('Cash',':', $varAmount , '|','CASH_DEP', ':' , $varAmount)"/>
                    </xsl:when>
                    <xsl:when test="$varCash &lt; 0">
                      <xsl:value-of select="concat('CASH_WDL',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                    </xsl:when>
                  </xsl:choose>              
                </xsl:when>
                
                <xsl:when test="(COL3 ='NON-TRADE ACTIVITY') and (COL10 = 0) and contains(COL4,'Clearing Fee')">
                  <xsl:choose>
                    <xsl:when test="$varCash &gt; 0">
                      <xsl:value-of select="concat('Cash',':', $varAmount , '|','Clearing Fees', ':' , $varAmount)"/>
                    </xsl:when>
                    <xsl:when test="$varCash &lt; 0">
                      <xsl:value-of select="concat('Clearing Fees',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                
                <xsl:when test="(COL3 ='NON-TRADE ACTIVITY') and (COL10 = 0) and contains(COL4,'Custody Fee')">
                  <xsl:choose>
                    <xsl:when test="$varCash &gt; 0">
                      <xsl:value-of select="concat('Cash',':', $varAmount , '|','CUST FEE', ':' , $varAmount)"/>
                    </xsl:when>
                    <xsl:when test="$varCash &lt; 0">
                      <xsl:value-of select="concat('CUST FEE',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>

                <xsl:when test="(COL3 ='NON-TRADE ACTIVITY') and (COL10 = 0) and (contains(COL4,'Margin Int') or contains(COL4,'Interest Charges'))">
                  <xsl:choose>
                    <xsl:when test="$varCash &gt; 0">
                      <xsl:value-of select="concat('Cash',':', $varAmount , '|','Interest Income', ':' , $varAmount)"/>
                    </xsl:when>
                    <xsl:when test="$varCash &lt; 0">
                      <xsl:value-of select="concat('Interest_Expense',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varCash &gt; 0">
                      <xsl:value-of select="concat('Cash',':', $varAmount , '|','MISC_INC', ':' , $varAmount)"/>
                    </xsl:when>
                    <xsl:when test="$varCash &lt; 0">
                      <xsl:value-of select="concat('MISC_EXP',':', $varAmount , '|','Cash', ':' , $varAmount)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>

              </xsl:choose>
            </JournalEntries>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>