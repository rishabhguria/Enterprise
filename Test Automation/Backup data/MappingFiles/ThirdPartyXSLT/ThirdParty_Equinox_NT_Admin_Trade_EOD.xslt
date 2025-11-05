<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


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

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[(AccountName!= 'STICHTING - 051520419') and (AccountName!= 'POT - 051545952')]">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

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
							<xsl:when test="TaxLotState='Amendment'">
								<xsl:value-of select ="'COR'"/>
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
								<xsl:value-of select="'L'"/>
							</xsl:when>
							<xsl:when test ="Side='Buy to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell short'">
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
							
							<xsl:when test ="Side='Buy to Close'">
								<xsl:value-of select="'Buy to Cover'"/>
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
						<xsl:value-of select="AccountName"/>
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
						<xsl:value-of select="FullSecurityName"/>
					</SEC_DESC>



					

					<TRADE_DATE>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TRADE_DATE>


					<SETTL_DATE>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</SETTL_DATE>




					<SETTL_CC>
						<xsl:value-of select="SettlCurrency"/>
					</SETTL_CC>

					<LOCAL_CC>
						<xsl:value-of select="CurrencySymbol"/>
					</LOCAL_CC>

					<ORIG_FACE>
						<xsl:value-of select="AllocatedQty"/>
					</ORIG_FACE>

					<CURR_FACE>
						<xsl:value-of select="''"/>
					</CURR_FACE>


					<xsl:variable name="AvgPrice1">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="AveragePrice * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="AveragePrice div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PRC>
						<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
					</PRC>


					<xsl:variable name="varPrincipal" select="AllocatedQty * AveragePrice * AssetMultiplier"/>
					<PRIN>
						<xsl:value-of select="$varPrincipal"/>
					</PRIN>


					<xsl:variable name="SoftSoftCommission_Expo">
						<xsl:choose>
							<xsl:when test="contains(SoftCommissionCharged,'E')">
								<xsl:call-template name="ScientificToNumber">
									<xsl:with-param name="ScientificN" select="SoftCommissionCharged"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SoftCommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<xsl:variable name="varCommissionCharged">
						<xsl:choose>
							<xsl:when test="contains(CommissionCharged,'E')">
								<xsl:call-template name="ScientificToNumber">
									<xsl:with-param name="ScientificN" select="CommissionCharged"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varCommissions">
						<xsl:value-of select="$varCommissionCharged + $SoftSoftCommission_Expo"/>
					</xsl:variable>

					<!--<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>-->

					<xsl:variable name="varCommission1">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$varCommissions"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varCommissions * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varCommissions div SettlCurrFxRate"/>
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
						<xsl:value-of select="ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + OtherBrokerFee"/>
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
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="NetAmount * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="NetAmount div SettlCurrFxRate"/>
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
						<xsl:value-of select="''"/>
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

					<STRIKE_PRC>
						<xsl:value-of select="''"/>
					</STRIKE_PRC>


					

					<EXPIRE_DATE>
						<xsl:value-of select="''"/>
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
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
