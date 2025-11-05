<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <FileHeader>
          <xsl:value-of select ="'true'"/>
        </FileHeader>

        <FileFooter>
          <xsl:value-of select ="'false'"/>
        </FileFooter>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <RecordAction>
          <xsl:value-of select="'RecordAction'"/>
        </RecordAction>

        <RecordType>
          <xsl:value-of select="'RecordType'"/>
        </RecordType>

        <Portfolio>
          <xsl:value-of select="'Asset Class'"/>
        </Portfolio>

        <Investment>
          <xsl:value-of select="'Investment'"/>
        </Investment>

        <LocationAccount>
          <xsl:value-of select="'LocationAccount'"/>
        </LocationAccount>

        <Strategy>
          <xsl:value-of select="'Strategy'"/>
        </Strategy>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <EventDate>
          <xsl:value-of select="'EventDate'"/>
        </EventDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>

        <ActualSettleDate>
          <xsl:value-of select="'ActualSettleDate'"/>
        </ActualSettleDate>

        <SecFeeAmount>
          <xsl:value-of select="'SecFeeAmount'"/>
        </SecFeeAmount>

        <NetCounterAmount>
          <xsl:value-of select="'NetCounterAmount'"/>
        </NetCounterAmount>

        <NetInvestmentAmount>
          <xsl:value-of select="'NetInvestmentAmount'"/>
        </NetInvestmentAmount>

        <TotCommission>
          <xsl:value-of select="'TotCommission'"/>
        </TotCommission>

        <UserTranId1>
          <xsl:value-of select="'UserTranId1'"/>
        </UserTranId1>

        <PriceDenomination>
          <xsl:value-of select="'PriceDenomination'"/>
        </PriceDenomination>

        <CounterInvestment>
          <xsl:value-of select="'CounterInvestment'"/>
        </CounterInvestment>

        <CounterFXDenomination>
          <xsl:value-of select="'CounterFXDenomination'"/>
        </CounterFXDenomination>

        <TradeFX>
          <xsl:value-of select="'TradeFX'"/>
        </TradeFX>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

		  <UnderlyingSymbol>
			  <xsl:value-of select ="'UnderlyingSymbol'"/>
		  </UnderlyingSymbol>

		  <ExpirationDate>
					  <xsl:value-of select ="'ExpirationDate'"/>
		  </ExpirationDate>

		  <StrikePrice>
					  <xsl:value-of select ="'StrikePrice'"/>
		  </StrikePrice>

		  <Put_Call>
			  <xsl:value-of select ="'PutOrCall'"/>
		  </Put_Call>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="//ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <FileHeader>
            <xsl:value-of select ="'false'"/>
          </FileHeader>
          
          <FileFooter>
            <xsl:value-of select ="'false'"/>
          </FileFooter>

          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="Prana_FundName">
            <xsl:value-of select="FundName"/>
          </xsl:variable>

          <xsl:variable name="PRANA_MasterFund_Name">
            <xsl:value-of select="document('../ReconMappingXml/MasterFundMapping.xml')/MasterFundMapping/PB[@Name= 'GS']/MasterFundData[@FundName=$Prana_FundName]/@MasterFundName"/>
          </xsl:variable>


          <RecordAction>
			  <xsl:choose>
				  <xsl:when test ="TaxLotState = 'Allocated'">
					  <xsl:value-of select="'New'"/>
				  </xsl:when>
				  <xsl:when test ="TaxLotState = 'Amemded'">
					  <xsl:value-of select="'c'"/>
				  </xsl:when>
				  <xsl:when test ="TaxLotState = 'Deleted'">
					  <xsl:value-of select="'x'"/>
				  </xsl:when>
			  </xsl:choose>
		  </RecordAction>

          <RecordType>
            <xsl:value-of select="Side"/>
          </RecordType>

          <Portfolio>
            <xsl:value-of select="$PRANA_MasterFund_Name"/>
          </Portfolio>

          <Investment>
			  <xsl:choose>
				  <xsl:when test ="Asset = 'EquityOption'">
					  <xsl:value-of select ="OSIOptionSymbol"/>
				  </xsl:when>
				  <xsl:when test ="contains(Symbol, '-') != false">
					  <xsl:value-of select ="SEDOL"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </Investment>

          <LocationAccount>
            <xsl:value-of select="concat($PRANA_MasterFund_Name, '_', FundName)"/>
          </LocationAccount>

          <Strategy>
            <xsl:value-of select="Strategy"/>
          </Strategy>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

          <EventDate>
            <xsl:value-of select="TradeDate"/>
          </EventDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <ActualSettleDate>
            <xsl:value-of select="SettlementDate"/>
          </ActualSettleDate>

          <SecFeeAmount>
			  <xsl:choose>
				  <xsl:when test ="Side = 'Sell' or Side = 'Sell short'">
					  <xsl:value-of select="GrossAmount*0.0000174"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select ="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </SecFeeAmount>

          <NetCounterAmount>
            <xsl:value-of select="''"/>
          </NetCounterAmount>

          <NetInvestmentAmount>
            <xsl:value-of select="NetAmount"/>
          </NetInvestmentAmount>

          <TotCommission>
            <xsl:value-of select="CommissionCharged"/>
          </TotCommission>

          <UserTranId1>
            <xsl:value-of select="EntityID"/>
          </UserTranId1>

          <PriceDenomination>
            <xsl:value-of select="CurrencySymbol"/>
          </PriceDenomination>

          <CounterInvestment>
            <xsl:value-of select="CurrencySymbol"/>
          </CounterInvestment>

          <CounterFXDenomination>
            <xsl:value-of select="CurrencySymbol"/>
          </CounterFXDenomination>

          <TradeFX>
            <xsl:value-of select="ForexRate"/>
          </TradeFX>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

			<UnderlyingSymbol>
						<xsl:value-of select ="UnderlyingSymbol"/>
			</UnderlyingSymbol>

			<ExpirationDate>
				<xsl:choose>
					<xsl:when test ="Asset = 'EquityOption'">
						<xsl:value-of select ="ExpirationDate"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</ExpirationDate>

			<StrikePrice>
				<xsl:choose>
					<xsl:when test ="number(StrikePrice)">
						<xsl:value-of select ="StrikePrice"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</StrikePrice>

			<Put_Call>
				<xsl:value-of select ="substring(PutOrCall,1,1)"/>
			</Put_Call>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
