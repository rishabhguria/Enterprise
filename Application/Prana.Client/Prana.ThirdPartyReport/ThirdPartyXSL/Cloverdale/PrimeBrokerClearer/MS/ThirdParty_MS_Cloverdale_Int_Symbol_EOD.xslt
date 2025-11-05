<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

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

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption' and CurrencySymbol='CAD']">


        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="true"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>


          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
                <xsl:value-of select="'Short'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="PB_NAME" select="'CLOVERDALE'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <!--Exercise / Assign Need To Ask-->

          <BLANK1>
            <xsl:value-of select="''"/>
          </BLANK1>


          <BLANK2>
            <xsl:value-of select="''"/>
          </BLANK2>



          <BLANK3>
            <xsl:value-of select="''"/>
          </BLANK3>

          <B_S>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close' or Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </B_S>

          <Product>
            <xsl:value-of select="Symbol"/>
          </Product>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>



          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <BLANK4>
            <xsl:value-of select="''"/>
          </BLANK4>



          <Account>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </Account>

          <BLANK5>
            <xsl:value-of select="''"/>
          </BLANK5>


          <OpenClose>
            <xsl:choose>
              <xsl:when test="contains(Side,'Open')">
                <xsl:value-of select="'Open'"/>
              </xsl:when>
              <xsl:when test="contains(Side,'Close')">
                <xsl:value-of select="'Close'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              
          </OpenClose>

          <BLANK6>
            <xsl:value-of select="''"/>
          </BLANK6>


          <xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_BROKER_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <ExecutingBroker>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
                <xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_BROKER_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExecutingBroker>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>



          <Commission>
            <xsl:value-of select="CommissionCharged"/>
          </Commission>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
