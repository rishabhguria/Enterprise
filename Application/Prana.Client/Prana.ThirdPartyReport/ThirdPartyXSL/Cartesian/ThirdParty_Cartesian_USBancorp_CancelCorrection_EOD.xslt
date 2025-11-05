<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="RoundOff">
		<xsl:param name="value"/>
		<xsl:variable name="intPart" select="floor($value)"/>
		<xsl:variable name="fraction" select="$value - $intPart"/>
		<xsl:choose>
			<xsl:when test="$fraction &gt;= 0.5">
				<xsl:value-of select="$intPart + 1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$intPart"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

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


			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>

				<Account>
					<xsl:value-of select="'Account'"/>
				</Account>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<TRADEDATE>
					<xsl:value-of select="'TRADEDATE'"/>
				</TRADEDATE>

				<SETTLEDATE>
					<xsl:value-of select="'SETTLEDATE'"/>

				</SETTLEDATE>

				<QUANTITY>
					<xsl:value-of select="'QUANTITY'"/>
				</QUANTITY>

			
				<PRICE>
					<xsl:value-of select="'PRICE'"/>
				</PRICE>

				<COMMISSIONAMT>
					<xsl:value-of select="'COMMISSIONAMT'"/>
				</COMMISSIONAMT>


				<OTHERFEES>
					<xsl:value-of select="'OTHERFEES'"/>
				</OTHERFEES>

				<EXCHANGEFEES>
					<xsl:value-of select="'EXCHANGEFEES'"/>
				</EXCHANGEFEES>

				<SECFEES>
					<xsl:value-of select="'SECFEES'"/>
				</SECFEES>

				<NETAMOUNTLOCAL>
					<xsl:value-of select="'NETAMOUNTLOCAL'"/>
				</NETAMOUNTLOCAL>
				<NETAMOUNTBASE>
					<xsl:value-of select="'NETAMOUNTBASE'"/>
				</NETAMOUNTBASE>

				<PRINCIPALLOCAL>
					<xsl:value-of select="'PRINCIPALLOCAL'"/>
				</PRINCIPALLOCAL>

					<PRINCIPALBASE>
						<xsl:value-of select="'PRINCIPALBASE'"/>
					</PRINCIPALBASE>

				<ACCRUEDINTEREST>
					<xsl:value-of select="'ACCRUEDINTEREST'"/>
				</ACCRUEDINTEREST>

				<TRANCODE>
					<xsl:value-of select="'TRANCODE'"/>
				</TRANCODE>

				<EXECUTINGBROKER>
					<xsl:value-of select="'EXECUTINGBROKER'"/>
				</EXECUTINGBROKER>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITYDESCRIPTION'"/>
				</SECURITYDESCRIPTION>
<COMMENT>
							<xsl:value-of select="'COMMENT'"/>
							</COMMENT>
				
				<SECURITYTYPE>
					<xsl:value-of select="'SECURITYTYPE'"/>
				</SECURITYTYPE>


				<TRANSACTIONSTATUS>
					<xsl:value-of select="'TRANSACTIONSTATUS'"/>
				</TRANSACTIONSTATUS>

				<TRANSACTIONSID>
					<xsl:value-of select="'TRANSACTIONSID'"/>
				</TRANSACTIONSID>
				
				<TRADECURRENCY>
            <xsl:value-of select="'TRADECURRENCY'"/>
			</TRADECURRENCY>
			
			<SETTLEMENTCURRENCY>
            <xsl:value-of select="'SETTLEMENTCURRENCY'"/>
			</SETTLEMENTCURRENCY>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">
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



					<xsl:variable name="PB_NAME" select="'USB'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='USB']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<Account>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Account>

					<CUSIP>
						<xsl:choose>
							<xsl:when test="CurrencySymbol!='USD'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<!--<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CustomUDA4"/>
									</xsl:otherwise>
								</xsl:choose>-->
								<xsl:value-of select="CUSIP"/>
							</xsl:otherwise>
						</xsl:choose>
					</CUSIP>

					<SEDOL>
						<xsl:choose>
							<xsl:when test="SettlCurrency!='USD'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SEDOL"/>
							</xsl:otherwise>
						</xsl:choose>
					</SEDOL>
					
					<TICKER>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</TICKER>

					<xsl:variable name="varTradeDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="TradeDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<TRADEDATE>
						<xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
					</TRADEDATE>
					<xsl:variable name="varSettlementDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="SettlementDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<SETTLEDATE>
						<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
					</SETTLEDATE>
					
					<QUANTITY>
						<xsl:choose>
							<xsl:when test="number(OrderQty)">
								<xsl:value-of select="OrderQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</QUANTITY>

					<!-- <ORIGFACEAMT> -->
					<!-- <xsl:value-of select="''"/> -->
					<!-- </ORIGFACEAMT> -->

					<xsl:variable name="varSettFxAmt">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
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

					<PRICE>
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="format-number(AvgPrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(AvgPrice,'0.####')"/>
							</xsl:otherwise>
						
						</xsl:choose>
					</PRICE>


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
					
					<xsl:variable name="Commission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>


					<COMMISSIONAMT>
						<xsl:value-of select="$Commission"/>
					</COMMISSIONAMT>

					<xsl:variable name="OthFees">
						<xsl:value-of select="OtherBrokerFees + MiscFees + OccFee + OrfFee + ClearingBrokerFee + TaxOnCommissions + TransactionLevy  + ClearingFee"/>
					</xsl:variable>

					<xsl:variable name = "OTH">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="$OthFees"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="$OthFees * $varFXRate"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="$OthFees div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<OTHERFEES>
						<xsl:value-of select="format-number($OthFees,'0.##')"/>
					</OTHERFEES>

					<EXCHANGEFEES>
						<xsl:value-of select="StampDuty"/>
					</EXCHANGEFEES>

					<xsl:variable name = "SECF">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="SecFee"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="SecFee * $varFXRate"/>
							</xsl:when>

							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="SecFee div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SECFEES>
						<xsl:value-of select="format-number(SecFee,'0.##')"/>
					</SECFEES>

					<xsl:variable name = "NETAMNT">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="c"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FixedIncome') and (Side='Buy' or Side='Buy to Close')">
								<xsl:value-of select="$varNetamount + AccruedInterest"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FixedIncome') and (Side='Sell' or Side='Sell short')">
								<xsl:value-of select="$varNetamount + AccruedInterest"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="$varNetamount * $varFXRate"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="$varNetamount div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NETAMOUNTLOCAL>
						<xsl:value-of select="$varNetamount"/>
					</NETAMOUNTLOCAL>
					
				<NETAMOUNTBASE>
					 <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
                <xsl:value-of select="format-number($varNetamount * FXRate_Taxlot,'#.####')"/>
              </xsl:when>
              <xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
                <xsl:value-of select="format-number($varNetamount div FXRate_Taxlot,'#.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
				</NETAMOUNTBASE>
					<xsl:variable name="Principal">
						<xsl:value-of select="OrderQty * AvgPrice * AssetMultiplier"/>
					</xsl:variable>

					<xsl:variable name="PRINCIPAL">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="$Principal"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
								<xsl:value-of select="$Principal * $varFXRate"/>
							</xsl:when>
							<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
								<xsl:value-of select="$Principal div $varFXRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PRINCIPALLOCAL>
						<xsl:value-of select="$Principal"/>
					</PRINCIPALLOCAL>

					<PRINCIPALBASE>
					 <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
                <xsl:value-of select="format-number($Principal * FXRate_Taxlot,'#.####')"/>
              </xsl:when>
              <xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
                <xsl:value-of select="format-number($Principal div FXRate_Taxlot,'#.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
					</PRINCIPALBASE>
					
					<ACCRUEDINTEREST>
						<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
					</ACCRUEDINTEREST>

					<TRANCODE>
						 <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="Side='Buy to Open'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>
                    <xsl:when test="Side='Buy to Close'">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>
                    <xsl:when test="Side='Sell to Open'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                    <xsl:when test="Side='Sell to Close'">
                      <xsl:value-of select="'Sell to Close'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''" />
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="Side='Buy'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="Side='Buy to Close'">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>
                    <xsl:when test="Side='Sell short'">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>
                    <xsl:when test="Side='Sell'">
                      <xsl:value-of select="'Sell'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>

					</TRANCODE>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:variable name="THIRDPARTY_BROKER">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@DTCCode"/>
					</xsl:variable>

					
					<EXECUTINGBROKER>
						<xsl:value-of select="CounterParty"/>
					</EXECUTINGBROKER>


					<SECURITYDESCRIPTION>
						<xsl:value-of select="CompanyName"/>
					</SECURITYDESCRIPTION>

					<COMMENT>
							<xsl:value-of select="''"/>
							</COMMENT>

					<SECURITYTYPE>
						<xsl:choose>
							<xsl:when test="contains(Symbol,'SWAP')">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'Equity'"/>
							</xsl:when>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'F'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'EquityOption'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SECURITYTYPE>


					<xsl:variable name="varTaxlotStateTx">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'COR'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'CAN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<TRANSACTIONSTATUS>
						<xsl:value-of select="$varTaxlotStateTx"/>
					</TRANSACTIONSTATUS>

					<TRANSACTIONSID>
					 <xsl:value-of  select="PBUniqueID"/>
					</TRANSACTIONSID>
					
				<TRADECURRENCY>
				<xsl:value-of select="CurrencySymbol"/>
				</TRADECURRENCY>
				
				<SETTLEMENTCURRENCY>
				<xsl:value-of select="SettlCurrency"/>
				</SETTLEMENTCURRENCY>

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



								<xsl:variable name="PB_NAME" select="'USB'"/>
								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="AccountName"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='USB']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>

								<Account>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</Account>

								<CUSIP>
									<xsl:choose>
										<xsl:when test="CurrencySymbol!='USD'">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>
										<xsl:otherwise>
											<!--<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CustomUDA4"/>
									</xsl:otherwise>
								</xsl:choose>-->
											<xsl:value-of select="CUSIP"/>
										</xsl:otherwise>
									</xsl:choose>

								</CUSIP>

								<SEDOL>
									<xsl:choose>
										<xsl:when test="SettlCurrency!='USD'">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="SEDOL"/>
										</xsl:otherwise>
									</xsl:choose>

								</SEDOL>
								<TICKER>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="OSIOptionSymbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="Symbol"/>
										</xsl:otherwise>
									</xsl:choose>
								</TICKER>

								<xsl:variable name="varTradeDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="OldTradeDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<TRADEDATE>
						<xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
					</TRADEDATE>
					<xsl:variable name="varSettlementDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="OldSettlementDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<SETTLEDATE>
						<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
					</SETTLEDATE>

								<QUANTITY>
									<xsl:choose>
										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="OldExecutedQuantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</QUANTITY>

								<!-- <ORIGFACEAMT> -->
								<!-- <xsl:value-of select="''"/> -->
								<!-- </ORIGFACEAMT> -->

								<xsl:variable name="varSettFxAmt">
									<xsl:choose>
										<xsl:when test="SettlCurrency != CurrencySymbol">
											<xsl:choose>
												<xsl:when test="FXConversionMethodOperator_Trade ='M'">
													<xsl:value-of select="OldAvgPrice * FXRate_Taxlot"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="OldAvgPrice div FXRate_Taxlot"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PRICE>
									<xsl:choose>
										<xsl:when test="SettlCurrency = CurrencySymbol">
											<xsl:value-of select="format-number(OldAvgPrice,'0.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number($varSettFxAmt,'0.####')"/>
										</xsl:otherwise>
										<!--<xsl:when test="SettlCurrAmt=0">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(SettlCurrAmt,'0.####')"/>
							</xsl:otherwise>-->
									</xsl:choose>
								</PRICE>


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

								<xsl:variable name="Commission">
									<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
								</xsl:variable>

							

								<COMMISSIONAMT>
										<xsl:value-of select="$Commission"/>
								</COMMISSIONAMT>

								<xsl:variable name = "OthFees">
									  <xsl:value-of select="OldOrfFee + OldSecFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldMiscFees + OldOtherBrokerFees + OldTaxOnCommissions"/>
									<!--<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy)"/>-->
								</xsl:variable>

								
								<OTHERFEES>
										<xsl:value-of select="format-number($OthFees,'0.##')"/>
								</OTHERFEES>

								<EXCHANGEFEES>
									<xsl:value-of select="OldStampDuty"/>
								</EXCHANGEFEES>


								<xsl:variable name = "SECF">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="SecFee"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
											<xsl:value-of select="SecFee * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
											<xsl:value-of select="SecFee div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<SECFEES>
									<xsl:value-of select="OldSecFee"/>
								</SECFEES>

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


							
								<NETAMOUNTLOCAL>
									<xsl:value-of select="$varOldNetAmount"/>
								</NETAMOUNTLOCAL>

								<NETAMOUNTBASE>
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Taxlot='M' and OldFXRate !=0">
											<xsl:value-of select="format-number($varOldNetAmount * OldFXRate,'#.####')"/>
										</xsl:when>
										<xsl:when test="FXConversionMethodOperator_Taxlot='D' and OldFXRate !=0">
											<xsl:value-of select="format-number($varOldNetAmount div OldFXRate,'#.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</NETAMOUNTBASE>
								<xsl:variable name="Principal">
									<xsl:value-of select="OldExecutedQuantity * OldAvgPrice * AssetMultiplier"/>
								</xsl:variable>

								<xsl:variable name="PRINCIPAL">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$Principal"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
											<xsl:value-of select="$Principal * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
											<xsl:value-of select="$Principal div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PRINCIPALLOCAL>
										<xsl:value-of select="$Principal"/>
								</PRINCIPALLOCAL>

								<PRINCIPALBASE>
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
											<xsl:value-of select="format-number($Principal * FXRate_Taxlot,'#.####')"/>
										</xsl:when>
										<xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
											<xsl:value-of select="format-number($Principal div FXRate_Taxlot,'#.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</PRINCIPALBASE>
								<ACCRUEDINTEREST>
									<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
								</ACCRUEDINTEREST>

								<TRANCODE>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption'">
											<xsl:choose>
												<xsl:when test="OldSide='Buy to Open'">
													<xsl:value-of select="'Buy to Open'"/>
												</xsl:when>
												<xsl:when test="OldSide='Buy to Close'">
													<xsl:value-of select="'Buy to Close'"/>
												</xsl:when>
												<xsl:when test="OldSide='Sell to Open'">
													<xsl:value-of select="'Sell to Open'"/>
												</xsl:when>
												<xsl:when test="OldSide='Sell to Close'">
													<xsl:value-of select="'Sell to Close'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="''" />
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="OldSide='Buy'">
													<xsl:value-of select="'Buy'"/>
												</xsl:when>
												<xsl:when test="OldSide='Buy to Close'">
													<xsl:value-of select="'Buy to Close'"/>
												</xsl:when>
												<xsl:when test="OldSide='Sell short'">
													<xsl:value-of select="'Sell short'"/>
												</xsl:when>
												<xsl:when test="OldSide='Sell'">
													<xsl:value-of select="'Sell'"/>
												</xsl:when>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>

								</TRANCODE>

								   <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="OldCounterparty"/>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
              </xsl:variable>

              <xsl:variable name="Broker">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

								<EXECUTINGBROKER>
									<xsl:value-of select="$Broker"/>
								</EXECUTINGBROKER>


								<SECURITYDESCRIPTION>
									<xsl:value-of select="CompanyName"/>
								</SECURITYDESCRIPTION>

								<COMMENT>
							<xsl:value-of select="''"/>
							</COMMENT>

								<SECURITYTYPE>
									<xsl:choose>
										<xsl:when test="contains(Symbol,'SWAP')">
											<xsl:value-of select="'SWAP'"/>
										</xsl:when>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'Equity'"/>
										</xsl:when>
										<xsl:when test="Asset='FX'">
											<xsl:value-of select="'F'"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="'EquityOption'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SECURITYTYPE>


								<xsl:variable name="varTaxlotStateTx">
									<xsl:choose>
										<xsl:when test="TaxLotState='Allocated'">
											<xsl:value-of select ="'NEW'"/>
										</xsl:when>
										<xsl:when test="TaxLotState='Amemded'">
											<xsl:value-of select ="'COR'"/>
										</xsl:when>
										<xsl:when test="TaxLotState='Deleted'">
											<xsl:value-of select ="'CAN'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<TRANSACTIONSTATUS>
									<xsl:value-of select="'CANCEL'"/>
								</TRANSACTIONSTATUS>

								<TRANSACTIONSID>
								  <!-- <xsl:value-of select="substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-7,string-length(AmendTaxLotId1))"/> -->
								  <xsl:value-of select="PBUniqueID"/>
								</TRANSACTIONSID>

								<TRADECURRENCY>
									<xsl:value-of select="CurrencySymbol"/>
								</TRADECURRENCY>

								<SETTLEMENTCURRENCY>
									<xsl:value-of select="OldSettlCurrency"/>
								</SETTLEMENTCURRENCY>

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

							<xsl:variable name="PB_NAME" select="'USB'"/>
							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="AccountName"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='USB']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>

							<Account>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Account>

							<CUSIP>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<!--<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CustomUDA4"/>
									</xsl:otherwise>
								</xsl:choose>-->
										<xsl:value-of select="CUSIP"/>
									</xsl:otherwise>
								</xsl:choose>
							</CUSIP>

							<SEDOL>
								<xsl:choose>
									<xsl:when test="SettlCurrency!='USD'">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="SEDOL"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEDOL>

							<TICKER>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</TICKER>

							<xsl:variable name="varTradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<TRADEDATE>
								<xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
							</TRADEDATE>
							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<SETTLEDATE>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SETTLEDATE>

							<QUANTITY>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</QUANTITY>

							<!-- <ORIGFACEAMT> -->
							<!-- <xsl:value-of select="''"/> -->
							<!-- </ORIGFACEAMT> -->

							<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Trade ='M'">
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

							<PRICE>
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="format-number(AvgPrice,'0.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number(AvgPrice,'0.####')"/>
									</xsl:otherwise>

								</xsl:choose>
							</PRICE>


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

							<xsl:variable name="Commission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>


							<COMMISSIONAMT>
								<xsl:value-of select="$Commission"/>
							</COMMISSIONAMT>

							<xsl:variable name="OthFees">
								<xsl:value-of select="OtherBrokerFees + MiscFees + OccFee + OrfFee + ClearingBrokerFee + TaxOnCommissions + TransactionLevy  + ClearingFee"/>
							</xsl:variable>

							<xsl:variable name = "OTH">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$OthFees"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
										<xsl:value-of select="$OthFees * $varFXRate"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
										<xsl:value-of select="$OthFees div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<OTHERFEES>
								<xsl:value-of select="format-number($OthFees,'0.##')"/>
							</OTHERFEES>

							<EXCHANGEFEES>
								<xsl:value-of select="StampDuty"/>
							</EXCHANGEFEES>

							<xsl:variable name = "SECF">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="SecFee"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
										<xsl:value-of select="SecFee * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
										<xsl:value-of select="SecFee div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SECFEES>
								<xsl:value-of select="format-number(SecFee,'0.##')"/>
							</SECFEES>

							<xsl:variable name = "NETAMNT">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="c"/>
									</xsl:when>
									<xsl:when test="contains(Asset,'FixedIncome') and (Side='Buy' or Side='Buy to Close')">
										<xsl:value-of select="$varNetamount + AccruedInterest"/>
									</xsl:when>
									<xsl:when test="contains(Asset,'FixedIncome') and (Side='Sell' or Side='Sell short')">
										<xsl:value-of select="$varNetamount + AccruedInterest"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
										<xsl:value-of select="$varNetamount * $varFXRate"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
										<xsl:value-of select="$varNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<NETAMOUNTLOCAL>
								<xsl:value-of select="$varNetamount"/>
							</NETAMOUNTLOCAL>

							<NETAMOUNTBASE>
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
										<xsl:value-of select="format-number($varNetamount * FXRate_Taxlot,'#.####')"/>
									</xsl:when>
									<xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
										<xsl:value-of select="format-number($varNetamount div FXRate_Taxlot,'#.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NETAMOUNTBASE>
							<xsl:variable name="Principal">
								<xsl:value-of select="OrderQty * AvgPrice * AssetMultiplier"/>
							</xsl:variable>

							<xsl:variable name="PRINCIPAL">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$Principal"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
										<xsl:value-of select="$Principal * $varFXRate"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
										<xsl:value-of select="$Principal div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRINCIPALLOCAL>
								<xsl:value-of select="$Principal"/>
							</PRINCIPALLOCAL>

							<PRINCIPALBASE>
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
										<xsl:value-of select="format-number($Principal * FXRate_Taxlot,'#.####')"/>
									</xsl:when>
									<xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
										<xsl:value-of select="format-number($Principal div FXRate_Taxlot,'#.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</PRINCIPALBASE>

							<ACCRUEDINTEREST>
								<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
							</ACCRUEDINTEREST>

							<TRANCODE>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption'">
										<xsl:choose>
											<xsl:when test="Side='Buy to Open'">
												<xsl:value-of select="'Buy to Open'"/>
											</xsl:when>
											<xsl:when test="Side='Buy to Close'">
												<xsl:value-of select="'Buy to Close'"/>
											</xsl:when>
											<xsl:when test="Side='Sell to Open'">
												<xsl:value-of select="'Sell to Open'"/>
											</xsl:when>
											<xsl:when test="Side='Sell to Close'">
												<xsl:value-of select="'Sell to Close'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''" />
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="Side='Buy'">
												<xsl:value-of select="'Buy'"/>
											</xsl:when>
											<xsl:when test="Side='Buy to Close'">
												<xsl:value-of select="'Buy to Close'"/>
											</xsl:when>
											<xsl:when test="Side='Sell short'">
												<xsl:value-of select="'Sell short'"/>
											</xsl:when>
											<xsl:when test="Side='Sell'">
												<xsl:value-of select="'Sell'"/>
											</xsl:when>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>

							</TRANCODE>

							<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

							<xsl:variable name="THIRDPARTY_BROKER_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@PranaBrokerCode"/>
							</xsl:variable>

							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@DTCCode"/>
							</xsl:variable>


							<EXECUTINGBROKER>
								<xsl:value-of select="CounterParty"/>
							</EXECUTINGBROKER>


							<SECURITYDESCRIPTION>
								<xsl:value-of select="CompanyName"/>
							</SECURITYDESCRIPTION>

							<COMMENT>
							<xsl:value-of select="''"/>
							</COMMENT>

							<SECURITYTYPE>
								<xsl:choose>
									<xsl:when test="contains(Symbol,'SWAP')">
										<xsl:value-of select="'SWAP'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'Equity'"/>
									</xsl:when>
									<xsl:when test="Asset='FX'">
										<xsl:value-of select="'F'"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="'EquityOption'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SECURITYTYPE>
							

							<xsl:variable name="varTaxlotStateTx">
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEW'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Amended'">
										<xsl:value-of select ="'COR'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CAN'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<TRANSACTIONSTATUS>
								<xsl:value-of select="'NEW'"/>
							</TRANSACTIONSTATUS>

							<TRANSACTIONSID>
						   <xsl:value-of select="concat(PBUniqueID,position())"/>
							</TRANSACTIONSID>

							<TRADECURRENCY>
								<xsl:value-of select="CurrencySymbol"/>
							</TRADECURRENCY>

							<SETTLEMENTCURRENCY>
								<xsl:value-of select="SettlCurrency"/>
							</SETTLEMENTCURRENCY>

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