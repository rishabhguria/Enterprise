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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol">
              <xsl:value-of select="COL2"/>
            </xsl:variable>


            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <xsl:variable name="PB_FUND_NAME" select="COL1"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

            <NetPosition>
              <xsl:choose>


                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>

                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide" select="COL37"/>

            <SideTagValue>

              <xsl:choose>
                <xsl:when test="$varSide ='Long'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$varSide ='Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>

            </SideTagValue>





            <xsl:variable name="Costbasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL31"/>
              </xsl:call-template>
            </xsl:variable>


            <CostBasis>
              <xsl:choose>

                <xsl:when test="$Costbasis &gt; 0">
                  <xsl:value-of select="$Costbasis"/>
                </xsl:when>

                <xsl:when test="$Costbasis &lt; 0">
                  <xsl:value-of select="$Costbasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>


            </CostBasis>


            <xsl:variable name="varDay">
              <xsl:value-of select ="substring(COL20,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select ="substring(COL20,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select ="substring-before(COL39,';')"/>
            </xsl:variable>

            <xsl:variable name="varTradeDate">
              <xsl:value-of select ="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="$varYear"/>
            </PositionStartDate>

            <xsl:variable name="varOriginalPurchaseDate">
              <xsl:value-of select ="substring-before(COL39,';')"/>
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="$varOriginalPurchaseDate"/>
            </PositionSettlementDate>

            <xsl:variable name="varCurrencySymbol">
              <xsl:value-of select ="COL11"/>
            </xsl:variable>
            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="PRANA_CURRENCY_ID">
              <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>

            <CurrencyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_CURRENCY_ID)">
                  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>

            <SettlCurrencyName>
              <xsl:value-of select="$varCurrencySymbol"/>
            </SettlCurrencyName>
            <xsl:variable name ="FXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test ="$FXRate ">
                  <xsl:value-of select ="$FXRate"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>