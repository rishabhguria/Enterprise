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
            <xsl:with-param name="Number" select="COL21"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend) ">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="'Morgan Stanley'"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="''"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
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
			
			<xsl:variable name="SEDOL" select="normalize-space(COL11)"/>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$SEDOL!=''">
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

                <xsl:when test ="$SEDOL!=''">
                  <xsl:value-of select="$SEDOL"/>
                </xsl:when>
				  
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>



            <Amount>
              <xsl:choose>
                <xsl:when test="number($varDividend)">
                  <xsl:value-of select="$varDividend"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Amount>

            <PayoutDate>
              <!--<xsl:choose>
								<xsl:when test="contains(COL41,' ')">
									<xsl:value-of select="concat(substring-before(substring-after(COL41,'/'),'/'),'/',substring-before(COL41,'/'),'/',substring-before(substring-after(substring-after(COL41,'/'),'/'),' '))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(substring-before(substring-after(COL41,'/'),'/'),'/',substring-before(COL41,'/'),'/',substring-after(substring-after(COL41,'/'),'/'))"/>
								</xsl:otherwise>
							</xsl:choose>-->
              <xsl:value-of select="COL18"/>

            </PayoutDate>

            <ExDate>
              <!--<xsl:choose>
								<xsl:when test="contains(COL29,' ')">
									<xsl:value-of select="concat(substring-before(substring-after(COL29,'/'),'/'),'/',substring-before(COL29,'/'),'/',substring-before(substring-after(substring-after(COL29,'/'),'/'),' '))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(substring-before(substring-after(COL29,'/'),'/'),'/',substring-before(COL29,'/'),'/',substring-after(substring-after(COL29,'/'),'/'))"/>
								</xsl:otherwise>
							</xsl:choose>-->
              <xsl:value-of select="COL17"/>
            </ExDate>

            <RecordDate>

              <xsl:value-of select="''"/>
            </RecordDate>


            <Currency>
              <xsl:value-of select="normalize-space(COL13)"/>
            </Currency>



            <Description>
              <xsl:choose>
                
                <xsl:when test="normalize-space(COL6) = 'DividendTax'">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'Dividend Received'"/>
                </xsl:when>
                <xsl:when test ="$varDividend &lt; 0">
                  <xsl:value-of select ="'Dividend Charged'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </Description>


            <PBSymbol>
              <xsl:value-of select="$PB_Symbol"/>
            </PBSymbol>

            <ActivityType>
              <xsl:choose>
                
                <xsl:when test="normalize-space(COL6) = 'DividendTax'">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
                
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test ="$varDividend &lt; 0">
                  <xsl:value-of select ="'DividendExpense'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </ActivityType>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>