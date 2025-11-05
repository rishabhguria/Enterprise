<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>
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
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
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
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
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
    <xsl:param name="Suffix"/>
    <xsl:if test="COL9=OPT">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),3,2)"/>

      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL5),' '),8,8) div 1000,'#.00')"/>
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

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL16"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'IB'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="COL9 ='OPT'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="UnderlyingSymbol">
              <xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
            </xsl:variable>
            <xsl:variable name="ExpiryDay">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),5,2)"/>
            </xsl:variable>
            <xsl:variable name="Month">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),3,2)"/>

            </xsl:variable>
            <xsl:variable name="ExpiryYear">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),1,2)"/>
            </xsl:variable>

            <xsl:variable name="PutORCall">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),7,1)"/>
            </xsl:variable>
            <xsl:variable name="StrikePrice">
              <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL5),' '),8,8) div 1000,'#.00')"/>
            </xsl:variable>

            <xsl:variable name="MonthCode">
              <xsl:choose>
                <xsl:when test="$PutORCall='C'">
                  <xsl:choose>
                    <xsl:when test="$Month=1 ">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$Month=2 ">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$Month=3 ">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                    <xsl:when test="$Month=4 ">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when test="$Month=5 ">
                      <xsl:value-of select="'E'"/>
                    </xsl:when>
                    <xsl:when test="$Month=6 ">
                      <xsl:value-of select="'F'"/>
                    </xsl:when>
                    <xsl:when test="$Month=7  ">
                      <xsl:value-of select="'G'"/>
                    </xsl:when>
                    <xsl:when test="$Month=8  ">
                      <xsl:value-of select="'H'"/>
                    </xsl:when>
                    <xsl:when test="$Month=9 ">
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
                </xsl:when>
                <xsl:when test="$PutORCall='P'">
                  <xsl:choose>
                    <xsl:when test="$Month=1 ">
                      <xsl:value-of select="'M'"/>
                    </xsl:when>
                    <xsl:when test="$Month=2 ">
                      <xsl:value-of select="'N'"/>
                    </xsl:when>
                    <xsl:when test="$Month=3 ">
                      <xsl:value-of select="'O'"/>
                    </xsl:when>
                    <xsl:when test="$Month=4 ">
                      <xsl:value-of select="'P'"/>
                    </xsl:when>
                    <xsl:when test="$Month=5 ">
                      <xsl:value-of select="'Q'"/>
                    </xsl:when>
                    <xsl:when test="$Month=6 ">
                      <xsl:value-of select="'R'"/>
                    </xsl:when>
                    <xsl:when test="$Month=7  ">
                      <xsl:value-of select="'S'"/>
                    </xsl:when>
                    <xsl:when test="$Month=8  ">
                      <xsl:value-of select="'T'"/>
                    </xsl:when>
                    <xsl:when test="$Month=9 ">
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
                </xsl:when>
              </xsl:choose>
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

            <xsl:variable name="varoptSymbol">
              <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>

            </xsl:variable>

            <Symbol>
             <xsl:choose>
               <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="COL9 ='OPT'">
                  <xsl:value-of select="$varoptSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>

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


            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="COL17">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="CostBasis" select="COL17"/>


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

            <xsl:variable name ="Side" select="COL15"/>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Asset ='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$Side='BUY'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$Side='COVER'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$Side='SELL'">
                      <xsl:value-of select ="'D'"/>
                    </xsl:when>
                    <xsl:when test="$Side='SHORT'">
                      <xsl:value-of select ="'C'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Side='BUY'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$Side='COVER'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$Side='SELL'">
                      <xsl:value-of select ="'2'"/>
                    </xsl:when>
                    <xsl:when test="$Side='SHORT'">
                      <xsl:value-of select ="'5'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>

            </SideTagValue>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name ="Date" select="COL6"/>


            <xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month1" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day1" select="substring-before($Date,'/')"/>



            <PositionStartDate>
              <xsl:value-of select="COL6"/>
            </PositionStartDate>

            <CUSIP>
              <xsl:value-of select="COL12"/>
            </CUSIP>

            <ISIN>
              <xsl:value-of select="COL13"/>
            </ISIN>

            <xsl:variable name="COL54">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL54 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL55">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL55 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL56">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL56 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL57">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL57 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL58">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL58 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL59">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL59 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="Commission" select="COL20"/>

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

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>