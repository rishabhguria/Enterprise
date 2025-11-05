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
            <xsl:with-param name="Number" select="normalize-space(COL8)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
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

            <xsl:variable name="varCurrency" select="'USD'"/>
			
            <CurrencyName>
              <xsl:value-of select ="$varCurrency"/>
            </CurrencyName>
						
            <Symbol>
              <xsl:value-of select ="normalize-space(COL5)"/>
            </Symbol>
			
			<xsl:variable name="Prana_PRE_Acronym_Name">
              <xsl:choose>
                <xsl:when test="normalize-space(COL4)= 'Bonds Interest Receivable'">
                  <xsl:value-of select="'BondInterestReceivable'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
			
			<xsl:variable name="Prana_POST_Acronym_Name">
              <xsl:choose>
                <xsl:when test="normalize-space(COL11) = 'Bonds Interest Income'">
                  <xsl:value-of select="'BondInterestIncome'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="$Cash &gt; 0">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:when test="$Cash &lt; 0">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
                  <xsl:value-of select="concat($Prana_PRE_Acronym_Name, ':' , $AbsCash , '|' , $Prana_POST_Acronym_Name, ':' , $AbsCash)"/>
            </JournalEntries>

            <Date>
                <xsl:value-of select="COL7"/>
            </Date>
         
            <xsl:variable name="Description" select="normalize-space(COL6)"/>
			
            <!-- <Description> -->
              <!-- <xsl:value-of select="''"/> -->
            <!-- </Description> -->
			  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>