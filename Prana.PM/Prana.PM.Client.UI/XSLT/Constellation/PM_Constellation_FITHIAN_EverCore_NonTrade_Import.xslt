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
            <xsl:with-param name="Number" select="normalize-space(COL7)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Evercore'"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>
				  <xsl:when test="$varSubAccountDesc ='Asset Income' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='Sell an Asset' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='Purchase an Asset' and $Cash &gt; 0 ">
					  <xsl:value-of select="'MoneyMarketSecurity'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='Purchase an Asset' and $Cash &lt; 0 ">
					  <xsl:value-of select="'MISC_EXP'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
				<xsl:choose>
					<xsl:when test="$varSubAccountDesc ='Asset Income' and $Cash &gt; 0 ">
						<xsl:value-of select="'DividendIncomeMM'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='Sell an Asset' and $Cash &gt; 0 ">
						<xsl:value-of select="'MoneyMarketSecurity'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='Purchase an Asset' and $Cash &gt; 0 ">
						<xsl:value-of select="'Cash'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='Purchase an Asset' and $Cash &lt; 0 ">
						<xsl:value-of select="'Cash'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>

				</xsl:choose>
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

            <xsl:variable name="Day" select="substring(COL10,7,2)"/>
            <xsl:variable name="Month" select="substring(COL10,5,2)"/>
            <xsl:variable name="Year" select="substring(COL10,1,4)"/>
            <!--<Date>
              <xsl:value-of select="concat($Month,'/', $Day, '/', $Year)"/>
            </Date>-->
            <Date>
              <xsl:value-of select="COL2"/>
            </Date>
          
            <Description>
			   <xsl:value-of select="normalize-space(COL4)"/>
            </Description>
			  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>