<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template name="Translate">
    <xsl:param name="Number" />
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL33)"/>
          </xsl:call-template>
        </xsl:variable>
        
      

        <xsl:if test="number($varCash)" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''" />
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select ="COL3"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
              <xsl:value-of select="COL21"/>
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

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL10='Interest Expence' and $varCash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>
                <xsl:when test="contains(COL10,'Interest') and $varCash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>
                <xsl:when test="COL10='Cash Deposit' and $varCash &gt; 0">
                  <xsl:value-of select="'CASH_DEP'"/>
                </xsl:when>
                <xsl:when test="COL10='Cash Withdrawal' and $varCash &lt; 0">
                  <xsl:value-of select="'CASH_WDL'"/>
                </xsl:when>
				 <xsl:when test="COL12='Custody Fee' and $varCash &lt; 0">
                  <xsl:value-of select="'CUST FEE'"/>
                </xsl:when>
				 <xsl:when test="COL12='Debit Interest' and $varCash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>
				<xsl:when test="COL12='Debit Interest' and $varCash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>
				<xsl:when test="COL12='Credit Interest' and $varCash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>
				
				<xsl:when test="COL12='Negative Credit Int Charge' and $varCash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>
				
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="concat('Cash:', $varAmount , '|', $PRANA_ACRONYM_NAME, ':' , $varAmount)"/>
                </xsl:when>

                <xsl:when  test="$varCash &lt; 0">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $varAmount , '|Cash:' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>
            
            <xsl:variable name="varDescription">
              <xsl:value-of select="(normalize-space(COL12))" />
            </xsl:variable>
            <Description>
              <xsl:value-of select="$varDescription" />
            </Description>
            <FXRate>
              <xsl:choose>
                <xsl:when test="contains(COL21,'USD') ">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'0'"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

            <xsl:variable name="varYYYY">
              <xsl:value-of select="substring(COL29,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varMM">
              <xsl:value-of select="substring(COL29,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varDD">
              <xsl:value-of select="substring(COL29,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varPAYFULLDATE">
              <xsl:value-of select="concat($varMM,'/',$varDD,'/',$varYYYY)"/>
            </xsl:variable>

            <Date>
              <xsl:value-of select="$varPAYFULLDATE"/>
            </Date>
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>