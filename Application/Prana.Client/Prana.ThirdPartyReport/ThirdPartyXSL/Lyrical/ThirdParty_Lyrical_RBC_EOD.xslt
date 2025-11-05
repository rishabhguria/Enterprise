<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

     

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

        
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <xsl:variable name="PB_NAME" select="'RBC'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>


          <MessageType>
            <xsl:value-of select="'TSISIN'"/>
          </MessageType>

          <MessageTSIType>
            <xsl:choose>              
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'RVP'"/>
                  </xsl:when>                                  
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'DVP'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>             
          </MessageTSIType>

          <MessageTSIFunction>
            <xsl:value-of select="'NEWM'"/>
          </MessageTSIFunction>

          <MessageVersion>
            <xsl:value-of select="'1'"/>
          </MessageVersion>

          <MessageStatus>
            <xsl:value-of select="'AP'"/>
          </MessageStatus>

          <ValidateYN>
            <xsl:value-of select="''"/>
          </ValidateYN>

          <CustodyAccountNumber>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </CustodyAccountNumber>

          <CustodyAccountDescription>
            <xsl:value-of select="''"/>
          </CustodyAccountDescription>


          <xsl:variable name="varTradeDate">
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </xsl:variable>
          <xsl:variable name="i" select="position()" />
         
            

          <ClientReference>
            <xsl:choose>
              <xsl:when test="$i &lt; 10">
                <xsl:value-of select="concat($varTradeDate,'0',$i)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="concat($varTradeDate,$i)"/>
              </xsl:otherwise>
            </xsl:choose>
          </ClientReference>

          <PreviousClientReference>
            <xsl:value-of select="''"/>
          </PreviousClientReference>

          <RelatedReference>
            <xsl:value-of select="''"/>
          </RelatedReference>

          <TradeReference>
            <xsl:value-of select="''"/>
          </TradeReference>

          <CommonReference>
            <xsl:value-of select="''"/>
          </CommonReference>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <PlaceOfSettlement>
            <xsl:value-of select="'DTC'"/>
          </PlaceOfSettlement>

          <AssetIDType>
            <xsl:value-of select="'ISIN'"/>
          </AssetIDType>

          <AssetId>
            <xsl:value-of select="ISIN"/>
          </AssetId>

          <AssetDescriptionLine1>
            <xsl:value-of select="''"/>
          </AssetDescriptionLine1>

          <AssetDescriptionLine2>
            <xsl:value-of select="''"/>
          </AssetDescriptionLine2>

          <AssetDescriptionLine3>
            <xsl:value-of select="''"/>
          </AssetDescriptionLine3>

          <AssetType>
            <xsl:value-of select="'EQUITIES'"/>
          </AssetType>

          <Units>
            <xsl:value-of select="AllocatedQty"/>
          </Units>

          <InterestRate>
            <xsl:value-of select="''"/>
          </InterestRate>

          <Yield>
            <xsl:value-of select="''"/>
          </Yield>

          <MaturityDate>
            <xsl:value-of select="''"/>
          </MaturityDate>

          <SettlementCurrencyCode>
            <xsl:value-of select="'USD'"/>
          </SettlementCurrencyCode>

          <TradeCurrency>
            <xsl:value-of select="'USD'"/>
          </TradeCurrency>

          <SettlementAmount>
            <xsl:value-of select="format-number(NetAmount,'##.##')"/>
          </SettlementAmount>

          <UnitPrice>
            <xsl:value-of select="format-number(AveragePrice,'##.####')"/>
          </UnitPrice>

          <GrossSettlementAmount>
            <xsl:value-of select="''"/>
          </GrossSettlementAmount>

          <Commission>
			  <xsl:choose>
				  <xsl:when test="number(CommissionCharged)">
					  <xsl:value-of select="format-number(CommissionCharged,'##.##')"/>
				  </xsl:when>				 
				  <xsl:otherwise>
					  <xsl:value-of select="0"/>
				  </xsl:otherwise>
			  </xsl:choose>			 
          </Commission>

          <MiscCharges>
            <xsl:value-of select="''"/>
          </MiscCharges>

          <AccruedInterest>
            <xsl:value-of select="''"/>
          </AccruedInterest>

          <StampDuty>
            <xsl:value-of select="''"/>
          </StampDuty>

          <Taxes>
            <xsl:value-of select="''"/>
          </Taxes>

          <FeeAmount>
			  <xsl:choose>
				  <xsl:when test="number(SecFee)">
					  <xsl:value-of select="format-number(SecFee,'##.##')"/>
				  </xsl:when>

				  <xsl:when test="number(StampDuty)">
					  <xsl:value-of select="format-number(StampDuty,'##.##')"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="0"/>
				  </xsl:otherwise>
			  </xsl:choose>			             
          </FeeAmount>

          <BrokerClearingAgentIDType>
            <xsl:value-of select="'DTC'"/>
          </BrokerClearingAgentIDType>

          <BrokerClearingAgentCode>
            <xsl:choose>
              <xsl:when test="CounterParty='JPM'">
                <xsl:value-of select="'352'"/>
              </xsl:when>
              <xsl:when test="CounterParty='BGCE'">
                <xsl:value-of select="'161'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMHI'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMBC'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
            </xsl:choose>
          </BrokerClearingAgentCode>

          <BrokerClearingAgentLine1>
            <xsl:value-of select="''"/>
          </BrokerClearingAgentLine1>

          <BrokerClearingAgentLine2>
            <xsl:value-of select="''"/>
          </BrokerClearingAgentLine2>

          <BrokerClearingAgentLine3>
            <xsl:value-of select="''"/>
          </BrokerClearingAgentLine3>

          <AccountNumberAtPSET>
            <xsl:value-of select="''"/>
          </AccountNumberAtPSET>

          <ExecutingBrokerIDType>
            <xsl:value-of select="'DTC'"/>
          </ExecutingBrokerIDType>

          <ExecutingBrokerCode>
            <xsl:choose>
              <xsl:when test="CounterParty='JPM'">
                <xsl:value-of select="'352'"/>
              </xsl:when>
              <xsl:when test="CounterParty='BGCE'">
                <xsl:value-of select="'161'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMHI'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMBC'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
            </xsl:choose>
           
          </ExecutingBrokerCode>

          <ExecutingBrokerLine1>
            <xsl:value-of select="''"/>
          </ExecutingBrokerLine1>

          <ExecutingBrokerLine2>
            <xsl:value-of select="''"/>
          </ExecutingBrokerLine2>

          <ExecutingBrokerLine3>
            <xsl:value-of select="''"/>
          </ExecutingBrokerLine3>

          <BeneficiaryAccountNumber>
            <xsl:value-of select="''"/>
          </BeneficiaryAccountNumber>

          <ExchangeRate>
            <xsl:value-of select="''"/>
          </ExchangeRate>

          <FXCurrency>
            <xsl:value-of select="''"/>
          </FXCurrency>

          <FXInstructionsLine1>
            <xsl:value-of select="''"/>
          </FXInstructionsLine1>

          <FXInstructionsLine2>
            <xsl:value-of select="''"/>
          </FXInstructionsLine2>

          <FXInstructionsLine3>
            <xsl:value-of select="''"/>
          </FXInstructionsLine3>

          <FXInstructionsLine4>
            <xsl:value-of select="''"/>
          </FXInstructionsLine4>

          <FXInstructionsLine5>
            <xsl:value-of select="''"/>
          </FXInstructionsLine5>

          <FXInstructionsLine6>
            <xsl:value-of select="''"/>
          </FXInstructionsLine6>

          <SpecialInstructionsLine1>
            <xsl:value-of select="''"/>
          </SpecialInstructionsLine1>

          <SpecialInstructionsLine2>
            <xsl:value-of select="''"/>
          </SpecialInstructionsLine2>

          <ChangeinBeneficialOwnership>
            <xsl:value-of select="''"/>
          </ChangeinBeneficialOwnership>

          <StampDutycountryidentifier>
            <xsl:value-of select="''"/>
          </StampDutycountryidentifier>

          <StampDutyCode>
            <xsl:value-of select="''"/>
          </StampDutyCode>

          <CharityID>
            <xsl:value-of select="''"/>
          </CharityID>

          <CharityCountryCode>
            <xsl:value-of select="''"/>
          </CharityCountryCode>

          <NationalityDeclarationISOCountryCode>
            <xsl:value-of select="''"/>
          </NationalityDeclarationISOCountryCode>

          <AccountTransfer>
            <xsl:value-of select="''"/>
          </AccountTransfer>

          <AmortizedFaceValue>
            <xsl:value-of select="''"/>
          </AmortizedFaceValue>

          <PoolFactor>
            <xsl:value-of select="''"/>
          </PoolFactor>

          <PSETCountryCode>
            <xsl:value-of select="''"/>
          </PSETCountryCode>

          <PhysicalIndicator>
            <xsl:value-of select="''"/>
          </PhysicalIndicator>

          <TransactionNetAmount>
            <xsl:value-of select="''"/>
          </TransactionNetAmount>

          <LinkageType>
            <xsl:value-of select="''"/>
          </LinkageType>

          <RecuDecuBrokerID>
            <xsl:value-of select="''"/>
          </RecuDecuBrokerID>

          <RecuDecuBrokerCode>
            <xsl:value-of select="''"/>
          </RecuDecuBrokerCode>

          <RecuDecuSafekeepingAccount>
            <xsl:value-of select="''"/>
          </RecuDecuSafekeepingAccount>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>