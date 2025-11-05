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
            <xsl:with-param name="Number" select="normalize-space(COL21)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'BNY'"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>
				  <xsl:when test="$varSubAccountDesc ='EB TEMP INV FD' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				   <xsl:when test="$varSubAccountDesc ='EB TEMP INV FD' and $Cash &lt; 0 ">
					  <xsl:value-of select="'ContributedCapital-OffShore'"/>
				  </xsl:when>
                    <xsl:when test="$varSubAccountDesc ='CR INTEREST-ACCOUNT 1095958400' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				   <xsl:when test="$varSubAccountDesc ='ADR FEE' and $Cash &lt; 0 ">
					  <xsl:value-of select="'ADR Fee'"/>
				  </xsl:when>
				<xsl:when test="$varSubAccountDesc ='CSDR Cash Penalties, 000000000' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				 <xsl:when test="$varSubAccountDesc ='CSDR Cash Penalties, 000000000' and $Cash &lt; 0 ">
					  <xsl:value-of select="'OtherExpense'"/>
				  </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
				<xsl:choose>
					<xsl:when test="$varSubAccountDesc ='EB TEMP INV FD' and $Cash &gt; 0 ">
					  <xsl:value-of select="'ContributedCapital-OffShore'"/>
				  </xsl:when>
					  <xsl:when test="$varSubAccountDesc ='EB TEMP INV FD' and $Cash &lt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
					  <xsl:when test="$varSubAccountDesc ='CR INTEREST-ACCOUNT 1095958400' and $Cash &gt; 0 ">
					  <xsl:value-of select="'Interest_Income'"/>
				  </xsl:when>
                    <xsl:when test="$varSubAccountDesc ='ADR FEE' and $Cash &lt; 0 ">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
					 <xsl:when test="$varSubAccountDesc ='CSDR Cash Penalties, 000000000' and $Cash &gt; 0 ">
					  <xsl:value-of select="'MISCINC'"/>
				  </xsl:when>
					 <xsl:when test="$varSubAccountDesc ='CSDR Cash Penalties, 000000000' and $Cash &lt; 0 ">
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
              <xsl:value-of select="COL32"/>
            </Date>
			  <xsl:variable name = "PB_CURRENCY_NAME" >
				  <xsl:value-of select="COL20"/>
			  </xsl:variable>

			  <xsl:variable name="varFXRATE">
				  <xsl:choose>
					  <xsl:when test ="$PB_CURRENCY_NAME='USD'">
						  <xsl:value-of select="'1'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'0'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <FXRate>
				  <xsl:value-of select="$varFXRATE"/>
			  </FXRate>
			  
            <Description>
			   <xsl:value-of select="normalize-space(COL8)"/>
            </Description>
			  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>