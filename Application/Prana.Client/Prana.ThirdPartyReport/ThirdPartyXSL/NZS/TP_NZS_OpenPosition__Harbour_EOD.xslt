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


          <MODELDATE>
            <xsl:value-of select ="'MODEL_DATE'"/>
          </MODELDATE>

          <MODELCODE>
            <xsl:value-of select ="'MODEL_CODE'"/>
          </MODELCODE>

          <SUBADVISORCODE>
            <xsl:value-of select ="'SUBADVISOR_CODE'"/>
          </SUBADVISORCODE>

          <MODELNAME>
            <xsl:value-of select ="'MODEL_NAME'"/>
          </MODELNAME>


          <TICKER>
            <xsl:value-of select ="'TICKER'"/>
          </TICKER>

          <SECURITYNAME>
            <xsl:value-of select ="'SECURITY_NAME'"/>
          </SECURITYNAME>

          <SEDOL>
            <xsl:value-of select ="'SEDOL'"/>
          </SEDOL>

          <CUSIP>
            <xsl:value-of select ="'CUSIP'"/>
          </CUSIP>

          <ISIN>
            <xsl:value-of select ="'ISIN'"/>
          </ISIN>

          <BLOOMBERGID>
            <xsl:value-of select ="'BLOOMBERG_ID'"/>
          </BLOOMBERGID>

          <BLOOMBERG>
            <xsl:value-of select ="'BLOOMBERG_YELLOW_KEY'"/>
          </BLOOMBERG>

          <PRIMARYMIC>
            <xsl:value-of select ="'PRIMARY_MIC'"/>
          </PRIMARYMIC>

          <SECURITYTYPE>
            <xsl:value-of select ="'SECURITY_TYPE'"/>
          </SECURITYTYPE>

          <TRADEDATE>
            <xsl:value-of select ="'TRADE_DATE'"/>
          </TRADEDATE>
          <TRADETYPE>
            <xsl:value-of select ="'TRADE_TYPE'"/>
          </TRADETYPE>
          <CURRENCY>
            <xsl:value-of select ="'TRADING_CURRENCY'"/>
          </CURRENCY>

          <BODWEIGHT>
            <xsl:value-of select ="'BOD_WEIGHT'"/>
          </BODWEIGHT>

          <TRADEDWEIGHT>
            <xsl:value-of select ="'TRADED_WEIGHT'"/>
          </TRADEDWEIGHT>


          <ENDINGWEIGHT>
            <xsl:value-of select ="'ENDING_WEIGHT'"/>
          </ENDINGWEIGHT>

          <NOTES>
            <xsl:value-of select ="'NOTES'"/>
          </NOTES>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <MODELDATE>
            <xsl:value-of select ="TradeDate"/>
          </MODELDATE>

          <MODELCODE>
            <xsl:value-of select ="'DIF'"/>
          </MODELCODE>

          <SUBADVISORCODE>
            <xsl:value-of select ="'NZS'"/>
          </SUBADVISORCODE>

          <MODELNAME>
            <xsl:value-of select ="'Harbor Disruptive and Innovation Fund'"/>
          </MODELNAME>

          <TICKER>
            <xsl:value-of select ="Symbol"/>
          </TICKER>

          <SECURITYNAME>
            <xsl:value-of select ="SecurityName"/>
          </SECURITYNAME>

          <SEDOL>
            <xsl:value-of select ="SEDOLSymbol"/>
          </SEDOL>

          <CUSIP>
            <xsl:value-of select ="CUSIPSymbol"/>
          </CUSIP>

          <ISIN>
            <xsl:value-of select ="ISINSymbol"/>
          </ISIN>

          <BLOOMBERGID>
            <xsl:value-of select ="''"/>
          </BLOOMBERGID>

          <BLOOMBERG>
            <xsl:value-of select ="AssetName"/>
          </BLOOMBERG>

          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'Harbour Open Position'"/>
          </xsl:variable>

          <xsl:variable name="PRANA_MIC_NAME" select="Exchange"/>
          <xsl:variable name="varCCY" select="LocalCurrency"/>

          <xsl:variable name="THIRDPARTY_MIC_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdPartyMIC_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@MIC=$PRANA_MIC_NAME and @CCY=$varCCY]/@PranaMIC"/>
          </xsl:variable>

          <xsl:variable name="MIC">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_MIC_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_MIC_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <PRIMARYMIC>
            <xsl:value-of select ="$MIC"/>
          </PRIMARYMIC>
          
          <SECURITYTYPE>
            <xsl:value-of select ="AssetName"/>
          </SECURITYTYPE>

          <TRADEDATE>
            <xsl:value-of select ="TradeDate"/>
          </TRADEDATE>
		  
          <TRADETYPE>
            <xsl:value-of select ="PositionIndicator"/>
          </TRADETYPE>
          
          <CURRENCY>
            <xsl:value-of select ="LocalCurrency"/>
          </CURRENCY>
		  
		  <!-- <MarketValue_PreviousDay_Base> -->
            <!-- <xsl:value-of select ="format-number(MarketValue_PreviousDay_Base,'#.##')"/> -->
          <!-- </MarketValue_PreviousDay_Base> -->
		  
		  <!-- <TotalMarketValue_PreviousDay_Base> -->
            <!-- <xsl:value-of select ="format-number(TotalMarketValue_PreviousDay_Base,'#.##')"/> -->
          <!-- </TotalMarketValue_PreviousDay_Base> -->

          <BODWEIGHT>
            <xsl:value-of select ="format-number(PrevoiusPortfolioWeight,'#.####')"/>
          </BODWEIGHT>
		  
		  <!-- <NetNotional_Base> -->
            <!-- <xsl:value-of select ="format-number(NetNotional_Base,'#.##')"/> -->
          <!-- </NetNotional_Base> -->
		  
		  <!-- <NAV_PreviousDay_Base> -->
            <!-- <xsl:value-of select ="format-number(NAV_PreviousDay_Base,'#.##')"/> -->
          <!-- </NAV_PreviousDay_Base> -->

          <TRADEDWEIGHT>
				<xsl:choose>
					<xsl:when test="AssetName='Equity' and number(TRADED_WEIGHT)">
						<xsl:value-of select ="format-number(TRADED_WEIGHT,'#0.####')"/>					
					</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>				
				</xsl:otherwise>
				</xsl:choose>            
          </TRADEDWEIGHT>
		  
		   <!-- <MarketValue_Ending_Base> -->
            <!-- <xsl:value-of select ="format-number(MarketValue_Ending_Base,'#.##')"/> -->
          <!-- </MarketValue_Ending_Base> -->
		  
		   <!-- <TotalMarketValue_Ending_Base> -->
            <!-- <xsl:value-of select ="format-number(TotalMarketValue_Ending_Base,'#.##')"/> -->
          <!-- </TotalMarketValue_Ending_Base> -->


          <ENDINGWEIGHT>
            <xsl:value-of select ="format-number(CurrPortfolioWeight,'#.####')"/>
          </ENDINGWEIGHT>

          <NOTES>
            <xsl:value-of select ="''"/>
          </NOTES>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>