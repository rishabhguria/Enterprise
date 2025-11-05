<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <AssetClass>
            <xsl:value-of select="AssetClass"/>
          </AssetClass>
		  
					<ISIN>
						<xsl:choose>
							<xsl:when test="ISINSymbol !='' and ISINSymbol !='*'">
								<xsl:value-of select="ISINSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ISIN>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <Security>			
			<xsl:choose>
							<xsl:when test="SecurityName !='' and SecurityName !='*'">
								 <xsl:value-of select="SecurityName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
          </Security>

          <CCY>
            <xsl:value-of select="TradeCurrency"/>
          </CCY>

          <Fund>
            <xsl:value-of select="MasterFund"/>
          </Fund>

          <Account>
            <xsl:value-of select="Account"/>
          </Account>
          
          <Quantity>
          <xsl:value-of select="Quantity"/>
          </Quantity>

          <UnitCostLC>
			<xsl:value-of select ="format-number(UnitCost,'#0.0000')"/>
          </UnitCostLC>

          <MP>
            <xsl:value-of select="MarkPrice"/>
          </MP>
          
          <CostLC>
			<xsl:value-of select ="format-number(CostBasis_Local,'#0.00')"/>
          </CostLC>

          <CostBC>
			<xsl:value-of select ="format-number(CostBasis_Base,'#0.00')"/>
          </CostBC>

          <MVLC>
			<xsl:value-of select ="format-number(MarketValue_Local,'#0.00')"/>
          </MVLC>

          <MVBC>
			<xsl:value-of select ="format-number(MarketValue_Base,'#0.00')"/>
          </MVBC>

		  
		    <UnrealizedGLBC>  
			<xsl:value-of select ="format-number(UnrealizedGainLoss,'#0.00')"/>
          </UnrealizedGLBC>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>