<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public int Now(string date)
		{
		DateTime d1 = DateTime.Parse(date);
		DateTime d2 = DateTime.Today;

		int result = DateTime.Compare(d1,d2);
		return result;
		}

	</msxsl:script>



	
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

  

	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      
      <ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<trddate>
						<xsl:value-of select="'trddate'"/>
					</trddate>

					<setdate>
						<xsl:value-of select="'setdate'"/>
					</setdate>

					<SYCODE>
						<xsl:value-of select="'SYCODE'"/>
					</SYCODE>

					<TRXTYPE>
						<xsl:value-of select="'TRXTYPE'"/>
					</TRXTYPE>

					<qty>
						<xsl:value-of select="'qty'"/>
					</qty>

					<PRICE>
						<xsl:value-of select="'PRICE'"/>
					</PRICE>
					
					<Commission>
						<xsl:value-of select="'Commission'"/>
					</Commission>

					<SettlementCCY>
						<xsl:value-of select="'SettlementCCY'"/>
					</SettlementCCY>

					<ContraParty>
						<xsl:value-of select="'CounterParty'"/>
					</ContraParty>

					<Exchange>
						<xsl:value-of select="'Exchange'"/>
					</Exchange>
					
					<CLRBRKRACCT>
						<xsl:value-of select="'CLRBRKRACCT'"/>
					</CLRBRKRACCT>

					<SettleFXRate>
						<xsl:value-of select="'SettleFXRate'"/>
					</SettleFXRate>

					<evreference>
						<xsl:value-of select="'evreference'"/>
					</evreference>

					<CBFee>
						<xsl:value-of select="'CBFee'"/>
					</CBFee>

					<ExFee>
						<xsl:value-of select="'ExFee'"/>
					</ExFee>

					<Interest>
						<xsl:value-of select="'Interest'"/>
					</Interest>
					
					<Ofee>
						<xsl:value-of select="'Ofee'"/>							
					</Ofee>

					<SecFee>
						<xsl:value-of select="'SecFee'"/>				
					</SecFee>

					<NetProceeds>
						<xsl:value-of select="'NetProceeds'"/>
					</NetProceeds>

					<PositionCCY>
						<xsl:value-of select="'PositionCCY'"/>
					</PositionCCY>

					<PosFXRate>
						<xsl:value-of select="'PosFXRate'"/>
					</PosFXRate>

					<Blank>
						<xsl:value-of select="'Blank'"/>
					</Blank>

					<strategy>
					  <xsl:value-of select="'strategy'"/>
					</strategy>

					<FNID>
						<xsl:value-of select="'FNID'"/>
					</FNID>

					<CPS>
						<xsl:value-of select="'CPS'"/>
					</CPS>

					<bips>
						<xsl:value-of select="'bips'"/>
					</bips>

					<Status>
						<xsl:value-of select="'Status'"/>
					</Status>

					<bareference>
						<xsl:value-of select="'bareference'"/>
					</bareference>

					<OwnerTrader>
						<xsl:value-of select="'OwnerTrader'"/>
					</OwnerTrader>

					<SoftCommPct>
						<xsl:value-of select="'SoftCommPct'"/>
					</SoftCommPct>

					<DealDescription>
						<xsl:value-of select="'DealDescription'"/>					
					</DealDescription>

					<DealRate>
						<xsl:value-of select="'DealRate'"/>
					</DealRate>

					<Giveupbrokercode>
						<xsl:value-of select="'Giveupbrokercode'"/>
					</Giveupbrokercode>

					<Giveupcmmsntypecode>
						<xsl:value-of select="'Giveupcmmsntypecode'"/>
					</Giveupcmmsntypecode>

					<GiveUpCommRate>
						<xsl:value-of select="'GiveUpCommRate'"/>
					</GiveUpCommRate>

					<GiveUpCommAmt>
						<xsl:value-of select="'GiveUpCommAmt'"/>
					</GiveUpCommAmt>

					<GovtFees>
						<xsl:value-of select="'GovtFees'"/>
					</GovtFees>

					<Remarks>
						<xsl:value-of select="'Remarks'"/>
					</Remarks> 	
					
					<EVType>
						<xsl:value-of select="'EVType'"/>
					</EVType>

					<TermDate>
						<xsl:value-of select="'TermDate'"/>
					</TermDate>

					<ExcludeOtherFeesfromNetProceeds>
						<xsl:value-of select="'ExcludeOtherFeesfromNetProceeds'"/>
					</ExcludeOtherFeesfromNetProceeds>

					<ExcludeOtherFeesfromBrCash>
						<xsl:value-of select="'ExcludeOtherFeesfromBrCash'"/>
					</ExcludeOtherFeesfromBrCash>

					<ExcludeCommissionfromProceeds>
						<xsl:value-of select="'ExcludeCommissionfromProceeds'"/>
					</ExcludeCommissionfromProceeds>

					<GiveUpBrokerCommPostingRule>
						<xsl:value-of select="'GiveUpBrokerCommPostingRule'"/>
					</GiveUpBrokerCommPostingRule>

					<CommTypeCode>
						<xsl:value-of select="'FLAT'"/>
					</CommTypeCode>

					<Route>
						<xsl:value-of select="'Route'"/>
					</Route>

					<UploadStatus>
						<xsl:value-of select="'UploadStatus'"/>
					</UploadStatus>

					<PairOffMethod>
						<xsl:value-of select="'PairOffMethod'"/>
					</PairOffMethod>

					<UDCNamesValues>
						<xsl:value-of select="'UDCNamesValues'"/>
					</UDCNamesValues>

					<TargetSettlement>
						<xsl:value-of select="'TargetSettlement'"/>
					</TargetSettlement>

					<ContractType>
						<xsl:value-of select="'ContractType'"/>
					</ContractType>
					
					<!-- <Ticker> -->
						<!-- <xsl:value-of select="'Ticker'"/> -->
					<!-- </Ticker> -->
					
					<!-- <Assets> -->
						<!-- <xsl:value-of select="'Assets'"/> -->
					<!-- </Assets> -->
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			 
			 <xsl:for-each select="ThirdPartyFlatFileDetail[TaxLotState!='Sent'][Symbol !='JPM PRIME MMKT-INST'][AccountName !='Pleasant Lake - 3E720023' and AccountName !='First Republic - PLP'  and AccountName !='Interactive Broker - PLP'  and AccountName !='PLP Funds Concentrated LP - 3E720079'  and AccountName !='Cassini'  and AccountName !='PLP Funds Concentrated LP-JPM'  and AccountName !='Blackstone'  and AccountName !='PLAKE_BEMAP']">	
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
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

				
              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
             <trddate>
                <xsl:value-of select="concat(substring-before($varTradeDate,'/'),'/',substring-before(substring-after($varTradeDate,'/'),'/'),'/', substring-after(substring-after($varTradeDate,'/'),'/'))"/>
              </trddate>
			  	
              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
					<setdate>
						<xsl:value-of select="concat(substring-before($varSettlementDate,'/'),'/',substring-before(substring-after($varSettlementDate,'/'),'/'),'/', substring-after(substring-after($varSettlementDate,'/'),'/'))"/>
					</setdate>

					<SYCODE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="normalize-space(OSIOptionSymbol)"/>
							</xsl:when>
							
							<xsl:when test ="Asset='Equity'">
								<xsl:value-of select="concat(Symbol,' ',CurrencySymbol)"/>
							</xsl:when>
							
							<!-- <xsl:when test ="Asset='Equity'"> -->
								<!-- <xsl:value-of select="concat(substring-after(substring-after(Symbol,' CurrencySymbol'),' CurrencySymbol'),'-',substring-before(Symbol,' CurrencySymbol'),'-',substring-before(substring-after(Symbol,' CurrencySymbol'),' CurrencySymbol'))"/> -->
							<!-- </xsl:when> -->
							
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SYCODE>

					<TRXTYPE>
						<xsl:choose>
							<xsl:when test="Side='Buy' ">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' ">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' ">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CVS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Open'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Open'">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close'">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRXTYPE>

					<qty>
						<xsl:choose>
							<xsl:when test="number(CumQty)">
								<xsl:value-of select="format-number(CumQty,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</qty>
			<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
												<xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AvgPrice"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varPrice">
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varSettFxAmt"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
					<PRICE>
						<xsl:value-of select="format-number($varPrice,'0.########')"/>
					</PRICE>
						
					<!--<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged + TaxOnCommissions"/>
					</xsl:variable>-->
				<xsl:variable name="varCommission" select="(CommissionCharged + SoftCommissionCharged)"/>
					 <!--commission-->
			<xsl:variable name="varFXComm">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="$varCommission * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varCommission div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varCommission"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varComm">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="$varCommission"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXComm"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--commission-->
					<Commission>
						<xsl:value-of select="$varComm"/>
					</Commission>
					
					<SettlementCCY>
						<xsl:value-of select="SettlCurrency"/>
					</SettlementCCY>

					<ContraParty>
						<xsl:value-of select="CounterParty"/>
					</ContraParty>

				
					<Exchange>
						<xsl:value-of select="''"/>
					</Exchange>

					<xsl:variable name="PB_NAME" select="'OPUS'"/>
					
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFundName=$PRANA_FUND_NAME]/@PBFundName"/>
					</xsl:variable>

					<CLRBRKRACCT>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</CLRBRKRACCT>

					<SettleFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
														
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettleFXRate>

					<evreference>
						  <xsl:choose>
				<xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="concat(PBUniqueID,position())"/>
                  </xsl:when>
                
                  <xsl:otherwise>
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:otherwise>
                </xsl:choose>
					</evreference>

					<CBFee>
						<xsl:value-of select="''"/>
					</CBFee>

					<ExFee>
						<xsl:value-of select="''"/>
					</ExFee>
 <!--AccruedInterest-->
			<xsl:variable name="varFXAccInt">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="AccruedInterest * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccruedInterest div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="AccruedInterest"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varAccruedInterest">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="AccruedInterest"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXAccInt"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--AccruedInterest-->
					<Interest>
						<xsl:value-of select="$varAccruedInterest"/>
					</Interest>

					
					
							<!--Ofee-->
			<xsl:variable name="varFXOfee">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="(OtherBrokerFees + ClearingBrokerFee + ClearingFee) * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="(OtherBrokerFees + ClearingBrokerFee + ClearingFee) div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + ClearingFee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varOfee">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + ClearingFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXOfee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--Ofee-->
				
					
					<Ofee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($varOfee)">
										<xsl:value-of select="$varOfee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</Ofee>

					<!--SecFee-->
			<xsl:variable name="varFXASecfee">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="SecFee * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SecFee div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="SecFee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varSecFee">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="SecFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXASecfee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--SecFee-->
					<SecFee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($varSecFee)">
										<xsl:value-of select="$varSecFee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</SecFee>
			
							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varSettlementAmount">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varNetamount * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
					<NetProceeds>
						
		  <xsl:choose>
		  <xsl:when test="Side='Buy' or Side='Buy to Close' and Asset='FixedIncome'">
            <xsl:value-of select="$varSettlementAmount + AccruedInterest"/>
			</xsl:when>
			<xsl:when test="Side='Sell' or Side='Sell short' and Asset='FixedIncome'">
			<xsl:value-of select="$varSettlementAmount - AccruedInterest"/>
			</xsl:when>
			<xsl:otherwise>
			<xsl:value-of select="$varSettlementAmount"/>
			</xsl:otherwise>
			</xsl:choose>
          
					</NetProceeds>

					<PositionCCY>
						<xsl:value-of select="CurrencySymbol"/>
					</PositionCCY>

					<PosFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
														
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
										
					</PosFXRate>

					<Blank>
						<xsl:value-of select="''"/>
					</Blank>

					<strategy>
					<xsl:value-of select="'A-A'"/>
					</strategy>

					<FNID>
						<xsl:value-of select="''"/>
					</FNID>

					<CPS>
						<xsl:value-of select="''"/>
					</CPS>

					<bips>
						<xsl:value-of select="''"/>
					</bips>

					<Status>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Status>

					<bareference>
					  <xsl:choose>
						  <xsl:when test ="Symbol='DBMXF'">
							  <xsl:value-of select="CompanyName"/>
						  </xsl:when>
						  
						  <xsl:when test ="Asset='EquityOption'">
							  <xsl:value-of select="OSIOptionSymbol"/>
						  </xsl:when>

						  <xsl:when test ="Asset='Equity'">
							  <xsl:value-of select="SEDOL"/>
						  </xsl:when>
						  
						  <xsl:when test ="Asset='FixedIncome'">
							  <xsl:value-of select="CompanyName"/>
						  </xsl:when>
						  
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
                      </xsl:choose>
					</bareference>

					<OwnerTrader>
						<xsl:value-of select="''"/>
					</OwnerTrader>

					<SoftCommPct>
						<xsl:value-of select="''"/>
					</SoftCommPct>

					<DealDescription>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'Eternal Equity Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</DealDescription>

					<DealRate>
						<xsl:value-of select="0"/>
					</DealRate>

					<Giveupbrokercode>
						<xsl:value-of select="''"/>
					</Giveupbrokercode>

					<Giveupcmmsntypecode>
						<xsl:value-of select="''"/>
					</Giveupcmmsntypecode>

					<GiveUpCommRate>
						<xsl:value-of select="0"/>
					</GiveUpCommRate>

					<GiveUpCommAmt>
						<xsl:value-of select="0"/>
					</GiveUpCommAmt>

					<GovtFees>
						<xsl:value-of select="0"/>
					</GovtFees>

					<Remarks>
						  <xsl:choose>
				<xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="concat(PBUniqueID,position())"/>
                  </xsl:when>
                
                  <xsl:otherwise>
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:otherwise>
                </xsl:choose>
					</Remarks> 	
					
					<EVType>
						<xsl:value-of select="'TRD'"/>
					</EVType>

					<TermDate>
						<xsl:value-of select="''"/>
					</TermDate>

					<ExcludeOtherFeesfromNetProceeds>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromNetProceeds>

					<ExcludeOtherFeesfromBrCash>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromBrCash>

					<ExcludeCommissionfromProceeds>
						<xsl:value-of select="''"/>
					</ExcludeCommissionfromProceeds>

					<GiveUpBrokerCommPostingRule>
						<xsl:value-of select="''"/>
					</GiveUpBrokerCommPostingRule>

					<CommTypeCode>
						<xsl:value-of select="'FLAT'"/>
					</CommTypeCode>

					<Route>
						<xsl:value-of select="''"/>
					</Route>

					<UploadStatus>
						<xsl:value-of select="''"/>
					</UploadStatus>

					<PairOffMethod>
						<xsl:value-of select="''"/>
					</PairOffMethod>

					<UDCNamesValues>
						<xsl:value-of select="''"/>
					</UDCNamesValues>

					<TargetSettlement>
						<xsl:value-of select="''"/>
					</TargetSettlement>

					<ContractType>
						<xsl:value-of select="''"/>
					</ContractType>
					
					<!-- <Ticker> -->
						<!-- <xsl:value-of select="Symbol"/> -->
					<!-- </Ticker> -->
					
					<!-- <Assets> -->
						<!-- <xsl:value-of select="Asset"/> -->
					<!-- </Assets> -->
			
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:when>

          <xsl:otherwise>	
			     <xsl:if test ="number(OldExecutedQuantity)">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="'Deleted'"/>
					</TaxLotState>

				    <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="OldTradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
             <trddate>
                <xsl:value-of select="concat(substring-before($varTradeDate,'/'),'/',substring-before(substring-after($varTradeDate,'/'),'/'),'/', substring-after(substring-after($varTradeDate,'/'),'/'))"/>
              </trddate>
			  	
              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="OldSettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
					<setdate>
						<xsl:value-of select="concat(substring-before($varSettlementDate,'/'),'/',substring-before(substring-after($varSettlementDate,'/'),'/'),'/', substring-after(substring-after($varSettlementDate,'/'),'/'))"/>
					</setdate>
					<SYCODE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="normalize-space(OSIOptionSymbol)"/>
							</xsl:when>
							
							<xsl:when test ="Asset='Equity'">
								<xsl:value-of select="concat(Symbol,' ',CurrencySymbol)"/>
							</xsl:when>
							
							<!-- <xsl:when test ="Asset='Equity'"> -->
								<!-- <xsl:value-of select="concat(substring-after(substring-after(Symbol,' CurrencySymbol'),' CurrencySymbol'),'-',substring-before(Symbol,' CurrencySymbol'),'-',substring-before(substring-after(Symbol,' CurrencySymbol'),' CurrencySymbol'))"/> -->
							<!-- </xsl:when> -->
							
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SYCODE>

					<TRXTYPE>
						<xsl:choose>
							<xsl:when test="Side='Buy' ">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' ">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' ">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CVS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Open'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Open'">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close'">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRXTYPE>

					<qty>
						<xsl:choose>
							<xsl:when test="number(OldExecutedQuantity)">
								<xsl:value-of select="format-number(OldExecutedQuantity,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</qty>
					
								<xsl:variable name="varSettFxAmt">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != CurrencySymbol">
											<xsl:choose>
												<xsl:when test="OldFXConversionMethodOperator ='M'">
													<xsl:value-of select="OldAvgPrice * OldFXRate"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="OldAvgPrice div OldFXRate"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varPrice">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency = CurrencySymbol">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varSettFxAmt"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

					<PRICE>
						<xsl:value-of select="format-number($varPrice,'0.########')"/>
					</PRICE>
					
					<xsl:variable name="COMM">
						<xsl:value-of select="OldCommission + OldSoftCommission + OldTaxOnCommissions"/>
					</xsl:variable>
				
					 <!--commission-->
			<xsl:variable name="varFXComm">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="$COMM * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$COMM div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$COMM"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varComm">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency = CurrencySymbol">
						<xsl:value-of select="$COMM"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXComm"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--commission-->
					<Commission>
						<xsl:value-of select="$varComm"/>
					</Commission>
					
					<SettlementCCY>
						<xsl:value-of select="OldSettlCurrency"/>
					</SettlementCCY>
					
					

					<ContraParty>
						<xsl:value-of select="OldCounterparty"/>
					</ContraParty>

				
					<Exchange>
						<xsl:value-of select="''"/>
					</Exchange>

					<xsl:variable name="PB_NAME" select="'Opus'"/>
					
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFundName=$PRANA_FUND_NAME]/@PBFundName"/>
					</xsl:variable>

					<CLRBRKRACCT>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</CLRBRKRACCT>

					<SettleFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>														
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettleFXRate>

					<evreference>
						<xsl:value-of select="PBUniqueID"/>
					</evreference>

					<CBFee>
						<xsl:value-of select="''"/>
					</CBFee>

					<ExFee>
						<xsl:value-of select="''"/>
					</ExFee>

					 <!--AccruedInterest-->
			<xsl:variable name="varFXAccInt">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="OldAccruedInterest * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="OldAccruedInterest div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="OldAccruedInterest"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varAccruedInterest">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency = CurrencySymbol">
						<xsl:value-of select="OldAccruedInterest"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXAccInt"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--AccruedInterest-->
					<Interest>
						<xsl:value-of select="$varAccruedInterest"/>
					</Interest>

						<!--Ofee-->
			<xsl:variable name="varFXOfee">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="(OldOtherBrokerFees + OldClearingBrokerFee + OldClearingFee) * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="(OldOtherBrokerFees + OldClearingBrokerFee + OldClearingFee) div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldClearingFee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varOfee">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency = CurrencycSymbol">
						<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldClearingFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXOfee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--Ofee-->
				
					
					<Ofee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($varOfee)">
										<xsl:value-of select="$varOfee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</Ofee>

				<!--SecFee-->
			<xsl:variable name="varFXASecfee">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="OldSecFee * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="OldSecFee div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="OldSecFee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varSecFee">
				<xsl:choose>
					<xsl:when test="OldSettlCurrency = CurrencySymbol">
						<xsl:value-of select="OldSecFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXASecfee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--SecFee-->
					<SecFee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($varSecFee)">
										<xsl:value-of select="$varSecFee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</SecFee>

		<xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
					
				<xsl:variable name="varFXRate">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != CurrencySymbol">
											<xsl:value-of select="OldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varSettlementAmount">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='M'">
											<xsl:value-of select="$varOldNetAmount * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='D'">
											<xsl:value-of select="$varOldNetAmount div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
					<NetProceeds>						
		  <xsl:choose>
		  <xsl:when test="OldSide='Buy' or OldSide='Buy to Close' and Asset='FixedIncome'">
            <xsl:value-of select="$varSettlementAmount + OldAccruedInterest"/>
			</xsl:when>
			<xsl:when test="OldSide='Sell' or OldSide='Sell short' and Asset='FixedIncome'">
			<xsl:value-of select="$varSettlementAmount - OldAccruedInterest"/>
			</xsl:when>
			<xsl:otherwise>
			<xsl:value-of select="$varSettlementAmount"/>
			</xsl:otherwise>
			</xsl:choose>          
					</NetProceeds>

					<PositionCCY>
						<xsl:value-of select="CurrencySymbol"/>
					</PositionCCY>

					<PosFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
														
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
										
					</PosFXRate>

					<Blank>
						<xsl:value-of select="''"/>
					</Blank>

					<strategy>
					<xsl:value-of select="'A-A'"/>
					</strategy>

					<FNID>
						<xsl:value-of select="''"/>
					</FNID>

					<CPS>
						<xsl:value-of select="''"/>
					</CPS>

					<bips>
						<xsl:value-of select="''"/>
					</bips>

					<Status>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Status>

					<bareference>
						<xsl:choose>
							<xsl:when test ="Symbol='DBMXF'">
								<xsl:value-of select="CompanyName"/>
							</xsl:when>

							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>

							<xsl:when test ="Asset='Equity'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>

							<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select="CompanyName"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</bareference>

					<OwnerTrader>
						<xsl:value-of select="''"/>
					</OwnerTrader>

					<SoftCommPct>
						<xsl:value-of select="''"/>
					</SoftCommPct>

					<DealDescription>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'PLP Equity Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</DealDescription>

					<DealRate>
						<xsl:value-of select="0"/>
					</DealRate>

					<Giveupbrokercode>
						<xsl:value-of select="''"/>
					</Giveupbrokercode>

					<Giveupcmmsntypecode>
						<xsl:value-of select="''"/>
					</Giveupcmmsntypecode>

					<GiveUpCommRate>
						<xsl:value-of select="0"/>
					</GiveUpCommRate>

					<GiveUpCommAmt>
						<xsl:value-of select="0"/>
					</GiveUpCommAmt>

					<GovtFees>
						<xsl:value-of select="0"/>
					</GovtFees>

					<Remarks>
						<xsl:value-of select="PBUniqueID"/>
					</Remarks> 	
					
					<EVType>
						<xsl:value-of select="'TRD'"/>
					</EVType>

					<TermDate>
						<xsl:value-of select="''"/>
					</TermDate>

					<ExcludeOtherFeesfromNetProceeds>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromNetProceeds>

					<ExcludeOtherFeesfromBrCash>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromBrCash>

					<ExcludeCommissionfromProceeds>
						<xsl:value-of select="''"/>
					</ExcludeCommissionfromProceeds>

					<GiveUpBrokerCommPostingRule>
						<xsl:value-of select="''"/>
					</GiveUpBrokerCommPostingRule>

					<CommTypeCode>
						<xsl:value-of select="'FLAT'"/>
					</CommTypeCode>

					<Route>
						<xsl:value-of select="''"/>
					</Route>

					<UploadStatus>
						<xsl:value-of select="''"/>
					</UploadStatus>

					<PairOffMethod>
						<xsl:value-of select="''"/>
					</PairOffMethod>

					<UDCNamesValues>
						<xsl:value-of select="''"/>
					</UDCNamesValues>

					<TargetSettlement>
						<xsl:value-of select="''"/>
					</TargetSettlement>

					<ContractType>
						<xsl:value-of select="''"/>
					</ContractType>
					
					<!-- <Ticker> -->
						<!-- <xsl:value-of select="Symbol"/> -->
					<!-- </Ticker> -->
					
					<!-- <Assets> -->
						<!-- <xsl:value-of select="Asset"/> -->
					<!-- </Assets> -->
			
					
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
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

				
              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
             <trddate>
                <xsl:value-of select="concat(substring-before($varTradeDate,'/'),'/',substring-before(substring-after($varTradeDate,'/'),'/'),'/', substring-after(substring-after($varTradeDate,'/'),'/'))"/>
              </trddate>
			  	
              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
					<setdate>
						<xsl:value-of select="concat(substring-before($varSettlementDate,'/'),'/',substring-before(substring-after($varSettlementDate,'/'),'/'),'/', substring-after(substring-after($varSettlementDate,'/'),'/'))"/>
					</setdate>

					<SYCODE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="normalize-space(OSIOptionSymbol)"/>
							</xsl:when>
							
							<xsl:when test ="Asset='Equity'">
								<xsl:value-of select="concat(Symbol,' ',CurrencySymbol)"/>
							</xsl:when>
							
							<!-- <xsl:when test ="Asset='Equity'"> -->
								<!-- <xsl:value-of select="concat(substring-after(substring-after(Symbol,' CurrencySymbol'),' CurrencySymbol'),'-',substring-before(Symbol,' CurrencySymbol'),'-',substring-before(substring-after(Symbol,' CurrencySymbol'),' CurrencySymbol'))"/> -->
							<!-- </xsl:when> -->
							
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SYCODE>

					<TRXTYPE>
						<xsl:choose>
							<xsl:when test="Side='Buy' ">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' ">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' ">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CVS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Open'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Open'">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close'">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRXTYPE>

					<qty>
						<xsl:choose>
							<xsl:when test="number(CumQty)">
								<xsl:value-of select="format-number(CumQty,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</qty>
				
				<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
												<xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AvgPrice"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varPrice">
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varSettFxAmt"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

					<PRICE>
						<xsl:value-of select="format-number($varPrice,'0.########')"/>
					</PRICE>
					
					<!--<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged + TaxOnCommissions"/>
					</xsl:variable>-->
				<xsl:variable name="varCommission" select="(CommissionCharged + SoftCommissionCharged)"/>
					 <!--commission-->
			<xsl:variable name="varFXComm">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="$varCommission * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varCommission div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varCommission"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varComm">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="$varCommission"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXComm"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--commission-->
					<Commission>
						<xsl:value-of select="$varComm"/>
					</Commission>

					<SettlementCCY>
						<xsl:value-of select="SettlCurrency"/>
					</SettlementCCY>

					<ContraParty>
						<xsl:value-of select="CounterParty"/>
					</ContraParty>

				
					<Exchange>
						<xsl:value-of select="''"/>
					</Exchange>

					<xsl:variable name="PB_NAME" select="'Opus'"/>
					
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFundName=$PRANA_FUND_NAME]/@PBFundName"/>
					</xsl:variable>

					<CLRBRKRACCT>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</CLRBRKRACCT>

					<SettleFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
														
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettleFXRate>

					<evreference>
						   <xsl:value-of select="concat(PBUniqueID,position())"/>
					</evreference>

					<CBFee>
						<xsl:value-of select="''"/>
					</CBFee>

					<ExFee>
						<xsl:value-of select="''"/>
					</ExFee>

				 <!--AccruedInterest-->
			<xsl:variable name="varFXAccInt">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="AccruedInterest * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccruedInterest div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="AccruedInterest"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varAccruedInterest">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="AccruedInterest"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXAccInt"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--AccruedInterest-->
					<Interest>
						<xsl:value-of select="$varAccruedInterest"/>
					</Interest>
					
							<!--Ofee-->
			<xsl:variable name="varFXOfee">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="(OtherBrokerFees + ClearingBrokerFee + ClearingFee) * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="(OtherBrokerFees + ClearingBrokerFee + ClearingFee) div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + ClearingFee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varOfee">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + ClearingFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXOfee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--Ofee-->
				
					
					<Ofee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($varOfee)">
										<xsl:value-of select="$varOfee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</Ofee>

					<!--SecFee-->
			<xsl:variable name="varFXASecfee">
				<xsl:choose>
					<xsl:when test="SettlCurrency != CurrencySymbol">
						<xsl:choose>
							<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
								<xsl:value-of select="SecFee * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SecFee div FXRate_Taxlot"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="SecFee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varSecFee">
				<xsl:choose>
					<xsl:when test="SettlCurrency = CurrencySymbol">
						<xsl:value-of select="SecFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varFXASecfee"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--SecFee-->
					<SecFee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number($varSecFee)">
										<xsl:value-of select="$varSecFee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</SecFee>

							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varSettlementAmount">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varNetamount * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
					<NetProceeds>
						
		  <xsl:choose>
		  <xsl:when test="Side='Buy' or Side='Buy to Close' and Asset='FixedIncome'">
            <xsl:value-of select="$varSettlementAmount + AccruedInterest"/>
			</xsl:when>
			<xsl:when test="Side='Sell' or Side='Sell short' and Asset='FixedIncome'">
			<xsl:value-of select="$varSettlementAmount - AccruedInterest"/>
			</xsl:when>
			<xsl:otherwise>
			<xsl:value-of select="$varSettlementAmount"/>
			</xsl:otherwise>
			</xsl:choose>
          
					</NetProceeds>

					<PositionCCY>
						<xsl:value-of select="CurrencySymbol"/>
					</PositionCCY>

					<PosFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
														
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
										
					</PosFXRate>

					<Blank>
						<xsl:value-of select="''"/>
					</Blank>

					<strategy>
					<xsl:value-of select="'A-A'"/>
					</strategy>

					<FNID>
						<xsl:value-of select="''"/>
					</FNID>

					<CPS>
						<xsl:value-of select="''"/>
					</CPS>

					<bips>
						<xsl:value-of select="''"/>
					</bips>

					<Status>
						<!-- <xsl:choose> -->
							<!-- <xsl:when test="TaxLotState='Allocated'"> -->
								<!-- <xsl:value-of select ="'NEW'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="TaxLotState='Amemded'"> -->
								<!-- <xsl:value-of select ="'NEW'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:when test="TaxLotState='Deleted'"> -->
								<!-- <xsl:value-of select ="'DELETE'"/> -->
							<!-- </xsl:when> -->
							<!-- <xsl:otherwise> -->
								<!-- <xsl:value-of select ="''"/> -->
							<!-- </xsl:otherwise> -->
						<!-- </xsl:choose> -->
						<xsl:value-of select ="'NEW'"/>
					</Status>

					<bareference>
						<xsl:choose>
							<xsl:when test ="Symbol='DBMXF'">
								<xsl:value-of select="CompanyName"/>
							</xsl:when>

							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>

							<xsl:when test ="Asset='Equity'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>

							<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select="CompanyName"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</bareference>

					<OwnerTrader>
						<xsl:value-of select="''"/>
					</OwnerTrader>

					<SoftCommPct>
						<xsl:value-of select="''"/>
					</SoftCommPct>

					<DealDescription>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'PLP Equity Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</DealDescription>

					<DealRate>
						<xsl:value-of select="0"/>
					</DealRate>

					<Giveupbrokercode>
						<xsl:value-of select="''"/>
					</Giveupbrokercode>

					<Giveupcmmsntypecode>
						<xsl:value-of select="''"/>
					</Giveupcmmsntypecode>

					<GiveUpCommRate>
						<xsl:value-of select="0"/>
					</GiveUpCommRate>

					<GiveUpCommAmt>
						<xsl:value-of select="0"/>
					</GiveUpCommAmt>

					<GovtFees>
						<xsl:value-of select="0"/>
					</GovtFees>

					<Remarks>
						   <xsl:value-of select="concat(PBUniqueID,position())"/>
					</Remarks> 	
					
					<EVType>
						<xsl:value-of select="'TRD'"/>
					</EVType>

					<TermDate>
						<xsl:value-of select="''"/>
					</TermDate>

					<ExcludeOtherFeesfromNetProceeds>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromNetProceeds>

					<ExcludeOtherFeesfromBrCash>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromBrCash>

					<ExcludeCommissionfromProceeds>
						<xsl:value-of select="''"/>
					</ExcludeCommissionfromProceeds>

					<GiveUpBrokerCommPostingRule>
						<xsl:value-of select="''"/>
					</GiveUpBrokerCommPostingRule>

					<CommTypeCode>
						<xsl:value-of select="'FLAT'"/>
					</CommTypeCode>

					<Route>
						<xsl:value-of select="''"/>
					</Route>

					<UploadStatus>
						<xsl:value-of select="''"/>
					</UploadStatus>

					<PairOffMethod>
						<xsl:value-of select="''"/>
					</PairOffMethod>

					<UDCNamesValues>
						<xsl:value-of select="''"/>
					</UDCNamesValues>

					<TargetSettlement>
						<xsl:value-of select="''"/>
					</TargetSettlement>

					<ContractType>
						<xsl:value-of select="''"/>
					</ContractType>
					
					<!-- <Ticker> -->
						<!-- <xsl:value-of select="Symbol"/> -->
					<!-- </Ticker> -->
					
					<!-- <Assets> -->
						<!-- <xsl:value-of select="Asset"/> -->
					<!-- </Assets> -->
			
					
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