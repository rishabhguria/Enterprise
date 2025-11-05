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

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>


				<REC_TYP>
					<xsl:value-of select="'REC_TYP'"/>
				</REC_TYP>

				<ASSET_TYP>					
					<xsl:value-of select="'ASSET_TYP'"/>						
				</ASSET_TYP>

				<TRAN_STAT>
					<xsl:value-of select ="'TRAN_STAT'"/>
				</TRAN_STAT>

				<LS_IND>
					
					<xsl:value-of select="'LS_IND'"/>
					
				</LS_IND>

				<TRAN_CODE>
					
					<xsl:value-of select="'TRAN_CODE'"/>
				</TRAN_CODE>

				<CLIENT_REF>
					<xsl:value-of select="'CLIENT_REF'"/>
				</CLIENT_REF>


				
				<ACCT_ID>
					<xsl:value-of select="'ACCT_ID'"/>
				</ACCT_ID>


			
				<BRKR_ACCTID>					
					<xsl:value-of select="'BRKR_ACCTID'"/>					
				</BRKR_ACCTID>


				<BRKR>
					<xsl:value-of select="'BRKR'"/>
				</BRKR>

				<BRKR_DESC>
					<xsl:value-of select="'BRKR_DESC'"/>
				</BRKR_DESC>

				<CLR_BRKR>
					<xsl:value-of select="'CLR_BRKR'"/>
				</CLR_BRKR>

				<CLR_BRKR_DESC>
					<xsl:value-of select="'CLR_BRKR_DESC'"/>
				</CLR_BRKR_DESC>


				<SEC_ID_TYPE>
					
						<xsl:value-of select="'SEC_ID_TYPE'"/>
					
				</SEC_ID_TYPE>


				<SEC_ID>
					
					<xsl:value-of select="'SEC_ID'"/>
						
				</SEC_ID>

				<UNDR_SEC_TYP>
					<xsl:value-of select="'UNDR_SEC_TYP'"/>
				</UNDR_SEC_TYP>

				<UNDR_SEC_ID>
					<xsl:value-of select="'UNDR_SEC_ID'"/>
				</UNDR_SEC_ID>

				<SEC_DESC>
					<xsl:value-of select="'SEC_DESC'"/>
				</SEC_DESC>




				<TRADE_DATE>
					<xsl:value-of select="'TRADE_DATE'"/>
				</TRADE_DATE>


				<SETTL_DATE>
					<xsl:value-of select="'SETTL_DATE'"/>
				</SETTL_DATE>




				<SETTL_CC>
					<xsl:value-of select="'SETTL_CC'"/>
				</SETTL_CC>

				<LOCAL_CC>
					<xsl:value-of select="'LOCAL_CC'"/>
				</LOCAL_CC>

				<ORIG_FACE>
					<xsl:value-of select="'ORIG_FACE'"/>
				</ORIG_FACE>

				<CURR_FACE>
					<xsl:value-of select="'CURR_FACE'"/>
				</CURR_FACE>



				<PRC>
					<xsl:value-of select="'PRC'"/>
				</PRC>


				
				<PRIN>
					<xsl:value-of select="'PRIN'"/>
				</PRIN>

				
				
				<COMM>
					<xsl:value-of select="'COMM'"/>
				</COMM>


				<FEES>
					<xsl:value-of select="'FEES'"/>
				</FEES>


				<TAX2>
					<xsl:value-of select="'TAX2'"/>
				</TAX2>

				<INT>
					<xsl:value-of select="'INT'"/>
				</INT>

				<NEG_INT>
					<xsl:value-of select="'NEG_INT'"/>
				</NEG_INT>

				<CONSTANT>
					<xsl:value-of select="'CONSTANT'"/>
				</CONSTANT>

			
				<LOCAL_AMT>
					<xsl:value-of select="'LOCAL_AMT'"/>
				</LOCAL_AMT>


				<SETTL_AMT>
					<xsl:value-of select="'SETTL_AMT'"/>
				</SETTL_AMT>


				<FX_DEAL>
					<xsl:value-of select="'FX_DEAL'"/>
				</FX_DEAL>


				<FX_CC_RCV>
					<xsl:value-of select="'FX_CC_RCV'"/>
				</FX_CC_RCV>


				<FX_CC_DEL>
					<xsl:value-of select="'FX_CC_DEL'"/>
				</FX_CC_DEL>


				<CLASS_HEDGE>
					<xsl:value-of select="'CLASS_HEDGE'"/>
				</CLASS_HEDGE>

				<NOT_AMT_RCV>
					<xsl:value-of select="'NOT_AMT_RCV'"/>
				</NOT_AMT_RCV>

				<NOT_AMT_DEL>
					<xsl:value-of select="'NOT_AMT_DEL'"/>
				</NOT_AMT_DEL>

				<STRAT_CODE>
					<xsl:value-of select="'STRAT_CODE'"/>
				</STRAT_CODE>


			

				<EXPIRE_DATE>
					<xsl:value-of select="'EXPIRE_DATE'"/>
				</EXPIRE_DATE>


				<CNTRY_QUOT>
					<xsl:value-of select="'CNTRY_QUOT'"/>
				</CNTRY_QUOT>

				<INT_PYBL>
					<xsl:value-of select="'INT_PYBL'"/>
				</INT_PYBL>

				<INT_RCVBL>
					<xsl:value-of select="'INT_RCVBL'"/>
				</INT_RCVBL>

				<SPRD_PYBL>
					<xsl:value-of select="'SPRD_PYBL'"/>
				</SPRD_PYBL>

				<SPRD_RCVBL>
					<xsl:value-of select="'SPRD_RCVBL'"/>
				</SPRD_RCVBL>

				<EXCHG>
					<xsl:value-of select="'EXCHG'"/>
				</EXCHG>

				<INFO1>
					<xsl:value-of select="'INFO1'"/>
				</INFO1>

				<INFO2>
					<xsl:value-of select="'INFO2'"/>
				</INFO2>

				<INFO3>
					<xsl:value-of select="'INFO3'"/>
				</INFO3>

				<INFO4>
					<xsl:value-of select="'INFO4'"/>
				</INFO4>

				<INFO5>
					<xsl:value-of select="'INFO5'"/>
				</INFO5>

				<INFO6>
					<xsl:value-of select="'INFO6'"/>
				</INFO6>


				<INSTR_ID>
					<xsl:value-of select="'INSTR_ID'"/>
				</INSTR_ID>

				<FMSECNR>
					<xsl:value-of select="'FMSECNR'"/>
				</FMSECNR>

				<SHORT_CODE>
					<xsl:value-of select="'SHORT_CODE'"/>
				</SHORT_CODE>

				<GMI_CODE>
					<xsl:value-of select="'GMI_CODE'"/>
				</GMI_CODE>
				

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
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
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<REC_TYP>
								<xsl:value-of select="'TRN'"/>
							</REC_TYP>

							<ASSET_TYP>
								<xsl:choose>
									<xsl:when test ="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test ="Asset='EquityOption'">
										<xsl:value-of select="'Option'"/>
									</xsl:when>
									<xsl:when test ="Asset='FixedIncom'">
										<xsl:value-of select="'BOND'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ASSET_TYP>

							<TRAN_STAT>
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEW'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CANC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</TRAN_STAT>

							<LS_IND>
								<xsl:choose>
									<xsl:when test ="Side='Buy'">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:when test ="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</LS_IND>

							<TRAN_CODE>
								<xsl:choose>
									<xsl:when test ="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test ="Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test ="Side='Sell short'">
										<xsl:value-of select="'SELL SHORT'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TRAN_CODE>

							<CLIENT_REF>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
							</CLIENT_REF>
							
							
							<xsl:variable name="PB_NAME" select="''"/>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="'AccountName'"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ACCT_ID>
								<xsl:value-of select="$varAccountName"/>
							</ACCT_ID>


							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<BRKR_ACCTID>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</BRKR_ACCTID>



							<xsl:variable name="THIRDPARTY_COUNTERPARTY">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
							</xsl:variable>
							<xsl:variable name="varBrokerName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
										<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<BRKR>
								<xsl:value-of select="$varBrokerName"/>
							</BRKR>

							<BRKR_DESC>
								<xsl:value-of select="''"/>
							</BRKR_DESC>

							<CLR_BRKR>
								<xsl:value-of select="''"/>
							</CLR_BRKR>

							<CLR_BRKR_DESC>
								<xsl:value-of select="''"/>
							</CLR_BRKR_DESC>
							

							<SEC_ID_TYPE>
								<xsl:choose>
									<xsl:when test ="SEDOL!=''">
										<xsl:value-of select="'SEDOL'"/>
									</xsl:when>
									
								
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEC_ID_TYPE>


							<SEC_ID>
								<xsl:choose>
									<xsl:when test ="SEDOL!=''">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEC_ID>

							<UNDR_SEC_TYP>
								<xsl:value-of select="''"/>
							</UNDR_SEC_TYP>

							<UNDR_SEC_ID>
								<xsl:value-of select="''"/>
							</UNDR_SEC_ID>

							<SEC_DESC>
								<xsl:value-of select="CompanyName"/>
							</SEC_DESC>



							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TRADE_DATE>
								<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
							</TRADE_DATE>


							<SETTL_DATE>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SETTL_DATE>


							
							
							<SETTL_CC>
								<xsl:value-of select="SettlCurrency"/>
							</SETTL_CC>

							<LOCAL_CC>
								<xsl:value-of select="CurrencySymbol"/>
							</LOCAL_CC>

							<ORIG_FACE>
								<xsl:value-of select="OrderQty"/>
							</ORIG_FACE>

							<CURR_FACE>
								<xsl:value-of select="''"/>
							</CURR_FACE>
							
							
							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRC>
								<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
							</PRC>
							

							<xsl:variable name="varPrincipal" select="OrderQty * AvgPrice * AssetMultiplier"/>
							<PRIN>
								<xsl:value-of select="$varPrincipal"/>
							</PRIN>
							
							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<COMM>
								<xsl:value-of select="format-number($varCommission1,'0.##')"/>
							</COMM>


							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varOtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varOtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<FEES>
								<xsl:value-of select="$varOtherFees1"/>
							</FEES>
							

							<TAX2>
								<xsl:value-of select="''"/>
							</TAX2>

							<INT>
								<xsl:value-of select="''"/>
							</INT>

							<NEG_INT>
								<xsl:value-of select="''"/>
							</NEG_INT>

							<CONSTANT>
								<xsl:value-of select="AssetMultiplier"/>
							</CONSTANT>



							<xsl:variable name="varLocalAmount">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varNetamount * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varNetamount div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<LOCAL_AMT>
								<xsl:value-of select="$varLocalAmount"/>
							</LOCAL_AMT>


							<SETTL_AMT>
								<xsl:value-of select="''"/>
							</SETTL_AMT>


							<FX_DEAL>
								<xsl:value-of select="FXRate"/>
							</FX_DEAL>


							<FX_CC_RCV>
								<xsl:value-of select="''"/>
							</FX_CC_RCV>


							<FX_CC_DEL>
								<xsl:value-of select="''"/>
							</FX_CC_DEL>


							<CLASS_HEDGE>
								<xsl:value-of select="''"/>
							</CLASS_HEDGE>

							<NOT_AMT_RCV>
								<xsl:value-of select="''"/>
							</NOT_AMT_RCV>

							<NOT_AMT_DEL>
								<xsl:value-of select="''"/>
							</NOT_AMT_DEL>

							<STRAT_CODE>
								<xsl:value-of select="''"/>
							</STRAT_CODE>


							<xsl:variable name="varExpirationDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<EXPIRE_DATE>
								<xsl:value-of select="concat(substring-before($varExpirationDate,'/'),'/',substring-before(substring-after($varExpirationDate,'/'),'/'),'/',substring-after(substring-after($varExpirationDate,'/'),'/'))"/>
							</EXPIRE_DATE>

							
							<CNTRY_QUOT>
								<xsl:value-of select="''"/>
							</CNTRY_QUOT>

							<INT_PYBL>
								<xsl:value-of select="''"/>
							</INT_PYBL>

							<INT_RCVBL>
								<xsl:value-of select="''"/>
							</INT_RCVBL>

							<SPRD_PYBL>
								<xsl:value-of select="''"/>
							</SPRD_PYBL>

							<SPRD_RCVBL>
								<xsl:value-of select="''"/>
							</SPRD_RCVBL>

							<EXCHG>
								<xsl:value-of select="Exchange"/>
							</EXCHG>

							<INFO1>
								<xsl:value-of select="''"/>
							</INFO1>

							<INFO2>
								<xsl:value-of select="''"/>
							</INFO2>

							<INFO3>
								<xsl:value-of select="''"/>
							</INFO3>

							<INFO4>
								<xsl:value-of select="''"/>
							</INFO4>

							<INFO5>
								<xsl:value-of select="''"/>
							</INFO5>

							<INFO6>
								<xsl:value-of select="''"/>
							</INFO6>


							<INSTR_ID>
								<xsl:value-of select="''"/>
							</INSTR_ID>

							<FMSECNR>
								<xsl:value-of select="''"/>
							</FMSECNR>

							<SHORT_CODE>
								<xsl:value-of select="''"/>
							</SHORT_CODE>

							<GMI_CODE>
								<xsl:value-of select="''"/>
							</GMI_CODE>


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
								
								<REC_TYP>
									<xsl:value-of select="'TRN'"/>
								</REC_TYP>

								<ASSET_TYP>
									<xsl:choose>
										<xsl:when test ="Asset='Equity'">
											<xsl:value-of select="'STOCK'"/>
										</xsl:when>
										<xsl:when test ="Asset='EquityOption'">
											<xsl:value-of select="'Option'"/>
										</xsl:when>
										<xsl:when test ="Asset='FixedIncom'">
											<xsl:value-of select="'BOND'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</ASSET_TYP>

								<TRAN_STAT>									
									<xsl:value-of select ="'CANC'"/>									
								</TRAN_STAT>

								<LS_IND>
									<xsl:choose>
										<xsl:when test ="OldSide='Buy'">
											<xsl:value-of select="'L'"/>
										</xsl:when>
										<xsl:when test ="OldSide='Sell'">
											<xsl:value-of select="'S'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</LS_IND>

								<TRAN_CODE>
									<xsl:choose>
										<xsl:when test ="OldSide='Buy'">
											<xsl:value-of select="'BUY'"/>
										</xsl:when>
										<xsl:when test ="OldSide='Sell'">
											<xsl:value-of select="'SELL'"/>
										</xsl:when>
										<xsl:when test ="OldSide='Sell short'">
											<xsl:value-of select="'SELL SHORT'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</TRAN_CODE>

								<CLIENT_REF>
									<xsl:value-of select="substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-7,string-length(AmendTaxLotId1))"/>
								</CLIENT_REF>


								<xsl:variable name="PB_NAME" select="''"/>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="'CFFW'"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>


								<xsl:variable name="varAccountName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<ACCT_ID>
									<xsl:value-of select="$varAccountName"/>
								</ACCT_ID>


								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_BROKER">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
								</xsl:variable>
								<BRKR_ACCTID>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_BROKER!= ''">
											<xsl:value-of select="$THIRDPARTY_BROKER"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</BRKR_ACCTID>



								<xsl:variable name="THIRDPARTY_COUNTERPARTY">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
								</xsl:variable>
								<xsl:variable name="varBrokerName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
											<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<BRKR>
									<xsl:value-of select="$varBrokerName"/>
								</BRKR>

								<BRKR_DESC>
									<xsl:value-of select="''"/>
								</BRKR_DESC>

								<CLR_BRKR>
									<xsl:value-of select="''"/>
								</CLR_BRKR>

								<CLR_BRKR_DESC>
									<xsl:value-of select="''"/>
								</CLR_BRKR_DESC>


								<SEC_ID_TYPE>
									<xsl:choose>
										<xsl:when test ="SEDOL!=''">
											<xsl:value-of select="'SEDOL'"/>
										</xsl:when>


										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SEC_ID_TYPE>


								<SEC_ID>
									<xsl:choose>
										<xsl:when test ="SEDOL!=''">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SEC_ID>

								<UNDR_SEC_TYP>
									<xsl:value-of select="''"/>
								</UNDR_SEC_TYP>

								<UNDR_SEC_ID>
									<xsl:value-of select="''"/>
								</UNDR_SEC_ID>

								<SEC_DESC>
									<xsl:value-of select="CompanyName"/>
								</SEC_DESC>



								<xsl:variable name="TradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="varSettlementDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								


								<TRADE_DATE>
									<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
								</TRADE_DATE>


								<SETTL_DATE>
									<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
								</SETTL_DATE>


								<SETTL_CC>
									<xsl:value-of select="OldSettlCurrency"/>
								</SETTL_CC>

								<LOCAL_CC>
									<xsl:value-of select="CurrencySymbol"/>
								</LOCAL_CC>

								<ORIG_FACE>
									<xsl:value-of select="OldExecutedQuantity"/>
								</ORIG_FACE>

								<CURR_FACE>
									<xsl:value-of select="''"/>
								</CURR_FACE>



								<xsl:variable name="varAvgPrice1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="OldAvgPrice * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="OldAvgPrice div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PRC>
									<xsl:value-of select="format-number($varAvgPrice1,'0.#######')"/>
								</PRC>


								<xsl:variable name="varPrincipal" select="OldExecutedQuantity * $varAvgPrice1 * AssetMultiplier"/>
								<PRIN>
									<xsl:value-of select="$varPrincipal"/>
								</PRIN>

								<xsl:variable name="varCommission">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>

								<xsl:variable name="varCommission1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varCommission"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<COMM>
									<xsl:value-of select="format-number($varCommission1,'0.##')"/>
								</COMM>


								<xsl:variable name="varOldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
								</xsl:variable>

								<xsl:variable name="varOtherFees1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varOldOtherFees"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varOldOtherFees * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varOldOtherFees div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<FEES>
									<xsl:value-of select="$varOtherFees1"/>
								</FEES>


								<TAX2>
									<xsl:value-of select="''"/>
								</TAX2>

								<INT>
									<xsl:value-of select="''"/>
								</INT>

								<NEG_INT>
									<xsl:value-of select="''"/>
								</NEG_INT>

								<CONSTANT>
									<xsl:value-of select="AssetMultiplier"/>
								</CONSTANT>

								<xsl:variable name="varOldNetAmount">
									<xsl:choose>
										<xsl:when test="contains(Side,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
										</xsl:when>
										<xsl:when test="contains(Side,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varLocalAmount">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varOldNetAmount * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varOldNetAmount div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<LOCAL_AMT>
									<xsl:value-of select="$varLocalAmount"/>
								</LOCAL_AMT>


								<SETTL_AMT>
									<xsl:value-of select="''"/>
								</SETTL_AMT>


								<FX_DEAL>
									<xsl:value-of select="FXRate"/>
								</FX_DEAL>


								<FX_CC_RCV>
									<xsl:value-of select="''"/>
								</FX_CC_RCV>


								<FX_CC_DEL>
									<xsl:value-of select="''"/>
								</FX_CC_DEL>


								<CLASS_HEDGE>
									<xsl:value-of select="''"/>
								</CLASS_HEDGE>

								<NOT_AMT_RCV>
									<xsl:value-of select="''"/>
								</NOT_AMT_RCV>

								<NOT_AMT_DEL>
									<xsl:value-of select="''"/>
								</NOT_AMT_DEL>

								<STRAT_CODE>
									<xsl:value-of select="''"/>
								</STRAT_CODE>


								<xsl:variable name="varExpirationDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="ExpirationDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<EXPIRE_DATE>
									<xsl:value-of select="concat(substring-before($varExpirationDate,'/'),'/',substring-before(substring-after($varExpirationDate,'/'),'/'),'/',substring-after(substring-after($varExpirationDate,'/'),'/'))"/>
								</EXPIRE_DATE>


								<CNTRY_QUOT>
									<xsl:value-of select="''"/>
								</CNTRY_QUOT>

								<INT_PYBL>
									<xsl:value-of select="''"/>
								</INT_PYBL>

								<INT_RCVBL>
									<xsl:value-of select="''"/>
								</INT_RCVBL>

								<SPRD_PYBL>
									<xsl:value-of select="''"/>
								</SPRD_PYBL>

								<SPRD_RCVBL>
									<xsl:value-of select="''"/>
								</SPRD_RCVBL>

								<EXCHG>
									<xsl:value-of select="Exchange"/>
								</EXCHG>

								<INFO1>
									<xsl:value-of select="''"/>
								</INFO1>

								<INFO2>
									<xsl:value-of select="''"/>
								</INFO2>

								<INFO3>
									<xsl:value-of select="''"/>
								</INFO3>

								<INFO4>
									<xsl:value-of select="''"/>
								</INFO4>

								<INFO5>
									<xsl:value-of select="''"/>
								</INFO5>

								<INFO6>
									<xsl:value-of select="''"/>
								</INFO6>


								<INSTR_ID>
									<xsl:value-of select="''"/>
								</INSTR_ID>

								<FMSECNR>
									<xsl:value-of select="''"/>
								</FMSECNR>

								<SHORT_CODE>
									<xsl:value-of select="''"/>
								</SHORT_CODE>

								<GMI_CODE>
									<xsl:value-of select="''"/>
								</GMI_CODE>
								
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
							<REC_TYP>
								<xsl:value-of select="'TRN'"/>
							</REC_TYP>

							<ASSET_TYP>
								<xsl:choose>
									<xsl:when test ="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test ="Asset='EquityOption'">
										<xsl:value-of select="'Option'"/>
									</xsl:when>
									<xsl:when test ="Asset='FixedIncom'">
										<xsl:value-of select="'BOND'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ASSET_TYP>

							<TRAN_STAT>								
								<xsl:value-of select ="'NEW'"/>									
							</TRAN_STAT>

							<LS_IND>
								<xsl:choose>
									<xsl:when test ="Side='Buy'">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:when test ="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</LS_IND>

							<TRAN_CODE>
								<xsl:choose>
									<xsl:when test ="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test ="Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test ="Side='Sell short'">
										<xsl:value-of select="'SELL SHORT'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TRAN_CODE>

							<CLIENT_REF>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
							</CLIENT_REF>


							<xsl:variable name="PB_NAME" select="''"/>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="'CFFW'"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ACCT_ID>
								<xsl:value-of select="$varAccountName"/>
							</ACCT_ID>


							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<BRKR_ACCTID>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</BRKR_ACCTID>



							<xsl:variable name="THIRDPARTY_COUNTERPARTY">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
							</xsl:variable>
							<xsl:variable name="varBrokerName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
										<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<BRKR>
								<xsl:value-of select="$varBrokerName"/>
							</BRKR>

							<BRKR_DESC>
								<xsl:value-of select="''"/>
							</BRKR_DESC>

							<CLR_BRKR>
								<xsl:value-of select="''"/>
							</CLR_BRKR>

							<CLR_BRKR_DESC>
								<xsl:value-of select="''"/>
							</CLR_BRKR_DESC>


							<SEC_ID_TYPE>
								<xsl:choose>
									<xsl:when test ="SEDOL!=''">
										<xsl:value-of select="'SEDOL'"/>
									</xsl:when>


									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEC_ID_TYPE>


							<SEC_ID>
								<xsl:choose>
									<xsl:when test ="SEDOL!=''">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEC_ID>

							<UNDR_SEC_TYP>
								<xsl:value-of select="''"/>
							</UNDR_SEC_TYP>

							<UNDR_SEC_ID>
								<xsl:value-of select="''"/>
							</UNDR_SEC_ID>

							<SEC_DESC>
								<xsl:value-of select="CompanyName"/>
							</SEC_DESC>



							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TRADE_DATE>
								<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
							</TRADE_DATE>


							<SETTL_DATE>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SETTL_DATE>





							<SETTL_CC>
								<xsl:value-of select="SettlCurrency"/>
							</SETTL_CC>

							<LOCAL_CC>
								<xsl:value-of select="CurrencySymbol"/>
							</LOCAL_CC>

							<ORIG_FACE>
								<xsl:value-of select="OrderQty"/>
							</ORIG_FACE>

							<CURR_FACE>
								<xsl:value-of select="''"/>
							</CURR_FACE>


							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PRC>
								<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
							</PRC>


							<xsl:variable name="varPrincipal" select="OrderQty * AvgPrice * AssetMultiplier"/>
							<PRIN>
								<xsl:value-of select="$varPrincipal"/>
							</PRIN>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<COMM>
								<xsl:value-of select="format-number($varCommission1,'0.##')"/>
							</COMM>


							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varOtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varOtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<FEES>
								<xsl:value-of select="$varOtherFees1"/>
							</FEES>


							<TAX2>
								<xsl:value-of select="''"/>
							</TAX2>

							<INT>
								<xsl:value-of select="''"/>
							</INT>

							<NEG_INT>
								<xsl:value-of select="''"/>
							</NEG_INT>

							<CONSTANT>
								<xsl:value-of select="AssetMultiplier"/>
							</CONSTANT>



							<xsl:variable name="varLocalAmount">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varNetamount * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varNetamount div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<LOCAL_AMT>
								<xsl:value-of select="$varLocalAmount"/>
							</LOCAL_AMT>


							<SETTL_AMT>
								<xsl:value-of select="''"/>
							</SETTL_AMT>


							<FX_DEAL>
								<xsl:value-of select="FXRate"/>
							</FX_DEAL>


							<FX_CC_RCV>
								<xsl:value-of select="''"/>
							</FX_CC_RCV>


							<FX_CC_DEL>
								<xsl:value-of select="''"/>
							</FX_CC_DEL>


							<CLASS_HEDGE>
								<xsl:value-of select="''"/>
							</CLASS_HEDGE>

							<NOT_AMT_RCV>
								<xsl:value-of select="''"/>
							</NOT_AMT_RCV>

							<NOT_AMT_DEL>
								<xsl:value-of select="''"/>
							</NOT_AMT_DEL>

							<STRAT_CODE>
								<xsl:value-of select="''"/>
							</STRAT_CODE>


							<xsl:variable name="varExpirationDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<EXPIRE_DATE>
								<xsl:value-of select="concat(substring-before($varExpirationDate,'/'),'/',substring-before(substring-after($varExpirationDate,'/'),'/'),'/',substring-after(substring-after($varExpirationDate,'/'),'/'))"/>
							</EXPIRE_DATE>


							<CNTRY_QUOT>
								<xsl:value-of select="''"/>
							</CNTRY_QUOT>

							<INT_PYBL>
								<xsl:value-of select="''"/>
							</INT_PYBL>

							<INT_RCVBL>
								<xsl:value-of select="''"/>
							</INT_RCVBL>

							<SPRD_PYBL>
								<xsl:value-of select="''"/>
							</SPRD_PYBL>

							<SPRD_RCVBL>
								<xsl:value-of select="''"/>
							</SPRD_RCVBL>

							<EXCHG>
								<xsl:value-of select="Exchange"/>
							</EXCHG>

							<INFO1>
								<xsl:value-of select="''"/>
							</INFO1>

							<INFO2>
								<xsl:value-of select="''"/>
							</INFO2>

							<INFO3>
								<xsl:value-of select="''"/>
							</INFO3>

							<INFO4>
								<xsl:value-of select="''"/>
							</INFO4>

							<INFO5>
								<xsl:value-of select="''"/>
							</INFO5>

							<INFO6>
								<xsl:value-of select="''"/>
							</INFO6>


							<INSTR_ID>
								<xsl:value-of select="''"/>
							</INSTR_ID>

							<FMSECNR>
								<xsl:value-of select="''"/>
							</FMSECNR>

							<SHORT_CODE>
								<xsl:value-of select="''"/>
							</SHORT_CODE>

							<GMI_CODE>
								<xsl:value-of select="''"/>
							</GMI_CODE>
							
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
