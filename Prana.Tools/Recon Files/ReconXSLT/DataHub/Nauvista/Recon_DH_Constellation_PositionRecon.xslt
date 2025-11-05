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


  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:choose>
          <xsl:when test="number(COL8) and normalize-space(COL6) != 'Cash and Equivalents'">

            <PositionMaster>
              <!--   Fund -->
              <!--fundname section-->
              <xsl:variable name="varPBName">
                <xsl:value-of select="'Jefferies'"/>
              </xsl:variable>

              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="COL3"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <xsl:variable name = "PB_Symbol_NAME" >
                <xsl:value-of select="normalize-space(COL4)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_Symbol_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="varPositionStartDate">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varQuantity">
                <xsl:value-of select="COL8"/>
              </xsl:variable>

              <xsl:variable name="varOptionSymbol">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varEquitySymbol">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="varFutureSymbol">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varCUSIP">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varRIC">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varBloomberg">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varSEDOL">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varOSISymbol">
                <xsl:value-of select="COL19"/>
              </xsl:variable>

              <xsl:variable name="varOptionExpiry">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varPBSymbol">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="CompanyName">
                <xsl:value-of select="COL4"/>
              </xsl:variable>

              <xsl:variable name="varMarkPrice">
                <xsl:value-of select="COL13"/>
              </xsl:variable>
              <xsl:variable name="varNetNotionalValue">
                <xsl:value-of select="COL11"/>
              </xsl:variable>

              <xsl:variable name="varNetNotionalValueBase">
                <xsl:value-of select="COL10"/>
              </xsl:variable>


              <xsl:variable name="varCounterPartyID">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varAssetType">
                <xsl:value-of select="COL6"/>
              </xsl:variable>

              <xsl:variable name="varCommission">
                <xsl:value-of select="0"/>
              </xsl:variable>

              <xsl:variable name="varMarketValue">
                <xsl:value-of select="COL17"/>
              </xsl:variable>

              <xsl:variable name="varMarketValueBase">
                <xsl:value-of select="COL16"/>
              </xsl:variable>

              <xsl:variable name="varSMRequest">
                <xsl:value-of select="'TRUE'"/>
              </xsl:variable>

              <FundName>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME=''">
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <PositionStartDate>
                <xsl:value-of select="$varPositionStartDate"/>
              </PositionStartDate>

              <xsl:choose>
                <xsl:when test="$varAssetType='Options'">

                  <Symbol>
                    <xsl:value-of select="''"/>
                  </Symbol>

                  <IDCOOptionSymbol>
                    <xsl:value-of select="concat($varOSISymbol,'U')"/>
                  </IDCOOptionSymbol>
                </xsl:when>

                <xsl:when test="$varAssetType = 'Equity' or $varAssetType = 'Fixed Income'">
                  <xsl:variable name="varSuffix">
                    <xsl:call-template name="GetSuffix">
                      <xsl:with-param name="Suffix" select="COL6"/>
                    </xsl:call-template>
                  </xsl:variable>

                  <Symbol>
                    <xsl:choose>
                      <xsl:when test="$PRANA_Symbol_NAME != ''">
                        <xsl:value-of select="$PRANA_Symbol_NAME"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test="COL5 != 'USD'">
                            <xsl:value-of select="''"/>
                          </xsl:when>

                          <xsl:otherwise>
                            <xsl:value-of select="$varEquitySymbol"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Symbol>

                  <SEDOL>
                    <xsl:choose>
                      <xsl:when test="$PRANA_Symbol_NAME != ''">
                        <xsl:value-of select="''"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test="COL5 != 'USD'">
                            <xsl:value-of select="normalize-space(COL2)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </SEDOL>

                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>

                </xsl:when>
              </xsl:choose>


              <xsl:choose>
                <xsl:when test="$varAssetType = 'EQUITY'">
                  <xsl:variable name="varSuffix">
                    <xsl:call-template name="GetSuffix">
                      <xsl:with-param name="Suffix" select="substring-after($varEquitySymbol, '.')"/>
                    </xsl:call-template>
                  </xsl:variable>

                  <Symbol>
                    <xsl:choose>
                      <xsl:when test="contains($varEquitySymbol, '.') != false">
                        <xsl:value-of select="concat(substring-before($varEquitySymbol, '.'), $varSuffix)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$varEquitySymbol"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Symbol>

                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>

                </xsl:when>
              </xsl:choose>

              <PBSymbol>
                <xsl:value-of select="$varPBSymbol"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="$CompanyName"/>
              </CompanyName>

              <Quantity>
                <xsl:value-of select="$varQuantity"/>
              </Quantity>

              <MarkPrice>
                <xsl:choose>
                  <xsl:when test ="boolean(number($varMarkPrice))">
                    <xsl:value-of select="$varMarkPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <MarketValue>
                <xsl:choose>
                  <xsl:when test ="number($varMarketValue) ">
                    <xsl:value-of select="$varMarketValue"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test ="number($varMarketValueBase) ">
                    <xsl:value-of select="$varMarketValueBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test ="number($varNetNotionalValue) ">
                    <xsl:value-of select="$varNetNotionalValue"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test ="number($varNetNotionalValueBase) ">
                    <xsl:value-of select="$varNetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>

              <CUSIPSymbol>
                <xsl:value-of select="COL20"/>
              </CUSIPSymbol>

              <SEDOLSymbol>
                <xsl:value-of select="COL19"/>
              </SEDOLSymbol>

              <ISINSymbol>
                <xsl:value-of select="COL21"/>
              </ISINSymbol>

              <SMRequest>
                <xsl:value-of select="$varSMRequest"/>
              </SMRequest>

            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <FundName>
                    <xsl:value-of select="''"/>
              </FundName>

              <PositionStartDate>
                <xsl:value-of select="''"/>
              </PositionStartDate>

                  <Symbol>
                     <xsl:value-of select="''"/>
                  </Symbol>

                  <SEDOL>
                      <xsl:value-of select="''"/>
                  </SEDOL>

                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>
                  
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <MarkPrice>
                    <xsl:value-of select="0"/>
              </MarkPrice>

              <MarketValue>
                    <xsl:value-of select="0"/>
              </MarketValue>

              <MarketValueBase>
                    <xsl:value-of select="0"/>
              </MarketValueBase>
              
              <NetNotionalValue>
                    <xsl:value-of select="0"/>
              </NetNotionalValue>

              <NetNotionalValueBase>
                    <xsl:value-of select="0"/>
              </NetNotionalValueBase>

              <CUSIPSymbol>
                <xsl:value-of select="''"/>
              </CUSIPSymbol>

              <SEDOLSymbol>
                <xsl:value-of select="''"/>
              </SEDOLSymbol>

              <ISINSymbol>
                <xsl:value-of select="''"/>
              </ISINSymbol>

              <SMRequest>
                <xsl:value-of select="'false'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>        
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
