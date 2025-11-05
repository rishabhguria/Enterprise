<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }

  </msxsl:script>

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

  <xsl:template name="MonthCodes">
    <xsl:param name="vrMonth"/>
    <xsl:choose>
      <xsl:when test="$vrMonth='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Feb'">
        <xsl:value-of select="'2'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Mar'">
        <xsl:value-of select="'3'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Apr'">
        <xsl:value-of select="'4'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='May'">
        <xsl:value-of select="'5'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Jun'">
        <xsl:value-of select="'6'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Jul'">
        <xsl:value-of select="'7'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Aug'">
        <xsl:value-of select="'8'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL16"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSedol">
                <xsl:value-of select="substring(COL27,2)"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  
                  <xsl:when test="$varSedol!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                 
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <SEDOL>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  
                  <xsl:when test="$varSedol!=''">
                    <xsl:value-of select="$varSedol"/>
                  </xsl:when>

                  
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>

              <xsl:variable name="PB_FUND_NAME" select="''"/>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>

                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="$Quantity * -1"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <xsl:variable name="AvgPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL25"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$AvgPrice &gt; 0">
                    <xsl:value-of select="$AvgPrice"/>

                  </xsl:when>
                  <xsl:when test="$AvgPrice &lt; 0">
                    <xsl:value-of select="$AvgPrice * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </AvgPX>


              <xsl:variable name="varTardeDate">
                <xsl:value-of select="COL11"/>
              </xsl:variable>

              <TradeDate>
                <xsl:value-of select="$varTardeDate"/>
              </TradeDate>


              <xsl:variable name="varSide" select="normalize-space(COL13)"/>
              <Side>
                <xsl:choose>
                  <xsl:when test="$varSide='Purchases'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>

                  <xsl:when test="$varSide='Sales'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>


              <xsl:variable name="varTotalCommission">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL17"/>
                </xsl:call-template>
              </xsl:variable>
              <Commission>
                <xsl:choose>
                  <xsl:when test="$varTotalCommission &gt; 0">
                    <xsl:value-of select="$varTotalCommission"/>

                  </xsl:when>
                  <xsl:when test="$varTotalCommission &lt; 0">
                    <xsl:value-of select="$varTotalCommission * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </Commission>
              
              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValue &gt; 0">
                    <xsl:value-of select="$NetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValue &lt; 0">
                    <xsl:value-of select="$NetNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

              <xsl:variable name="NetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL7"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$NetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>


              <CurrencySymbol>
                <xsl:value-of select="COL9"/>
              </CurrencySymbol>
              
              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>

              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>


            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>


              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <Commission>
                <xsl:value-of select="0"/>
              </Commission>


              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>

              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>


              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <Side>
                <xsl:value-of select="''"/>
              </Side>


              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>
              
              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>


            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


