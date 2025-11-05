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
				  <xsl:with-param name="Number" select="COL58"/>
			  </xsl:call-template>
		  </xsl:variable>
		  
        <xsl:if test="number($varCash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_ACRONYM_NAME">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>
            
            <xsl:variable name="PB_FUND_NAME" select="COL3"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
              <xsl:value-of select="COL55"/>
            </xsl:variable>

            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
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
					  <xsl:when test="contains(COL14,'Interest') and ($varCash &gt; 0)">
						  <xsl:value-of select="'Interest_Receivable'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL14,'Interest') and ($varCash &lt; 0)">
						  <xsl:value-of select="'Interest_Payable'"/>
					  </xsl:when>

					  <xsl:when test="contains(COL14,'Custody Fee') and ($varCash &lt; 0)">
						  <xsl:value-of select="'CUST FEE'"/>
					  </xsl:when>

					  <xsl:when test="contains(COL14,'Stock Loan Fee') and ($varCash &lt; 0)">
						  <xsl:value-of select="'SL FEE PAY'"/>
					  </xsl:when>

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
			  <!--<JournalEntries>
              <xsl:choose>
                <xsl:when test="contains($PB_ACRONYM_NAME,'Interest') and ($varCash &gt; 0)">
                  <xsl:value-of select="concat('Interest_Receivable', ':' , $varAmount, '|Cash:',$varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'Interest') and ($varCash &lt; 0)">
                  <xsl:value-of select="concat('Cash',':', $varAmount , '|','Interest_Payable', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'Custody Fee') and ($varCash &lt; 0)">
                  <xsl:value-of select="concat('Cash',':', $varAmount , '|','CUST FEE', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when test="contains($PB_ACRONYM_NAME,'Stock Loan Fee') and ($varCash &lt; 0)">
                  <xsl:value-of select="concat('Cash',':', $varAmount , '|','SL FEE PAY', ':' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>-->


			  <xsl:variable name="varDescriptionName">
				  <xsl:value-of select="COL15"/>
			  </xsl:variable>
            <Description>
              <xsl:value-of select="$varDescriptionName"/>
            </Description>
       
           

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>