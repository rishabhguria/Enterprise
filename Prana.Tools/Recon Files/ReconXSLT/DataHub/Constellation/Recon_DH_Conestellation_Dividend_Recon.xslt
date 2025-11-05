<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <msxsl:script language="C#" implements-prefix="my">
    public static string Now(int year, int month, int day)
    {
    DateTime prevTradeDay = new DateTime(year, month, day);
    prevTradeDay = prevTradeDay.AddDays(-1);
    while (prevTradeDay.DayOfWeek == DayOfWeek.Saturday || prevTradeDay.DayOfWeek == DayOfWeek.Sunday)
    {
    prevTradeDay = prevTradeDay.AddDays(-1);
    }
    return prevTradeDay.ToString();
    }
  </msxsl:script>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

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
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>
  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL22"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($varDividend)">
            <PositionMaster>
              <xsl:variable name="PB_Name">
                <xsl:value-of select="''"/>
              </xsl:variable>
              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="COL2"/>
              </xsl:variable>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <xsl:variable name="PB_Symbol" select="normalize-space(COL9)"/>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
              </xsl:variable>
              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>


              <xsl:variable name="varSymbol">
                <xsl:value-of select="COL11"/>
              </xsl:variable>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varSymbol!=''">
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
                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
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

              
                <xsl:variable name="varDividendLocal">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL21"/>
                  </xsl:call-template>
                </xsl:variable>
               
              <DividendLocal>
                <xsl:choose>
                  <xsl:when test="number($varDividendLocal)">
                    <xsl:value-of select="$varDividendLocal"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </DividendLocal>


              <xsl:variable name="varEXDate">
                <xsl:value-of select="normalize-space(COL17)"/>
              </xsl:variable>

              <!--<xsl:variable name="varDay">
                <xsl:value-of select="substring($varRecordDate,7,2)"/>
              </xsl:variable>

              <xsl:variable name="varMonth">
                <xsl:value-of select="substring($varRecordDate,5,2)"/>
              </xsl:variable>
              <xsl:variable name="varYear">
                <xsl:value-of select="substring($varRecordDate,1,4)"/>
              </xsl:variable>-->

              <xsl:variable name="varPDate">
                <xsl:value-of select="COL18"/>
              </xsl:variable>
              <ExpirationDate>
                <xsl:value-of select="$varEXDate"/>
              </ExpirationDate>

              <xsl:variable name="varRecordDate">
                <xsl:value-of select="normalize-space(COL17)"/>
              </xsl:variable>
              <RecordDate>
                <xsl:value-of select="''"/>
              </RecordDate>

              <xsl:variable name="varPayDate">
                <xsl:value-of select="normalize-space(COL79)"/>
              </xsl:variable>

              <xsl:variable name="varPDay">
                <xsl:value-of select="substring($varPayDate,7,2)"/>
              </xsl:variable>

              <xsl:variable name="varPMonth">
                <xsl:value-of select="substring($varPayDate,5,2)"/>
              </xsl:variable>
              <xsl:variable name="varPYear">
                <xsl:value-of select="substring($varPayDate,1,4)"/>
              </xsl:variable>

              
              <ClosingDate>
                <!--<xsl:choose>
                  <xsl:when test="contains($varPDate,' ')">
                    <xsl:value-of select="substring-before($varPDate,' ')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varPDate"/>
                  </xsl:otherwise>
                </xsl:choose>-->
                <xsl:value-of select="$varEXDate"/>
              </ClosingDate>

              <SecurityName>
                <xsl:value-of select="$PB_Symbol"/>
              </SecurityName>

              <Currency>
                <xsl:value-of select="'USD'"/>
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

              <DividendBase>
                <xsl:value-of select="''"/>
              </DividendBase>

              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>

              <RecordDate>
                <xsl:value-of select="''"/>
              </RecordDate>

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