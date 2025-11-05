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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(substring(COL1,656,1),'P') or contains(substring(COL1,656,1),'C')">

      <xsl:variable name="varUnderlying">
        <xsl:value-of select="normalize-space(substring(COL1,644,6))"/>
      </xsl:variable>

      <xsl:variable name="varExYear">
        <xsl:value-of select="substring(COL1,650,2)"/>
      </xsl:variable>

      <xsl:variable name="varExDay">
        <xsl:value-of select="substring(COL1,664,2)"/>
      </xsl:variable>

      <xsl:variable name="varMonthCode">
        <xsl:value-of select="substring(COL1,652,2)"/>
      </xsl:variable>

      <xsl:variable name="varStrike">
        <xsl:value-of select="format-number(substring(COL1,97,7) div 100, '#.00')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(COL1,656,1)"/>
      </xsl:variable>
      
      <xsl:variable name="varExpiryDay">
        <xsl:choose>
          <xsl:when test="substring($varExDay,1,1)= '0'">
            <xsl:value-of select="substring($varExDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$varExDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varMonth">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="$varMonthCode"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>

      <xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varMonth,$varStrike,'D',$varExpiryDay))"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring(COL1,78,17)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="varEntryCondition">
          <xsl:choose>
            <xsl:when test="COL2='Cash and Equivalents'">
              <xsl:value-of select="0"/>
            </xsl:when>
            <xsl:when test="COL2='Currency'">
              <xsl:value-of select="0"/>
            </xsl:when>
            <xsl:when test="COL2='FX Forward'">
              <xsl:choose>
                <xsl:when test="COL10!=0">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="1"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($varQuantity) and normalize-space(substring(COL1,534,11))!='CASH' and normalize-space(substring(COL1,355,13)) !='CASH RESERVES'">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Fidelity'"/>
              </xsl:variable>


				      <xsl:variable name="PB_SYMBOL_NAME">
					       <xsl:value-of select="normalize-space(substring(COL1,335,20))"/>
			       	</xsl:variable>


              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>
              
              <xsl:variable name="Symbol" select="normalize-space(substring(COL1,50,8))"/>

          

              <xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="contains(substring(COL1,656,1),'P') or contains(substring(COL1,656,1),'C')">
                    <xsl:value-of select="'EquityOption'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varSedol">
                <xsl:value-of select="normalize-space(substring(COL1,884,13))"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>


                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="normalize-space(substring(COL1,644,6))"/>
                      <xsl:with-param name="Suffix" select="''"/>
                    </xsl:call-template>
                  </xsl:when>

                  <xsl:when test="$varSedol !=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
				
                  <xsl:when test="$Symbol!=''">					
                    <xsl:value-of select="$Symbol"/>				
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

            <xsl:when test="$Asset='EquityOption'">
              <xsl:value-of select="''"/>
            </xsl:when> 
            
            <xsl:when test="$varSedol !=''">
              <xsl:value-of select="$varSedol"/>
            </xsl:when>

            <xsl:when test="$Symbol!=''">
              <xsl:value-of select="''"/>
            </xsl:when>
           
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</SEDOL>

              <xsl:variable name="PB_FUND_NAME" select="substring(COL1,10,6)"/>
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


              <xsl:variable name="Quantity">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="$varQuantity div 10000"/>
                </xsl:call-template>
              </xsl:variable>
              <Quantity>
                <xsl:choose>
                  <xsl:when  test="contains(substring(COL1,96,1),'+')">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>
                  <xsl:when  test="contains(substring(COL1,96,1),'-')">
                    <xsl:value-of select="$Quantity * -1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,59,17) div 100000000"/>
                </xsl:call-template>
              </xsl:variable>              
              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>
                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice * (1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <MarkPriceBase>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>

                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice * (1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </MarkPriceBase>

              <xsl:variable name="MarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,569,16) div 10"/>
                </xsl:call-template>
              </xsl:variable>

              <MarketValue>

                <xsl:choose>
                  <xsl:when  test="contains(substring(COL1,586,1),'+')">
                    <xsl:value-of select="$MarketValue"/>
                  </xsl:when>
                  <xsl:when  test="contains(substring(COL1,586,1),'-')">
                    <xsl:value-of select="$MarketValue* (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <MarketValueBase>

                <xsl:choose>
                  <xsl:when  test="contains(substring(COL1,586,1),'+')">
                    <xsl:value-of select="$MarketValue"/>
                  </xsl:when>
                  <xsl:when  test="contains(substring(COL1,586,1),'-')">
                    <xsl:value-of select="$MarketValue* (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>


              <Side>
                <xsl:choose>
                  <xsl:when  test="contains(substring(COL1,96,1),'+')">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when  test="contains(substring(COL1,96,1),'-')">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>


              <CompanyName>
                <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
              </CompanyName>
				<SMRequest>
					<xsl:value-of select ="''"/>
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

              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>
              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

              <SettlementDate>
                <xsl:value-of select ="''"/>
              </SettlementDate>

              <MarkPriceBase>
                <xsl:value-of select="0"/>
              </MarkPriceBase>
              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>


              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
				<SMRequest>
					<xsl:value-of select ="'True'"/>
				</SMRequest>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


