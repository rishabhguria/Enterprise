<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
	
   <xsl:for-each select="ThirdPartyFlatFileDetail ">

        <ThirdPartyFlatFileDetail>
		
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
			<Portfolio>
			<xsl:value-of select ="AccountName"/>
			</Portfolio>
			
			<Security>
			<xsl:value-of select ="Symbol"/>
			</Security>	
			
			<SecurityDescription>
			<xsl:value-of select ="SecurityDescription"/>
			</SecurityDescription>	
			
			<ISIN>
			<xsl:value-of select ="ISINSymbol"/>
			</ISIN>	
			
			<Cusip>
			<xsl:value-of select ="CUSIPSymbol"/>
			</Cusip>	
			
			<Sedol>
			<xsl:value-of select ="SEDOLSymbol"/>
			</Sedol>
			<OCCSymbol>
						<xsl:value-of select ="''"/>			
			</OCCSymbol>
			
			<BloombergID>
			<xsl:value-of select ="BloombergSymbol"/>
			</BloombergID>	
			
			<AssetType>
			<xsl:choose>
				   <xsl:when test="AssetClass ='Equity'">
				   	<xsl:value-of select="'Stock'"/>
				   </xsl:when>								
				<xsl:when test="AssetClass ='Cash'">
				   	<xsl:value-of select="'Cash'"/>
				   </xsl:when>
				<xsl:when test="AssetClass ='EquityOption'">
					<xsl:value-of select="'Option'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
				</xsl:choose>
			</AssetType>
			
			<Currency>
			<xsl:value-of select ="LocalCurrency"/>
			</Currency>
			<xsl:variable name="PB_NAME">
				<xsl:value-of select="'BAML'"/>
			</xsl:variable>
			 <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_CUSTODIAN_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CustodianMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@CustodianName"/>
          </xsl:variable>
			<Custodian>
			         <xsl:choose>               
                         <xsl:when test ="$THIRDPARTY_CUSTODIAN_NAME != ''">
                      <xsl:value-of select ="$THIRDPARTY_CUSTODIAN_NAME"/>
                     </xsl:when>
                      <xsl:otherwise>
                         <xsl:value-of select ="''"/>
                       </xsl:otherwise>
                     </xsl:choose>
				</Custodian>
			
			<TradeDate>
				<xsl:value-of select ="TradeDate"/>
			 </TradeDate>
			<Price>
				<xsl:value-of select ="MarkPrice"/>
			</Price>
			
			<Cost>
				<xsl:value-of select ="TotalCost_Local"/>
			</Cost>
			
			<Amount>
				<xsl:value-of select ="MarketValue"/>
			</Amount>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

			
		</ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>