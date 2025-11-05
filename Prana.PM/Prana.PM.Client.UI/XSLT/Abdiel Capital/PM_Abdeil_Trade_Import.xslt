<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="contains($PutOrCall,'CALL')">
      <xsl:choose>
        <xsl:when test="$Month='A' ">
          <xsl:value-of select="01"/>
        </xsl:when>
        <xsl:when test="$Month='B' ">
          <xsl:value-of select="02"/>
        </xsl:when>
        <xsl:when test="$Month='C' ">
          <xsl:value-of select="03"/>
        </xsl:when>
        <xsl:when test="$Month='D' ">
          <xsl:value-of select="04"/>
        </xsl:when>
        <xsl:when test="$Month='E' ">
          <xsl:value-of select="05"/>
        </xsl:when>
        <xsl:when test="$Month='F' ">
          <xsl:value-of select="06"/>
        </xsl:when>
        <xsl:when test="$Month='G'  ">
          <xsl:value-of select="07"/>
        </xsl:when>
        <xsl:when test="$Month='H'  ">
          <xsl:value-of select="08"/>
        </xsl:when>
        <xsl:when test="$Month='I' ">
          <xsl:value-of select="09"/>
        </xsl:when>
        <xsl:when test="$Month='J' ">
          <xsl:value-of select="10"/>
        </xsl:when>
        <xsl:when test="$Month='K' ">
          <xsl:value-of select="11"/>
        </xsl:when>
        <xsl:when test="$Month='L' ">
          <xsl:value-of select="12"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="contains($PutOrCall,'PUT')">
      <xsl:choose>
        <xsl:when test="$Month='M' ">
          <xsl:value-of select="01"/>
        </xsl:when>
        <xsl:when test="$Month='N' ">
          <xsl:value-of select="02"/>
        </xsl:when>
        <xsl:when test="$Month='O' ">
          <xsl:value-of select="03"/>
        </xsl:when>
        <xsl:when test="$Month='P' ">
          <xsl:value-of select="04"/>
        </xsl:when>
        <xsl:when test="$Month='Q' ">
          <xsl:value-of select="05"/>
        </xsl:when>
        <xsl:when test="$Month='R' ">
          <xsl:value-of select="06"/>
        </xsl:when>
        <xsl:when test="$Month='S'  ">
          <xsl:value-of select="07"/>
        </xsl:when>
        <xsl:when test="$Month='T'  ">
          <xsl:value-of select="08"/>
        </xsl:when>
        <xsl:when test="$Month='U' ">
          <xsl:value-of select="09"/>
        </xsl:when>
        <xsl:when test="$Month='V' ">
          <xsl:value-of select="10"/>
        </xsl:when>
        <xsl:when test="$Month='W' ">
          <xsl:value-of select="11"/>
        </xsl:when>
        <xsl:when test="$Month='X' ">
          <xsl:value-of select="12"/>
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
    <xsl:if test="substring-before(COL21,' ')='CALL' or substring-before(COL21,' ')='PUT'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,' '),2,2)"/>
      </xsl:variable>
      <!--<xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,' '),3,2)"/>
      </xsl:variable>-->
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,' '),4,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,' '),6),'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:value-of select="substring(substring-after($Symbol,' '),1,1)"/>
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
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>


  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition)">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'State Street'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSingleQuote">'</xsl:variable>

            <xsl:variable name="Symbol">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <xsl:variable name="varCusip">
              <xsl:choose>
                <xsl:when test="contains(COL5,$varSingleQuote)">
                  <xsl:value-of select="translate(COL5,$varSingleQuote,'')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL5"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol =''">
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

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$Symbol =''">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>

                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>

            </CUSIP>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varAvgPrice">
              <xsl:choose>
                <xsl:when test="$varCusip ='8579929T0'">
                  <xsl:value-of select="$AvgPrice div 100"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varAvgPrice &gt; 0">
                  <xsl:value-of select="$varAvgPrice"/>
                </xsl:when>
                <xsl:when test="$varAvgPrice &lt; 0">
                  <xsl:value-of select="$varAvgPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>



            <xsl:variable name="Side" select="COL14"/>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Side='BUY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Side='CBUY'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>

                <xsl:when test="$Side='SELL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>



            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name ="Date" select="COL20"/>


            <xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>



            <PositionStartDate>             
              <xsl:value-of select="$Date"/>
            </PositionStartDate>


            <PositionSettlementDate>              
              <xsl:value-of select="COL21"/>
            </PositionSettlementDate>


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


            <xsl:variable name="OtherBrokerFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>

              <xsl:choose>

                <xsl:when test="$OtherBrokerFees &gt; 0">
                  <xsl:value-of select="$OtherBrokerFees"/>
                </xsl:when>

                <xsl:when test="$OtherBrokerFees &lt; 0">
                  <xsl:value-of select="$OtherBrokerFees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </SecFee>

          
			  <CounterPartyID>
          <xsl:value-of select="'14'"/>
			  </CounterPartyID>


          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>