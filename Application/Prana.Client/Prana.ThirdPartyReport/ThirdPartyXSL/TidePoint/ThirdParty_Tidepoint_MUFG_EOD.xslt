<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="ScientificToNumber">
    <xsl:param name="ScientificN"/>
    <xsl:variable name="vExponent" select="substring-after($ScientificN,'E')"/>
    <xsl:variable name="vMantissa" select="substring-before($ScientificN,'E')"/>
    <xsl:variable name="vFactor"
				 select="substring('100000000000000000000000000000000000000000000',
                              1, substring($vExponent,2) + 1)"/>
    <xsl:choose>
      <xsl:when test="starts-with($vExponent,'-')">
        <xsl:value-of select="$vMantissa div $vFactor"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$vMantissa * $vFactor"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>


          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
         
         
          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'MUFG'"/>
          </xsl:variable>

          <xsl:variable name = "varPRANA_FUND">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <xsl:variable name ="var_Prime_Broker">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_PrimeBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@AccountName=$varPRANA_FUND]/@PranaBroker"/>
          </xsl:variable>

          <FundCode>          
			<xsl:value-of select ="AccountNo"/>
          </FundCode>

          <FundStucture>
            <xsl:value-of select ="'Class1'"/>
          </FundStucture>

          <Strategy>
            <xsl:value-of select ="''"/>
          </Strategy>

          <Custodian>
            <xsl:choose>  
              <xsl:when test ="$var_Prime_Broker != ''"> 
             <xsl:value-of select ="$var_Prime_Broker"/> 
             </xsl:when> 
               <xsl:otherwise> 
               <xsl:value-of select ="''"/> 
              </xsl:otherwise>
          </xsl:choose> 
          </Custodian>

          <CustodianAccount>
            <xsl:value-of select ="AccountName"/>
          </CustodianAccount>

          <TransferCustodianAccount>
            <xsl:value-of select ="''"/>
          </TransferCustodianAccount>

          <PrimeBroker>
            <xsl:choose>  
              <xsl:when test ="$var_Prime_Broker != ''"> 
             <xsl:value-of select ="$var_Prime_Broker"/> 
             </xsl:when> 
               <xsl:otherwise> 
               <xsl:value-of select ="''"/> 
              </xsl:otherwise>
          </xsl:choose>
          </PrimeBroker>

          <InvestmentID>            
                <xsl:choose>
				<xsl:when test="Asset='FX' or Asset='FXForward'">
				  <xsl:value-of select="Symbol"/>
                  </xsl:when>
				
				<xsl:when test="contains(Asset,'Future')">
                    <xsl:value-of select="BBCode"/>
                  </xsl:when>
				
                  <xsl:when test="BBCode!='' ">
                    <xsl:value-of select="substring-before(BBCode,' EQUITY')"/>
                  </xsl:when>

                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>

                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="ISIN"/>
                  </xsl:when>

                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="CUSIP"/>
                  </xsl:when>
                  
                  <xsl:when test="contains(Asset,'Option')">
                    <xsl:value-of select="OSIOptionSymbol"/>
                  </xsl:when>
				  
				  
                  <xsl:otherwise>
                    <xsl:value-of select="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>              
          </InvestmentID>

          <ISIN>
            <xsl:value-of select ="ISIN"/>
          </ISIN>

          <Sedol>
            <xsl:value-of select ="SEDOL"/>
          </Sedol>

          <Cusip>
            <xsl:value-of select ="CUSIP"/>
          </Cusip>

          <OCCTicker>
            <xsl:value-of select ="OSIOptionSymbol"/>
          </OCCTicker>

          <Ticker>
            <xsl:choose>
              <xsl:when test="BBCode!='' ">
                <xsl:value-of select="BBCode"/>
              </xsl:when>

              <xsl:when test="SEDOL!=''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>

              <xsl:when test="ISIN!=''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>

              <xsl:when test="CUSIP!=''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>

              <xsl:when test="contains(Asset,'Option')">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </Ticker>

          <MarkitLoanID>
            <xsl:value-of select ="''"/>
          </MarkitLoanID>

          <RIC>
            <xsl:value-of select ="RICCode"/>
          </RIC>

          <BloombergGlobalID>
            <xsl:value-of select ="''"/>
          </BloombergGlobalID>

          <Description>
            <xsl:value-of select ="FullSecurityName"/>
          </Description>

          <ExchangeCode>
            <xsl:value-of select ="''"/>
          </ExchangeCode>

          <xsl:variable name="varKey">
            <xsl:call-template name="substring-after-last">
              <xsl:with-param name="string" select="BBCode"/>
              <xsl:with-param name="delimiter" select="' '"/>
            </xsl:call-template>
          </xsl:variable>

          <FutureCategory>
            <xsl:choose>
              <xsl:when test="($varKey = 'Index' or $varKey = 'INDEX') and Asset = 'Future'">
                <xsl:value-of select="'Index'"/>
              </xsl:when>
              <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY' or $varKey = 'Curncy' or $varKey = 'CURNCY') and Asset = 'Future'">
                <xsl:value-of select="'Currency'"/>
              </xsl:when>
              <!--<xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY' or $varKey = 'Curncy' or $varKey = 'CURNCY') and Asset = 'FutureOption'">
                <xsl:value-of select="'CMDFUTOPT'"/>
              </xsl:when>
              <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY') and Exchange = 'LME' ">
                <xsl:value-of select="'MTLFWD'"/>
              </xsl:when>-->
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FutureCategory>

          <AssetType>
            <xsl:choose>
			<xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'Swap'"/>
              </xsl:when>
              <xsl:when test="Asset='Equity'">                
                <xsl:value-of select="'Equity'"/>
              </xsl:when>
              <xsl:when test="Asset='Future'">
                <xsl:value-of select="'Future'"/>
              </xsl:when>
              <xsl:when test="Asset='FutureOption'">
                <xsl:value-of select="'Option'"/>
              </xsl:when>

              <xsl:when test="Asset='Cash'">
                <xsl:value-of select="'Cash'"/>
              </xsl:when>

              <xsl:when test="Asset='Indices'">
                <xsl:value-of select="'Indices'"/>
              </xsl:when>

              <xsl:when test="Asset='FixedIncome'">
                <xsl:value-of select="'Bond'"/>
              </xsl:when>

              <xsl:when test="Asset='PrivateEquity'">
                <xsl:value-of select="'PrivateEquity'"/>
              </xsl:when>

              <xsl:when test="Asset='FXOption'">
                <xsl:value-of select="'FXOption'"/>
              </xsl:when>
              
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'Forward FX'"/>
              </xsl:when>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'Spot FX'"/>
              </xsl:when>
              <xsl:when test="Asset='ConvertibleBond'">
                <xsl:value-of select="'Covertible Bond'"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'Option'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>
          </AssetType>


          <SwapType>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'Equity Swap'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </SwapType>

          <OptionType>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'Equity Option'"/>
              </xsl:when>
			   <xsl:when test="Asset='FutureOption'">
                <xsl:value-of select="'Future Option'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OptionType>

          <xsl:variable name="i" select="position()" />        
          <TradeID>
             <xsl:choose>
			<xsl:when test="$i &lt; 10">
								<xsl:value-of select="concat(PBUniqueID,'0000',$i)"/>
							</xsl:when>
							<xsl:when test="$i &lt; 100">
								<xsl:value-of select="concat(PBUniqueID,'000',$i)"/>
							</xsl:when>
							<xsl:when test="$i &lt; 1000">
								<xsl:value-of select="concat(PBUniqueID,'00',$i)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="concat(PBUniqueID,position())"/>
							</xsl:otherwise>
						</xsl:choose>
              
                 
          </TradeID>

          <TradeStatus>
            <xsl:choose>
              <xsl:when test="TaxLotState = 'Allocated'">
                <xsl:value-of select="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Amended'">
                <xsl:value-of select="'Correct'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Deleted'">
                <xsl:value-of select="'Cancel'"/>
              </xsl:when>
            </xsl:choose>
          </TradeStatus>
		  
		   <xsl:variable name="varTransactionType">
            <xsl:choose>
              <xsl:when test ="TransactionType='SellShort'">
                <xsl:value-of select ="'Sell Short'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='BuytoClose'">
                <xsl:value-of select ="'Buy to Close'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='BuytoOpen'">
                <xsl:value-of select ="'Buy to Open'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='SelltoClose'">
                <xsl:value-of select ="'Sell to Close'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='SelltoOpen'">
                <xsl:value-of select ="'Sell to Open'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='ShortAddition'">
                <xsl:value-of select ="'Short Addition'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='ShortWithdrawal'">
                <xsl:value-of select ="'Short Withdrawal'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='ShortWithdrawalCashInLieu'">
                <xsl:value-of select ="'Short Withdrawal Cash In Lieu'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongWithdrawalCashInLieu'">
                <xsl:value-of select ="'Long Withdrawal Cash In Lieu'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongWithdrawal'">
                <xsl:value-of select ="'Long Withdrawal'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongCostAdj'">
                <xsl:value-of select ="'Long Cost Adj'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongAddition'">
                <xsl:value-of select ="'Long Addition'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='DLCostAndPNL'">
                <xsl:value-of select ="'DL Cost And PNL'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSClosingPx'">
                <xsl:value-of select ="'Cash Settle At Closing Date Spot PX'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='DLCostAndPNL'">
                <xsl:value-of select ="'DL Cost And PNL'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSCost'">
                <xsl:value-of select ="'Cash Settle At Cost'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSSwp'">
                <xsl:value-of select ="'Swap Expire'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSSwpRl'">
                <xsl:value-of select ="'Swap Expire and Rollover'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSZero'">
                <xsl:value-of select ="'Cash Settle At Zero Price'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='DLCost'">
                <xsl:value-of select ="'Deliver FX At Cost'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="TransactionType"/>
              </xsl:otherwise>

            </xsl:choose>
          </xsl:variable>

          <TradeType>
           
            <xsl:choose>
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'Forward FX'"/>
              </xsl:when>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'Spot FX'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CoverShort'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SellShort'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </TradeType>

          <CorporateActionType>
            <xsl:value-of select ="''"/>
          </CorporateActionType>

          <TradeDate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </SettleDate>

          <Quantity>
		   <xsl:choose>
				  <xsl:when test="Asset='FX' or Asset='FXForward'">
					  <xsl:value-of select ="''"/>
				  </xsl:when>				 
				  <xsl:otherwise>
					   <xsl:value-of select ="AllocatedQty"/>
				  </xsl:otherwise>
			  </xsl:choose>            
          </Quantity>

          <OriginalFace>
            <xsl:value-of select ="''"/>
          </OriginalFace>

          <Price>
		  <xsl:choose>
				  <xsl:when test="Asset='FX' or Asset='FXForward'">
					  <xsl:value-of select ="''"/>
				  </xsl:when>				 
				  <xsl:otherwise>
					   <xsl:value-of select ="AveragePrice"/>
				  </xsl:otherwise>
			  </xsl:choose>
            
          </Price>

          <QuantityFactor>
            <xsl:value-of select ="AssetMultiplier"/>
          </QuantityFactor>

          <PriceFactor>
            <xsl:value-of select ="''"/>
          </PriceFactor>

          <InvestmentCurrency>
            <xsl:value-of select ="CurrencySymbol"/>
          </InvestmentCurrency>

          <SettlementCurrency>
            <xsl:value-of select ="SettlCurrency"/>
          </SettlementCurrency>

          <GrossAmount>
		  <xsl:choose>
				  <xsl:when test="Asset='FX' or Asset='FXForward'">
					  <xsl:value-of select ="''"/>
				  </xsl:when>				 
				  <xsl:otherwise>
					   <xsl:value-of select ="format-number(GrossAmount,'##.####')"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </GrossAmount>

          <NetAmount>
		  <xsl:choose>
				  <xsl:when test="Asset='FX' or Asset='FXForward'">
					  <xsl:value-of select ="''"/>
				  </xsl:when>				 
				  <xsl:otherwise>
					   <xsl:value-of select ="format-number(NetAmount,'##.####')"/>  
				  </xsl:otherwise>
			  </xsl:choose>
		            
          </NetAmount>

          <AccruedInterest>
		   <xsl:value-of select ="format-number(AccruedInterest,'##.####')"/>           
          </AccruedInterest>

          <WithholdingTaxPercent>
            <xsl:value-of select ="''"/>
          </WithholdingTaxPercent>


          <BondLoanMaturityDate>
		   <xsl:choose>
              <xsl:when test="Asset='FixedIncome'">
                <xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
		  
          </BondLoanMaturityDate>

          <Commission>
            <xsl:value-of select ="(CommissionCharged + SoftCommissionCharged)"/>
          </Commission>

          <SECFee>
            <xsl:value-of select ="SecFee"/>
          </SECFee>

          <OtherFee>
            <xsl:value-of select ="OtherBrokerFee"/>
          </OtherFee>
		  
		   

          <FXBuyCurrency>
            <xsl:choose>
				  <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains(TransactionType,'Sell')">
					  <xsl:value-of select ="VsCurrencyName"/>
				  </xsl:when>
				  <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains(TransactionType,'Buy')">
					  <xsl:value-of select="LeadCurrencyName"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </FXBuyCurrency>

          <FXBuyAmount>
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains(TransactionType,'Buy')">
                <xsl:value-of select="ExecutedQty" />
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains(TransactionType,'Sell')">
                <xsl:value-of select="NetAmount" />                
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXBuyAmount>

          <FXSellCurrency>
            <xsl:choose>
              <xsl:when test="(Asset='FX' or Asset='FXForward') and contains(TransactionType,'Sell')">
                <xsl:value-of select ="LeadCurrencyName"/>
              </xsl:when>
              <xsl:when test="(Asset='FX' or Asset='FXForward') and contains(TransactionType,'Buy')">
                <xsl:value-of select="VsCurrencyName"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXSellCurrency>

          <FXSellAmount>
           <xsl:choose>
              <xsl:when test="(Asset='FX' or Asset='FXForward') and contains(TransactionType,'Buy')">
               <xsl:value-of select="NetAmount"/>                 
              </xsl:when>
              <xsl:when test="(Asset='FX' or Asset='FXForward') and contains(TransactionType,'Sell')">
                <xsl:value-of select="ExecutedQty" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXSellAmount>

          <FXRate>
            <xsl:choose>
              <xsl:when test="(Asset='FX' or Asset='FXForward')">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXRate>

          <RepoDirection>
            <xsl:value-of select ="''"/>
          </RepoDirection>

          <RepoOpenFlag>
            <xsl:value-of select ="''"/>
          </RepoOpenFlag>

          <RepoStartDate>
            <xsl:value-of select ="''"/>
          </RepoStartDate>


          <RepoTerminationDate>
            <xsl:value-of select ="''"/>
          </RepoTerminationDate>

          <RepoHaircut>
            <xsl:value-of select ="''"/>
          </RepoHaircut>

          <RepoRate>
            <xsl:value-of select ="''"/>
          </RepoRate>

          <CollateralCurrency>
            <xsl:value-of select ="''"/>
          </CollateralCurrency>

          <SwapStatus>
            <xsl:value-of select ="''"/>
          </SwapStatus>

          <CreditDerivativeEffectiveDate>
            <xsl:value-of select ="''"/>
          </CreditDerivativeEffectiveDate>

          <CreditDerivativeFirstPayDate>
            <xsl:value-of select ="''"/>
          </CreditDerivativeFirstPayDate>

          <CreditDerivativeMaturityDate>
            <xsl:value-of select ="''"/>
          </CreditDerivativeMaturityDate>

          <CreditDerivativeCoupon>
            <xsl:value-of select ="''"/>
          </CreditDerivativeCoupon>

          <CreditDerivativeReferenceObligation>
            <xsl:value-of select ="''"/>
          </CreditDerivativeReferenceObligation>

          <CreditDerivativeRedCode>
            <xsl:value-of select ="''"/>
          </CreditDerivativeRedCode>

          <CreditDerivativeDirection>
            <xsl:value-of select ="''"/>
          </CreditDerivativeDirection>

          <UnderlyingTicker>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="UnderlyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingTicker>

         

          <UnderlyingISIN>
            <xsl:value-of select ="''"/>
          </UnderlyingISIN>

          <UnderlyingCusip>
            <xsl:value-of select ="''"/>
          </UnderlyingCusip>

          <UnderlyingSedol>
            <xsl:value-of select ="''"/>
          </UnderlyingSedol>


          <SwapMaturityDate>
            <xsl:value-of select ="''"/>
          </SwapMaturityDate>

          <SwapUpfrontFee>
            <xsl:value-of select ="''"/>
          </SwapUpfrontFee>

          <SwapUpfrontFeeCurrency>
            <xsl:value-of select ="''"/>
          </SwapUpfrontFeeCurrency>

          <SwapReceiveLegNotional>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegNotional>

          <SwapReceiveLegCurrency>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegCurrency>

          <SwapReceiveLegRateType>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegRateType>

          <SwapReceiveLegIndex>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegIndex>


          <SwapReceiveLegRate>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegRate>

          <SwapReceiveLegSpread>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegSpread>

          <SwapReceiveLegEffectiveDate>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegEffectiveDate>

          <SwapReceiveLegFirstPayDate>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegFirstPayDate>

          <SwapReceiveLegAccrualBasis>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegAccrualBasis>

          <SwapReceiveLegPayFrequency>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegPayFrequency>

          <SwapReceiveLegBDConvention>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegBDConvention>

          <SwapReceiveLegRateResetFrequency>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegRateResetFrequency>

          <SwapReceiveLegRateResetBDConvention>
            <xsl:value-of select ="''"/>
          </SwapReceiveLegRateResetBDConvention>

          <SwapPayLegNotional>
            <xsl:value-of select ="''"/>
          </SwapPayLegNotional>

          <SwapPayLegCurrency>
            <xsl:value-of select ="''"/>
          </SwapPayLegCurrency>

          <SwapPayLegRateType>
            <xsl:value-of select ="''"/>
          </SwapPayLegRateType>

          <SwapPayLegIndex>
            <xsl:value-of select ="''"/>
          </SwapPayLegIndex>

          <SwapPayLegRate>
            <xsl:value-of select ="''"/>
          </SwapPayLegRate>

          <SwapPayLegSpread>
            <xsl:value-of select ="''"/>
          </SwapPayLegSpread>

          <SwapPayLegEffectiveDate>
            <xsl:value-of select ="''"/>
          </SwapPayLegEffectiveDate>

          <SwapPayLegFirstPayDate>
            <xsl:value-of select ="''"/>
          </SwapPayLegFirstPayDate>

          <SwapPayLegAccrualBasis>
            <xsl:value-of select ="''"/>
          </SwapPayLegAccrualBasis>

          <SwapPayLegPayFrequency>
            <xsl:value-of select ="''"/>
          </SwapPayLegPayFrequency>

          <SwapPayLegBDConvention>
            <xsl:value-of select ="''"/>
          </SwapPayLegBDConvention>

          <SwapPayLegRateResetFrequency>
            <xsl:value-of select ="''"/>
          </SwapPayLegRateResetFrequency>

          <SwapPayLegRateResetBDConvention>
            <xsl:value-of select ="''"/>
          </SwapPayLegRateResetBDConvention>

          <OptionUnderlying>
            <xsl:value-of select ="''"/>
          </OptionUnderlying>

          <OptionStrike>
            <xsl:value-of select ="''"/>
          </OptionStrike>

          <OptionExerciseStyle>
            <xsl:value-of select ="''"/>
          </OptionExerciseStyle>

          <PutCallFlag>
            <xsl:value-of select ="''"/>
          </PutCallFlag>

          <OptionMaturityDate>
            <xsl:value-of select ="''"/>
          </OptionMaturityDate>

          <OptionPremium>
            <xsl:value-of select ="''"/>
          </OptionPremium>

          <CallCurrency>
            <xsl:value-of select ="''"/>
          </CallCurrency>

          <PutCurrency>
            <xsl:value-of select ="''"/>
          </PutCurrency>

          <OptionBarrierType>
            <xsl:value-of select ="''"/>
            
          </OptionBarrierType>

          <OptionBarrierDown>
            <xsl:value-of select ="''"/>
          </OptionBarrierDown>

          <OptionBarrierUp>
            <xsl:value-of select ="''"/>
          </OptionBarrierUp>

          <OptionDoubleBarrierDown>
            <xsl:value-of select ="''"/>
          </OptionDoubleBarrierDown>

          <OptionDoubleBarrierUp>
            <xsl:value-of select ="''"/>
          </OptionDoubleBarrierUp>

          <CustomClientID>
            <xsl:value-of select ="''"/>
          </CustomClientID>


          <CustomNotes>

            <xsl:value-of select ="''"/>
          </CustomNotes>



          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>