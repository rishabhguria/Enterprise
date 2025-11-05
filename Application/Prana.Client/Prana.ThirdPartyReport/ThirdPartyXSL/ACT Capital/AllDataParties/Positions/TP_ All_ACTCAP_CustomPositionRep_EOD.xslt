<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <PositionSide>
            <xsl:value-of select="PositionSide"/>
          </PositionSide>
          
          <PortfolioName>
            <xsl:value-of select="PortfolioName"/>
          </PortfolioName>

          <GsecAccount>
            <xsl:value-of select="GsecAccount"/>
          </GsecAccount>

          <SecurityName>
            <xsl:value-of select="SecurityName"/>
          </SecurityName>

          <Quantity>
            <xsl:value-of select="Quantity"/>
          </Quantity>

          <Price>
            <xsl:value-of select="Price"/>
          </Price>

          <MarketValue>            
			<xsl:value-of select="format-number(MarketValue,'0.##')"/>
          </MarketValue>

          <UnitCost>
			<xsl:value-of select="format-number(UnitCost,'0.########')"/>
          </UnitCost>

          <CostBasis>
			<xsl:value-of select="format-number(CostBasis,'0.##')"/>
          </CostBasis>

          <LongTerm>
            <xsl:value-of select="LongTerm"/>
          </LongTerm>

          <ShortTerm>
            <xsl:value-of select="ShortTerm"/>
          </ShortTerm>

          <PercentGainLoss>
			<xsl:value-of select="format-number(PercentGainLoss,'0.########')"/>
          </PercentGainLoss>

          <PercentEquity>
			<xsl:value-of select="format-number(PercentEquity,'0.##########')"/>
          </PercentEquity>

          <Expiration>
            <xsl:value-of select="Expiration"/>
          </Expiration>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <ContractSize>
            <xsl:value-of select="ContractSize"/>
          </ContractSize>

          <OptionIndicator>
            <xsl:value-of select="OptionIndicator"/>
          </OptionIndicator>

          <UnderlyingCusip>
            <xsl:value-of select="UnderlyingCusip"/>
          </UnderlyingCusip>

          <UnderlyingTicker>
            <xsl:value-of select="UnderlyingTicker"/>
          </UnderlyingTicker>
          
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