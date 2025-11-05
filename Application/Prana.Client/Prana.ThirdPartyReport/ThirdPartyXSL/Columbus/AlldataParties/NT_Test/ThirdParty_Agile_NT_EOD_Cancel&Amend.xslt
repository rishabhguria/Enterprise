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


	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
		

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
							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter>
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>
							
							

							<RecordTypeCode>
								<xsl:value-of select="'TRN'"/>
							</RecordTypeCode>


							<xsl:variable name ="varSecurityType">
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="'CFD'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AssetType>
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="'CFD'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test="(Asset='EquityOption' or Asset='FutureOption') and  PutOrCall='CALL'">
										<xsl:value-of select="'CALLOPT'"/>
									</xsl:when>
									<xsl:when test="(Asset='EquityOption' or Asset='FutureOption') and PutOrCall='PUT'">
										<xsl:value-of select="'PUTOPT'"/>
									</xsl:when>

									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'BOND'"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="'FUTURE'"/>
									</xsl:when>

									<xsl:when test="Asset='Warrant'">
										<xsl:value-of select="'WARRANT'"/>
									</xsl:when>

									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="'FX'"/>
									</xsl:when>



									<xsl:otherwise>
										<xsl:value-of select="Asset"/>
									</xsl:otherwise>
								</xsl:choose>
							</AssetType>


							<xsl:variable name ="varAllocationState">
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEW'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CAN'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<TransactionStatus>
								<xsl:value-of select="$varAllocationState"/>
							</TransactionStatus>

							<LongShortIndicator>
								<xsl:choose>

									<xsl:when test="(Asset='FX' or Asset='FXForward') and Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Buy' or Side='Sell' or  Side='Buy to Open' or Side='Sell to Close'">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Buy to Close' or Side='Sell to Open'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
								</xsl:choose>


							</LongShortIndicator>

							<TransactionCode>

								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="'FXDL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SHORT'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'COVER'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Side"/>
									</xsl:otherwise>
								</xsl:choose>
							</TransactionCode>

							<ClientTradeRefNo>
								<xsl:value-of select="substring(PBUniqueID,4,3)"/>
							</ClientTradeRefNo>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'NT'"/>
							</xsl:variable>

							<xsl:variable name ="varAccount">
								<xsl:choose>
									<xsl:when test ="Asset='FXForward'">
										<xsl:value-of  select="concat('SC','/',ISIN)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AccountId>
								<xsl:value-of select="$varAccount"/>
							</AccountId>

							<!--Comment done by Sanjay-->
							<xsl:variable name = "PRANA_GUICODE_NAME">
								<xsl:value-of select="ISIN"/>
							</xsl:variable>

							<xsl:variable name="PRANA_GUICODE_MAPPING">
								<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_GuiAccountMapping.xml')/GuiAccountMapping/PB[@Name='NT']/GuiAccountData[@GuiIsinCode=$PRANA_GUICODE_NAME]/@GuiAccountCode"/>
							</xsl:variable>


							<xsl:variable name ="varAccount1">
								<xsl:choose>
									<xsl:when test ="Asset='FXForward'">
										<xsl:value-of  select="concat('1750261','-',$PRANA_GUICODE_MAPPING)"/>
									</xsl:when>
									<xsl:when test ="Asset='FX'">
										<xsl:value-of  select="'1750261'"/>
									</xsl:when>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="'6007884'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="'1750261'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PrimeBrokerCustodianAccountID>
								<xsl:value-of select="$varAccount1"/>
							</PrimeBrokerCustodianAccountID>


							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

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
							<BrokerCounterparty>
								<xsl:value-of select="''"/>
							</BrokerCounterparty>


							<BrokerDescription>
								<xsl:value-of select="CounterParty"/>
							</BrokerDescription>

							<ClearingBroker>
								<xsl:value-of select="''"/>
							</ClearingBroker>

							<ClearingBrokerdescription>
								<xsl:value-of select="''"/>
							</ClearingBrokerdescription>

							<xsl:variable name="varSecurityName">
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="''"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'ISIN'"/>
									</xsl:when>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="Asset='Equity' and SEDOL=''">
										<xsl:value-of select="'CUSIP'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'SEDOL'"/>
									</xsl:when>
									<xsl:when test="Asset='Future' or Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="'TICKER'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="'TICKER'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<SecurityIdentifierType>
								<xsl:value-of select="$varSecurityName"/>
							</SecurityIdentifierType>

							<xsl:variable name="VarSecurityIdentifier">
								<xsl:value-of select="normalize-space(substring-before(BBCode,'EQUITY'))"/>
							</xsl:variable>

							<xsl:variable name="SecurityIdentifier">
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="''"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="ISIN"/>
									</xsl:when>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="Asset='Equity' and SEDOL=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="substring-before(BBCode,' ')"/>
									</xsl:when>

									<xsl:when test="Asset='FutureOption'">
										<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
									</xsl:when>

									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="$VarSecurityIdentifier"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<SecurityIdentifier>
								<xsl:value-of select ="$SecurityIdentifier"/>
							</SecurityIdentifier>


							<UnderlyingSecurityIdentifierType>
								<xsl:choose>
									<xsl:when test ="IsSwapped='true' ">
										<xsl:value-of  select="'SEDOL'"/>
									</xsl:when>
									<xsl:when test ="Asset='EquityOption'">
										<xsl:value-of select="'TICKER'"/>
									</xsl:when>
								</xsl:choose>

							</UnderlyingSecurityIdentifierType>

							<UnderlyingSecurityIdentifier>
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="SEDOL"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="substring-before(OSIOptionSymbol,' ')"/>
									</xsl:when>
								</xsl:choose>
							</UnderlyingSecurityIdentifier>

							<SecurityDescription>
								<xsl:value-of select="translate(CompanyName,',','')"/>
							</SecurityDescription>


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

							<TradeDate>
								<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlementDate>


							<Settlementcurrency>

								<xsl:choose>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="LeadCurrencyName"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="SettlCurrency"/>
									</xsl:otherwise>
								</xsl:choose>

							</Settlementcurrency>

							<LocalCCY>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="VsCurrencyName"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CurrencySymbol"/>
									</xsl:otherwise>
								</xsl:choose>

							</LocalCCY>

							<QuantityOriginalFace>

								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>


								</xsl:choose>

							</QuantityOriginalFace>

							<CurrentFace>
								<xsl:value-of select="''"/>
							</CurrentFace>

							<Price>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="number(AvgPrice)">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>


								</xsl:choose>
							</Price>

							<xsl:variable name="varAssetMultiplier">
								<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
							</xsl:variable>


							<Principal>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="number($varAssetMultiplier)">
										<xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
									</xsl:when>

								</xsl:choose>
							</Principal>


							<CommissionAmount>
								<xsl:choose>
									<xsl:when test="number(CommissionCharged)">
										<xsl:value-of select="CommissionCharged"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>

								</xsl:choose>

							</CommissionAmount>

							<xsl:variable name="varSecFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="SecFee"/>
								</xsl:call-template>
							</xsl:variable>



							<xsl:variable name="varStampDuty">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="StampDuty"/>
								</xsl:call-template>
							</xsl:variable>


							<xsl:variable name="varOrfFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="OrfFee"/>
								</xsl:call-template>
							</xsl:variable>

							

							<xsl:variable name="varTaxorfees">
								<xsl:value-of select="(SecFee + StampDuty + OrfFee + MiscFees + TransactionLevy + ClearingFee + TaxOnCommissions + OccFee)"/>
							</xsl:variable>
							<Taxorfees>
								<xsl:choose>
									<xsl:when test="number($varTaxorfees)">
										<xsl:value-of select="$varTaxorfees"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
								<!--<xsl:value-of select="$varTaxorfees"/>-->
							</Taxorfees>

							<Tax2>
								<xsl:choose>
									<xsl:when test="number(OtherBrokerFee)">
										<xsl:value-of select="OtherBrokerFee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Tax2>

							<Interest>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="format-number(AccruedInterest,'#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</Interest>

							<NegativeInterest>
								<xsl:value-of select="''"/>
							</NegativeInterest>


							<ConsiderationConstant>
								<xsl:value-of select="AssetMultiplier"/>
							</ConsiderationConstant>

							<NetAmountLocal>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($varNetamount + AccruedInterest,'#.00')"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetAmountLocal>

							<xsl:variable name="varNetAmountSettled">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
										<xsl:value-of select="($varNetamount+ AccruedInterest)* SettlCurrFxRate "/>
										<!--<xsl:value-of select="((AllocatedQty * AveragePrice * AssetMultiplier) + AccruedInterest + (CommissionCharged + (SecFee or StampDuty) + OtherBrokerFee))* SettlCurrFxRate"/>-->
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Open' or Side='Sell to Close' or Side='Sell short'">
										<xsl:value-of select="($varNetamount + AccruedInterest)* SettlCurrFxRate "/>
										<!--<xsl:value-of select="((AllocatedQty * AveragePrice * AssetMultiplier) + AccruedInterest - (CommissionCharged + (SecFee or StampDuty) + OtherBrokerFee))* SettlCurrFxRate"/>-->
									</xsl:when>
								</xsl:choose>
							</xsl:variable>

							<NetAmountSettled>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="number($varNetAmountSettled)">
										<xsl:value-of select="format-number($varNetAmountSettled,'#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetAmountSettled>

							<InternalNetNotional>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
										<xsl:value-of select="0"/>
									</xsl:when>
									<xsl:when test="number($varNetAmountSettled)">
										<xsl:value-of select="format-number($varNetamount + AccruedInterest,'#.00')"/>
										<!--<xsl:value-of select="format-number($varNetAmountSettled,'#.00')"/>-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</InternalNetNotional>


							<FXdealingrate>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</FXdealingrate>

							<FXcurrencyReceived>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:choose>
											<xsl:when test="contains(Side,'Buy')">
												<xsl:value-of select="LeadCurrencyName"/>
											</xsl:when>
											<xsl:when test="contains(Side,'Sell')">
												<xsl:value-of select="VsCurrencyName"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</FXcurrencyReceived>

							<FXcurrencyDelivered>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:choose>
											<xsl:when test="contains(Side,'Sell')">
												<xsl:value-of select="LeadCurrencyName"/>
											</xsl:when>
											<xsl:when test="contains(Side,'Buy')">
												<xsl:value-of select="VsCurrencyName"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</FXcurrencyDelivered>

							<ClassSpecificHedge>
								<xsl:value-of select="''"/>
							</ClassSpecificHedge>

							<NotionalAmountReceived>
								<xsl:choose>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="format-number(OrderQty * AvgPrice, '#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</NotionalAmountReceived>

							<NotionalAmountdelivered>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="OrderQty "/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</NotionalAmountdelivered>

							<StrategyCodes>
								<xsl:value-of select="''"/>
							</StrategyCodes>

							<StrikePrice>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="StrikePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</StrikePrice>


							<xsl:variable name="varExpirationDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

						

							<ExpireDate>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="concat(substring-after(substring-after($varExpirationDate,'/'),'/'),substring-before($varExpirationDate,'/'),substring-before(substring-after($varExpirationDate,'/'),'/'))"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExpireDate>

							<CountryofQuotation>
								<xsl:choose>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="substring(LeadCurrencyName,1,2)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring(CurrencySymbol,1,2)"/>
									</xsl:otherwise>
								</xsl:choose>

							</CountryofQuotation>

							<Blank>
								<xsl:value-of select="''"/>
							</Blank>

							<Blank1>
								<xsl:value-of select="''"/>
							</Blank1>

							<Blank2>
								<xsl:value-of select="''"/>
							</Blank2>

							<Blank3>
								<xsl:value-of select="''"/>
							</Blank3>

							<Exchange>
								
								<xsl:value-of select="''"/>
							</Exchange>


							<AdditionalInfo1>
								<xsl:value-of select="''"/>
							</AdditionalInfo1>

							<AdditionalInfo2>
								<xsl:value-of select="''"/>
							</AdditionalInfo2>

							<AdditionalInfo3>
								<xsl:value-of select="''"/>
							</AdditionalInfo3>

							<AdditionalInfo4>
								<xsl:value-of select="''"/>
							</AdditionalInfo4>

							<AdditionalInfo5>
								<xsl:value-of select="''"/>
							</AdditionalInfo5>

							<AdditionalInfo6>
								<xsl:value-of select="''"/>
							</AdditionalInfo6>


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

								<FileHeader>
									<xsl:value-of select="'true'"/>
								</FileHeader>

								<FileFooter>
									<xsl:value-of select="'true'"/>
								</FileFooter>
								
								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>


								<RecordTypeCode>
									<xsl:value-of select="'TRN'"/>
								</RecordTypeCode>


								<xsl:variable name ="varSecurityType">
									<xsl:choose>
										<xsl:when test ="IsSwapped='true'">
											<xsl:value-of  select="'CFD'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of  select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<AssetType>
									<xsl:choose>
										<xsl:when test ="IsSwapped='true'">
											<xsl:value-of  select="'CFD'"/>
										</xsl:when>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'STOCK'"/>
										</xsl:when>
										<xsl:when test="(Asset='EquityOption' or Asset='FutureOption') and  PutOrCall='CALL'">
											<xsl:value-of select="'CALLOPT'"/>
										</xsl:when>
										<xsl:when test="(Asset='EquityOption' or Asset='FutureOption') and PutOrCall='PUT'">
											<xsl:value-of select="'PUTOPT'"/>
										</xsl:when>

										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'BOND'"/>
										</xsl:when>
										<xsl:when test="Asset='Future'">
											<xsl:value-of select="'FUTURE'"/>
										</xsl:when>

										<xsl:when test="Asset='Warrant'">
											<xsl:value-of select="'WARRANT'"/>
										</xsl:when>

										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="'FX'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="Asset"/>
										</xsl:otherwise>
									</xsl:choose>
								</AssetType>


								<TransactionStatus>
									<xsl:value-of select ="'CAN'"/>
								</TransactionStatus>

								<LongShortIndicator>
									<xsl:choose>

										<xsl:when test="(Asset='FX' or Asset='FXForward') and OldSide='Sell'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy' or OldSide='Sell' or  OldSide='Buy to Open' or OldSide='Sell to Close'">
											<xsl:value-of select="'L'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short' or OldSide='Buy to Close' or OldSide='Sell to Open'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
									</xsl:choose>


								</LongShortIndicator>

								<TransactionCode>

									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="'FXDL'"/>
										</xsl:when>

										<xsl:when test="OldSide='Buy' or OldSide='Buy to Open'">
											<xsl:value-of select="'BUY'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell' or OldSide='Sell to Close'">
											<xsl:value-of select="'SELL'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="'SHORT'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'COVER'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="OldSide"/>
										</xsl:otherwise>
									</xsl:choose>
								</TransactionCode>

								<ClientTradeRefNo>
									<xsl:value-of select="substring(PBUniqueID,4,3)"/>
								</ClientTradeRefNo>

								<xsl:variable name="PB_NAME">
									<xsl:value-of select="'NT'"/>
								</xsl:variable>

								<xsl:variable name ="varAccount">
									<xsl:choose>
										<xsl:when test ="Asset='FXForward'">
											<xsl:value-of  select="concat('SC','/',ISIN)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of  select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<AccountId>
									<xsl:value-of select="$varAccount"/>
								</AccountId>
	
								<xsl:variable name = "PRANA_GUICODE_NAME">
									<xsl:value-of select="ISIN"/>
								</xsl:variable>

								<xsl:variable name="PRANA_GUICODE_MAPPING">
									<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_GuiAccountMapping.xml')/GuiAccountMapping/PB[@Name='NT']/GuiAccountData[@GuiIsinCode=$PRANA_GUICODE_NAME]/@GuiAccountCode"/>
								</xsl:variable>


								<xsl:variable name ="varAccount1">
									<xsl:choose>
										<xsl:when test ="Asset='FXForward'">
											<xsl:value-of  select="concat('1750261','-',$PRANA_GUICODE_MAPPING)"/>
										</xsl:when>
										<xsl:when test ="Asset='FX'">
											<xsl:value-of  select="'1750261'"/>
										</xsl:when>
										<xsl:when test ="IsSwapped='true'">
											<xsl:value-of  select="'6007884'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of  select="'1750261'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PrimeBrokerCustodianAccountID>
									<xsl:value-of select="$varAccount1"/>
								</PrimeBrokerCustodianAccountID>


								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

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
								<BrokerCounterparty>
									<xsl:value-of select="''"/>
								</BrokerCounterparty>


								<BrokerDescription>
									<xsl:value-of select="CounterParty"/>
								</BrokerDescription>

								<ClearingBroker>
									<xsl:value-of select="''"/>
								</ClearingBroker>

								<ClearingBrokerdescription>
									<xsl:value-of select="''"/>
								</ClearingBrokerdescription>

								<xsl:variable name="varSecurityName">
									<xsl:choose>
										<xsl:when test ="IsSwapped='true'">
											<xsl:value-of  select="''"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'ISIN'"/>
										</xsl:when>
										<xsl:when test="Asset='FX'or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="Asset='Equity' and SEDOL=''">
											<xsl:value-of select="'CUSIP'"/>
										</xsl:when>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'SEDOL'"/>
										</xsl:when>
										<xsl:when test="Asset='Future' or Asset='EquityOption' or Asset='FutureOption'">
											<xsl:value-of select="'TICKER'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="'TICKER'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<SecurityIdentifierType>
									<xsl:value-of select="$varSecurityName"/>
								</SecurityIdentifierType>

								<xsl:variable name="VarSecurityIdentifier">
									<xsl:value-of select="normalize-space(substring-before(BBCode,'EQUITY'))"/>
								</xsl:variable>

								<xsl:variable name="SecurityIdentifier">
									<xsl:choose>
										<xsl:when test ="IsSwapped='true'">
											<xsl:value-of  select="''"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="ISIN"/>
										</xsl:when>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="Asset='Equity' and SEDOL=''">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>
										<xsl:when test="Asset='Future'">
											<xsl:value-of select="substring-before(BBCode,' ')"/>
										</xsl:when>

										<xsl:when test="Asset='FutureOption'">
											<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
										</xsl:when>

										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="$VarSecurityIdentifier"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="Symbol"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<SecurityIdentifier>
									<xsl:value-of select ="$SecurityIdentifier"/>
								</SecurityIdentifier>


								<UnderlyingSecurityIdentifierType>
									<xsl:choose>
										<xsl:when test ="IsSwapped='true' ">
											<xsl:value-of  select="'SEDOL'"/>
										</xsl:when>
										<xsl:when test ="Asset='EquityOption'">
											<xsl:value-of select="'TICKER'"/>
										</xsl:when>
									</xsl:choose>

								</UnderlyingSecurityIdentifierType>

								<UnderlyingSecurityIdentifier>
									<xsl:choose>
										<xsl:when test ="IsSwapped='true'">
											<xsl:value-of  select="SEDOL"/>
										</xsl:when>
										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="substring-before(OSIOptionSymbol,' ')"/>
										</xsl:when>
									</xsl:choose>
								</UnderlyingSecurityIdentifier>

								<SecurityDescription>
									<xsl:value-of select="translate(CompanyName,',','')"/>
								</SecurityDescription>


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

								<TradeDate>
									<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
								</TradeDate>


								<SettlementDate>
									<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
								</SettlementDate>

								<Settlementcurrency>

									<xsl:choose>
										<xsl:when test="Asset='FX'or Asset='FXForward'">
											<xsl:value-of select="LeadCurrencyName"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="SettlCurrency"/>
										</xsl:otherwise>
									</xsl:choose>

								</Settlementcurrency>

								<LocalCCY>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="VsCurrencyName"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="CurrencySymbol"/>
										</xsl:otherwise>
									</xsl:choose>

								</LocalCCY>

								<QuantityOriginalFace>

									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>

										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="OldExecutedQuantity"/>
										</xsl:when>


									</xsl:choose>

								</QuantityOriginalFace>

								<CurrentFace>
									<xsl:value-of select="''"/>
								</CurrentFace>

								<Price>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="number(OldAvgPrice)">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>


									</xsl:choose>
								</Price>

								<xsl:variable name="varAssetMultiplier">
									<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier)"/>
								</xsl:variable>


								<Principal>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="number($varAssetMultiplier)">
											<xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
										</xsl:when>

									</xsl:choose>
								</Principal>

								<xsl:variable name="COMM">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>
								<CommissionAmount>
									<xsl:choose>
										<xsl:when test="number(OldCommission)">
											<xsl:value-of select="OldCommission"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>

								</CommissionAmount>

								

								<!--<xsl:variable name="varOldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
								</xsl:variable>-->

								<xsl:variable name="varTaxorfees">
									<xsl:value-of select="(OldSecFee + OldStampDuty + OldOrfFee + OldMiscFees + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldOccFee)"/>
								</xsl:variable>
								<Taxorfees>
									<xsl:choose>
										<xsl:when test="number($varTaxorfees)">
											<xsl:value-of select="$varTaxorfees"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
									<!--<xsl:value-of select="$varTaxorfees"/>-->
								</Taxorfees>

								<Tax2>
									<xsl:choose>
										<xsl:when test="number(OldOtherBrokerFee)">
											<xsl:value-of select="OldOtherBrokerFee"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Tax2>

								<Interest>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="format-number(OldAccruedInterest,'#.00')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>

								</Interest>

								<NegativeInterest>
									<xsl:value-of select="''"/>
								</NegativeInterest>


								<ConsiderationConstant>
									<xsl:value-of select="AssetMultiplier"/>
								</ConsiderationConstant>


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
								<NetAmountLocal>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number($varOldNetAmount + AccruedInterest,'#.00')"/>
										</xsl:otherwise>
									</xsl:choose>
								</NetAmountLocal>

								<xsl:variable name="varNetAmountSettled">
									<xsl:choose>
										<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
											<xsl:value-of select="($varOldNetAmount+ AccruedInterest)* SettlCurrFxRate "/>
											<!--<xsl:value-of select="((AllocatedQty * AveragePrice * AssetMultiplier) + AccruedInterest + (CommissionCharged + (SecFee or StampDuty) + OtherBrokerFee))* SettlCurrFxRate"/>-->
										</xsl:when>
										<xsl:when test="Side='Sell' or Side='Sell to Open' or Side='Sell to Close' or Side='Sell short'">
											<xsl:value-of select="($varOldNetAmount + AccruedInterest)* SettlCurrFxRate "/>
											<!--<xsl:value-of select="((AllocatedQty * AveragePrice * AssetMultiplier) + AccruedInterest - (CommissionCharged + (SecFee or StampDuty) + OtherBrokerFee))* SettlCurrFxRate"/>-->
										</xsl:when>
									</xsl:choose>
								</xsl:variable>

								<NetAmountSettled>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="number($varNetAmountSettled)">
											<xsl:value-of select="format-number($varNetAmountSettled,'#.00')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</NetAmountSettled>

								<InternalNetNotional>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
											<xsl:value-of select="0"/>
										</xsl:when>
										<xsl:when test="number($varNetAmountSettled)">
											<xsl:value-of select="format-number($varOldNetAmount + AccruedInterest,'#.00')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</InternalNetNotional>


								<FXdealingrate>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>

								</FXdealingrate>

								<FXcurrencyReceived>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:choose>
												<xsl:when test="contains(OldSide,'Buy')">
													<xsl:value-of select="LeadCurrencyName"/>
												</xsl:when>
												<xsl:when test="contains(OldSide,'Sell')">
													<xsl:value-of select="VsCurrencyName"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</FXcurrencyReceived>

								<FXcurrencyDelivered>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:choose>
												<xsl:when test="contains(OldSide,'Sell')">
													<xsl:value-of select="LeadCurrencyName"/>
												</xsl:when>
												<xsl:when test="contains(OldSide,'Buy')">
													<xsl:value-of select="VsCurrencyName"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</FXcurrencyDelivered>

								<ClassSpecificHedge>
									<xsl:value-of select="''"/>
								</ClassSpecificHedge>

								<NotionalAmountReceived>
									<xsl:choose>
										<xsl:when test="Asset='FX'or Asset='FXForward'">
											<xsl:value-of select="format-number(OldExecutedQuantity * OldAvgPrice, '#.00')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</NotionalAmountReceived>

								<NotionalAmountdelivered>
									<xsl:choose>
										<xsl:when test="Asset='FX' or Asset='FXForward'">
											<xsl:value-of select="OldExecutedQuantity "/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</NotionalAmountdelivered>

								<StrategyCodes>
									<xsl:value-of select="''"/>
								</StrategyCodes>

								<StrikePrice>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
											<xsl:value-of select="StrikePrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</StrikePrice>


								<xsl:variable name="varExpirationDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="ExpirationDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

												
								<ExpireDate>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
											<xsl:value-of select="concat(substring-after(substring-after($varExpirationDate,'/'),'/'),substring-before($varExpirationDate,'/'),substring-before(substring-after($varExpirationDate,'/'),'/'))"/>
									</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</ExpireDate>

								<CountryofQuotation>
									<xsl:choose>
										<xsl:when test="Asset='FX'or Asset='FXForward'">
											<xsl:value-of select="substring(LeadCurrencyName,1,2)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(CurrencySymbol,1,2)"/>
										</xsl:otherwise>
									</xsl:choose>

								</CountryofQuotation>

								<Blank>
									<xsl:value-of select="''"/>
								</Blank>

								<Blank1>
									<xsl:value-of select="''"/>
								</Blank1>

								<Blank2>
									<xsl:value-of select="''"/>
								</Blank2>

								<Blank3>
									<xsl:value-of select="''"/>
								</Blank3>

								<Exchange>
									
									<xsl:value-of select="''"/>
								</Exchange>


								<AdditionalInfo1>
									<xsl:value-of select="''"/>
								</AdditionalInfo1>

								<AdditionalInfo2>
									<xsl:value-of select="''"/>
								</AdditionalInfo2>

								<AdditionalInfo3>
									<xsl:value-of select="''"/>
								</AdditionalInfo3>

								<AdditionalInfo4>
									<xsl:value-of select="''"/>
								</AdditionalInfo4>

								<AdditionalInfo5>
									<xsl:value-of select="''"/>
								</AdditionalInfo5>

								<AdditionalInfo6>
									<xsl:value-of select="''"/>
								</AdditionalInfo6>

								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>
						
						
						<ThirdPartyFlatFileDetail>
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter>
							
							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>
							
							

							<RecordTypeCode>
								<xsl:value-of select="'TRN'"/>
							</RecordTypeCode>


							<xsl:variable name ="varSecurityType">
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="'CFD'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AssetType>
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="'CFD'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test="(Asset='EquityOption' or Asset='FutureOption') and  PutOrCall='CALL'">
										<xsl:value-of select="'CALLOPT'"/>
									</xsl:when>
									<xsl:when test="(Asset='EquityOption' or Asset='FutureOption') and PutOrCall='PUT'">
										<xsl:value-of select="'PUTOPT'"/>
									</xsl:when>

									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'BOND'"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="'FUTURE'"/>
									</xsl:when>

									<xsl:when test="Asset='Warrant'">
										<xsl:value-of select="'WARRANT'"/>
									</xsl:when>

									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="'FX'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="Asset"/>
									</xsl:otherwise>
								</xsl:choose>
							</AssetType>


							
							<TransactionStatus>
								<xsl:value-of select="'NEW'"/>
							</TransactionStatus>

							<LongShortIndicator>
								<xsl:choose>

									<xsl:when test="(Asset='FX' or Asset='FXForward') and Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Buy' or Side='Sell' or  Side='Buy to Open' or Side='Sell to Close'">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Buy to Close' or Side='Sell to Open'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
								</xsl:choose>


							</LongShortIndicator>

							<TransactionCode>

								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="'FXDL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SHORT'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'COVER'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Side"/>
									</xsl:otherwise>
								</xsl:choose>
							</TransactionCode>

							<ClientTradeRefNo>
								<xsl:value-of select="substring(PBUniqueID,4,3)"/>
							</ClientTradeRefNo>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'NT'"/>
							</xsl:variable>

							<xsl:variable name ="varAccount">
								<xsl:choose>
									<xsl:when test ="Asset='FXForward'">
										<xsl:value-of  select="concat('SC','/',ISIN)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AccountId>
								<xsl:value-of select="$varAccount"/>
							</AccountId>

						
							<xsl:variable name = "PRANA_GUICODE_NAME">
								<xsl:value-of select="ISIN"/>
							</xsl:variable>

							<xsl:variable name="PRANA_GUICODE_MAPPING">
								<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_GuiAccountMapping.xml')/GuiAccountMapping/PB[@Name='NT']/GuiAccountData[@GuiIsinCode=$PRANA_GUICODE_NAME]/@GuiAccountCode"/>
							</xsl:variable>


							<xsl:variable name ="varAccount1">
								<xsl:choose>
									<xsl:when test ="Asset='FXForward'">
										<xsl:value-of  select="concat('1750261','-',$PRANA_GUICODE_MAPPING)"/>
									</xsl:when>
									<xsl:when test ="Asset='FX'">
										<xsl:value-of  select="'1750261'"/>
									</xsl:when>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="'6007884'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="'1750261'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PrimeBrokerCustodianAccountID>
								<xsl:value-of select="$varAccount1"/>
							</PrimeBrokerCustodianAccountID>


							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

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
							<BrokerCounterparty>
								<xsl:value-of select="''"/>
							</BrokerCounterparty>


							<BrokerDescription>
								<xsl:value-of select="CounterParty"/>
							</BrokerDescription>

							<ClearingBroker>
								<xsl:value-of select="''"/>
							</ClearingBroker>

							<ClearingBrokerdescription>
								<xsl:value-of select="''"/>
							</ClearingBrokerdescription>

							<xsl:variable name="varSecurityName">
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="''"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'ISIN'"/>
									</xsl:when>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="Asset='Equity' and SEDOL=''">
										<xsl:value-of select="'CUSIP'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'SEDOL'"/>
									</xsl:when>
									<xsl:when test="Asset='Future' or Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="'TICKER'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="'TICKER'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<SecurityIdentifierType>
								<xsl:value-of select="$varSecurityName"/>
							</SecurityIdentifierType>

							<xsl:variable name="VarSecurityIdentifier">
								<xsl:value-of select="normalize-space(substring-before(BBCode,'EQUITY'))"/>
							</xsl:variable>

							<xsl:variable name="SecurityIdentifier">
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="''"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="ISIN"/>
									</xsl:when>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="Asset='Equity' and SEDOL=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="substring-before(BBCode,' ')"/>
									</xsl:when>

									<xsl:when test="Asset='FutureOption'">
										<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
									</xsl:when>

									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="$VarSecurityIdentifier"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<SecurityIdentifier>
								<xsl:value-of select ="$SecurityIdentifier"/>
							</SecurityIdentifier>


							<UnderlyingSecurityIdentifierType>
								<xsl:choose>
									<xsl:when test ="IsSwapped='true' ">
										<xsl:value-of  select="'SEDOL'"/>
									</xsl:when>
									<xsl:when test ="Asset='EquityOption'">
										<xsl:value-of select="'TICKER'"/>
									</xsl:when>
								</xsl:choose>

							</UnderlyingSecurityIdentifierType>

							<UnderlyingSecurityIdentifier>
								<xsl:choose>
									<xsl:when test ="IsSwapped='true'">
										<xsl:value-of  select="SEDOL"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="substring-before(OSIOptionSymbol,' ')"/>
									</xsl:when>
								</xsl:choose>
							</UnderlyingSecurityIdentifier>

							<SecurityDescription>
								<xsl:value-of select="translate(CompanyName,',','')"/>
							</SecurityDescription>


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

							<TradeDate>
								<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlementDate>


							<Settlementcurrency>

								<xsl:choose>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="LeadCurrencyName"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="SettlCurrency"/>
									</xsl:otherwise>
								</xsl:choose>

							</Settlementcurrency>

							<LocalCCY>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="VsCurrencyName"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CurrencySymbol"/>
									</xsl:otherwise>
								</xsl:choose>

							</LocalCCY>

							<QuantityOriginalFace>


								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>


								</xsl:choose>

							</QuantityOriginalFace>

							<CurrentFace>
								<xsl:value-of select="''"/>
							</CurrentFace>

							<Price>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="number(AvgPrice)">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>


								</xsl:choose>
							</Price>

							<xsl:variable name="varAssetMultiplier">
								<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
							</xsl:variable>


							<Principal>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="number($varAssetMultiplier)">
										<xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
									</xsl:when>

								</xsl:choose>
							</Principal>


							<CommissionAmount>
								<xsl:choose>
									<xsl:when test="number(CommissionCharged)">
										<xsl:value-of select="CommissionCharged"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>

								</xsl:choose>

							</CommissionAmount>

							<xsl:variable name="varSecFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="SecFee"/>
								</xsl:call-template>
							</xsl:variable>



							<xsl:variable name="varStampDuty">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="StampDuty"/>
								</xsl:call-template>
							</xsl:variable>


							<xsl:variable name="varOrfFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="OrfFee"/>
								</xsl:call-template>
							</xsl:variable>



							<xsl:variable name="varTaxorfees">
								<xsl:value-of select="(SecFee + StampDuty + OrfFee + MiscFees + TransactionLevy + ClearingFee + TaxOnCommissions + OccFee)"/>
							</xsl:variable>
							<Taxorfees>
								<xsl:choose>
									<xsl:when test="number($varTaxorfees)">
										<xsl:value-of select="$varTaxorfees"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
								<!--<xsl:value-of select="$varTaxorfees"/>-->
							</Taxorfees>

							<Tax2>
								<xsl:choose>
									<xsl:when test="number(OtherBrokerFee)">
										<xsl:value-of select="OtherBrokerFee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Tax2>

							<Interest>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="format-number(AccruedInterest,'#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</Interest>

							<NegativeInterest>
								<xsl:value-of select="''"/>
							</NegativeInterest>


							<ConsiderationConstant>
								<xsl:value-of select="AssetMultiplier"/>
							</ConsiderationConstant>

							<NetAmountLocal>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($varNetamount + AccruedInterest,'#.00')"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetAmountLocal>

							<xsl:variable name="varNetAmountSettled">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
										<xsl:value-of select="($varNetamount+ AccruedInterest)* SettlCurrFxRate "/>
										<!--<xsl:value-of select="((AllocatedQty * AveragePrice * AssetMultiplier) + AccruedInterest + (CommissionCharged + (SecFee or StampDuty) + OtherBrokerFee))* SettlCurrFxRate"/>-->
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Open' or Side='Sell to Close' or Side='Sell short'">
										<xsl:value-of select="($varNetamount + AccruedInterest)* SettlCurrFxRate "/>
										<!--<xsl:value-of select="((AllocatedQty * AveragePrice * AssetMultiplier) + AccruedInterest - (CommissionCharged + (SecFee or StampDuty) + OtherBrokerFee))* SettlCurrFxRate"/>-->
									</xsl:when>
								</xsl:choose>
							</xsl:variable>

							<NetAmountSettled>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="number($varNetAmountSettled)">
										<xsl:value-of select="format-number($varNetAmountSettled,'#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetAmountSettled>

							<InternalNetNotional>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='Future' or IsSwapped ='true' or Asset='FXForward'">
										<xsl:value-of select="0"/>
									</xsl:when>
									<xsl:when test="number($varNetAmountSettled)">
										<xsl:value-of select="format-number($varNetamount + AccruedInterest,'#.00')"/>
										<!--<xsl:value-of select="format-number($varNetAmountSettled,'#.00')"/>-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</InternalNetNotional>


							<FXdealingrate>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</FXdealingrate>

							<FXcurrencyReceived>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:choose>
											<xsl:when test="contains(Side,'Buy')">
												<xsl:value-of select="LeadCurrencyName"/>
											</xsl:when>
											<xsl:when test="contains(Side,'Sell')">
												<xsl:value-of select="VsCurrencyName"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</FXcurrencyReceived>

							<FXcurrencyDelivered>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:choose>
											<xsl:when test="contains(Side,'Sell')">
												<xsl:value-of select="LeadCurrencyName"/>
											</xsl:when>
											<xsl:when test="contains(Side,'Buy')">
												<xsl:value-of select="VsCurrencyName"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</FXcurrencyDelivered>

							<ClassSpecificHedge>
								<xsl:value-of select="''"/>
							</ClassSpecificHedge>

							<NotionalAmountReceived>
								<xsl:choose>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="format-number(OrderQty * AvgPrice, '#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</NotionalAmountReceived>

							<NotionalAmountdelivered>
								<xsl:choose>
									<xsl:when test="Asset='FX' or Asset='FXForward'">
										<xsl:value-of select="OrderQty "/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</NotionalAmountdelivered>

							<StrategyCodes>
								<xsl:value-of select="''"/>
							</StrategyCodes>

							<StrikePrice>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="StrikePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</StrikePrice>


							<xsl:variable name="varExpirationDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>



							<ExpireDate>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="concat(substring-after(substring-after($varExpirationDate,'/'),'/'),substring-before($varExpirationDate,'/'),substring-before(substring-after($varExpirationDate,'/'),'/'))"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExpireDate>

							<CountryofQuotation>
								<xsl:choose>
									<xsl:when test="Asset='FX'or Asset='FXForward'">
										<xsl:value-of select="substring(LeadCurrencyName,1,2)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring(CurrencySymbol,1,2)"/>
									</xsl:otherwise>
								</xsl:choose>

							</CountryofQuotation>

							<Blank>
								<xsl:value-of select="''"/>
							</Blank>

							<Blank1>
								<xsl:value-of select="''"/>
							</Blank1>

							<Blank2>
								<xsl:value-of select="''"/>
							</Blank2>

							<Blank3>
								<xsl:value-of select="''"/>
							</Blank3>

							<Exchange>
								
								<xsl:value-of select="''"/>
							</Exchange>


							<AdditionalInfo1>
								<xsl:value-of select="''"/>
							</AdditionalInfo1>

							<AdditionalInfo2>
								<xsl:value-of select="''"/>
							</AdditionalInfo2>

							<AdditionalInfo3>
								<xsl:value-of select="''"/>
							</AdditionalInfo3>

							<AdditionalInfo4>
								<xsl:value-of select="''"/>
							</AdditionalInfo4>

							<AdditionalInfo5>
								<xsl:value-of select="''"/>
							</AdditionalInfo5>

							<AdditionalInfo6>
								<xsl:value-of select="''"/>
							</AdditionalInfo6>

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
