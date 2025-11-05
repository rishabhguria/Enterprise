<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>

  
  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//Comparision">
        
        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL24"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($varDividend)">
            <PositionMaster>
              <xsl:variable name="PB_Name">
                <xsl:value-of select="''"/>
              </xsl:variable>
              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="''"/>
              </xsl:variable>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              
              <xsl:variable name="PB_Symbol" select="normalize-space(COL7)"/>
              
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

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

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

              <xsl:variable name="varGrossAmount">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL24"/>
                </xsl:call-template>
              </xsl:variable>
              
               <xsl:variable name="varNetAmount">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL26"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varTotalCost">
                <xsl:value-of select="$varGrossAmount - $varNetAmount"/>
              </xsl:variable>
              <TotalCostLocal>
                <xsl:choose>
                  <xsl:when test="number($varTotalCost)">
                    <xsl:value-of select="$varTotalCost"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCostLocal>

              <TotalCostBase>
                <xsl:choose>
                  <xsl:when test="number($varTotalCost)">
                    <xsl:value-of select="$varTotalCost"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCostBase>

              <xsl:variable name="varExDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL20"/>
                </xsl:call-template>
              </xsl:variable>
              <ExpirationDate>
                <xsl:value-of select="$varExDate"/>
              </ExpirationDate>

              <xsl:variable name="varRDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL21"/>
                </xsl:call-template>
              </xsl:variable>
              <RecordDate>
                <xsl:value-of select="$varRDate"/>
              </RecordDate>
              
              <xsl:variable name="varPDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL22"/>
                </xsl:call-template>
              </xsl:variable>
              <PayoutDate>
                <xsl:value-of select="$varPDate"/>
              </PayoutDate>

              <ClosingDate>
                <xsl:value-of select="$varExDate"/>
              </ClosingDate>

              <SecurityName>
                <xsl:value-of select="$PB_Symbol"/>
              </SecurityName>

              <Currency>
                <xsl:value-of select="COL2"/>
              </Currency>

            </PositionMaster>
          </xsl:when>
          
          <xsl:otherwise>
            <PositionMaster>
              <PortfolioAccount>
                    <xsl:value-of select ="''"/>
              </PortfolioAccount>

              <Symbol>
                    <xsl:value-of select="''"/>
              </Symbol>

              <DividendBase>
                    <xsl:value-of select="0"/>
              </DividendBase>

              <DividendLocal>
                    <xsl:value-of select="0"/>
              </DividendLocal>

              <TotalCostLocal>
                    <xsl:value-of select="0"/>
              </TotalCostLocal>

              <TotalCostBase>
                    <xsl:value-of select="0"/>
              </TotalCostBase>

              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>

              <RecordDate>
                <xsl:value-of select="''"/>
              </RecordDate>

              <PayoutDate>
                <xsl:value-of select="''"/>
              </PayoutDate>

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