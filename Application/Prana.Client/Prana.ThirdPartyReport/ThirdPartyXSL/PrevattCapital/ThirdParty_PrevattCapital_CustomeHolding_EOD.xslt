<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
       <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
			

					<Date>
						<xsl:value-of select="'Date'"/>
					</Date>
					
					<Product>
						<xsl:value-of select="'Product'"/>
					</Product>

					<Identifier>
						<xsl:value-of select="'Identifier'"/>
					</Identifier>

					<IdentifierType>
						<xsl:value-of select="'Identifier Type'"/>
					</IdentifierType>

					<Ticker>
						<xsl:value-of select="'Ticker'"/>
					</Ticker>

					<SecurityName>
						<xsl:value-of select="'Security Name'"/>
					</SecurityName>

					<SecurityType>
						<xsl:value-of select="'Security Type'"/>
					</SecurityType>

					<ofShares>
					   <xsl:value-of select="'# of Shares'"/>
					</ofShares>

					<SecurityPrice>
						<xsl:value-of select="'Security Price'"/>
					</SecurityPrice>

					<Weight>
						<xsl:value-of select="'Weight (%)'"/>
					</Weight>
					
					<Country>
						<xsl:value-of select="'Country'"/>
					</Country>

					<MarketValue>
						<xsl:value-of select="'Market Value'"/>
					</MarketValue>

					<MarketValueLocal>
						<xsl:value-of select="'Market Value (Local)'"/>
					</MarketValueLocal>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
		
      <xsl:for-each select="ThirdPartyFlatFileDetail[Quantity !=0]">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
			

					<Date>
						<xsl:value-of select="TradeDate"/>
					</Date>
					
					<Product>
						<xsl:choose>
							<xsl:when test="AssetClass='EquityOption' and CountryName= 'United States'">
								<xsl:value-of select="'US EquityOption'"/>
							</xsl:when>
							<xsl:when test="AssetClass='Equity' and TradeCurrency= 'USD'">
								<xsl:value-of select="'US Equity'"/>
							</xsl:when>
							<xsl:when test="AssetClass='Equity' and TradeCurrency != 'USD'">
								<xsl:value-of select="concat(CountryName, ' Equity')"/>
							</xsl:when>
							<xsl:when test="AssetClass='Cash'">
								<xsl:value-of select="'Cash'"/>
							</xsl:when>
							<xsl:when test="AssetClass='FixedIncome' and CountryName= 'United States'">
								<xsl:value-of select="'US FixedIncome'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Product>

					<Identifier>
						<xsl:choose>
							<xsl:when test="AssetClass='Equity'  and CUSIPSymbol != ''">
								<xsl:value-of select="CUSIPSymbol"/>
							</xsl:when>
							<xsl:when test="AssetClass='EquityOption' and OSISymbol != ''">
								<xsl:value-of select="OSISymbol"/>
							</xsl:when>
							
							<xsl:when test="AssetClass='Cash'">
								<xsl:value-of select="'Cash'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Identifier>

					<IdentifierType>
						<xsl:choose>
							<xsl:when test="AssetClass='Equity' and CUSIPSymbol != ''">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="AssetClass='EquityOption' and OSISymbol != ''">
								<xsl:value-of select="'OSI'"/>
							</xsl:when>
							<xsl:when test="AssetClass='FX' or AssetClass='FXForward'">
								<xsl:value-of select="'FX'"/>
							</xsl:when>
							<xsl:when test="AssetClass='FixedIncome'  and CUSIPSymbol != ''">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="AssetClass='Cash'">
								<xsl:value-of select="'Cash'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</IdentifierType>

					<Ticker>
						<xsl:value-of select="Symbol"/>
					</Ticker>

					<SecurityName>
						<xsl:value-of select ="SecurityName"/>
					</SecurityName>

					<SecurityType>
						<xsl:value-of select="AssetClass"/>
					</SecurityType>

					<ofShares>
					<xsl:value-of select="Quantity"/>
					</ofShares>

					<SecurityPrice>
						<xsl:choose>
							<xsl:when test="AssetClass='Cash'">
								<xsl:value-of select="EndDateFXRate"/>							
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="MarkPrice"/>						
						</xsl:otherwise>
						</xsl:choose>
						
					</SecurityPrice>

					<Weight>
						<xsl:value-of select="CurrPortfolioWeight"/>
					</Weight>
					
					<Country>
						<xsl:value-of select="CountryName"/>
					</Country>

					<MarketValue>
						<xsl:value-of select="format-number(MarketValue_Base,'0.##')"/>
					</MarketValue>

					<MarketValueLocal>
						<xsl:value-of select="format-number(MarketValue_Local,'0.##')"/>
					</MarketValueLocal>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>