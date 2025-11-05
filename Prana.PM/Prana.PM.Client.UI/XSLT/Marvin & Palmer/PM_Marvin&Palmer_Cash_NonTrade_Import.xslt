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
            <xsl:with-param name="Number" select="normalize-space(COL60)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'JPM'"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>

                <xsl:when test="contains($varSubAccountDesc,'INTEREST') and $Cash &gt; 0 ">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'DISBURSEMENT') and $Cash &gt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>		 
				 <xsl:when test="contains($varSubAccountDesc,'INTEREST') and $Cash &lt; 0 ">
                  <xsl:value-of select="'Interest_Payable'"/>
                </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'DISBURSEMENT') and $Cash &lt; 0">
					  <xsl:value-of select="'CashTransferOut'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'COMMISSIONS')">
					  <xsl:value-of select="'Cust_Fee'"/>
				  </xsl:when>
				  <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>
				   <xsl:when test="contains($varSubAccountDesc,'INTEREST') and $Cash &gt; 0 ">
                  <xsl:value-of select="'Interest_Receivable'"/>
                </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'DISBURSEMENT') and $Cash &gt; 0">
					  <xsl:value-of select="'CashTransferIn'"/>
				  </xsl:when>
				
				   <xsl:when test="contains($varSubAccountDesc,'INTEREST') and $Cash &lt; 0 ">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'DISBURSEMENT') and $Cash &lt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'COMMISSIONS')">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
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

            <xsl:variable name="varCurrency" select="normalize-space(COL23)"/>
            <CurrencyName>
              <xsl:value-of select ="$varCurrency"/>
            </CurrencyName>

            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="$Cash &gt; 0">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:when test="$Cash &lt; 0">
                  <xsl:value-of select="$Cash * -1"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:value-of select="concat($PRANA_ACRONYM_NAME_PRE, ':' , $AbsCash , '|' , $PRANA_ACRONYM_NAME_POST, ':' , $AbsCash)"/>
            </JournalEntries>
                   <xsl:variable name="varDay">
                           <xsl:value-of select="substring(COL3,7,2)"/>
                   </xsl:variable>
                   <xsl:variable name="varYear">
                           <xsl:value-of select="substring(COL3,1,4)"/>
                   </xsl:variable>

                   <xsl:variable name="varMonth">
                           <xsl:value-of select="substring(COL3,5,2)"/>
                   </xsl:variable>
                   <xsl:variable name="varCurrentDate">
                           <xsl:value-of select="concat($varMonth,'/',$varDay, '/', $varYear)"/>
                   </xsl:variable>
            <Date>
              <xsl:value-of select="$varCurrentDate"/>
            </Date>


            <xsl:variable name="Description" select="$varSubAccountDesc"/>
            <Description>
              <xsl:choose>
				   <xsl:when test="contains($varSubAccountDesc,'INTEREST') ">
                  <xsl:value-of select="COL13"/>
                </xsl:when>
				  <xsl:when test="contains($varSubAccountDesc,'DISBURSEMENT') ">
					  <xsl:value-of select="COL13"/>
				  </xsl:when>			
				  <xsl:when test="contains($varSubAccountDesc,'COMMISSIONS')">
					  <xsl:value-of select="COL14"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>