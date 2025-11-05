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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'A'" />
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'B'" />
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'C'" />
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'D'" />
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'E'" />
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'F'" />
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'G'" />
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'H'" />
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'I'" />
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'J'" />
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'K'" />
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'L'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'M'" />
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'N'" />
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'O'" />
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'P'" />
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'Q'" />
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'R'" />
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'S'" />
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'T'" />
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'U'" />
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'V'" />
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'W'" />
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'X'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

 <xsl:template name="Option">
    <xsl:param name="varSymbol"/>
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($varSymbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($varSymbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),1,2)"/>
		<!--<xsl:value-of select="substring(substring-after(COL6,' '),1,2)"/>-->
    </xsl:variable>
     <xsl:variable name="ExpiryMonth">
      <!--<xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),4,2)"/>-->
		 <xsl:value-of select="substring(substring-after(COL6,' '),3,3)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <!--<xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)" />-->
		<xsl:value-of select="substring(substring-after(COL6,' '),6,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
     <!--<xsl:value-of select="substring-after(translate($var, '0123456789.', ''),'   ')"/>-->
		<xsl:value-of select="substring(COL6,string-length(COL6),1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
       <xsl:value-of select="format-number(substring(translate($var,translate($var, '0123456789.', ''), ''),5,7),'##.00')" />
     </xsl:variable>
    <xsl:variable name="MonthCodVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$ExpiryMonth" />
        <xsl:with-param name="PutOrCall" select="$PutORCall" />
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
	 <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)" />
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)">

          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select = "COL6"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            
            <xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>
			
			<xsl:variable name="varSymbols">
              <xsl:value-of select="normalize-space(COL6)" />
            </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="string-length(normalize-space($varSymbol)) &gt; 8">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>        

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test ="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                     <xsl:with-param name="varSymbol" select="normalize-space(COL6)" />
                  </xsl:call-template>
                </xsl:when>
                <xsl:when test="$varSymbols!='*' or $varSymbols!=' '">
                  <xsl:value-of select="$varSymbols" />
                </xsl:when>             
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>            

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

            <xsl:variable name ="Side" select="COL7"/>
			  
            <Side>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$Quantity &gt; 0">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>
                    <xsl:when test="$Quantity &lt; 0 ">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Quantity &gt; 0">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="$Quantity &lt; 0">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Side>        

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($Quantity)">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="number($varMarkPrice) ">
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
                <xsl:when test ="number($varMarkPrice) ">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>
			
			<xsl:variable name="varNetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL10)"/>
              </xsl:call-template>
            </xsl:variable>
			  
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValue &gt; 0">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValue &lt; 0">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <CurrencySymbol>
              <xsl:value-of select="'USD'"/>
            </CurrencySymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration from lower to upper case conversion-->

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
