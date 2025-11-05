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
  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>

    <xsl:variable name="varUSymbol">
      <xsl:choose>
        <xsl:when test="substring($varBBCode,2,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,2,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,3,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,3,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,4,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,4,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,5,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,5,1) = '2'">
          <xsl:value-of select="substring-before(v,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,6,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,6,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varUnderlyingLength">
      <xsl:value-of select="string-length($varUSymbol)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike1">
      <xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +8)), '#.00')"/>
    </xsl:variable>
    
    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(($varStrike1 div 1000), '#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +5),2)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength + 3),2)"/>
    </xsl:variable>

    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength + 7),1)"/>
    </xsl:variable>
    
    <xsl:variable name="MonthCodeVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="number($varMonthCode)"/>
        <xsl:with-param name="PutOrCall" select="$PutORCall"/>
      </xsl:call-template>
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
    <xsl:value-of select="normalize-space(concat('O:', $varUSymbol, ' ', $varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
  </xsl:template>
  
  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL14"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and contains(COL45,'Cash')!='true'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name ="PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL3)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="string-length(COL6) &gt; 17">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <xsl:variable name="Symbol" select="normalize-space(COL6)"/>

            <xsl:variable name="varOption">
              <xsl:call-template name="ConvertBBCodetoTicker">
                <xsl:with-param name="varBBCode" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
				        <xsl:when test="$Asset='Option'">
					        <xsl:value-of select="$varOption"/>
				        </xsl:when>
                <xsl:when test="$Asset='Equity'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

			  <xsl:variable name = "PB_FUND_NAME">
				  <xsl:value-of select="concat(COL8,COL26)"/>
			  </xsl:variable>
			  

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
            </Quantity>

            <xsl:variable name="Side" select="normalize-space(COL10)"/>


            <Side>
              <xsl:choose>

				  <xsl:when test="contains(COL41,'CALL') or contains(COL41,'PUT')">

					  <xsl:choose>

						  <xsl:when test="contains($Side,'purchase')">
							  <xsl:value-of select="'Buy to Open'"/>
						  </xsl:when>

						  <xsl:when test="contains($Side,'sale')">
							  <xsl:value-of select="'Sell to Close'"/>
						  </xsl:when>


						  <xsl:when test="contains($Side,'buy to cover')">
							  <xsl:value-of select="'Buy to Close'"/>
						  </xsl:when>

						  <xsl:when test="contains($Side,'sell short')">
							  <xsl:value-of select="'Sell to Open'"/>
						  </xsl:when>

						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>

					  </xsl:choose>
				  </xsl:when>
				  
				  <xsl:otherwise>
					  <xsl:choose>

						  <xsl:when test="contains($Side,'sale')">
							  <xsl:value-of select="'Sell'"/>
						  </xsl:when>

						  <xsl:when test="contains($Side,'purchase')">
							  <xsl:value-of select="'Buy'"/>
						  </xsl:when>

						  <xsl:when test="contains($Side,'buy to cover')">
							  <xsl:value-of select="'Buy to Close'"/>
						  </xsl:when>

						  <xsl:when test="contains($Side,'sell short')">
							  <xsl:value-of select="'Sell short'"/>
						  </xsl:when>

						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>

					  </xsl:choose>
				  </xsl:otherwise>
              </xsl:choose>
            </Side>

            <xsl:variable name="TradePrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <AvgPX>
              <xsl:choose>

                <xsl:when test="$TradePrice &gt; 0">
                  <xsl:value-of select="$TradePrice"/>
                </xsl:when>

                <xsl:when test="$TradePrice &lt; 0">
                  <xsl:value-of select="$TradePrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AvgPX>


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="Date" select="COL11"/>

            <TradeDate>
              <xsl:value-of select="$Date"/>

            </TradeDate>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>

              <xsl:choose>

                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>

                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Commission>

            <xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL34"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValueBase>

              <xsl:choose>

                <xsl:when test="$NetNotionalValueBase&gt; 0">
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

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
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

			  <SMRequest>
				  <xsl:value-of select="'true'"/>
			  </SMRequest>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>