<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
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
    <xsl:if test="string-length($Symbol) &gt; 18">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after($Symbol,'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(substring-after($Symbol,' '),' '),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after($Symbol,'/'),'/'),' ')"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after($Symbol,' '),' '),' '),2),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after($Symbol,' '),' '),' '),1,1)"/>
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

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and normalize-space(COL3)!='SpotFX'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:call-template name="Option">
                <xsl:with-param name="Symbol" select="normalize-space(COL4)"/>
              </xsl:call-template>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varOptionSymbol!=''">
                  <xsl:value-of select="$varOptionSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

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

            <xsl:variable name ="varSide">
              <xsl:value-of select ="normalize-space(COL3)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varOptionSymbol != ''">
                  <xsl:choose>
                    <xsl:when test="$varSide = 'Buy'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$varSide = 'Buy'">
                        <xsl:value-of select="'1'"/>
                      </xsl:when>
                      <xsl:when test="$varSide = 'CoverShort'">
                        <xsl:value-of select="'B'"/>
                      </xsl:when>
                      <xsl:when test="$varSide = 'SellShort'">
                        <xsl:value-of select="'5'"/>
                      </xsl:when>
                      <xsl:when test="$varSide = 'Sell'">
                        <xsl:value-of select="'2'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>


            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>
                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
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

            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>

            <SecFee>
              <xsl:choose>
                <xsl:when test="$SecFee &gt; 0">
                  <xsl:value-of select="$SecFee"/>
                </xsl:when>
                <xsl:when test="$SecFee &lt; 0">
                  <xsl:value-of select="$SecFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name="varBaseFX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varLocalFX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="normalize-space(COL19)"/>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test="$varCurrency != 'USD'">
                    <xsl:choose>
                        <xsl:when test="$varCurrency = 'AUD' or $varCurrency = 'EUR' or $varCurrency = 'GBP' or $varCurrency = 'NZD'">
                            <xsl:value-of select="$varLocalFX div $varBaseFX"/>
                        </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$varBaseFX div $varLocalFX"/>
                      </xsl:otherwise>
                     </xsl:choose>                
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL15)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <PositionStartDate>
              <xsl:value-of select="COL1"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="COL2"/>
            </PositionSettlementDate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>