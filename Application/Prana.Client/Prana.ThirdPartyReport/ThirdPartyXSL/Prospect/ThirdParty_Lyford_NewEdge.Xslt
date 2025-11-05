<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <ClientOrderID>
          <xsl:value-of select="'Client trade reference Number'"/>
        </ClientOrderID>


        <TradeSeqNo>
          <xsl:value-of select="'Trade Sequence Number'"/>
        </TradeSeqNo>

        <AssetType>
          <xsl:value-of select="'Asset Type'"/>
        </AssetType>

        <TradeAction>
          <xsl:value-of select="'Trade Action'"/>
        </TradeAction>

        <ExecutionBroker>
          <xsl:value-of select="'Counter Party or Executing Broker'"/>
        </ExecutionBroker>

        <Account>
          <xsl:value-of select="'Account Number'"/>
        </Account>

        <AccountType>
          <xsl:value-of select="'Account Type'"/>
        </AccountType>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <QUIK>
          <xsl:value-of select="'QUIK'"/>
        </QUIK>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

		
        <Symbol>
          <xsl:value-of select="'TICKER'"/>
        </Symbol>

        <CurrencyTraded>
          <xsl:value-of select="'Local Currency Code'"/>
        </CurrencyTraded>

        <SettleCurrency>
          <xsl:value-of select="'Settlement Currency Code'"/>
        </SettleCurrency>

        <BASEFXrate>
          <xsl:value-of select="'Main FX rate'"/>
        </BASEFXrate>

        <PricingFactor>
          <xsl:value-of select="'Pricing Factor'"/>
        </PricingFactor>

        <Filler>
          <xsl:value-of select="'Filler'"/>
        </Filler>
        
        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>
        
        <AvgPx>
          <xsl:value-of select="'Price Local'"/>
        </AvgPx>

        <AccruedInterest>
          <xsl:value-of select="'Accrued Interest Local'"/>
        </AccruedInterest>


        <PrincipalLocal>
          <xsl:value-of select="'Principal Local'"/>
        </PrincipalLocal>

        <Commission>
          <xsl:value-of select="'Commission Local'"/>
        </Commission>

        <Fees>
          <xsl:value-of select="'Other Fees Local'"/>
        </Fees>

        <OtherLocalCharges>
          <xsl:value-of select="'Other Local Charges'"/>
        </OtherLocalCharges>

        <LeviesLocal>
          <xsl:value-of select="'Levies Local'"/>
        </LeviesLocal>

        <SecFeesLocal>
          <xsl:value-of select="'SEC Fees Local'"/>
        </SecFeesLocal>

        <NetAmount>
          <xsl:value-of select="'Net Proceeds Local'"/>
        </NetAmount>

        <SettlementPrice>
          <xsl:value-of select="'Settlement Price'"/>
        </SettlementPrice>

        <AccruedInterestSettlement>
          <xsl:value-of select="'Accrued Interest Settlement'"/>
        </AccruedInterestSettlement>


        <PrincipalSettlement>
          <xsl:value-of select="'Principal Settlement'"/>
        </PrincipalSettlement>

        <CommissionSettlement>
          <xsl:value-of select="'Commission Settlement'"/>
        </CommissionSettlement>


        <FeesSettlement>
          <xsl:value-of select="'Fees Settlement'"/>
        </FeesSettlement>


        <ChargesSettlement>
          <xsl:value-of select="'Charges Settlement'"/>
        </ChargesSettlement>

        <LeviesSettlement>
          <xsl:value-of select="'Levies Settlement'"/>
        </LeviesSettlement>

        <SecFeesSettlement>
          <xsl:value-of select="'SEC Fees Settlement'"/>
        </SecFeesSettlement>

        <NetProceedsSettlement>
          <xsl:value-of select="'Net Proceeds Settlement'"/>
        </NetProceedsSettlement>


        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

		  <OSI>
			  <xsl:value-of select="'OSIOptionSymbol'"/>
		  </OSI>

		  <StrikePrice>
			  <xsl:value-of select="'Strike Price'"/>
		  </StrikePrice>

		  <PutorCall>
			  <xsl:value-of select="'Put/Call'"/>
		  </PutorCall>

		  <ExpirationDate>
			  <xsl:value-of select="'Expiration Date'"/>
		  </ExpirationDate>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset = 'Future' or Asset= 'FutureOption']">
        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="true"/>
          </RowHeader>

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="true"/>
          </IsCaptionChangeRequired>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <ClientOrderID>
            <xsl:value-of select="TradeRefID"/>
          </ClientOrderID>

          <TradeSeqNo>
            <xsl:value-of select="''"/>
          </TradeSeqNo>

          <xsl:variable name="varAssetType">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'STOCK'"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'OPTION'"/>
              </xsl:when>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'FX'"/>
              </xsl:when>
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'FXFWD'"/>
              </xsl:when>
              <xsl:when test="Asset='Future'">
                <xsl:value-of select="'FUTURE'"/>
              </xsl:when>
              <xsl:when test="Asset='FutureOption'">
                <xsl:value-of select="'FutureOption'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <AssetType>
            <xsl:value-of select="$varAssetType"/>
          </AssetType>

          <!--Need To Confirm Here-->
          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Asset != 'Future'">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                    <xsl:value-of select="'SS'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'BC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Side"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close'or Side='Sell short' or Side='Sell to Open'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>



          <TradeAction>
            <xsl:value-of select="$varSide"/>
          </TradeAction>


          <ExecutionBroker>
            <xsl:value-of select="CounterParty"/>
          </ExecutionBroker>

          <Account>
            <xsl:value-of select="AccountNo"/>
          </Account>

          <AccountType>
            <xsl:choose>
              <xsl:when test ="(Side='Buy to Close' or Side='Sell short' or Side='Sell to Open') and Asset = 'Equity'">
                <xsl:value-of select="'SHORT'"/>
              </xsl:when>
              <xsl:when test ="(Side='Buy' or Side='Sell' or Side='Buy to Open' or Side='Sell to Close') and Asset = 'Equity'">
                <xsl:value-of select="'MARGIN'"/>
              </xsl:when>
              <xsl:when test ="Asset='EquityOption' or Asset='Future' or Asset='FutureOption'">
                <xsl:value-of select="'MARGIN'"/>
              </xsl:when>
              <xsl:when test="Asset='FX' or Asset='FXForward'">
                <xsl:value-of select="'CASH'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'MARGIN'"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountType>
          
          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <QUIK>
            <xsl:value-of select="''"/>
          </QUIK>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>



			<Symbol>
				<xsl:choose>
					<xsl:when test ="Asset='FX' or Asset='FXForward'">
						<xsl:value-of select="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="Symbol"/>
					</xsl:otherwise>
				</xsl:choose>
			</Symbol>

			<CurrencyTraded>
				<xsl:choose>
					<xsl:when test ="Asset='FX' or Asset='FXForward'">
						<xsl:value-of select="VsCurrencyName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="CurrencySymbol"/>
					</xsl:otherwise>
				</xsl:choose>
			</CurrencyTraded>

			<SettleCurrency>
				<xsl:choose>
					<xsl:when test ="Asset='FX' or Asset='FXForward'">
						<xsl:value-of select="LeadCurrencyName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="CurrencySymbol"/>
					</xsl:otherwise>
				</xsl:choose>
			</SettleCurrency>

			<BASEFXrate>
				<xsl:value-of select="ForexRate"/>
			</BASEFXrate>

			<PricingFactor>
				<xsl:value-of select="''"/>
			</PricingFactor>

			<Filler>
				<xsl:value-of select="''"/>
			</Filler>


			<Quantity>
				<xsl:value-of select="AllocatedQty"/>
			</Quantity>

			<AvgPx>
				<xsl:value-of select="AveragePrice"/>
			</AvgPx>

			<AccruedInterest>
				<xsl:value-of select="0"/>
			</AccruedInterest>

			<PrincipalLocal>
				<xsl:value-of select="GrossAmount"/>
			</PrincipalLocal>


			<Commission>
				<xsl:value-of select="CommissionCharged"/>
			</Commission>

			<Fees>
				<xsl:value-of select="OtherBrokerFee"/>
			</Fees>

			<OtherLocalCharges>
				<xsl:value-of select="TaxOnCommissions + StampDuty + ClearingFee"/>
			</OtherLocalCharges>

			<LeviesLocal>
				<xsl:value-of select="TransactionLevy"/>
			</LeviesLocal>

			<SecFeesLocal>
				<xsl:value-of select="MiscFees"/>
			</SecFeesLocal>

			<NetAmount>
				<xsl:value-of select="NetAmount"/>
			</NetAmount>

			<SettlementPrice>
				<xsl:value-of select="''"/>
			</SettlementPrice>

			<AccruedInterestSettlement>
				<xsl:value-of select="0"/>
			</AccruedInterestSettlement>

			<PrincipalSettlement>
				<xsl:value-of select="GrossAmount"/>
			</PrincipalSettlement>

			<CommissionSettlement>
				<xsl:value-of select="CommissionCharged"/>
			</CommissionSettlement>


			<FeesSettlement>
				<xsl:value-of select="OtherBrokerFee"/>
			</FeesSettlement>

			<ChargesSettlement>
				<xsl:value-of select="TaxOnCommissions + StampDuty + ClearingFee"/>
			</ChargesSettlement>

			<LeviesSettlement>
				<xsl:value-of select="TransactionLevy"/>
			</LeviesSettlement>

			<SecFeesSettlement>
				<xsl:value-of select="MiscFees"/>
			</SecFeesSettlement>

			<NetProceedsSettlement>
				<xsl:value-of select="NetAmount"/>
			</NetProceedsSettlement>

			<TradeDate>
				<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
			</TradeDate>

			<SettlementDate>
				<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
			</SettlementDate>


			<OSI>
				<xsl:value-of select="OSIOptionSymbol"/>
			</OSI>
			
			<StrikePrice>
				<xsl:value-of select="StrikePrice"/>
			</StrikePrice>

			<PutorCall>
				<xsl:value-of select="PutOrCall"/>
			</PutorCall>

			<ExpirationDate>
				<xsl:choose>
					<xsl:when test="Asset = 'Future' or Asset = 'FutureOption'">
						<xsl:value-of select="substring(substring-after(Symbol,' '),1,2)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
					</xsl:otherwise>
				</xsl:choose>
			</ExpirationDate>
          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
