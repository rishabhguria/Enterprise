<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
			
		 <HoldingsDate>
			<xsl:value-of select="TradeDate"/>
		 </HoldingsDate>
			
			         <xsl:variable name="PB_NAME" select="''"/>
		
			         <xsl:variable name="PB_FUND_NAME" select="AccountName"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaCode"/>
						</xsl:variable>
						<PortfolioIdentifier>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</PortfolioIdentifier>
		

			<PortfolioName>
             <xsl:value-of select="AccountName"/>
            </PortfolioName>

          <SecurityIdentifier>
            <xsl:value-of select="Symbol"/>
          </SecurityIdentifier>

			
			<AlternateIdentifier1>
				<xsl:choose>
					<xsl:when test="AssetClass='Future'">
						<xsl:value-of select="BloombergSymbol"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="SEDOLSymbol"/>
					</xsl:otherwise>
				</xsl:choose>
				         
          </AlternateIdentifier1>
			
			<AlternateIdentifier2>
           <xsl:value-of select="CUSIPSymbol"/>
          </AlternateIdentifier2>
			
			<AlternateIdentifier3>
           <xsl:value-of select="ISINSymbol"/>
          </AlternateIdentifier3>
			
			<MajorAssetClass>
				<xsl:value-of select="AssetClass"/>			
			</MajorAssetClass>
			
			<MinorAssetClass>
				<xsl:choose>
					<xsl:when test="AssetClass='Equity'">
						<xsl:value-of select="'Common Stock'"/>					
					</xsl:when>
				<xsl:when test="AssetClass='FixedIncome'">
						<xsl:value-of select="'Corporates'"/>					
					</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>				
				</xsl:otherwise>
			    </xsl:choose>			
			</MinorAssetClass>
			
			<FullSecurityDescription>
				<xsl:value-of select="SecurityDescription"/>			
			</FullSecurityDescription>
			
		  <Units>
            <xsl:value-of select="OpenPositions"/>
          </Units>
			
			<MarketValue>
				<xsl:value-of select="MarketValue"/>			
			</MarketValue>
				
			<MarketValueCurrencCode>
				<xsl:value-of select="LocalCurrency"/>			
			</MarketValueCurrencCode>
		


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>