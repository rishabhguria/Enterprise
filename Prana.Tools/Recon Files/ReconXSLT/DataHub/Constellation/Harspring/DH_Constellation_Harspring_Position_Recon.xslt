<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name ="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($varQuantity) and (COL3='LONG' or COL3='SHORT')">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Data Hub'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL7"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
                <xsl:value-of select="normalize-space(COL6)"/>
              </xsl:variable>

              <xsl:variable name="varSedol">
                <xsl:value-of select="normalize-space(COL25)"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$varSedol!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varSedol ='' or $varSedol ='*'">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>

                </xsl:choose>
              </Symbol>

              <SEDOL>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$varSedol!=''">
                    <xsl:value-of select="$varSedol"/>
                  </xsl:when>

                  <xsl:when test="$varSedol ='' or $varSedol ='*'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SEDOL>


              <xsl:variable name="PB_FUND_NAME" select="COL1"/>
              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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




              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>





              <CounterParty>
                <xsl:value-of select="'BTIG'"/>
              </CounterParty>



              <xsl:variable name="Quantity">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL8"/>
                </xsl:call-template>
              </xsl:variable>
              <Quantity>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="$Quantity"/>

                  </xsl:when>
                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="$Quantity * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Quantity>





              <xsl:variable name="AvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$AvgPX &gt; 0">
                    <xsl:value-of select="$AvgPX"/>

                  </xsl:when>
                  <xsl:when test="$AvgPX &lt; 0">
                    <xsl:value-of select="$AvgPX * (1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </AvgPX>


              <xsl:variable name="varSide">
                <xsl:value-of select="COL3"/>
              </xsl:variable>
              <Side>
                <xsl:choose>
                  <xsl:when test="$varSide ='LONG'">
                    <xsl:value-of select="'Buy'"/>

                  </xsl:when>
                  <xsl:when test="$varSide ='SHORT'">
                    <xsl:value-of select="'Sell short'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <OriginalPurchaseDate>
                <xsl:value-of select="''"/>
              </OriginalPurchaseDate>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <CompanyName>
                <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
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



              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>




              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>




              <Quantity>
                <xsl:value-of select="0"/>

              </Quantity>




              <Side>
                <xsl:value-of select ="''"/>
              </Side>

              <OriginalPurchaseDate>
                <xsl:value-of select ="''"/>
              </OriginalPurchaseDate>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

              <CompanyName>
                <xsl:value-of select ="''"/>
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

</xsl:stylesheet>

