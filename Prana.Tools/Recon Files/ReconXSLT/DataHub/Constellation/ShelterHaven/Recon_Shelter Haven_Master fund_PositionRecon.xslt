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

  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:param name="varPutCall"/>
    <xsl:if test="$varPutCall='C'">
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
        <xsl:when test="$Month='07' ">
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
    <xsl:if test="$varPutCall='P'">
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
    <xsl:if test="contains(COL16,'Options')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="COL26"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(COL22,'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(COL22,'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(substring-after(COL22,'/'),'/'),3,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-before(normalize-space(COL15),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-before(substring-after(substring-after(normalize-space(COL15),'@'),' '),' '),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="MonthCode">
        <xsl:call-template name="MonthCodevar">
          <xsl:with-param name="Month" select="$ExpiryMonth"/>
          <xsl:with-param name="varPutCall" select="$PutORCall"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
	  <DocumentElement>
		  <xsl:for-each select ="//Comparision">

			  <xsl:variable name="Quantity">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL5"/>
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
				  <xsl:when test="number($Quantity) and not(contains(COL16,'Cash')) and not(contains(COL16,'Payables'))">
					  <PositionMaster>
						  <xsl:variable name="PB_NAME">
							  <xsl:value-of select="'MSFS'"/>
						  </xsl:variable>

						  <xsl:variable name="PB_SUFFIX_NAME">
							  <xsl:value-of select="substring-after(normalize-space(COL52),'.')"/>
						  </xsl:variable>

						  <xsl:variable name="PRANA_SUFFIX_NAME">
							  <xsl:value-of select="document('../../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@TickerSuffixCode"/>
						  </xsl:variable>

						  <xsl:variable name="PB_SYMBOL_NAME">
							  <xsl:choose>
								  <xsl:when test="contains(COL16,'Options')">
									  <xsl:value-of select="normalize-space(COL3)"/>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select="normalize-space(COL58)"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </xsl:variable>


              <xsl:variable name="Asste">
                <xsl:choose>
                  <xsl:when test="contains(COL16,'Options')">
                    <xsl:value-of select="'EquityOption'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

						  <xsl:variable name="PRANA_SYMBOL_NAME">
							  <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						  </xsl:variable>



						  <xsl:variable name="varSymbol">
							  <xsl:choose>
								  <xsl:when test="contains(COL3,'.')">
									  <xsl:value-of select="substring-before(normalize-space(COL3),'.')"/>
								  </xsl:when>
								  <xsl:when test="normalize-space(COL16)='Equities - Swap'">
									  <xsl:value-of select="substring-after(normalize-space(COL3),'T')"/>
								  </xsl:when>
								  <xsl:otherwise>
									  
									  <xsl:value-of select="normalize-space(COL3)"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </xsl:variable>
						  <Symbol>
							  <xsl:choose>
								  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
									  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								  </xsl:when>

                  <xsl:when test="$Asste='EquityOption'">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="COL26"/>
                      <xsl:with-param name="Suffix" select="''"/>
                    </xsl:call-template>
                  </xsl:when>                  
                  
								  <xsl:when test="$varSymbol!=''">
									  <xsl:choose>
										  <xsl:when test="substring(COL18,3)='SW'">
											  <xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME,'/SWAP')"/>
										  </xsl:when>
										  
										  <xsl:otherwise>
											  
											  <xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
										  </xsl:otherwise>
									  </xsl:choose>

								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select="$PB_SYMBOL_NAME"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </Symbol>

						  <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL18)"/>

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

						  <xsl:variable name="AvgPrice">
							  <xsl:call-template name="Translate">
								  <xsl:with-param name="Number" select="COL9"/>
							  </xsl:call-template>
						  </xsl:variable>

						  <MarkPrice>
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
						  </MarkPrice>

						  <xsl:variable name="AvgPriceBase">
							  <xsl:call-template name="Translate">
								  <xsl:with-param name="Number" select="COL6"/>
							  </xsl:call-template>
						  </xsl:variable>

						  <MarkPriceBase>
							  <xsl:choose>
								  <xsl:when test="$AvgPriceBase &gt; 0">
									  <xsl:value-of select="$AvgPriceBase"/>

								  </xsl:when>
								  <xsl:when test="$AvgPriceBase &lt; 0">
									  <xsl:value-of select="$AvgPriceBase * (-1)"/>
								  </xsl:when>

								  <xsl:otherwise>
									  <xsl:value-of select="0"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </MarkPriceBase>

						  <xsl:variable name="MarketValue">
							  <xsl:call-template name="Translate">
								  <xsl:with-param name="Number" select="COL10"/>
							  </xsl:call-template>
						  </xsl:variable>
						  <MarketValue>
							  <xsl:choose>
								  <xsl:when test="number($MarketValue)">
									  <xsl:value-of select="$MarketValue"/>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select="0"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </MarketValue>

						  <xsl:variable name="MarketValueBase">
							  <xsl:call-template name="Translate">
								  <xsl:with-param name="Number" select="COL7"/>
							  </xsl:call-template>
						  </xsl:variable>
						  <MarketValueBase>
							  <xsl:choose>
								  <xsl:when test="number($MarketValueBase)">
									  <xsl:value-of select="$MarketValueBase"/>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select="0"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </MarketValueBase>

						  <TradeDate>
							  <xsl:value-of select="''"/>
						  </TradeDate>

						  <SettlementDate>
							  <xsl:value-of select ="''"/>
						  </SettlementDate>

						  <NetNotionalValue>
							  <xsl:value-of select="0"/>
						  </NetNotionalValue>

						  <xsl:variable name ="Side" select="COL4"/>
						  <Side>
							  <xsl:choose>
								  <xsl:when test="$Side='L'">
									  <xsl:value-of select="'Buy'"/>
								  </xsl:when>
								  <xsl:when test="$Side='S'">
									  <xsl:value-of select="'Sell Short'"/>
								  </xsl:when>
							  </xsl:choose>
						  </Side>

						  <PBSymbol>
							  <xsl:value-of select="$PB_SYMBOL_NAME"/>
						  </PBSymbol>
						  <CompanyName>
							  <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						  </CompanyName>

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

						  <MarkPrice>
							  <xsl:value-of select="0"/>
						  </MarkPrice>
						  <NetNotionalValue>
							  <xsl:value-of select="0"/>
						  </NetNotionalValue>

						  <MarketValue>
							  <xsl:value-of select="0"/>
						  </MarketValue>

						  <TradeDate>
							  <xsl:value-of select ="''"/>
						  </TradeDate>

						  <SettlementDate>
							  <xsl:value-of select ="''"/>
						  </SettlementDate>

						  <CompanyName>
							  <xsl:value-of select="''"/>
						  </CompanyName>

					  </PositionMaster>
				  </xsl:otherwise>
			  </xsl:choose>

		  </xsl:for-each>
	  </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


