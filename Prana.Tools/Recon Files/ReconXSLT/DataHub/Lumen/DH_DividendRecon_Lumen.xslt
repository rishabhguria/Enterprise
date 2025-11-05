<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <msxsl:script language="C#" implements-prefix="my"> public static string Now(int year, int month, int day) { DateTime prevTradeDay = new DateTime(year, month, day); prevTradeDay = prevTradeDay.AddDays(-1); while (prevTradeDay.DayOfWeek == DayOfWeek.Saturday || prevTradeDay.DayOfWeek == DayOfWeek.Sunday) { prevTradeDay = prevTradeDay.AddDays(-1); } return prevTradeDay.ToString(); } </msxsl:script>
  
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
      <xsl:for-each select="//Comparision">
        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($varDividend)">
            <PositionMaster>
              <xsl:variable name="PB_Name">
                <xsl:value-of select="'Scotia'"/>
              </xsl:variable>
              <xsl:variable name="PB_FUND_NAME">
                <xsl:value-of select="normalize-space(COL4)"/>
              </xsl:variable>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <xsl:variable name="PB_Symbol" select="normalize-space(COL6)"/>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
              </xsl:variable>
              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>
			  
              <xsl:variable name="varSymbol">
                <xsl:value-of select="COL9"/>
              </xsl:variable>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
				  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>

                  <xsl:when test="COL6!=''">
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
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="COL6!=''">
                    <xsl:value-of select="normalize-space(COL6)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>
			  <DividendBase>
                <xsl:choose>
                  <xsl:when test="number($varDividend)">
                    <xsl:value-of select="$varDividend"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </DividendBase>
              
              <DividendLocal>
                <xsl:choose>
                  <xsl:when test="number($varDividend)">
                    <xsl:value-of select="$varDividend"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </DividendLocal>
			  
			  
              
              <xsl:variable name="varEXDate">
                <xsl:value-of select="COL10"/>
              </xsl:variable>
              <xsl:variable name="varPDate">
                <xsl:value-of select="COL74"/>
              </xsl:variable>
              <ExpirationDate>
                <xsl:value-of select="COL12"/>
              </ExpirationDate>
			  <ClosingDate>
                <xsl:value-of select="$varEXDate"/>
              </ClosingDate>
              <SecurityName>
                <xsl:value-of select="$PB_Symbol"/>
              </SecurityName>
              <Currency>
                <xsl:value-of select="COL16"/>
              </Currency>
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <PortfolioAccount>
                <xsl:value-of select="''"/>
              </PortfolioAccount>
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>
              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>
              <DividendLocal>
                <xsl:value-of select="''"/>
              </DividendLocal>
			  <DividendBase>
                <xsl:value-of select="''"/>
              </DividendBase>
              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>
			  <ClosingDate>
                <xsl:value-of select="''"/>
              </ClosingDate>
			  
              <SecurityName>
                <xsl:value-of select="''"/>
              </SecurityName>
              <Currency>
                <xsl:value-of select="''"/>
              </Currency>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>