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
            <xsl:with-param name="Number" select="normalize-space(COL23)"/>
          </xsl:call-template>
        </xsl:variable>
		  <xsl:variable name = "varSubAccountDesc">
			  <xsl:value-of select="normalize-space(COL16)"/>
		  </xsl:variable>
        <xsl:if test="number($Cash) and $varSubAccountDesc != 'INTACC'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'IB'"/>
            </xsl:variable>

          

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>
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

            <xsl:variable name="varCurrency" select="normalize-space(COL10)"/>
            <CurrencyName>
              <xsl:value-of select ="$varCurrency"/>
            </CurrencyName>

            <xsl:variable name="varCash">
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
					<xsl:when test="$varCash &gt; 0">
						<xsl:value-of select="concat('Cash', ':' , $varCash , '|' , 'Suspense', ':' , $varCash)"/>
					</xsl:when>
					<xsl:when test="$varCash &lt; 0">
						<xsl:value-of select="concat('Suspense', ':' , $varCash , '|' , 'Cash', ':' , $varCash)"/>
					</xsl:when>
				</xsl:choose>
            </JournalEntries>
			  <xsl:variable name="varDay">
				  <xsl:value-of select="substring(COL13,7,2)"/>
			  </xsl:variable>
			  <xsl:variable name="varYear">
				  <xsl:value-of select="substring(COL13,1,4)"/>
			  </xsl:variable>

			  <xsl:variable name="varMonth">
				  <xsl:value-of select="substring(COL13,5,2)"/>
			  </xsl:variable>
			  <xsl:variable name="varCurrentDate">
				  <xsl:value-of select="concat($varMonth,'/',$varDay, '/', $varYear)"/>
			  </xsl:variable>
            <Date>
              <xsl:value-of select="$varCurrentDate"/>
            </Date>


            <xsl:variable name="Description" select="$varSubAccountDesc"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>