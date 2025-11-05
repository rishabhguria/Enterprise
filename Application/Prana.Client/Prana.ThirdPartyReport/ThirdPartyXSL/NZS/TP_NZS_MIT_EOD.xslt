<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>
        
        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>
        
        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>
        
        <MarketValueUSD>
          <xsl:value-of select="'Market Value (USD)'"/>
        </MarketValueUSD>
        
        <PriceLocal>
          <xsl:value-of select="'Price (Local)'"/>
        </PriceLocal>
        
        <UnderlyingSecurityGroup>
          <xsl:value-of select="'Underlying Security Group'"/>
        </UnderlyingSecurityGroup>
        
        <PortfolioName>
          <xsl:value-of select="'Portfolio Name'"/>
        </PortfolioName>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <!--<xsl:for-each select="ThirdPartyFlatFileDetail">-->
       <xsl:for-each select="ThirdPartyFlatFileDetail[CompanyFundID ='1']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <SecurityDescription>
            
            <xsl:choose>
              <xsl:when test="AssetClass = 'Equity'">
                <xsl:value-of select="SecurityName"/>
              </xsl:when>
              <xsl:when test="AssetClass = 'Cash'">
                <xsl:value-of select="TradeCurrency"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>

          </SecurityDescription>
          
          <ISIN>
            
            <xsl:choose>
              <xsl:when test="AssetClass = 'Equity'">
                <xsl:value-of select="ISINSymbol"/>
              </xsl:when>
              <xsl:when test="AssetClass = 'Cash'">
                <xsl:value-of select="''"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ISIN>
          
          <Quantity>
            <xsl:value-of select="Quantity"/>
          </Quantity>
          
          <MarketValueUSD>
            <xsl:value-of select ="format-number(MarketValue_Base,'#0.00')"/>
          </MarketValueUSD>
          
          <PriceLocal>
            <xsl:value-of select ="CostBasis_Local"/>
          </PriceLocal>
          
          <UnderlyingSecurityGroup>
            <xsl:choose>
              <xsl:when test="contains(AssetClass, 'Equity')">
                <xsl:value-of select="'EQUITY'"/>
              </xsl:when>

              <xsl:when test="contains(AssetClass, 'Cash')">
                <xsl:value-of select="'CASH'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingSecurityGroup>
          
         
          <PortfolioName>
            <xsl:value-of select="Account"/>
          </PortfolioName>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>