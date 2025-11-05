<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template name="DateFormatTrans">
    <xsl:param name="TDate"/>
    <xsl:value-of select="concat(substring-before($TDate,'-'),substring-before(substring-after($TDate,'-'),'-'),substring-after(substring-after($TDate,'-'),'-'))"/>
  </xsl:template>
  
  
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
		  <ThirdPartyFlatFileDetail>

		    <RowHeader>
             <xsl:value-of select ="'false'"/>
           </RowHeader>
          
           <TaxLotState>
             <xsl:value-of select ="TaxLotState"/>
           </TaxLotState>

        <TransactionType>
        <xsl:value-of select ="'TransactionType'"/>
        </TransactionType>
        
        <AccountNumber>
        <xsl:value-of select ="'AccountNumber'"/>
        </AccountNumber>
        
        <TransactionReferenceNumber>
        <xsl:value-of select ="'TransactionReferenceNumber'"/>
        </TransactionReferenceNumber>
        
        <SenderID>
        <xsl:value-of select ="'SenderID'"/>
        </SenderID>
        
        <RelatedReferenceNumber>
        <xsl:value-of select ="'RelatedReferenceNumber'"/>
        </RelatedReferenceNumber>
        
        <TradeDate>
        <xsl:value-of select ="'TradeDate'"/>
        </TradeDate>
        
        <SettlementDate>
        <xsl:value-of select ="'SettlementDate'"/>
        </SettlementDate>
        
        <FX>
        <xsl:value-of select ="'FX'"/>
        </FX>
        
        <ContraCCY>
        <xsl:value-of select ="'ContraCCY'"/>
        </ContraCCY>
        
        <SecurityIDType>
        <xsl:value-of select ="'SecurityIDType'"/>
        </SecurityIDType>
        
        <SecurityID>
        <xsl:value-of select ="'SecurityID'"/>
        </SecurityID>
        
        <SecurityDescription>
        <xsl:value-of select ="'SecurityDescription'"/>
        </SecurityDescription>
        
        <AssetClass>
        <xsl:value-of select ="'AssetClass'"/>
        </AssetClass>
        
        <MaturityDate>
        <xsl:value-of select ="'MaturityDate'"/>
        </MaturityDate>
        
        <InterestRate>
        <xsl:value-of select ="'InterestRate'"/>
        </InterestRate>
        
        <IssueDate>
        <xsl:value-of select ="'IssueDate'"/>
        </IssueDate>
        
        <DealPrice>
        <xsl:value-of select ="'DealPrice'"/>
        </DealPrice>
        
        <DealPriceType>
        <xsl:value-of select ="'DealPriceType'"/>
        </DealPriceType>
        
        <PlaceOfTrade>
        <xsl:value-of select ="'PlaceOfTrade'"/>
        </PlaceOfTrade>
        
        <TransactionDescription>
        <xsl:value-of select ="'TransactionDescription'"/>
        </TransactionDescription>
        
        <DelayedDate>
        <xsl:value-of select ="'DelayedDate'"/>
        </DelayedDate>
        
        <CapitalGainAverageDate>
        <xsl:value-of select ="'CapitalGainAverageDate'"/>
        </CapitalGainAverageDate>
        
        <CapitalGainAveragePrice>
        <xsl:value-of select ="'CapitalGainAveragePrice'"/>
        </CapitalGainAveragePrice>
        
        <CapitalGainPriceCategory>
        <xsl:value-of select ="'CapitalGainPriceCategory'"/>
        </CapitalGainPriceCategory>
        
        <Quantity>
        <xsl:value-of select ="'Quantity'"/>
        </Quantity>
        
        <AmortizedAmount>
        <xsl:value-of select ="'AmortizedAmount'"/>
        </AmortizedAmount>
        
        <SettlementCurrency>
        <xsl:value-of select ="'SettlementCurrency'"/>
        </SettlementCurrency>
        
        <SettlementAmount>
        <xsl:value-of select ="'SettlementAmount'"/>
        </SettlementAmount>
        
        <PrincipalAmount>
        <xsl:value-of select ="'PrincipalAmount'"/>
        </PrincipalAmount>
        
        <AccruedInterestType>
        <xsl:value-of select ="'AccruedInterestType'"/>
        </AccruedInterestType>
        
        <AccruedInterestAmount>
        <xsl:value-of select ="'AccruedInterestAmount'"/>
        </AccruedInterestAmount>
        
        <TaxesType>
        <xsl:value-of select ="'TaxesType'"/>
        </TaxesType>
        
        <TaxesAmount>
        <xsl:value-of select ="'TaxesAmount'"/>
        </TaxesAmount>
        
        <CommissionsType>
        <xsl:value-of select ="'CommissionsType'"/>
        </CommissionsType>
        
        <CommissionsAmount>
        <xsl:value-of select ="'CommissionsAmount'"/>
        </CommissionsAmount>
        
        <ChargesFees1Type>
        <xsl:value-of select ="'Charges/Fees1Type'"/>
        </ChargesFees1Type>
        
        <ChargesFees1Amount>
        <xsl:value-of select ="'Charges/Fees1Amount'"/>
        </ChargesFees1Amount>
        
        <ChargesFees2Type>
        <xsl:value-of select ="'Charges/Fees2Type'"/>
        </ChargesFees2Type>
        
        <ChargesFees2Amount>
        <xsl:value-of select ="'Charges/Fees2Amount'"/>
        </ChargesFees2Amount>
        
        <ChargesFees3Type>
        <xsl:value-of select ="'Charges/Fees3Type'"/>
        </ChargesFees3Type>
        
        <ChargesFees3Amount>
        <xsl:value-of select ="'Charges/Fees3Amount'"/>
        </ChargesFees3Amount>
        
        <ChargesFees4Type>
        <xsl:value-of select ="'Charges/Fees4Type'"/>
        </ChargesFees4Type>
        
        <ChargesFees4Amount>
        <xsl:value-of select ="'Charges/Fees4Amount'"/>
        </ChargesFees4Amount>
        
        <NetMovementType>
        <xsl:value-of select ="'NetMovementType'"/>
        </NetMovementType>
        
        <NetMovementAmount>
        <xsl:value-of select ="'NetMovementAmount'"/>
        </NetMovementAmount>
        
        <SSI>
        <xsl:value-of select ="'SSI'"/>
        </SSI>
        
        <TransactionSubType>
        <xsl:value-of select ="'TransactionSubType'"/>
        </TransactionSubType>
        
        <SettlementTransactionConditionIndicator>
        <xsl:value-of select ="'SettlementTransactionConditionIndicator'"/>
        </SettlementTransactionConditionIndicator>
        
        <PlaceOfSettlementType>
        <xsl:value-of select ="'PlaceOfSettlementType'"/>
        </PlaceOfSettlementType>
        
        <PlaceOfSettlement>
        <xsl:value-of select ="'PlaceOfSettlement'"/>
        </PlaceOfSettlement>
        
        <BuyerSellerIDType>
        <xsl:value-of select ="'BuyerSellerIDType'"/>
        </BuyerSellerIDType>
        
        <BuyerSellerID>
        <xsl:value-of select ="'BuyerSellerID'"/>
        </BuyerSellerID>
        
        <BuyerSellerName>
        <xsl:value-of select ="'BuyerSellerName'"/>
        </BuyerSellerName>
        
        <BuyerSellerNameAddress>
        <xsl:value-of select ="'BuyerSellerNameAddress'"/>
        </BuyerSellerNameAddress>
        
        <BuyerSellerAcct>
        <xsl:value-of select ="'BuyerSellerAcct'"/>
        </BuyerSellerAcct>
        
        <RecDelAgentIDType>
        <xsl:value-of select ="'RecDelAgentIDType'"/>
        </RecDelAgentIDType>
        
        <RecDelAgentID>
        <xsl:value-of select ="'RecDelAgentID'"/>
        </RecDelAgentID>
        
        <RecDelAgentName>
        <xsl:value-of select ="'RecDelAgentName'"/>
        </RecDelAgentName>
        
        <RecDelAgentNameAddress>
        <xsl:value-of select ="'RecDelAgentNameAddress'"/>
        </RecDelAgentNameAddress>
        
        <RecDelAgentAcct>
        <xsl:value-of select ="'RecDelAgentAcct'"/>
        </RecDelAgentAcct>
        
        <IntermediaryIDType>
        <xsl:value-of select ="'IntermediaryIDType'"/>
        </IntermediaryIDType>
        
        <IntermediaryID>
        <xsl:value-of select ="'IntermediaryID'"/>
        </IntermediaryID>
        
        <IntermediaryName>
        <xsl:value-of select ="'IntermediaryName'"/>
        </IntermediaryName>
        
        <IntermediaryNameAddress>
        <xsl:value-of select ="'IntermediaryNameAddress'"/>
        </IntermediaryNameAddress>
        
        <IntermediaryAcct>
        <xsl:value-of select ="'IntermediaryAcct'"/>
        </IntermediaryAcct>
        
        <CashRecDelAgentIDType>
        <xsl:value-of select ="'CashRecDelAgentIDType'"/>
        </CashRecDelAgentIDType>
        
        <CashRecDelAgentID>
        <xsl:value-of select ="'CashRecDelAgentID'"/>
        </CashRecDelAgentID>
        
        <CashBeneficiaryID>
        <xsl:value-of select ="'CashBeneficiaryID'"/>
        </CashBeneficiaryID>
        
        <CashBeneficiaryAcct>
        <xsl:value-of select ="'CashBeneficiaryAcct'"/>
        </CashBeneficiaryAcct>
        
        <PlaceOfSafekeepingType>
        <xsl:value-of select ="'PlaceOfSafekeepingType'"/>
        </PlaceOfSafekeepingType>
        
        <PlaceOfSafekeeping>
        <xsl:value-of select ="'PlaceOfSafekeeping'"/>
        </PlaceOfSafekeeping>
        
        <CBO>
        <xsl:value-of select ="'CBO'"/>
        </CBO>
        
        <NameAndAddress>
        <xsl:value-of select ="'NameAndAddress'"/>
        </NameAndAddress>
        
        <SettlementInformation>
        <xsl:value-of select ="'SettlementInformation'"/>
        </SettlementInformation>
        
        <TaxID>
        <xsl:value-of select ="'TaxID'"/>
        </TaxID>
        
        <StampDutyExemption>
        <xsl:value-of select ="'StampDutyExemption'"/>
        </StampDutyExemption>
        
        <PriorityIndicator>
        <xsl:value-of select ="'PriorityIndicator'"/>
        </PriorityIndicator>
        
        <TradeTransactionConditionIndicator>
        <xsl:value-of select ="'TradeTransactionConditionIndicator'"/>
        </TradeTransactionConditionIndicator>
        
        <CurrentSettlementTransactionNumber>
        <xsl:value-of select ="'CurrentSettlementTransactionNumber'"/>
        </CurrentSettlementTransactionNumber>
        
        <TotalOfLinkedSettlementInstructions>
        <xsl:value-of select ="'TotalOfLinkedSettlementInstructions'"/>
        </TotalOfLinkedSettlementInstructions>
        
        <PoolReferenceNumber>
        <xsl:value-of select ="'PoolReferenceNumber'"/>
        </PoolReferenceNumber>
        
        <LinkageTypeIndicator>
        <xsl:value-of select ="'LinkageTypeIndicator'"/>
        </LinkageTypeIndicator>
        
        <CommonReference>
        <xsl:value-of select ="'CommonReference'"/>
        </CommonReference>
        
        <MessageFunction>
        <xsl:value-of select ="'MessageFunction'"/>
        </MessageFunction>
        
        <NewSecurityCancel>
        <xsl:value-of select ="'NewSecurityCancel'"/>
        </NewSecurityCancel>
        
        <CancelorRetainOriginalFX>
        <xsl:value-of select ="'CancelorRetainOriginalFX'"/>
        </CancelorRetainOriginalFX>
        
        <CurrencyofDenomination>
        <xsl:value-of select ="'CurrencyofDenomination'"/>
        </CurrencyofDenomination>
        
        <OptionStyle>
        <xsl:value-of select ="'OptionStyle'"/>
        </OptionStyle>
        
        <OptionType>
        <xsl:value-of select ="'OptionType'"/>
        </OptionType>
        
        <ContractSize>
        <xsl:value-of select ="'ContractSize'"/>
        </ContractSize>
        
        <ExercisePrice>
        <xsl:value-of select ="'ExercisePrice'"/>
        </ExercisePrice>
        
        <ExpirationDate>
        <xsl:value-of select ="'ExpirationDate'"/>
        </ExpirationDate>
        
        <UnderlyingSecurityIDType>
        <xsl:value-of select ="'UnderlyingSecurityIDType'"/>
        </UnderlyingSecurityIDType>
        
        <UnderlyingSecurityID>
        <xsl:value-of select ="'UnderlyingSecurityID'"/>
        </UnderlyingSecurityID>
        
        <UnderlyingSecurityDescription>
        <xsl:value-of select ="'UnderlyingSecurityDescription'"/>
        </UnderlyingSecurityDescription>
        
        <OpenCloseIndicator>
        <xsl:value-of select ="'Open/CloseIndicator'"/>
        </OpenCloseIndicator>
			  <ProcessingGroup>
				  <xsl:value-of select ="'Processing Group'"/> 
			  </ProcessingGroup>
			     <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

  </ThirdPartyFlatFileDetail>


      <xsl:for-each select="ThirdPartyFlatFileDetail">
        
        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
         	  <ThirdPartyFlatFileDetail>

		    <RowHeader>
             <xsl:value-of select ="'false'"/>
           </RowHeader>
          
           <TaxLotState>
             <xsl:value-of select ="TaxLotState"/>
           </TaxLotState>

        <TransactionType>
        <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side = 'Buy to Close'">
                    <xsl:value-of select="'DVP'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side = 'Sell short' or Side = 'Sell to Open'">
                    <xsl:value-of select="'RVP'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
        </TransactionType>
				    <xsl:variable name="PB_NAME">
                <xsl:value-of select="'SSBS'"/>
              </xsl:variable>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <AccountNumber>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="FundAccntNo"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AccountNumber>
            
         <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              

              <xsl:variable name="varTransactionReferenceDate">
                <xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
              </xsl:variable>
        
        <TransactionReferenceNumber>
           <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position(),'C')"/>
        </TransactionReferenceNumber>
        
        <SenderID>
        <xsl:value-of select ="''"/>
        </SenderID>
        
        <RelatedReferenceNumber>
        <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position())"/>
        </RelatedReferenceNumber>
        
        <TradeDate>
        <xsl:value-of select ="$varTradeDate"/>
        </TradeDate>
           <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              
        <SettlementDate>
        <xsl:value-of select ="$varSettlementDate"/>
        </SettlementDate>
        
        <FX>
        <xsl:value-of select ="'N'"/>
        </FX>
        
        <ContraCCY>
        <xsl:value-of select ="''"/>
        </ContraCCY>
        
        <SecurityIDType>
        <xsl:value-of select ="'SEDOL'"/>
        </SecurityIDType>
        
        <SecurityID>
      <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
        </SecurityID>
        
        <SecurityDescription>
        <xsl:value-of select ="CompanyName"/>
        </SecurityDescription>
        
        <AssetClass>
        <xsl:value-of select ="Asset"/>
        </AssetClass>
        
        <MaturityDate>
        <xsl:value-of select ="''"/>
        </MaturityDate>
        
        <InterestRate>
        <xsl:value-of select ="''"/>
        </InterestRate>
        
        <IssueDate>
        <xsl:value-of select ="''"/>
        </IssueDate>
        
        <DealPrice>
         <xsl:value-of select="format-number(AvgPrice,'#.000000')"/>
        </DealPrice>
        
        <DealPriceType>
        <xsl:value-of select ="''"/>
        </DealPriceType>
        
        <PlaceOfTrade>
        <xsl:value-of select ="''"/>
        </PlaceOfTrade>
        
        <TransactionDescription>
        <xsl:value-of select ="''"/>
        </TransactionDescription>
        
        <DelayedDate>
        <xsl:value-of select ="''"/>
        </DelayedDate>
        
        <CapitalGainAverageDate>
        <xsl:value-of select ="''"/>
        </CapitalGainAverageDate>
        
        <CapitalGainAveragePrice>
        <xsl:value-of select ="''"/>
        </CapitalGainAveragePrice>
        
        <CapitalGainPriceCategory>
        <xsl:value-of select ="''"/>
        </CapitalGainPriceCategory>
        
        <Quantity>
         <xsl:choose>
                  <xsl:when test="number(OrderQty)">
                    <xsl:value-of select="format-number(OrderQty,'#.00')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
        </Quantity>
        
        <AmortizedAmount>
        <xsl:value-of select ="''"/>
        </AmortizedAmount>
        
        <SettlementCurrency>
        <xsl:value-of select ="SettlCurrency"/>
        </SettlementCurrency>
        
        <SettlementAmount>
        <xsl:value-of select ="$varNetamount"/>
        </SettlementAmount>
         <xsl:variable name="varAssetMultiplier">
                <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
              </xsl:variable>
        <PrincipalAmount>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY'">
                    <xsl:value-of select="round($varAssetMultiplier)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
                  </xsl:otherwise>
                </xsl:choose>
        </PrincipalAmount>
        
        <AccruedInterestType>
        <xsl:value-of select ="''"/>
        </AccruedInterestType>
        
        <AccruedInterestAmount>
        <xsl:value-of select ="''"/>
        </AccruedInterestAmount>
        
      	 <TaxesType>
			<xsl:choose>
					<xsl:when test="StampDuty = 0">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="StampDuty != '' or StampDuty != 0">
						<xsl:value-of select="'STAM - Stamp Duty'"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
						</xsl:choose>
				  </TaxesType>

				  <TaxesAmount>
					  <xsl:value-of select ="StampDuty"/>
				  </TaxesAmount>

        
        <CommissionsType>
        <xsl:value-of select ="'LOCO - Local Brokers Commission'"/>
        </CommissionsType>
        
        <CommissionsAmount>
     <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.##')"/>
        </CommissionsAmount>
        
        <ChargesFees1Type>
        <xsl:value-of select ="''"/>
        </ChargesFees1Type>
        
        <ChargesFees1Amount>
        <xsl:value-of select ="''"/>
        </ChargesFees1Amount>
        
        <ChargesFees2Type>
        <xsl:value-of select ="''"/>
        </ChargesFees2Type>
        
        <ChargesFees2Amount>
        <xsl:value-of select ="''"/>
        </ChargesFees2Amount>
        
        <ChargesFees3Type>
        <xsl:value-of select ="''"/>
        </ChargesFees3Type>
        
        <ChargesFees3Amount>
        <xsl:value-of select ="''"/>
        </ChargesFees3Amount>
        
        <ChargesFees4Type>
        <xsl:value-of select ="''"/>
        </ChargesFees4Type>
        
        <ChargesFees4Amount>
        <xsl:value-of select ="''"/>
        </ChargesFees4Amount>
        
        <NetMovementType>
        <xsl:value-of select ="''"/>
        </NetMovementType>
        
        <NetMovementAmount>
        <xsl:value-of select ="''"/>
        </NetMovementAmount>
        
        <SSI>
        <xsl:value-of select ="''"/>
        </SSI>
		 <xsl:variable name="varCurrencySymbol">
		  <xsl:value-of select="CurrencySymbol"/>
		 </xsl:variable>
		 <xsl:variable name="varCountry">
		  <xsl:value-of select="UDACountryName"/>
		 </xsl:variable>

		 <TransactionSubType>
		  <xsl:value-of select="'TRAD - Trade'"/>
		 </TransactionSubType>
        
        <SettlementTransactionConditionIndicator>
        <xsl:value-of select ="''"/>
        </SettlementTransactionConditionIndicator>

				  <xsl:variable name="TP_PlaceOfSettlementType">
					  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol][@CountryName = $varCountry]/@PlaceOfSettlementType"/>
				  </xsl:variable>
        <PlaceOfSettlementType>
			<xsl:choose>
				<xsl:when test="$TP_PlaceOfSettlementType != ''">
					<xsl:value-of select="$TP_PlaceOfSettlementType"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
        </PlaceOfSettlementType>

		  <xsl:variable name="TP_PlaceOfSettlement">
			  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@PlaceofSettlement"/>
		  </xsl:variable>
        <PlaceOfSettlement>
			<xsl:choose>
				<xsl:when test="$TP_PlaceOfSettlement != ''">
					<xsl:value-of select="$TP_PlaceOfSettlement"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
        </PlaceOfSettlement>

				  <xsl:variable name="TP_BuyerSellerIDType">
					  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@BuyerSellerIDType"/>
				  </xsl:variable>
        <BuyerSellerIDType>
			<xsl:choose>
				<xsl:when test="$TP_BuyerSellerIDType != ''">
					<xsl:value-of select="$TP_BuyerSellerIDType"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
        </BuyerSellerIDType>
        
       <xsl:variable name="TP_BuyerSellerID">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@BuyerSellerID"/>
							</xsl:variable>
							<BuyerSellerID>
								<xsl:choose>
									<xsl:when test="$TP_BuyerSellerID != ''">
										<xsl:value-of select="$TP_BuyerSellerID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</BuyerSellerID>
        
        <BuyerSellerName>
        <xsl:value-of select ="''"/>
        </BuyerSellerName>
        
        <BuyerSellerNameAddress>
        <xsl:value-of select ="''"/>
        </BuyerSellerNameAddress>
        
        <BuyerSellerAcct>
        <xsl:value-of select ="''"/>
        </BuyerSellerAcct>
				  
			 <xsl:variable name="TP_RecDelAgentIDType">
			  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@RecDelAgentIDType"/>
			 </xsl:variable>
        <RecDelAgentIDType>
			<xsl:choose>
				<xsl:when test="$TP_RecDelAgentIDType != ''">
					<xsl:value-of select="$TP_RecDelAgentIDType"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
        </RecDelAgentIDType>
        
       	<xsl:variable name="TP_RecDelAgentID">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@RecDelAgentID"/>
							</xsl:variable>
							<RecDelAgentID>
								<xsl:choose>
									<xsl:when test="$TP_RecDelAgentID != ''">
										<xsl:value-of select="$TP_RecDelAgentID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</RecDelAgentID>
        
        <RecDelAgentName>
        <xsl:value-of select ="''"/>
        </RecDelAgentName>
        
        <RecDelAgentNameAddress>
        <xsl:value-of select ="''"/>
        </RecDelAgentNameAddress>
        
        <RecDelAgentAcct>
        <xsl:value-of select ="''"/>
        </RecDelAgentAcct>
        
        <IntermediaryIDType>
        <xsl:value-of select ="''"/>
        </IntermediaryIDType>
        
        <IntermediaryID>
        <xsl:value-of select ="''"/>
        </IntermediaryID>
        
        <IntermediaryName>
        <xsl:value-of select ="''"/>
        </IntermediaryName>
        
        <IntermediaryNameAddress>
        <xsl:value-of select ="''"/>
        </IntermediaryNameAddress>
        
        <IntermediaryAcct>
        <xsl:value-of select ="''"/>
        </IntermediaryAcct>
        
        <CashRecDelAgentIDType>
        <xsl:value-of select ="''"/>
        </CashRecDelAgentIDType>
        
        <CashRecDelAgentID>
        <xsl:value-of select ="''"/>
        </CashRecDelAgentID>
        
        <CashBeneficiaryID>
        <xsl:value-of select ="''"/>
        </CashBeneficiaryID>
        
        <CashBeneficiaryAcct>
        <xsl:value-of select ="''"/>
        </CashBeneficiaryAcct>
        
        <PlaceOfSafekeepingType>
        <xsl:value-of select ="''"/>
        </PlaceOfSafekeepingType>
        
        <PlaceOfSafekeeping>
        <xsl:value-of select ="''"/>
        </PlaceOfSafekeeping>
        
        <CBO>
        <xsl:value-of select ="''"/>
        </CBO>
        
        <NameAndAddress>
        <xsl:value-of select ="''"/>
        </NameAndAddress>
        
        <SettlementInformation>
        <xsl:value-of select ="''"/>
        </SettlementInformation>
        
        <TaxID>
        <xsl:value-of select ="''"/>
        </TaxID>
        
        <StampDutyExemption>
        <xsl:value-of select ="''"/>
        </StampDutyExemption>
        
        <PriorityIndicator>
        <xsl:value-of select ="''"/>
        </PriorityIndicator>
        
        <TradeTransactionConditionIndicator>
        <xsl:value-of select ="''"/>
        </TradeTransactionConditionIndicator>
        
        <CurrentSettlementTransactionNumber>
        <xsl:value-of select ="''"/>
        </CurrentSettlementTransactionNumber>
        
        <TotalOfLinkedSettlementInstructions>
        <xsl:value-of select ="''"/>
        </TotalOfLinkedSettlementInstructions>
        
        <PoolReferenceNumber>
        <xsl:value-of select ="''"/>
        </PoolReferenceNumber>
        
        <LinkageTypeIndicator>
        <xsl:value-of select ="''"/>
        </LinkageTypeIndicator>
        
        <CommonReference>
        <xsl:value-of select ="''"/>
        </CommonReference>
        
        <MessageFunction>
        <xsl:value-of select ="''"/>
        </MessageFunction>
        
        <NewSecurityCancel>
        <xsl:value-of select ="''"/>
        </NewSecurityCancel>
        
        <CancelorRetainOriginalFX>
        <xsl:value-of select ="''"/>
        </CancelorRetainOriginalFX>
        
        <CurrencyofDenomination>
        <xsl:value-of select ="''"/>
        </CurrencyofDenomination>
        
        <OptionStyle>
        <xsl:value-of select ="''"/>
        </OptionStyle>
        
        <OptionType>
        <xsl:value-of select ="''"/>
        </OptionType>
        
        <ContractSize>
        <xsl:value-of select ="''"/>
        </ContractSize>
        
        <ExercisePrice>
        <xsl:value-of select ="''"/>
        </ExercisePrice>
        
        <ExpirationDate>
        <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="ExpirationDate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
        </ExpirationDate>
        
        <UnderlyingSecurityIDType>
        <xsl:value-of select ="''"/>
        </UnderlyingSecurityIDType>
        
        <UnderlyingSecurityID>
        <xsl:value-of select ="''"/>
        </UnderlyingSecurityID>
        
        <UnderlyingSecurityDescription>
        <xsl:value-of select ="''"/>
        </UnderlyingSecurityDescription>
        
        <OpenCloseIndicator>
        <xsl:value-of select ="''"/>
        </OpenCloseIndicator>

				  <ProcessingGroup>
					  <xsl:value-of select ="'IMP Nirvana Solutions'"/>
				  </ProcessingGroup>
			 <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

  </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'True'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="'Deleted'"/>
					</TaxLotState>


					<TransactionType>
						<xsl:choose>
							<xsl:when test="OldSide='Buy' or OldSide='Buy to Open' or OldSide = 'Buy to Close'">
								<xsl:value-of select="'DVP'"/>
							</xsl:when>
							<xsl:when test="OldSide='Sell' or OldSide='Sell to Close' or OldSide = 'Sell short' or OldSide = 'Sell to Open'">
								<xsl:value-of select="'RVP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TransactionType>
					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'SSBS'"/>
					</xsl:variable>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<AccountNumber>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE != ''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="FundAccntNo"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountNumber>


					<xsl:variable name="varOldTradeDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="OldTradeDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varTransactionReferenceDate">
						<xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),substring-before(substring-after($varOldTradeDate,'/'),'/'),substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
					</xsl:variable>
					<TransactionReferenceNumber>
						<xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position(),'C')"/>
					</TransactionReferenceNumber>


					<SenderID>
						<xsl:value-of select ="''"/>
					</SenderID>

					<RelatedReferenceNumber>
						<xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position())"/>
					</RelatedReferenceNumber>

					<TradeDate>
						<xsl:value-of select ="$varOldTradeDate"/>
					</TradeDate>

					<xsl:variable name="varOldSettlementDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="OldSettlementDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<SettlementDate>
						<xsl:value-of select="$varOldSettlementDate"/>
					</SettlementDate>

					<FX>
						<xsl:value-of select ="'N'"/>
					</FX>

					<ContraCCY>
						<xsl:value-of select ="''"/>
					</ContraCCY>

					<SecurityIDType>
						<xsl:value-of select ="'SEDOL'"/>
					</SecurityIDType>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="SEDOL != ''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<SecurityDescription>
						<xsl:value-of select ="CompanyName"/>
					</SecurityDescription>

					<AssetClass>
						<xsl:value-of select ="Asset"/>
					</AssetClass>

					<MaturityDate>
						<xsl:value-of select ="''"/>
					</MaturityDate>

					<InterestRate>
						<xsl:value-of select ="''"/>
					</InterestRate>

					<IssueDate>
						<xsl:value-of select ="''"/>
					</IssueDate>

					<DealPrice>
						<xsl:value-of select="format-number(OldAvgPrice,'#.000000')"/>
					</DealPrice>

					<DealPriceType>
						<xsl:value-of select ="''"/>
					</DealPriceType>

					<PlaceOfTrade>
						<xsl:value-of select ="''"/>
					</PlaceOfTrade>

					<TransactionDescription>
						<xsl:value-of select ="''"/>
					</TransactionDescription>

					<DelayedDate>
						<xsl:value-of select ="''"/>
					</DelayedDate>

					<CapitalGainAverageDate>
						<xsl:value-of select ="''"/>
					</CapitalGainAverageDate>

					<CapitalGainAveragePrice>
						<xsl:value-of select ="''"/>
					</CapitalGainAveragePrice>

					<CapitalGainPriceCategory>
						<xsl:value-of select ="''"/>
					</CapitalGainPriceCategory>

					<Quantity>
						<xsl:choose>
							<xsl:when test="number(OldExecutedQuantity)">
								<xsl:value-of select="format-number(OldExecutedQuantity,'#.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<AmortizedAmount>
						<xsl:value-of select ="''"/>
					</AmortizedAmount>

					<SettlementCurrency>
						<xsl:value-of select ="OldSettlCurrency"/>
					</SettlementCurrency>

					<SettlementAmount>
						<xsl:value-of select ="''"/>
					</SettlementAmount>
					<xsl:variable name="varAssetMultiplier">
						<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier)"/>
					</xsl:variable>
					<PrincipalAmount>
						<xsl:choose>
							<xsl:when test="CurrencySymbol ='JPY'">
								<xsl:value-of select="round($varAssetMultiplier)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
							</xsl:otherwise>
						</xsl:choose>
					</PrincipalAmount>

					<AccruedInterestType>
						<xsl:value-of select ="''"/>
					</AccruedInterestType>

					<AccruedInterestAmount>
						<xsl:value-of select ="''"/>
					</AccruedInterestAmount>

					 <TaxesType>
			<xsl:choose>
					<xsl:when test="StampDuty = 0">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="StampDuty != '' or StampDuty != 0">
						<xsl:value-of select="'STAM - Stamp Duty'"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
						</xsl:choose>
				  </TaxesType>

				  <TaxesAmount>
					  <xsl:value-of select ="StampDuty"/>
				  </TaxesAmount>


					<CommissionsType>
						<xsl:value-of select ="'LOCO - Local Brokers Commission'"/>
					</CommissionsType>

					<CommissionsAmount>
						<xsl:value-of select="format-number(OldCommission + OldSoftCommission,'#.##')"/>
					</CommissionsAmount>

					<ChargesFees1Type>
						<xsl:value-of select ="''"/>
					</ChargesFees1Type>

					<ChargesFees1Amount>
						<xsl:value-of select ="''"/>
					</ChargesFees1Amount>

					<ChargesFees2Type>
						<xsl:value-of select ="''"/>
					</ChargesFees2Type>

					<ChargesFees2Amount>
						<xsl:value-of select ="''"/>
					</ChargesFees2Amount>

					<ChargesFees3Type>
						<xsl:value-of select ="''"/>
					</ChargesFees3Type>

					<ChargesFees3Amount>
						<xsl:value-of select ="''"/>
					</ChargesFees3Amount>

					<ChargesFees4Type>
						<xsl:value-of select ="''"/>
					</ChargesFees4Type>

					<ChargesFees4Amount>
						<xsl:value-of select ="''"/>
					</ChargesFees4Amount>

					<NetMovementType>
						<xsl:value-of select ="''"/>
					</NetMovementType>

					<NetMovementAmount>
						<xsl:value-of select ="''"/>
					</NetMovementAmount>

					<SSI>
						<xsl:value-of select ="''"/>
					</SSI>
					<xsl:variable name="varCurrencySymbol">
						<xsl:value-of select="CurrencySymbol"/>
					</xsl:variable>
					<xsl:variable name="varCountry">
						<xsl:value-of select="Country"/>
					</xsl:variable>

					<TransactionSubType>
						<xsl:value-of select="'TRAD - Trade'"/>
					</TransactionSubType>

					<SettlementTransactionConditionIndicator>
						<xsl:value-of select ="''"/>
					</SettlementTransactionConditionIndicator>

					<xsl:variable name="TP_PlaceOfSettlementType">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@PlaceOfSettlementType"/>
					</xsl:variable>
					<PlaceOfSettlementType>
						<xsl:choose>
							<xsl:when test="$TP_PlaceOfSettlementType != ''">
								<xsl:value-of select="$TP_PlaceOfSettlementType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PlaceOfSettlementType>

					<xsl:variable name="TP_PlaceOfSettlement">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@PlaceofSettlement"/>
					</xsl:variable>
					<PlaceOfSettlement>
						<xsl:choose>
							<xsl:when test="$TP_PlaceOfSettlement != ''">
								<xsl:value-of select="$TP_PlaceOfSettlement"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PlaceOfSettlement>

					<xsl:variable name="TP_BuyerSellerIDType">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@BuyerSellerIDType"/>
					</xsl:variable>
					<BuyerSellerIDType>
						<xsl:choose>
							<xsl:when test="$TP_BuyerSellerIDType != ''">
								<xsl:value-of select="$TP_BuyerSellerIDType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BuyerSellerIDType>

					<xsl:variable name="TP_BuyerSellerID">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@BuyerSellerID"/>
							</xsl:variable>
							<BuyerSellerID>
								<xsl:choose>
									<xsl:when test="$TP_BuyerSellerID != ''">
										<xsl:value-of select="$TP_BuyerSellerID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</BuyerSellerID>
					<BuyerSellerName>
						<xsl:value-of select ="''"/>
					</BuyerSellerName>

					<BuyerSellerNameAddress>
						<xsl:value-of select ="''"/>
					</BuyerSellerNameAddress>

					<BuyerSellerAcct>
						<xsl:value-of select ="''"/>
					</BuyerSellerAcct>

					<xsl:variable name="TP_RecDelAgentIDType">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@RecDelAgentIDType"/>
					</xsl:variable>
					<RecDelAgentIDType>
						<xsl:choose>
							<xsl:when test="$TP_RecDelAgentIDType != ''">
								<xsl:value-of select="$TP_RecDelAgentIDType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</RecDelAgentIDType>


					<xsl:variable name="TP_RecDelAgentID">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@RecDelAgentID"/>
							</xsl:variable>
							<RecDelAgentID>
								<xsl:choose>
									<xsl:when test="$TP_RecDelAgentID != ''">
										<xsl:value-of select="$TP_RecDelAgentID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</RecDelAgentID>
					<RecDelAgentName>
						<xsl:value-of select ="''"/>
					</RecDelAgentName>

					<RecDelAgentNameAddress>
						<xsl:value-of select ="''"/>
					</RecDelAgentNameAddress>

					<RecDelAgentAcct>
						<xsl:value-of select ="''"/>
					</RecDelAgentAcct>

					<IntermediaryIDType>
						<xsl:value-of select ="''"/>
					</IntermediaryIDType>

					<IntermediaryID>
						<xsl:value-of select ="''"/>
					</IntermediaryID>

					<IntermediaryName>
						<xsl:value-of select ="''"/>
					</IntermediaryName>

					<IntermediaryNameAddress>
						<xsl:value-of select ="''"/>
					</IntermediaryNameAddress>

					<IntermediaryAcct>
						<xsl:value-of select ="''"/>
					</IntermediaryAcct>

					<CashRecDelAgentIDType>
						<xsl:value-of select ="''"/>
					</CashRecDelAgentIDType>

					<CashRecDelAgentID>
						<xsl:value-of select ="''"/>
					</CashRecDelAgentID>

					<CashBeneficiaryID>
						<xsl:value-of select ="''"/>
					</CashBeneficiaryID>

					<CashBeneficiaryAcct>
						<xsl:value-of select ="''"/>
					</CashBeneficiaryAcct>

					<PlaceOfSafekeepingType>
						<xsl:value-of select ="''"/>
					</PlaceOfSafekeepingType>

					<PlaceOfSafekeeping>
						<xsl:value-of select ="''"/>
					</PlaceOfSafekeeping>

					<CBO>
						<xsl:value-of select ="''"/>
					</CBO>

					<NameAndAddress>
						<xsl:value-of select ="''"/>
					</NameAndAddress>

					<SettlementInformation>
						<xsl:value-of select ="''"/>
					</SettlementInformation>

					<TaxID>
						<xsl:value-of select ="''"/>
					</TaxID>

					<StampDutyExemption>
						<xsl:value-of select ="''"/>
					</StampDutyExemption>

					<PriorityIndicator>
						<xsl:value-of select ="''"/>
					</PriorityIndicator>

					<TradeTransactionConditionIndicator>
						<xsl:value-of select ="''"/>
					</TradeTransactionConditionIndicator>

					<CurrentSettlementTransactionNumber>
						<xsl:value-of select ="''"/>
					</CurrentSettlementTransactionNumber>

					<TotalOfLinkedSettlementInstructions>
						<xsl:value-of select ="''"/>
					</TotalOfLinkedSettlementInstructions>

					<PoolReferenceNumber>
						<xsl:value-of select ="''"/>
					</PoolReferenceNumber>

					<LinkageTypeIndicator>
						<xsl:value-of select ="''"/>
					</LinkageTypeIndicator>

					<CommonReference>
						<xsl:value-of select ="''"/>
					</CommonReference>

					<MessageFunction>
						<xsl:value-of select ="''"/>
					</MessageFunction>

					<NewSecurityCancel>
						<xsl:value-of select ="''"/>
					</NewSecurityCancel>

					<CancelorRetainOriginalFX>
						<xsl:value-of select ="''"/>
					</CancelorRetainOriginalFX>

					<CurrencyofDenomination>
						<xsl:value-of select ="''"/>
					</CurrencyofDenomination>

					<OptionStyle>
						<xsl:value-of select ="''"/>
					</OptionStyle>

					<OptionType>
						<xsl:value-of select ="''"/>
					</OptionType>

					<ContractSize>
						<xsl:value-of select ="''"/>
					</ContractSize>

					<ExercisePrice>
						<xsl:value-of select ="''"/>
					</ExercisePrice>

					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExpirationDate>

					<UnderlyingSecurityIDType>
						<xsl:value-of select ="''"/>
					</UnderlyingSecurityIDType>

					<UnderlyingSecurityID>
						<xsl:value-of select ="''"/>
					</UnderlyingSecurityID>

					<UnderlyingSecurityDescription>
						<xsl:value-of select ="''"/>
					</UnderlyingSecurityDescription>

					<OpenCloseIndicator>
						<xsl:value-of select ="''"/>
					</OpenCloseIndicator>
					<ProcessingGroup>
						<xsl:value-of select ="'IMP Nirvana Solutions'"/>
					</ProcessingGroup>
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
            </xsl:if>
			  <ThirdPartyFlatFileDetail>

				  <RowHeader>
					  <xsl:value-of select ="'false'"/>
				  </RowHeader>

				  <TaxLotState>
					  <xsl:value-of select="'Allocated'"/>
				  </TaxLotState>

				  <TransactionType>
					  <xsl:choose>
						  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side = 'Buy to Close'">
							  <xsl:value-of select="'DVP'"/>
						  </xsl:when>
						  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side = 'Sell short' or Side = 'Sell to Open'">
							  <xsl:value-of select="'RVP'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </TransactionType>
				  <xsl:variable name="PB_NAME">
					  <xsl:value-of select="'SSBS'"/>
				  </xsl:variable>

				  <xsl:variable name = "PRANA_FUND_NAME">
					  <xsl:value-of select="AccountName"/>
				  </xsl:variable>

				  <xsl:variable name ="THIRDPARTY_FUND_CODE">
					  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
				  </xsl:variable>

				  <AccountNumber>
					  <xsl:choose>
						  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
							  <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="FundAccntNo"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </AccountNumber>

				  <xsl:variable name="varTradeDate">
					  <xsl:call-template name="DateFormat">
						  <xsl:with-param name="Date" select="TradeDate">
						  </xsl:with-param>
					  </xsl:call-template>
				  </xsl:variable>


				  <xsl:variable name="varTransactionReferenceDate">
					  <xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
				  </xsl:variable>

				  <TransactionReferenceNumber>
					  <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position(),'C')"/>
				  </TransactionReferenceNumber>

				  <SenderID>
					  <xsl:value-of select ="''"/>
				  </SenderID>

				  <RelatedReferenceNumber>
					  <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position())"/>
				  </RelatedReferenceNumber>

				  <TradeDate>
					  <xsl:value-of select ="$varTradeDate"/>
				  </TradeDate>
				  <xsl:variable name="varSettlementDate">
					  <xsl:call-template name="DateFormat">
						  <xsl:with-param name="Date" select="SettlementDate">
						  </xsl:with-param>
					  </xsl:call-template>
				  </xsl:variable>

				  <SettlementDate>
					  <xsl:value-of select ="$varSettlementDate"/>
				  </SettlementDate>

				  <FX>
					  <xsl:value-of select ="'N'"/>
				  </FX>

				  <ContraCCY>
					  <xsl:value-of select ="''"/>
				  </ContraCCY>

				  <SecurityIDType>
					  <xsl:value-of select ="'SEDOL'"/>
				  </SecurityIDType>

				  <SecurityID>
					  <xsl:choose>
						  <xsl:when test="SEDOL != ''">
							  <xsl:value-of select="SEDOL"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="Symbol"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </SecurityID>

				  <SecurityDescription>
					  <xsl:value-of select ="CompanyName"/>
				  </SecurityDescription>

				  <AssetClass>
					  <xsl:value-of select ="Asset"/>
				  </AssetClass>

				  <MaturityDate>
					  <xsl:value-of select ="''"/>
				  </MaturityDate>

				  <InterestRate>
					  <xsl:value-of select ="''"/>
				  </InterestRate>

				  <IssueDate>
					  <xsl:value-of select ="''"/>
				  </IssueDate>

				  <DealPrice>
					  <xsl:value-of select="format-number(AvgPrice,'#.000000')"/>
				  </DealPrice>

				  <DealPriceType>
					  <xsl:value-of select ="''"/>
				  </DealPriceType>

				  <PlaceOfTrade>
					  <xsl:value-of select ="''"/>
				  </PlaceOfTrade>

				  <TransactionDescription>
					  <xsl:value-of select ="''"/>
				  </TransactionDescription>

				  <DelayedDate>
					  <xsl:value-of select ="''"/>
				  </DelayedDate>

				  <CapitalGainAverageDate>
					  <xsl:value-of select ="''"/>
				  </CapitalGainAverageDate>

				  <CapitalGainAveragePrice>
					  <xsl:value-of select ="''"/>
				  </CapitalGainAveragePrice>

				  <CapitalGainPriceCategory>
					  <xsl:value-of select ="''"/>
				  </CapitalGainPriceCategory>

				  <Quantity>
					  <xsl:choose>
						  <xsl:when test="number(OrderQty)">
							  <xsl:value-of select="format-number(OrderQty,'#.00')"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="0"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Quantity>

				  <AmortizedAmount>
					  <xsl:value-of select ="''"/>
				  </AmortizedAmount>

				  <SettlementCurrency>
					  <xsl:value-of select ="SettlCurrency"/>
				  </SettlementCurrency>

				  <SettlementAmount>
					  <xsl:value-of select ="$varNetamount"/>
				  </SettlementAmount>
				  <xsl:variable name="varAssetMultiplier">
					  <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
				  </xsl:variable>
				  <PrincipalAmount>
					  <xsl:choose>
						  <xsl:when test="CurrencySymbol ='JPY'">
							  <xsl:value-of select="round($varAssetMultiplier)"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </PrincipalAmount>

				  <AccruedInterestType>
					  <xsl:value-of select ="''"/>
				  </AccruedInterestType>

				  <AccruedInterestAmount>
					  <xsl:value-of select ="''"/>
				  </AccruedInterestAmount>

			 <TaxesType>
			<xsl:choose>
					<xsl:when test="StampDuty = 0">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="StampDuty != '' or StampDuty != 0">
						<xsl:value-of select="'STAM - Stamp Duty'"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
						</xsl:choose>
				  </TaxesType>

				  <TaxesAmount>
					  <xsl:value-of select ="StampDuty"/>
				  </TaxesAmount>

				  <CommissionsType>
					  <xsl:value-of select ="'LOCO - Local Brokers Commission'"/>
				  </CommissionsType>

				  <CommissionsAmount>
					  <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.##')"/>
				  </CommissionsAmount>

				  <ChargesFees1Type>
					  <xsl:value-of select ="''"/>
				  </ChargesFees1Type>

				  <ChargesFees1Amount>
					  <xsl:value-of select ="''"/>
				  </ChargesFees1Amount>

				  <ChargesFees2Type>
					  <xsl:value-of select ="''"/>
				  </ChargesFees2Type>

				  <ChargesFees2Amount>
					  <xsl:value-of select ="''"/>
				  </ChargesFees2Amount>

				  <ChargesFees3Type>
					  <xsl:value-of select ="''"/>
				  </ChargesFees3Type>

				  <ChargesFees3Amount>
					  <xsl:value-of select ="''"/>
				  </ChargesFees3Amount>

				  <ChargesFees4Type>
					  <xsl:value-of select ="''"/>
				  </ChargesFees4Type>

				  <ChargesFees4Amount>
					  <xsl:value-of select ="''"/>
				  </ChargesFees4Amount>

				  <NetMovementType>
					  <xsl:value-of select ="''"/>
				  </NetMovementType>

				  <NetMovementAmount>
					  <xsl:value-of select ="''"/>
				  </NetMovementAmount>

				  <SSI>
					  <xsl:value-of select ="''"/>
				  </SSI>
				  <xsl:variable name="varCurrencySymbol">
					  <xsl:value-of select="CurrencySymbol"/>
				  </xsl:variable>
				  <xsl:variable name="varCountry">
					  <xsl:value-of select="Country"/>
				  </xsl:variable>

				  <TransactionSubType>
					  <xsl:value-of select="'TRAD - Trade'"/>
				  </TransactionSubType>

				  <SettlementTransactionConditionIndicator>
					  <xsl:value-of select ="''"/>
				  </SettlementTransactionConditionIndicator>

				  <xsl:variable name="TP_PlaceOfSettlementType">
					  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@PlaceOfSettlementType"/>
				  </xsl:variable>
				  <PlaceOfSettlementType>
					  <xsl:choose>
						  <xsl:when test="$TP_PlaceOfSettlementType != ''">
							  <xsl:value-of select="$TP_PlaceOfSettlementType"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </PlaceOfSettlementType>

				  <xsl:variable name="TP_PlaceOfSettlement">
					  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@PlaceofSettlement"/>
				  </xsl:variable>
				  <PlaceOfSettlement>
					  <xsl:choose>
						  <xsl:when test="$TP_PlaceOfSettlement != ''">
							  <xsl:value-of select="$TP_PlaceOfSettlement"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </PlaceOfSettlement>

				  <xsl:variable name="TP_BuyerSellerIDType">
					  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@BuyerSellerIDType"/>
				  </xsl:variable>
				  <BuyerSellerIDType>
					  <xsl:choose>
						  <xsl:when test="$TP_BuyerSellerIDType != ''">
							  <xsl:value-of select="$TP_BuyerSellerIDType"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </BuyerSellerIDType>

				 <xsl:variable name="TP_BuyerSellerID">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@BuyerSellerID"/>
							</xsl:variable>
							<BuyerSellerID>
								<xsl:choose>
									<xsl:when test="$TP_BuyerSellerID != ''">
										<xsl:value-of select="$TP_BuyerSellerID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</BuyerSellerID>

				  <BuyerSellerName>
					  <xsl:value-of select ="''"/>
				  </BuyerSellerName>

				  <BuyerSellerNameAddress>
					  <xsl:value-of select ="''"/>
				  </BuyerSellerNameAddress>

				  <BuyerSellerAcct>
					  <xsl:value-of select ="''"/>
				  </BuyerSellerAcct>

				  <xsl:variable name="TP_RecDelAgentIDType">
					  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@RecDelAgentIDType"/>
				  </xsl:variable>
				  <RecDelAgentIDType>
					  <xsl:choose>
						  <xsl:when test="$TP_RecDelAgentIDType != ''">
							  <xsl:value-of select="$TP_RecDelAgentIDType"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </RecDelAgentIDType>

					<xsl:variable name="TP_RecDelAgentID">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PlaceOfSettlementMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol and @CountryName = $varCountry]/@RecDelAgentID"/>
							</xsl:variable>
							<RecDelAgentID>
								<xsl:choose>
									<xsl:when test="$TP_RecDelAgentID != ''">
										<xsl:value-of select="$TP_RecDelAgentID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</RecDelAgentID>

				  <RecDelAgentName>
					  <xsl:value-of select ="''"/>
				  </RecDelAgentName>

				  <RecDelAgentNameAddress>
					  <xsl:value-of select ="''"/>
				  </RecDelAgentNameAddress>

				  <RecDelAgentAcct>
					  <xsl:value-of select ="''"/>
				  </RecDelAgentAcct>

				  <IntermediaryIDType>
					  <xsl:value-of select ="''"/>
				  </IntermediaryIDType>

				  <IntermediaryID>
					  <xsl:value-of select ="''"/>
				  </IntermediaryID>

				  <IntermediaryName>
					  <xsl:value-of select ="''"/>
				  </IntermediaryName>

				  <IntermediaryNameAddress>
					  <xsl:value-of select ="''"/>
				  </IntermediaryNameAddress>

				  <IntermediaryAcct>
					  <xsl:value-of select ="''"/>
				  </IntermediaryAcct>

				  <CashRecDelAgentIDType>
					  <xsl:value-of select ="''"/>
				  </CashRecDelAgentIDType>

				  <CashRecDelAgentID>
					  <xsl:value-of select ="''"/>
				  </CashRecDelAgentID>

				  <CashBeneficiaryID>
					  <xsl:value-of select ="''"/>
				  </CashBeneficiaryID>

				  <CashBeneficiaryAcct>
					  <xsl:value-of select ="''"/>
				  </CashBeneficiaryAcct>

				  <PlaceOfSafekeepingType>
					  <xsl:value-of select ="''"/>
				  </PlaceOfSafekeepingType>

				  <PlaceOfSafekeeping>
					  <xsl:value-of select ="''"/>
				  </PlaceOfSafekeeping>

				  <CBO>
					  <xsl:value-of select ="''"/>
				  </CBO>

				  <NameAndAddress>
					  <xsl:value-of select ="''"/>
				  </NameAndAddress>

				  <SettlementInformation>
					  <xsl:value-of select ="''"/>
				  </SettlementInformation>

				  <TaxID>
					  <xsl:value-of select ="''"/>
				  </TaxID>

				  <StampDutyExemption>
					  <xsl:value-of select ="''"/>
				  </StampDutyExemption>

				  <PriorityIndicator>
					  <xsl:value-of select ="''"/>
				  </PriorityIndicator>

				  <TradeTransactionConditionIndicator>
					  <xsl:value-of select ="''"/>
				  </TradeTransactionConditionIndicator>

				  <CurrentSettlementTransactionNumber>
					  <xsl:value-of select ="''"/>
				  </CurrentSettlementTransactionNumber>

				  <TotalOfLinkedSettlementInstructions>
					  <xsl:value-of select ="''"/>
				  </TotalOfLinkedSettlementInstructions>

				  <PoolReferenceNumber>
					  <xsl:value-of select ="''"/>
				  </PoolReferenceNumber>

				  <LinkageTypeIndicator>
					  <xsl:value-of select ="''"/>
				  </LinkageTypeIndicator>

				  <CommonReference>
					  <xsl:value-of select ="''"/>
				  </CommonReference>

				  <MessageFunction>
					  <xsl:value-of select ="''"/>
				  </MessageFunction>

				  <NewSecurityCancel>
					  <xsl:value-of select ="''"/>
				  </NewSecurityCancel>

				  <CancelorRetainOriginalFX>
					  <xsl:value-of select ="''"/>
				  </CancelorRetainOriginalFX>

				  <CurrencyofDenomination>
					  <xsl:value-of select ="''"/>
				  </CurrencyofDenomination>

				  <OptionStyle>
					  <xsl:value-of select ="''"/>
				  </OptionStyle>

				  <OptionType>
					  <xsl:value-of select ="''"/>
				  </OptionType>

				  <ContractSize>
					  <xsl:value-of select ="''"/>
				  </ContractSize>

				  <ExercisePrice>
					  <xsl:value-of select ="''"/>
				  </ExercisePrice>

				  <ExpirationDate>
					  <xsl:choose>
						  <xsl:when test="Asset='EquityOption'">
							  <xsl:value-of select="ExpirationDate"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </ExpirationDate>

				  <UnderlyingSecurityIDType>
					  <xsl:value-of select ="''"/>
				  </UnderlyingSecurityIDType>

				  <UnderlyingSecurityID>
					  <xsl:value-of select ="''"/>
				  </UnderlyingSecurityID>

				  <UnderlyingSecurityDescription>
					  <xsl:value-of select ="''"/>
				  </UnderlyingSecurityDescription>

				  <OpenCloseIndicator>
					  <xsl:value-of select ="''"/>
				  </OpenCloseIndicator>
				  <ProcessingGroup>
					  <xsl:value-of select ="'IMP Nirvana Solutions'"/>
				  </ProcessingGroup>
				  <EntityID>
					  <xsl:value-of select="EntityID"/>
				  </EntityID>

			  </ThirdPartyFlatFileDetail>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
