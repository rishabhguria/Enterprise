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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>


  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=JAN ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=FEB ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR' ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='APR' ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=MAY ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=JUN ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=JUL  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=AUG  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=SEP ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=OCT ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=NOV ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=DEC ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=JAN ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=FEB ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR' ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='APR' ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=MAY ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=JUN ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=JUL  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=AUG  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=SEP ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=OCT ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=NOV ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=DEC ">
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
    <xsl:if test="substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL11),' '),' '),' '),' '),' ') ='P' or substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL11),' '),' '),' '),' '),' ') ='C'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(COL11,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL11),' '),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(substring-after(substring-after(COL11,' '),' '),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL11),' '),' '),' '),' '),3,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL11),' '),' '),' '),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL11),' '),' '),' '),' '),' '),'##.##')"/>

      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="$ExpiryMonth"/>
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
      <!--</xsl:otherwise>-->
      <!--

			</xsl:choose>-->
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL16"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'WPS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>



            <xsl:variable name="PB_FUND_NAME" select="COL2"/>

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
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>

                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
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


            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL20"/>
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


            <xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <MiscFees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>
                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

            <xsl:variable name="varOtherBrokerFees1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL45"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOtherBrokerFees2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL46"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOtherBrokerFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$varOtherBrokerFees1 + $varOtherBrokerFees2"/>
              </xsl:call-template>
            </xsl:variable>
            <OtherBrokerFees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>
                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OtherBrokerFees>

            <xsl:variable name="Secfee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="Secfee &gt; 0">
                  <xsl:value-of select="Secfee"/>

                </xsl:when>
                <xsl:when test="Secfee &lt; 0">
                  <xsl:value-of select="Secfee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </SecFee>

            <xsl:variable name="varClearingFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$varClearingFee &gt; 0">
                  <xsl:value-of select="$varClearingFee"/>
                </xsl:when>
                <xsl:when test="$varClearingFee &lt; 0">
                  <xsl:value-of select="$varClearingFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </ClearingFee>

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
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


            <xsl:variable name="varNetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL12,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL12,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL12,1,4)"/>
            </xsl:variable>

            <TradeDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </TradeDate>

            <xsl:variable name="varSMonth">
              <xsl:value-of select="substring(COL14,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varSDay">
              <xsl:value-of select="substring(COL14,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varSYear">
              <xsl:value-of select="substring(COL14,1,4)"/>
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="concat($varSMonth,'/',$varSDay,'/',$varSYear)"/>
            </SettlementDate>


            <xsl:variable name="Side">
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
              </xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


