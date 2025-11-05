<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=01">
        <xsl:value-of select="'Jan'"/>
      </xsl:when>
      <xsl:when test="$Month=02">
        <xsl:value-of select="'Feb'"/>
      </xsl:when>
      <xsl:when test="$Month=03">
        <xsl:value-of select="'Mar'"/>
      </xsl:when>
      <xsl:when test="$Month=04">
        <xsl:value-of select="'Apr'"/>
      </xsl:when>
      <xsl:when test="$Month=05">
        <xsl:value-of select="'May'"/>
      </xsl:when>
      <xsl:when test="$Month=06">
        <xsl:value-of select="'Jun'"/>
      </xsl:when>
      <xsl:when test="$Month=07">
        <xsl:value-of select="'Jul'"/>
      </xsl:when>
      <xsl:when test="$Month=08">
        <xsl:value-of select="'Aug'"/>
      </xsl:when>
      <xsl:when test="$Month=09">
        <xsl:value-of select="'Sep'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'Oct'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'Nov'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'Dec'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>


	 <xsl:for-each select="ThirdPartyFlatFileDetail[CompanyAccountID='8'][(CurrencySymbol!= 'CLP' and CurrencySymbol!= 'COP' and CurrencySymbol!= 'KRW' and CurrencySymbol!='TWD' )]">
        <ThirdPartyFlatFileDetail>
          
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>


          <TradeType>

            <xsl:value-of select="'SPOT'"/>

          </TradeType>

          <xsl:variable name="varFormateMonth">
            <xsl:call-template name="MonthName">
              <xsl:with-param name="Month" select="substring-before(TradeDate,'/')"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varTradeDate">
            <xsl:value-of select="concat(substring-before(substring-after(TradeDate,'/'),'/'),'-',$varFormateMonth,'-',substring-after(substring-after(TradeDate,'/'),'/'))"/>
          </xsl:variable>
          <TradeDate>
            <xsl:value-of select="$varTradeDate"/>
          </TradeDate>

          <xsl:variable name="varSFormateMonth">
            <xsl:call-template name="MonthName">
              <xsl:with-param name="Month" select="substring-before(SettlementDate,'/')"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varSettlementDate">
            <xsl:value-of select="concat(substring-before(substring-after(SettlementDate,'/'),'/'),'-',$varSFormateMonth,'-',substring-after(substring-after(SettlementDate,'/'),'/'))"/>
          </xsl:variable>

          <ValueDate>
            <xsl:value-of select="$varSettlementDate"/>
          </ValueDate>

          <Account>
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>

            </xsl:choose>
          </Account>


          <BuySell>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>

          </BuySell>

          <CUR>
            <xsl:value-of select="CurrencySymbol"/>
          </CUR>

          <GivenAmount>
            <xsl:value-of select="format-number(NetAmount,'#.00')"/>
          </GivenAmount>

          <SpotRate>
            <xsl:value-of select="''"/>
          </SpotRate>

          <PTS>
            <xsl:value-of select="''"/>
          </PTS>

          <AllInRate>
            <xsl:value-of select="''"/>
          </AllInRate>

          <AltCur>
            <xsl:value-of select="'USD'"/>
          </AltCur>

          <AltAmount>
            <xsl:value-of select="''"/>
          </AltAmount>

          <FXBroker>
            <xsl:value-of select="''"/>
          </FXBroker>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

