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
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($Symbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="translate($var, '0123456789.', '')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(translate($var,translate($var, '0123456789.', ''), ''),7,8) div 1000,'##.00')"/>
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
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="varDay">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'/'),'/'))=1">
              <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'/'),'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before(substring-after($DateTime,'/'),'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'-'),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'-'),'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after($DateTime,'-'),'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(normalize-space($DateTime),'/'))=1">
              <xsl:value-of select="concat(0,substring-before($DateTime,'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before($DateTime,'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(normalize-space($DateTime),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before($DateTime,'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before($DateTime,'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:value-of select="substring-after(substring-after($DateTime,'/'),'/')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:value-of select="substring-after(substring-after($DateTime,'-'),'-')"/>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonth"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varDay"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varYear"/>

  </xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="varQty">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL7)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQty)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="normalize-space(COL30)"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL29)"/>
            </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL32!='' and COL32!='*'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			
			<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

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

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL32)"/>
                  </xsl:call-template>
                </xsl:when>


                <xsl:when test="$varSEDOL!='' and $varSEDOL!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                  <xsl:value-of select="$varCUSIP"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$varSEDOL!='' and $varSEDOL!='*'">
                  <xsl:value-of select="$varSEDOL"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <Quantity>
              <xsl:choose>
                <xsl:when test="$varQty &gt; 0">
                  <xsl:value-of select="$varQty"/>
                </xsl:when>

                <xsl:when test="$varQty &lt; 0">
                  <xsl:value-of select="$varQty * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL9)"/>
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

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$varSide='BUY'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>

                    <xsl:when test="$varSide='SELL'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varSide='BUY'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>

                    <xsl:when test="$varSide='SELL'">
                      <xsl:value-of select="'Sell'"/>
                    </xsl:when>

                    <xsl:when test="$varSide='SHORT SELL'">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>

                    <xsl:when test="$varSide='COVER BUY'">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <xsl:variable name="varCOL11">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL11)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCOL12">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCOL13">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL13)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCOL14">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL14)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCOL15">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL15)"/>
              </xsl:call-template>
            </xsl:variable>
            
            <xsl:variable name="varTotalCommissionandFees">
              <xsl:value-of select="$varCOL11 + $varCOL12 + $varCOL13 + $varCOL14 + $varCOL15"/>
            </xsl:variable>
            <TotalCommissionandFees>
              <xsl:choose>
                <xsl:when test="$varTotalCommissionandFees &gt; 0">
                  <xsl:value-of select="$varTotalCommissionandFees"/>
                </xsl:when>
                <xsl:when test="$varTotalCommissionandFees &lt; 0">
                  <xsl:value-of select="$varTotalCommissionandFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TotalCommissionandFees>


            <xsl:variable name="var1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL16)"/>
              </xsl:call-template>
            </xsl:variable>
            
            <xsl:variable name="var2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL17)"/>
              </xsl:call-template>
            </xsl:variable>
            
            <xsl:variable name="varNotional">
              <xsl:choose>
                <xsl:when test="COL17!=''">
                  <xsl:value-of select="$var1 + $var2"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$var1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNotional &gt; 0">
                  <xsl:value-of select="$varNotional"/>

                </xsl:when>
                <xsl:when test="$varNotional &lt; 0">
                  <xsl:value-of select="$varNotional * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="varTradeDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="$varTradeDate"/>
            </TradeDate>

            <xsl:variable name="varSettlementDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL5"/>
              </xsl:call-template>
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
            </SettlementDate>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>