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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07 ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08 ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
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
    <xsl:if test="COL8='OPTIONS'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-after(substring-after(normalize-space(COL9),'='),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL9),'-'),'-')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(COL9,'EXP '),'-')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after(COL9,'-'),'-'),'@')"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-before(substring-after(COL9,'@'),' '),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-before(COL9,' '),1,1)"/>
      </xsl:variable>

      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name ="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="concat(normalize-space(COL66),normalize-space(COL9))"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="normalize-space(COL8)='OPTIONS'">
                  <xsl:value-of select="'EqutiyOption'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL8)='FEDERAL BOND' or normalize-space(COL8)='CORPORATE DEBT' or normalize-space(COL8)='MUNICIPAL DEBT'">
                  <xsl:value-of select="'FixedIncome'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL8)='EQUITY' or normalize-space(COL8)='MUTUAL FUND'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <PBAssetType>
              <xsl:value-of select="$varAsset"/>
            </PBAssetType>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="varISIN">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL9"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="$varAsset='FixedIncome'">
                  <xsl:value-of select="$varCUSIP"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varSEDOL!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varCUSIP!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varISIN!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$varSymbol!='*'">
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
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varSEDOL!='*'">
                  <xsl:value-of select="$varSEDOL"/>
                </xsl:when>


                <xsl:when test="$varAsset='Equity' and $varCUSIP!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varISIN!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSymbol!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varCUSIP!='*'">
                  <xsl:value-of select="$varCUSIP"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varSEDOL!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varISIN!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$varSymbol!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <ISIN>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varISIN!='*'">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varSEDOL!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varCUSIP!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$varSymbol!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>

            <xsl:variable name="PB_FUND_NAME" select="translate(COL1,'*','')"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <Quantity>
              <xsl:choose>
                <xsl:when  test="number($varQuantity)">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:choose>
                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:value-of select="($varMarketValue div $CostBasis) div 100"/>
                </xsl:when>
                <xsl:when test="$varAsset='FixedIncome'">
                  <xsl:value-of select="($varMarketValue div $CostBasis) * 100"/>
                </xsl:when>
                <xsl:when test="$varAsset='Equity'">
                  <xsl:value-of select="($varMarketValue div $CostBasis)"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="$varMarketValue &gt; 0">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:when test="$varMarketValue &lt; 0">
                  <xsl:value-of select="$varMarketValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

           

           
            <AvgPX>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>

            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL29"/>
              </xsl:call-template>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>
           

            <Side>
              <xsl:choose>
                <xsl:when  test="number($varQuantity) &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="number($varQuantity) &lt; 0">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

