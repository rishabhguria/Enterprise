<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend) and (COL6='CASHDIV' or COL6='RECLAIM')">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>           

            <xsl:variable name="PB_Symbol" select="''"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="ExDate" select="COL8"/>

            <xsl:variable name="PayOutDate" select="COL10"/>

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

			  <xsl:variable name="varSymbol" select="COL1"/>
			  <xsl:variable name="varCusipSedol">
				  <xsl:choose>
					  <xsl:when test="string-length($varSymbol)=7">
						  <xsl:value-of select="'Sedol'"/>
					  </xsl:when>
					  <xsl:when test="string-length($varSymbol)&gt; 7">
						  <xsl:value-of select="'Cusip'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
            <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>
					  <xsl:when test="$varCusipSedol='Sedol'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$varCusipSedol='Cusip'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$PB_Symbol"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varCusipSedol='Cusip'">
                  <xsl:value-of select="''"/>
                </xsl:when>
				 <xsl:when test="$varCusipSedol='Sedol'">
					  <xsl:value-of select="$varSymbol"/>
				 </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varCusipSedol='Cusip'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
				 <xsl:when test="$varCusipSedol='Sedo'">
					  <xsl:value-of select="''"/>
				 </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <Amount>
              
             
				<xsl:value-of select="$varDividend"/>
            </Amount>

            <ActivityType>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </ActivityType>

			  <Description>
				  <xsl:choose>
					  <xsl:when test="$varDividend &lt; 0">
						  <xsl:value-of select="'WithholdingTax'"/>
					  </xsl:when>
					  <xsl:when test="$varDividend &gt; 0">
						  <xsl:value-of select="'DividendIncome'"/>
					  </xsl:when>
				  </xsl:choose>
			  </Description>

            <PayoutDate>
              <xsl:value-of select="$PayOutDate"/>
            </PayoutDate>

            <ExDate>
              <xsl:value-of select="$ExDate"/>
            </ExDate>

            <xsl:variable name="varCurrencyName" select="'USD'"/>
            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>