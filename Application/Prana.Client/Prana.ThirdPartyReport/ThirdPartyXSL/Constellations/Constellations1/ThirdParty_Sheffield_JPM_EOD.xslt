<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
     
      <ThirdPartyFlatFileDetail>
		  <RowHeader>
			  <xsl:value-of select ="'false'"/>
		  </RowHeader>
		  <TaxLotState>
			  <xsl:value-of select="TaxLotState"/>
		  </TaxLotState>

		  <FileHeader>
			  <xsl:value-of select ="'true'"/>
		  </FileHeader>

		  <FileFooter>
			  <xsl:value-of select ="'true'"/>
		  </FileFooter>		 


		  <TRADEID>
              <xsl:value-of select ="'TRADE ID'"/>
           </TRADEID>

		  <ACTION>
            <xsl:value-of select="'ACTION'"/>
          </ACTION>

		  <TRADEDATE>
          <xsl:value-of select ="'TRADE DATE'"/>
        </TRADEDATE>

		  <SETTLEDATE>
            <xsl:value-of select="'SETTLE DATE'"/>
          </SETTLEDATE>

		  <ACCOUNT>
            <xsl:value-of select="'ACCOUNT'"/>
        </ACCOUNT>

		  <METHOD>
          <xsl:value-of select="'METHOD'"/>
        </METHOD>

		  <SIDE>
          <xsl:value-of select="'SIDE'"/>
        </SIDE>

		  <SECURITY>
          <xsl:value-of select="'SECURITY'"/>
        </SECURITY>
		 
		  <SECID>
			  <xsl:value-of select="'SEC ID'"/>
		  </SECID>

		 <QUANTITY>
          <xsl:value-of select="'QUANTITY'"/>
        </QUANTITY>

		  <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

		  <COMMTYPE>
          <xsl:value-of select="'COMM TYPE'"/>
        </COMMTYPE>

		  <COMM>
          <xsl:value-of select="'COMM $'"/>
        </COMM>

		  <INTEREST>
          <xsl:value-of select="'INTEREST'"/>
        </INTEREST>

		  <EXECBRKR>
          <xsl:value-of select="'EXEC BRKR'"/>
        </EXECBRKR>
		    
		   <!--<WIS>
			  <xsl:value-of select="'WI'"/>
		  </WIS>-->

		  <TAXAMOUNT>
			  <xsl:value-of select="'TAX AMOUNT'"/>
		  </TAXAMOUNT>

		  <IMPACTNET>
			  <xsl:value-of select="'IMPACT NET'"/>
		  </IMPACTNET>

		 <SETTLECCY>
          <xsl:value-of select="'CURRENCY'"/>
        </SETTLECCY>

		 <TCODE1>
          <xsl:value-of select="'TCODE 1'"/>
        </TCODE1>
		  <DESC1>
			  <xsl:value-of select="'DESC1'"/>
		  </DESC1>

		<PUTCALL>
          <xsl:value-of select="'PUTCALL'"/>
        </PUTCALL>

		  <STRIKE>
          <xsl:value-of select="'STRIKE'"/>
        </STRIKE>

		  <EXPDATE>
          <xsl:value-of select="'EXP DATE'"/>
        </EXPDATE>

		 <PORTFOLIOSWAP>
          <xsl:value-of select="'PORTFOLIO SWAP'"/>
        </PORTFOLIOSWAP>

		  <PREFIGUREDPRINCIPAL>
			  <xsl:value-of select="'PREFIGUREDPRINCIPAL'"/>
		  </PREFIGUREDPRINCIPAL>

		  <EXCHNGRATE>
          <xsl:value-of select="'EXCHNG RATE'"/>
        </EXCHNGRATE>

		  <!--<STRATEGY>
          <xsl:value-of select="'STRATEGY'"/>
        </STRATEGY>-->

		  <SECCLEARANCECODE>
          <xsl:value-of select="'SEC CLEARANCE CODE'"/>
        </SECCLEARANCECODE>
        
     
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
  
      </ThirdPartyFlatFileDetail>


      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">
        <ThirdPartyFlatFileDetail>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<TaxLotState>
				<xsl:value-of select="TaxLotState"/>
			</TaxLotState>
			
			<FileHeader>
				<xsl:value-of select ="'true'"/>
			</FileHeader>
			
			<FileFooter>
				<xsl:value-of select ="'true'"/>
			</FileFooter>
         

			<xsl:variable name="varMethod">
				<xsl:choose>
					<xsl:when test="IsSwapped='true' and Asset = 'Equity'">
					<xsl:value-of select ="'SWAP'"/>
				    </xsl:when>
					<xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">						
						<xsl:value-of select ="'INTL'"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity'">
						<xsl:value-of select="'PRIME'"/>
					</xsl:when>
					<xsl:when test="Asset = 'EquityOption'">
						<xsl:value-of select="'CMTA'"/>
					</xsl:when>					
					<xsl:when test="(Asset = 'PrivateEquity' or Asset = 'FixedIncome') and CurrencySymbol!='USD'">
						<xsl:value-of select="'INTL'"/>
					</xsl:when>					
					
					<xsl:when test="(Asset = 'PrivateEquity' or Asset = 'FixedIncome') and CurrencySymbol ='USD'">
						<xsl:value-of select="'PRIME'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<TRADEID>
				<xsl:value-of select="concat($varMethod,PBUniqueID)"/>
			</TRADEID>


			<xsl:variable name="varTaxlotStateTx">
				<xsl:choose>
					<xsl:when test="TaxLotState='Allocated'">
						<xsl:value-of select ="'NEW'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Amended'">
						<xsl:value-of select ="'MODIFY'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Deleted'">
						<xsl:value-of select ="'CANCEL'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<ACTION>
				<xsl:value-of select="$varTaxlotStateTx"/>
			</ACTION>

			<TRADEDATE>
				<xsl:value-of select="TradeDate"/>
			</TRADEDATE>

			<SETTLEDATE>
				<xsl:value-of select="SettlementDate"/>
				<!--<xsl:choose>
					<xsl:when test="$varMethod='PRIME'">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="SettlementDate"/>
					</xsl:otherwise>
				</xsl:choose>-->
				
			</SETTLEDATE>


			<xsl:variable name="varAccountName">
				<xsl:choose>
					<xsl:when test="Asset='Equity' and IsSwapped='true'">
						<xsl:value-of select="'31750468'"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="'10238924'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<ACCOUNT>
				<!--<xsl:value-of select="AccountNo"/>-->
				<xsl:value-of select="$varAccountName"/>
			</ACCOUNT>

			<METHOD>
				<xsl:value-of select="$varMethod"/>
			</METHOD>


			<xsl:variable name="Sidevar">
				<xsl:choose>
					<xsl:when test="Side='Buy'">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:when test="Side='Sell'">
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:when test="Side='Sell short'">
						<xsl:value-of select="'SS'"/>
					</xsl:when>
					<xsl:when test="(Side='Buy to Cover' or Side='Buy to Close') and Asset = 'Equity'">
						<xsl:value-of select="'BC'"/>
					</xsl:when>
					<xsl:when test="Side='Buy to Close' and Asset = 'EquityOption'">
						<xsl:value-of select="'BC'"/>
					</xsl:when>
					<xsl:when test="Side='Sell to Open'">
						<xsl:value-of select="'SS'"/>
					</xsl:when>
					<xsl:when test="Side='Sell to Close'">
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:when test="Side='Buy to Open'">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<SIDE>
				<xsl:value-of select="$Sidevar"/>
			</SIDE>


			<xsl:variable name="underlyingBlanks">
				<xsl:call-template name="noofBlanks">
					<xsl:with-param name="count1" select="(6-string-length(UnderlyingSymbol))"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="varUnderlying">
				<xsl:value-of select="concat(UnderlyingSymbol,$underlyingBlanks)"/>
			</xsl:variable>

			<xsl:variable name="expirationDate">
				<xsl:value-of select="concat(substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
			</xsl:variable>

			<xsl:variable name="putCall">
				<xsl:value-of select="substring(PutOrCall,1,1)"/>
			</xsl:variable>

			<xsl:variable name="intStrike">
				<xsl:value-of select="substring-before(StrikePrice,'.')"/>
			</xsl:variable>

			<xsl:variable name="decimalStrike">
				<xsl:value-of select="concat(substring-after(Symbol,'.'),'0')"/>
			</xsl:variable>

			<xsl:variable name="intStrikeZeros">
				<xsl:call-template name="noofzeros">
					<xsl:with-param name="count" select="(5-string-length($intStrike))"/>
				</xsl:call-template>
			</xsl:variable>

			<xsl:variable name="varStrikePrice">
				<xsl:value-of select="concat($intStrikeZeros, $intStrike, $decimalStrike)"/>
			</xsl:variable>

			<xsl:variable name="varOSISymbolBefore">
				<xsl:value-of select="substring-before(OSIOptionSymbol,' ')"/>
			</xsl:variable>

			<xsl:variable name="varOSISymbolAfter">
				<xsl:value-of select="normalize-space(substring-after(OSIOptionSymbol,' '))"/>
			</xsl:variable>

			<xsl:variable name="varOSIMonth">
				<xsl:value-of select="substring($varOSISymbolAfter,3,2)"/>
			</xsl:variable>

			<xsl:variable name="varOSIDay">
				<xsl:value-of select="substring($varOSISymbolAfter,5,2)"/>
			</xsl:variable>

			<xsl:variable name="varOSIYear">
				<xsl:value-of select="substring($varOSISymbolAfter,1,2)"/>
			</xsl:variable>
			
			<xsl:variable name="varOSIDateSymbol">
				<xsl:value-of select="concat($varOSIMonth,$varOSIDay,$varOSIYear)"/>
			</xsl:variable>

			<xsl:variable name="varOSIPutStrick">
				<xsl:value-of select="substring($varOSISymbolAfter,7)"/>
			</xsl:variable>
			
			<xsl:variable name="varOSISymbol">
				<xsl:value-of select="concat($varOSISymbolBefore,$varOSIDateSymbol,$varOSIPutStrick)"/>
			</xsl:variable>

			<xsl:variable name="varSymbol">

				<xsl:choose>
					<xsl:when test="Asset = 'Equity' and SEDOL != ''">
						<!--<xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity'  and SEDOL != ''">-->
						<xsl:value-of select="SEDOL"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity'  and SEDOL = ''">
						<!--<xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity'  and SEDOL != ''">-->
						<xsl:value-of select="BBCode"/>
					</xsl:when>
					<xsl:when test="Asset = 'FixedIncome' and CUSIP != ''">
						<xsl:value-of select="CUSIP"/>
					</xsl:when>
					<xsl:when test="Asset = 'PrivateEquity' and CUSIP != ''">
						<xsl:value-of select="CUSIP"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="Asset = 'EquityOption' and OSIOptionSymbol != ''">
								<xsl:value-of select="OSIOptionSymbol"/>
								<!--<xsl:value-of select="concat($varOSISymbolBefore,$varOSISymbolAfter)"	/>-->
							</xsl:when>
							<xsl:when test="Asset = 'EquityOption' and OSIOptionSymbol = ''">
								<xsl:value-of select="concat($varUnderlying, $expirationDate, $putCall, StrikePrice)"/>
								<!--<xsl:value-of select="concat($varOSISymbolBefore,$varOSISymbolAfter)"	/>-->
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<SECURITY>
				<xsl:value-of select="$varSymbol"/>
			</SECURITY>


			<SECID>
				<xsl:choose>
					<xsl:when test="SEDOL != ''">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="CUSIP != '' and Asset='FixedIncome'">
						<xsl:value-of select="''"/>
					</xsl:when>
	
					<xsl:when test="SEDOL = ''">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</SECID>
			

			<QUANTITY>
				<xsl:value-of select="AllocatedQty"/>
			</QUANTITY>

			<PRICE>
				<!--<xsl:value-of select="AveragePrice"/>-->
				<xsl:choose>
					<xsl:when test="Asset='Equity' and IsSwapped='true'">
						<xsl:value-of select="format-number((NetAmount div AllocatedQty),'0.######')"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="AveragePrice"/>
					</xsl:otherwise>
				</xsl:choose>
			</PRICE>

			<xsl:variable name="Commission">
				<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
			</xsl:variable>

			<xsl:variable name="varFXRate">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:value-of select="FXRate_Taxlot"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="1"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="Commission1">
				<xsl:choose>
					<xsl:when test="$varFXRate=0">
						<xsl:value-of select="format-number($Commission,'##.00')"/>
					</xsl:when>
					<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
						<xsl:value-of select="format-number($Commission * $varFXRate,'##.00')"/>
					</xsl:when>

					<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
						<xsl:value-of select="format-number($Commission div $varFXRate,'##.00')"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="''"/>

					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varCommissionAmount">
				<xsl:choose>
					<xsl:when test="Asset = 'Equity' or Asset = 'EquityOption'">
						<xsl:value-of select="$Commission1"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<COMMTYPE>
				<xsl:choose>
					<xsl:when test="CommissionCharged !='' and CommissionCharged !=0">
						<xsl:value-of select ="'TOTAL'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>

			</COMMTYPE>

			<xsl:variable name="varTotalCom">
				<xsl:value-of select="CounterParty"/>
			</xsl:variable>
			<COMM>
				
				<xsl:choose>
					<xsl:when test="Asset='Equity' and IsSwapped='true'">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="CommissionCharged !='' and CommissionCharged !=0">
						<xsl:value-of select="CommissionCharged"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
				<!--<xsl:value-of select ="CommissionCharged + TaxOnCommissions + OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>-->
			</COMM>

			<INTEREST>
				<xsl:choose>
					<xsl:when test="CurrencySymbol != 'USD'">
						<xsl:value-of select="format-number(AccruedInterest,'#.##')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
				
				<!--<xsl:value-of select="format-number(AccruedInterest,'#.##')"/>-->
			</INTEREST>


			<xsl:variable name="PRANA_COUNTERPARTY">
				<xsl:value-of select="CounterParty"/>
			</xsl:variable>

			<xsl:variable name="PB_COUNTERPARTY">
				<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = 'GS']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@MLPBroker"/>
			</xsl:variable>

			<xsl:variable name="varCounterParty">
				<xsl:choose>
					<xsl:when test="$PB_COUNTERPARTY = ''">
						<xsl:value-of select="$PRANA_COUNTERPARTY"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PB_COUNTERPARTY"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			 
			<!--<WI>
				<xsl:value-of select="'1'"/>
			</WI>-->
			
			<EXECBRKR>
				<xsl:value-of select="$varCounterParty"/>
			</EXECBRKR>

			<TAXAMOUNT>
				<xsl:choose>
					<xsl:when test="Asset='Equity' and IsSwapped='true'">
						<xsl:value-of select="''"/>
					</xsl:when>
					
					<xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions) &gt;0 ">			
				     <xsl:value-of select="format-number((StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions),'0.##')"/>
						<!--<xsl:value-of select="format-number((StampDuty + SecFee + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions),'0.##')"/>-->				
					</xsl:when>
				<xsl:otherwise>
				<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
			</TAXAMOUNT>
			
			<IMPACTNET>
				<!--<xsl:choose>
				<xsl:when test="Asset='Equity' and IsSwapped='true'">
					<xsl:value-of select="''"/>
				</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Y'"/>
					</xsl:otherwise>
				</xsl:choose>-->
				<!--<xsl:value-of select="'Y'"/>-->
				<xsl:choose>
					<xsl:when test="Asset='Equity' and IsSwapped='true'">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="CurrencySymbol != 'USD'">
						<xsl:value-of select="'Y'"/>
					</xsl:when>					
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</IMPACTNET>

			<TCODE1>
				<xsl:value-of select="''"/>
			</TCODE1>
			
			<DESC1>
				<xsl:value-of select="''"/>
			</DESC1>
			
			<xsl:variable name="varSettleCurrency">
				<xsl:choose>					
					<xsl:when test="(Asset = 'PrivateEquity' or Asset = 'FixedIncome' or Asset='Equity') and CurrencySymbol!='USD'">
						<xsl:value-of select="SettlCurrency"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			
			<SETTLECCY>
				<xsl:value-of select="SettlCurrency"/>
				<!--<xsl:value-of select="$varSettleCurrency"/>-->
			</SETTLECCY>

			<PUTCALL>
				<xsl:value-of select="''"/>
			</PUTCALL>

			<STRIKE>
			<xsl:value-of select="''"/>
			</STRIKE>

			<EXPDATE>
				<xsl:value-of select="''"/>				
			</EXPDATE>

			<PORTFOLIOSWAP>
				<!--<xsl:value-of select="''"/>-->
				<xsl:choose>
					<xsl:when test="Asset='Equity' and IsSwapped='true'">
						<xsl:value-of select="concat('PQN_',SettlCurrency)"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</PORTFOLIOSWAP>

			<PREFIGUREDPRINCIPAL>
				<xsl:value-of select="''"/>
			</PREFIGUREDPRINCIPAL>

			<EXCHNGRATE>
				<!--<xsl:value-of select="''"/>-->
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:value-of select="FXRate_Taxlot"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</EXCHNGRATE>

			<!--<STRATEGY>
				<xsl:value-of select="''"/>
			</STRATEGY>-->

			<SECCLEARANCECODE>
				<xsl:choose>
			
						<xsl:when test="Asset='Equity' and IsSwapped='true'">
							<xsl:value-of select="''"/>
						</xsl:when>

					<xsl:when test="CurrencySymbol ='USD'">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="CurrencySymbol !='USD'">
								<xsl:choose>
									<xsl:when test="SettlCurrency='EUR'">
										<xsl:value-of select="'E'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'L'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						</xsl:choose>						
					</xsl:otherwise>
				</xsl:choose>

			</SECCLEARANCECODE>
			
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template> 

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>  
</xsl:stylesheet>

