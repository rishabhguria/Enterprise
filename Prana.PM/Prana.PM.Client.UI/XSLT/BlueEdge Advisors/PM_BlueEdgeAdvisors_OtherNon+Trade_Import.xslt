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
            <xsl:with-param name="Number" select="normalize-space(COL16)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash) and (COL2 ='AJC' or COL2 = 'WTC' or COL2 ='FWO' or COL2 = 'SSR' or COL2 = 'FWI')">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>
				  <xsl:when test="$varSubAccountDesc ='AJC' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='AJC' and $Cash &lt; 0 ">
					  <xsl:value-of select="'MISC_EXP'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='WTC' and $Cash &lt; 0 ">
					  <xsl:value-of select="'CASH_WDL'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='FWO' and $Cash &lt; 0 ">
					  <xsl:value-of select="'CASH_WDL'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='SSR' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  <xsl:when test="$varSubAccountDesc ='FWI' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
				<xsl:choose>
					<xsl:when test="$varSubAccountDesc ='AJC' and $Cash &gt; 0 ">
						<xsl:value-of select="'MISC_INC'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='AJC' and $Cash &lt; 0 ">
						<xsl:value-of select="'Cash'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='WTC' and $Cash &lt; 0 ">
						<xsl:value-of select="'Cash'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='FWO' and $Cash &lt; 0 ">
						<xsl:value-of select="'Cash'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='SSR' and $Cash &gt; 0 ">
						<xsl:value-of select="'Cash'"/>
					</xsl:when>
					<xsl:when test="$varSubAccountDesc ='FWI' and $Cash &gt; 0 ">
						<xsl:value-of select="'CASH_DEP'"/>
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

  <xsl:variable name="CurrentDate">
              <xsl:value-of select="COL4"/>
          </xsl:variable>
         <xsl:variable name="varDay">
            <xsl:value-of select="substring-after(substring-after($CurrentDate,'-'),'-')"/>
          </xsl:variable>
          <xsl:variable name="varYear">
            <xsl:value-of select="substring-before($CurrentDate,'-')"/>
          </xsl:variable>

          <xsl:variable name="varMonth">
            <xsl:value-of select="substring-before(substring-after($CurrentDate,'-'),'-')"/>
          </xsl:variable>
          <xsl:variable name="varCurrentDate">
            <xsl:value-of select="concat($varMonth,'/',$varDay, '/', $varYear)"/>
          </xsl:variable>
            <Date>
              <xsl:value-of select="$varCurrentDate"/>
            </Date>
			
			  
            <Description>
				<xsl:value-of select="COL2"/>
            </Description>
			  
			<CurrencyName>
				<xsl:value-of select="COL18"/>	
			 </CurrencyName>  
		  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>