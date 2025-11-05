<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
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
                <xsl:value-of select="'BC'"/>
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
            <xsl:value-of select ="'684990001'"/>
          </MinorAccount>

          <AE_RR>
            <xsl:value-of select ="''"/>
          </AE_RR >

          <CommRate>
            <xsl:value-of select ="''"/>
          </CommRate>

          <CommAmt>
            <xsl:value-of select ="''"/>
          </CommAmt>

          <Cusip>
            <xsl:value-of select="''"/>
          </Cusip>

          <OrderID>
            <xsl:value-of select ="''"/>
          </OrderID>

          <FullDistInd>
            <xsl:value-of select ="'N'"/>
          </FullDistInd>

          <StepOut_In_Brkr>
            <xsl:value-of select ="''"/>
          </StepOut_In_Brkr>

          <Exec-Brkr>
            <xsl:value-of select ="CounterParty"/>
          </Exec-Brkr>

          <Blotter>
            <xsl:value-of select="''" />
          </Blotter>

          <Discl_Text1>
            <xsl:value-of select ="''"/>
          </Discl_Text1>

          <Discl_Text2>
            <xsl:value-of select ="''"/>
          </Discl_Text2>

          <UserID>
            <xsl:value-of select ="''"/>
          </UserID>

          <SourceID>
            <xsl:value-of select ="''"/>
          </SourceID>

          <TrailerCode1>
            <xsl:value-of select ="''"/>
          </TrailerCode1>

          <TrailerCode2>
            <xsl:value-of select ="''"/>
          </TrailerCode2>

          <TrailerCode3>
            <xsl:value-of select ="''"/>
          </TrailerCode3>

          <TrailerCode4>
            <xsl:value-of select ="''"/>
          </TrailerCode4>

          <TrailerCode5>
            <xsl:value-of select ="''"/>
          </TrailerCode5>

          <Average_Price_Indicator>
            <xsl:value-of select="''"/>
          </Average_Price_Indicator>

          <New_Blk_Ind>
            <xsl:value-of select ="''"/>
          </New_Blk_Ind>

          <Rule80A>
            <xsl:value-of select ="''"/>
          </Rule80A>

          <Contra_Ind>
            <xsl:value-of select ="''"/>
          </Contra_Ind>

          <Contra_Account>
            <xsl:value-of select ="''"/>
          </Contra_Account>


          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
