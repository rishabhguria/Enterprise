<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
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
    <xsl:if test="COL10='OPTN'">

      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(COL7,'1')"/>
      </xsl:variable>

      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(COL7,$UnderlyingSymbol),1,2)"/>
      </xsl:variable>

      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after(COL7,$ExpiryYear),1,2)"/>
      </xsl:variable>

      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after(COL7,$ExpiryMonth),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(COL7,$ExpiryDay),1,1)"/>
      </xsl:variable>

      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(COL7,$UnderlyingSymbol),8),'#.00')"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,format-number($StrikePrice,'#.00'),'D',$Day)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($NetPosition)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Jeff'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL21)"/>
            </xsl:variable>

            <xsl:variable name="AssetType" select="normalize-space(COL10)"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:variable name="Symbol">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="string-length(COL6) &gt; 20">
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

                <xsl:when test="$Asset='EquityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$AssetType='EQTY'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$Asset='EquityOption'">
                  <xsl:value-of select="concat(COL6,'U')"/>
                </xsl:when>

                <xsl:when test="$AssetType='EQTY'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= $PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <ExecutingBroker>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL14"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExecutingBroker>

            <PositionStartDate>
              <xsl:value-of select="COL3"/>
            </PositionStartDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition&gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition&lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="Side" select="normalize-space(COL5)"/>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Side = 'BY' and substring($AssetType,1,3)= 'OPT'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>

                <xsl:when test="$Side = 'SL' and substring($AssetType,1,3)= 'OPT'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>

                <xsl:when test="$Side = 'SS' and substring($AssetType,1,3)= 'OPT'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>

                <xsl:when test="$Side = 'BY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$Side = 'CS'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$Side = 'SL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$Side = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
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
                <xsl:with-param name="Number" select="COL16"/>
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
            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>

            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>
            <xsl:variable name="varCoupon">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL24"/>
              </xsl:call-template>
            </xsl:variable>
            <NetAmount>
              <xsl:value-of select="$varCoupon"/>
            </NetAmount>

            <xsl:variable name="Stamp" select="''"/>

            <StampDuty>
              <xsl:choose>
                <xsl:when test="$Stamp &gt; 0">
                  <xsl:value-of select="$Stamp"/>
                </xsl:when>

                <xsl:when test="$Stamp &lt; 0">
                  <xsl:value-of select="$Stamp*-1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </StampDuty>

            <xsl:variable name="NetNotional">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL24"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varStamp">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="varFees" >
              <xsl:choose>
                <xsl:when test="$Side='BY' and $AssetType='EQTY'">
                  <xsl:value-of select="format-number($NetNotional - $NetPosition * $CostBasis - $Commission,'#.##') "/>
                </xsl:when>
                <xsl:when test="$Side='CS' and $AssetType='EQTY'">
                  <xsl:value-of select="format-number($NetNotional - $NetPosition * $CostBasis - $Commission,'#.##') "/>
                </xsl:when>


                <xsl:when test="$Side='BY' and $AssetType='OPTN'">
                  <xsl:value-of select="format-number($NetNotional - ($NetPosition * 100) * $CostBasis - $Commission,'#.##') "/>
                </xsl:when>

                <!--<xsl:when test="$Side='SL' and $AssetType='EQTY'">
                  <xsl:value-of select="format-number($NetPosition * $CostBasis - $Commission - $Stamp - $NetNotional,'#.##') "/>
                </xsl:when>-->

                <xsl:when test="$Side='SL' and $AssetType='EQTY'">
                  <xsl:value-of select="format-number(((($NetPosition * $CostBasis)- $Commission) - $varStamp)-$NetNotional ,'#.##') "/>
                </xsl:when>

                <xsl:when test="$Side='SL' and $AssetType='OPTN'">
                  <xsl:value-of select="format-number((((($NetPosition*100) * $CostBasis) - $Commission) - $varStamp)-$NetNotional ,'#.##') "/>
                </xsl:when>



              </xsl:choose>
            </xsl:variable>

            <Fees>
              <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

            <PBSymbol>
              <xsl:value-of select="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>