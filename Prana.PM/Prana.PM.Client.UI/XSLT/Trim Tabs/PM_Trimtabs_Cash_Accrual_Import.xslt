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
            <xsl:with-param name="Number" select="COL17"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL18"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="(number($Position) or number($varPosition)) and (normalize-space(COL2)='INVESTMENT INTEREST RECEIVABLE'
                or normalize-space(COL2)='ACCRUED UNITARY FEE EXPENSE' or normalize-space(COL2)='NET UNREAL CRNCY APPR ON INVESTMENTS - CASH'
                or normalize-space(COL2)='NET UNREAL CRNCY DEPR ON INVESTMENTS - CASH' or (contains(COL2,'MARK TO MARKET') and COL1 ='1011000300'))">
       
      
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Trimtabs'"/>
            </xsl:variable>

            <xsl:variable name = "PB_DR_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL2)"/>
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

            
            <Date>
              <xsl:value-of select="''"/>
            </Date>

            <CurrencyName>
              <xsl:value-of select="'USD'"/>
            </CurrencyName>



            <xsl:variable name="varAbs">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="varAbsCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>
            

            <xsl:variable name="varCash">
              <xsl:choose>
                <xsl:when test="normalize-space(COL2)='ACCRUED UNITARY FEE EXPENSE'">
                  <xsl:value-of select="$varAbs"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varAbsCash"/>
                </xsl:otherwise>
              </xsl:choose>
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
                <xsl:when test="normalize-space(COL2)='INVESTMENT INTEREST RECEIVABLE' and $Position &gt; 0">
                  <xsl:value-of select="'Interest_Receivable'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL2)='INVESTMENT INTEREST RECEIVABLE' and $Position &lt; 0">
                  <xsl:value-of select="'Interest Income'"/>
                </xsl:when>

                <xsl:when test="normalize-space(COL2)='ACCRUED UNITARY FEE EXPENSE' and $varAbs &gt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                
                <xsl:when test="normalize-space(COL2)='ACCRUED UNITARY FEE EXPENSE' and $varAbs &lt; 0">
                  <xsl:value-of select="'ACCRUEDUNITARYFEE'"/>
                </xsl:when>
                <xsl:when test="((normalize-space(COL2)='NET UNREAL CRNCY APPR ON INVESTMENTS - CASH' or normalize-space(COL2)='NET UNREAL CRNCY DEPR ON INVESTMENTS - CASH' or contains(COL2,'MARK TO MARKET')) and $varCash &gt; 0)">
                  <xsl:value-of select="'MTM'"/>
                </xsl:when>
                <xsl:when test="((normalize-space(COL2)='NET UNREAL CRNCY APPR ON INVESTMENTS - CASH' or normalize-space(COL2)='NET UNREAL CRNCY DEPR ON INVESTMENTS - CASH' or contains(COL2,'MARK TO MARKET')) and $varCash &lt; 0)">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
               
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="CR_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="normalize-space(COL2)='INVESTMENT INTEREST RECEIVABLE'  and $Position &gt; 0">
                  <xsl:value-of select="'Interest Income'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL2)='INVESTMENT INTEREST RECEIVABLE' and $Position &lt; 0">
                  <xsl:value-of select="'Interest_Receivable'"/>
                </xsl:when>
                
                <xsl:when test="normalize-space(COL2)='ACCRUED UNITARY FEE EXPENSE' and $varAbs &gt; 0">
                  <xsl:value-of select="'ACCRUEDUNITARYFEE'"/>
                </xsl:when>

                <xsl:when test="normalize-space(COL2)='ACCRUED UNITARY FEE EXPENSE' and $varAbs &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>

                <xsl:when test="((normalize-space(COL2)='NET UNREAL CRNCY APPR ON INVESTMENTS - CASH' or normalize-space(COL2)='NET UNREAL CRNCY DEPR ON INVESTMENTS - CASH' or contains(normalize-space(COL2),'MARK TO MARKET')) and $varCash &gt; 0)">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
                <xsl:when test="((normalize-space(COL2)='NET UNREAL CRNCY APPR ON INVESTMENTS - CASH' or normalize-space(COL2)='NET UNREAL CRNCY DEPR ON INVESTMENTS - CASH' or contains(normalize-space(COL2),'MARK TO MARKET')) and $varCash &lt; 0)">
                  <xsl:value-of select="'MTM'"/>
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
              <xsl:choose>
                <xsl:when test="contains(COL2,'MARK TO MARKET')">
                  <xsl:value-of select="'Mark to Market'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL2"/>
                </xsl:otherwise>
              </xsl:choose>
            </Description>
           
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>