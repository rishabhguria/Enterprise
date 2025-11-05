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

		  <PositionDate>
			  <xsl:value-of select="'Position Date'"/>
		  </PositionDate>

		  <Symbol>
			  <xsl:value-of select="'Symbol'"/>
		  </Symbol>

		  <BloombergSymbol>
			  <xsl:value-of select="'Bloomberg Symbol'"/>
		  </BloombergSymbol>

		  <Sedol>
			  <xsl:value-of select="'Sedol'"/>
		  </Sedol>

		  <SecurityName>
			  <xsl:value-of select="'Security Name'"/>
		  </SecurityName>

		  <Account>
			  <xsl:value-of select="'Account'"/>
		  </Account>

		  <AssetClass>
			  <xsl:value-of select="'Asset Class'"/>
		  </AssetClass>

		  <PositionSide>
			  <xsl:value-of select="'Position Side'"/>
		  </PositionSide>

		  <Quantity>
			  <xsl:value-of select="'Quantity'"/>
		  </Quantity>

		  <Currency>
			  <xsl:value-of select="'Currency'"/>
		  </Currency>

		  <TotalCostLocal>
			  <xsl:value-of select="'Total Cost (Local)'"/>
		  </TotalCostLocal>

		  <MarkPriceLocal>
			  <xsl:value-of select="'Mark Price (Local)'"/>
		  </MarkPriceLocal>

		  <MarketValueLocal>
			  <xsl:value-of select="'Market Value (Local)'"/>
		  </MarketValueLocal>

		  <MarketValue>
			  <xsl:value-of select="'Market Value'"/>
		  </MarketValue>

		  <!-- <Accruals> -->
			 
			  <!-- <xsl:value-of select="'Accruals'"/> -->
		  <!-- </Accruals> -->

		  <PercentNavTotal>
			  <xsl:value-of select="'% NAV (Total)'"/>
		  </PercentNavTotal>

		  <NAV>
			  <xsl:value-of select="'NAV'"/>
		  </NAV>

		  <FxRates>
			  <xsl:value-of select="'Fx rates'"/>
		  </FxRates>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
           <xsl:for-each select="ThirdPartyFlatFileDetail[(AssetClass='Equity' or AssetClass='EquityOption')]">        
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
			
			<PositionDate>
              <xsl:value-of select="HoldingDate"/>
             </PositionDate>


			<Symbol>
				<xsl:value-of select="Symbol"/>
			</Symbol>

			<BloombergSymbol>
				<xsl:value-of select="BloombergSymbol"/>
			</BloombergSymbol>

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


			<SecurityName>
				<xsl:choose>
					<xsl:when test="SecurityName !='' and SecurityName !='*'">
						<xsl:value-of select="SecurityName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</SecurityName>

		

			<xsl:variable name="PB_NAME" select="'BNP'"/>
			<xsl:variable name = "PRANA_FUND_NAME">
				<xsl:value-of select="Account"/>
			</xsl:variable>

			<xsl:variable name ="THIRDPARTY_ACCOUNT_CODE">
				<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
			</xsl:variable>
			<xsl:variable name ="THIRDPARTY_FUND_CODE">
				<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
			</xsl:variable>
			<Account>
				<xsl:choose>
					<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
						<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="Account"/>
					</xsl:otherwise>
				</xsl:choose>
			</Account>

          <AssetClass>
            <xsl:value-of select="AssetClass"/>
          </AssetClass>

			<PositionSide>
				<xsl:value-of select="PositionSide"/>
			</PositionSide>

			<Quantity>
				<xsl:value-of select="Quantity"/>
			</Quantity>

			<Currency>
              <xsl:value-of select="TradeCurrency"/>
            </Currency>


			<TotalCostLocal>
			  <xsl:value-of select ="format-number(CostBasis_Local,'#0.0000')"/>
           </TotalCostLocal>

			<MarkPriceLocal>
            <xsl:value-of select="MarkPrice"/>
          </MarkPriceLocal>
			
			<MarketValueLocal>
				<xsl:value-of select ="format-number(MarketValue_Local,'#0.00')"/>
			</MarketValueLocal>


			<MarketValue>
				<xsl:value-of select ="format-number(MarketValue_Base,'#0.00')"/>
			</MarketValue>

			<!-- <Accruals> -->
				<!-- <xsl:choose> -->
					<!-- <xsl:when test="AssetClass='Accrual'"> -->
						<!-- <xsl:value-of select="Quantity"/> -->
					<!-- </xsl:when> -->
					<!-- <xsl:otherwise> -->
						<!-- <xsl:value-of select="0"/> -->
					<!-- </xsl:otherwise> -->
				<!-- </xsl:choose> -->
			<!-- </Accruals> -->

			<PercentNavTotal>
				<xsl:value-of select="format-number(PercentEquity,'#0.##')"/>
			</PercentNavTotal>

			<NAV>
				<xsl:value-of select="format-number(Nav,'#0.##')"/>
			</NAV>

			<FxRates>
				<xsl:value-of select="format-number(EndDateFXRate,'#0.####')"/>
			</FxRates>
			

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>