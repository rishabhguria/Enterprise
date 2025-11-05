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

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="//ThirdPartyFlatFileDetail[AssetID != 2]">

        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>
          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <TradeDate>
            <xsl:value-of select ="TradeDate"/>
          </TradeDate>

          <SettDate>
            <xsl:value-of select ="SettlementDate"/>
          </SettDate>

          <MajorAccount>
            <xsl:value-of select ="'EG001'"/>
          </MajorAccount>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <!-- Side Starts-->
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Open'">
              <Side>
                <xsl:value-of select="'B'"/>
              </Side>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close'">
              <Side>
                <xsl:value-of select="'S'"/>
              </Side>
            </xsl:when>
            <xsl:when test="Side='Buy to Cover' or Side='Buy to Close'">
              <Side>
                <xsl:value-of select="'B'"/>
              </Side>
            </xsl:when>
            <xsl:when test="Side='Sell short' or Side='Sell to Open'">
              <Side>
                <xsl:value-of select="'SS'"/>
              </Side>
            </xsl:when>
            <xsl:otherwise >
              <Side>
                <xsl:value-of select="''"/>
              </Side>
            </xsl:otherwise>
          </xsl:choose >

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <MinorAccount>
            <!--<xsl:value-of select ="'684990001'"/>-->
            <xsl:value-of select ="''"/>
          </MinorAccount>

          <AE_RR>
            <xsl:value-of select ="''"/>
          </AE_RR >

          <CommRate>
            <xsl:value-of select ="''"/>
          </CommRate>

          <OrderType>
            <xsl:value-of select ="''"/>
          </OrderType>

          <ExchangeCd>
            <xsl:value-of select ="''"/>
          </ExchangeCd>

          <OrderID>
            <xsl:value-of select ="''"/>
          </OrderID>

          <OfficeCode>
            <xsl:value-of select ="'EG0'"/>
          </OfficeCode>

          <xsl:variable name ="varCount" select ="position()" />

          <xsl:variable name = "recordCount" >
            <xsl:call-template name="noofzeros">
              <xsl:with-param name="count" select="(9) - string-length($varCount)" />
            </xsl:call-template>
          </xsl:variable>

          <ExecID>
            <xsl:value-of select ="concat($recordCount,$varCount)"/>
            <!--<xsl:value-of select ="TradeRefID"/>-->
          </ExecID>

          <Exec-Brkr>
            <xsl:value-of select ="CounterParty"/>
          </Exec-Brkr>

          <ExecTime>
            <xsl:value-of select ="''"/>
            <!--<xsl:value-of select ="substring-after(TradeDateTime,' ')"/>-->
          </ExecTime>

          <Blotter>
            <xsl:value-of select="''" />
          </Blotter>

          <Cusip>
            <xsl:value-of select="''"/>
          </Cusip>

          <Cancelcode>
            <xsl:value-of select="''"/>
          </Cancelcode>

          <TrailerCode2>
            <xsl:value-of select ="''"/>
          </TrailerCode2>

          <TrailerCode_2>
            <xsl:value-of select ="''"/>
          </TrailerCode_2>

          <Rule80A>
            <xsl:value-of select ="''"/>
          </Rule80A>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
