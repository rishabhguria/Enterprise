<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>
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

 

 

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Position) and contains(COL6,'Cash &amp; Equivalents')!='true'">

            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="Symbol" select="COL10"/>

              <xsl:variable name="AssetType" select="normalize-space(COL7)"/>

              <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

              <xsl:variable name="PB_BROKER_NAME">
                <xsl:value-of select="normalize-space(COL24)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_BROKER_ID">
                <xsl:value-of select="document('../../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_BROKER_NAME]/@PranaBroker"/>
              </xsl:variable>

              <CounterParty>
                <xsl:value-of select="'BTIG'"/>
              </CounterParty>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL7)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <Symbol>

                <xsl:choose>

                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>


                  <xsl:when test="$Symbol !=''">
                    <xsl:value-of select="$Symbol"/>
                  </xsl:when>

                

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>

                </xsl:choose>

              </Symbol>

             

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($Position)">
                    <xsl:value-of select="$Position"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
               
              </Quantity>

              <xsl:variable name="Side" select="normalize-space(COL6)"/>

              <Side>
                <xsl:choose>

                  <xsl:when test="$Side='Long Positions'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>

                  <xsl:when test="$Side='Short Positions'">
                    <xsl:value-of select="'Sell short'"/>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </Side>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <xsl:variable name="varUnitCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL19"/>
                </xsl:call-template>
              </xsl:variable>

              <UnitCost>
                <xsl:choose>

                  <xsl:when test="$varUnitCost &gt; 0">
                    <xsl:value-of select="$varUnitCost"/>
                  </xsl:when>

                  <xsl:when test="$varUnitCost &lt; 0">
                    <xsl:value-of select="$varUnitCost*-1"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </UnitCost>

           
              <xsl:variable name="varTotalCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL22"/>
                </xsl:call-template>
              </xsl:variable>

              <TotalCost>
                <xsl:choose>
                  <xsl:when test="number($varTotalCost)">
                    <xsl:value-of select="$varTotalCost"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCost>

            

              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>

              <xsl:variable name="CompanyName" select="''"/>

              <CompanyName>
                <xsl:value-of select="$CompanyName"/>
              </CompanyName>


              
            </PositionMaster>

          </xsl:when>
          <xsl:otherwise>

            <PositionMaster>

              <FundName>
                <xsl:value-of select ="''"/>
              </FundName>

              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>

             

              <Quantity>
                <xsl:value-of select="''"/>
              </Quantity>

              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <TradeDate>
                <xsl:value-of select="' '"/>
              </TradeDate>

              <UnitCost>
                <xsl:value-of select="0"/>
              </UnitCost>

              <TotalCost>
                <xsl:value-of select="0"/>
              </TotalCost>

             
             
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

           

            </PositionMaster>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>
</xsl:stylesheet>
