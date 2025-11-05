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

        <FileHeader>
          <xsl:value-of select="'true'"/>
        </FileHeader>
        
        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <AssetClass>
          <xsl:value-of select="'Asset Class'"/>
        </AssetClass>

        <Sedol>
          <xsl:value-of select="'Sedol'"/>
        </Sedol>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <Security>
          <xsl:value-of select="'Security'"/>
        </Security>

        <CCY>
          <xsl:value-of select="'CCY'"/>
        </CCY>

        <Fund>
          <xsl:value-of select="'Fund'"/>
        </Fund>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <UnitCostLC>
          <xsl:value-of select="'Unit Cost (LC)'"/>
        </UnitCostLC>

        <MP>
          <xsl:value-of select="'MP'"/>
        </MP>

        <CostLC>
          <xsl:value-of select="'Cost (LC)'"/>
        </CostLC>

        <CostBC>
          <xsl:value-of select="'Cost (BC)'"/>
        </CostBC>

        <MVLC>
          <xsl:value-of select="'MV (LC)'"/>
        </MVLC>

        <MVBC>
          <xsl:value-of select="'MV (BC)'"/>
        </MVBC>


        <UnrealizedGLBC>
          <xsl:value-of select="'Unrealized G/L (BC)'"/>
        </UnrealizedGLBC>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">        
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <FileHeader>
            <xsl:value-of select="'true'"/>
          </FileHeader>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <AssetClass>
            <xsl:value-of select="AssetClass"/>
          </AssetClass>
		  
					<Sedol>
						<xsl:choose>
							<xsl:when test="SEDOLSymbol !='' and SEDOLSymbol !='*'">
								<xsl:value-of select="SEDOLSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Sedol>

          <Symbol>
		  <xsl:choose>
		      <xsl:when test="contains(BloombergSymbol, 'Equity')">
                 <xsl:value-of select="substring-before(BloombergSymbol,' Equity')"/>
			  </xsl:when>
			  
			  <xsl:when test="contains(BloombergSymbol, 'EQUITY')">
                 <xsl:value-of select="substring-before(BloombergSymbol,' EQUITY')"/>
			  </xsl:when>
			  
			  <xsl:when test="AssetClass='Cash'">
                  <xsl:value-of select="SecurityName"/>
			  </xsl:when>
			   </xsl:choose>
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
		  <xsl:variable name="PB_NAME" select="'Bloomberg Port'"/>
  <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="Account"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_ACCOUNT_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
          </xsl:variable>
		  <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountName"/>
          </xsl:variable>
          <Fund>
           <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Account"/>
              </xsl:otherwise>
            </xsl:choose>
          </Fund>

          


          <!-- <xsl:variable name = "PRANA_FUND_NAME"> -->
            <!-- <xsl:value-of select="Account"/> -->
          <!-- </xsl:variable> -->

          <!-- <xsl:variable name ="THIRDPARTY_FUND_CODE"> -->
            <!-- <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/> -->
          <!-- </xsl:variable> -->
          <Account>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_ACCOUNT_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_ACCOUNT_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
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