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
		<xsl:param name="Month" />
		<xsl:param name="PutOrCall" />
		<xsl:if test="$PutOrCall='CALL'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='PUT'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
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
			<xsl:value-of select="substring-before($var,' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(substring-after($var,''),'/'),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="normalize-space(substring-after(substring-before($var,'/'),' '))"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after($var,''),'/'),'/'),1,2)"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="normalize-space(substring-before($varSymbol,' '))"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			
			<xsl:value-of select="format-number(substring-after(substring-after($var,' '),' '),'##.00') " />
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
		  

        <xsl:if test="number($Quantity) and COL3 !='Cash' ">

          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select = "COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="varSymbols">
				  <xsl:value-of select="normalize-space(COL5)" />
			  </xsl:variable>
          
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
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

            <xsl:variable name ="Side" select="''"/>
            <Side>
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
            </Side>

            <!--QUANTITY-->

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

			<xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
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

			<xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>

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
			  
			<xsl:variable name="AvgPrice">
				<xsl:call-template name="Translate">
					<xsl:with-param name="Number" select="COL22"/>
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
		

            <CurrencySymbol>
              <xsl:value-of select="COL8"/>
            </CurrencySymbol>


			  <TradeDate>
				  <xsl:value-of select ="normalize-space(substring-before(COL15,' '))"/>
			  </TradeDate>
			  
			  <PBAssetName>
				  <xsl:value-of select="COL1"/>
			  </PBAssetName>

		  </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration from lower to upper case conversion-->

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
