<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

    <xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail[PortfolioName='G2 Investment Partners LP' or PortfolioName='MS Investment Partners LP']">
        <ThirdPartyFlatFileDetail>
          
           <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="PortfolioName"/>
          </xsl:variable>

          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Essentia']/FundData[@PranaFund=$PB_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
          
          <!--<PortfolioCode>
            <xsl:value-of select ="PortfolioName"/>
          </PortfolioCode>-->
          
          <PortfolioCode>
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME != ''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </PortfolioCode>

          <PortfolioName>
                <xsl:value-of select ="$PB_FUND_NAME"/>
          </PortfolioName>

          <HoldingDate>
            <xsl:value-of select="HoldingDate"/>
          </HoldingDate>


          
          <HoldingDescription>
            <xsl:value-of select="SecurityName"/>
          </HoldingDescription>
          
            <Symbol>
            <xsl:choose>
              <xsl:when test="AssetClass='Equity' and SEDOLSymbol != ''">
                <xsl:value-of select="SEDOLSymbol"/>
              </xsl:when>
                <xsl:when test="AssetClass='EquityOption' and OSISymbol != ''">
                <xsl:value-of select="OSISymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>
          
           <SymbolType>
            <xsl:choose>
              <xsl:when test="AssetClass='Equity' and SEDOLSymbol != ''">
                <xsl:value-of select="'SEDOL'"/>
              </xsl:when>
                <xsl:when test="AssetClass='EquityOption' and OSISymbol != ''">
                <xsl:value-of select="'OSI'"/>
              </xsl:when>
                <xsl:when test="AssetClass='FX' or AssetClass='FXForward'">
                <xsl:value-of select="'FX'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'Symbol'"/>
              </xsl:otherwise>
            </xsl:choose>
          </SymbolType>
          
          <DisplaySymbol>
            <xsl:value-of select="''"/>
          </DisplaySymbol>

          <Quantity>
           <xsl:value-of select="Quantity"/>
          </Quantity>

          <InstrumentPrice>
			<xsl:value-of select="format-number(MarkPrice,'0.######')"/>
          </InstrumentPrice>

          <InstrumentCurrency>
            <xsl:value-of select="TradeCurrency"/>
          </InstrumentCurrency>
          
           <HoldingIdentifier>
            <xsl:value-of select="''"/>
          </HoldingIdentifier>
          
           <AssetClass>
            <xsl:value-of select="AssetClass"/>
          </AssetClass>
          
           <InstrumentType>
            <xsl:value-of select="'STOCK'"/>
          </InstrumentType>
            
          <HoldingMarketValue>
            <xsl:value-of select="format-number(MarketValue,'0.##')"/>
          </HoldingMarketValue>
          
          <PortfolioGrossValue>
		  <xsl:value-of select="format-number(GrossMarketValue,'0.##')"/>
          </PortfolioGrossValue>
          
           <PortfolioWeight>
			<xsl:value-of select="format-number(PortfolioWeight,'0.######')"/>
          </PortfolioWeight>          

          <PortfolioCurrency>
            <xsl:value-of select="PortfolioCurrency"/>
          </PortfolioCurrency>
     
          <AUM>
			<xsl:value-of select="format-number(MasterFundNAV,'0.##')"/>
          </AUM>

           <AUMCurrency>
            <xsl:value-of select="PortfolioCurrency"/>
          </AUMCurrency>

          <Sector>
            <xsl:value-of select="''"/>
          </Sector>

           <Industry>
            <xsl:value-of select="''"/>
          </Industry>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>