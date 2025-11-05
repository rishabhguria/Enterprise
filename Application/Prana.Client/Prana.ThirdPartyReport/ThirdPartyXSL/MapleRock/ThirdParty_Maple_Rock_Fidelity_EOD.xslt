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
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:variable name="NetAmount" select="(OrderQty * AvgPrice) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>

				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'True'"/>
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

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="ExpirationDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<ClientReferencenumber>
								<xsl:value-of select="PBUniqueID"/>
							</ClientReferencenumber>
							<Side>
								<xsl:choose>
									<xsl:when test="Side='Buy' ">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' ">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' ">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'BC'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Open'">
										<xsl:value-of select="'BTO'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open'">
										<xsl:value-of select="'STO'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close'">
										<xsl:value-of select="'STC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>
							<Symbol>
								<xsl:choose>
									<xsl:when test ="Asset='EquityOption'">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>					
									<xsl:when test="Symbol!=''">
										<xsl:value-of select="Symbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>
							<CusipSedol>
								<xsl:choose>
									<xsl:when test="SEDOL!=''">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
								</xsl:choose>								
							</CusipSedol>
							<Qty>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="format-number(OrderQty,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Qty>							
							
							<xsl:variable name="PB_NAME" select="''"/>

								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
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
							<ExBroker>
								<xsl:value-of select="$Broker"/>
							</ExBroker>

							<SettleCcy>
								<xsl:value-of select="SettlCurrency"/>
							</SettleCcy>

							<Price>
								<xsl:value-of select="format-number(AvgPrice,'0.########')"/>
							</Price>
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
							<Account>
								<xsl:value-of select="$varAccountName"/>
							</Account>


							<TradeDate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>
							<SettleDate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SettleDate>

							<Asset>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="'OPTION'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Asset>

							<Interest>
								<xsl:value-of select="AccruedInterest"/>
							</Interest>

							<CancelCorrect>
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEW'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Amended'">
										<xsl:value-of select ="'DELETE'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'DELETE'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</CancelCorrect>

							<CommType>
								<xsl:value-of select="'P'"/>								
							</CommType>
							<xsl:variable name="COMMISS">
								<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) div OrderQty"/>
							</xsl:variable>
							<Commission>
								<xsl:choose>
									<xsl:when test="number($COMMISS)">
										<xsl:value-of select="format-number($COMMISS,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Commission>

							<NetMoney>
								<xsl:choose>
									<xsl:when test="number($NetAmount)">
										<xsl:value-of select="format-number($NetAmount,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							
							</NetMoney>

							<UnderlyingSymbol>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="UnderlyingSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
								
							</UnderlyingSymbol>

							<ExpirationDate>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="concat(substring-before($ExpirationDate,'/'),'/',substring-before(substring-after($ExpirationDate,'/'),'/'),'/',substring-after(substring-after($ExpirationDate,'/'),'/'))"/>
										</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExpirationDate>

							<OptionType>
								<xsl:value-of select="PutOrCall"/>
							</OptionType>

							<StrikePrice>
								<xsl:choose>
									<xsl:when test="number(StrikePrice)">
										<xsl:value-of select="StrikePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>								
							</StrikePrice>
							
<!--This tag for show total QTY in footer-->
							<ALLOCQTY>
								<xsl:value-of select="OrderQty"/>
							</ALLOCQTY>
							
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>

								<RowHeader>
									<xsl:value-of select ="'true'"/>
								</RowHeader>
								<FileHeader>
									<xsl:value-of select="'true'"/>
								</FileHeader>

								<FileFooter>
									<xsl:value-of select="'true'"/>
								</FileFooter>

								<TaxLotState>
									<xsl:value-of select="Deleted"/>
								</TaxLotState>


								<xsl:variable name="OldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<xsl:variable name="OldSettleDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="OldExpirationDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="ExpirationDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<ClientReferencenumber>
									<xsl:value-of select="PBUniqueID"/>
								</ClientReferencenumber>
								<Side>
									<xsl:choose>
										<xsl:when test="OldSide='Buy' ">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell' ">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short' ">
											<xsl:value-of select="'SS'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'BC'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Open'">
											<xsl:value-of select="'BTO'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell to Open'">
											<xsl:value-of select="'STO'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell to Close'">
											<xsl:value-of select="'STC'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Side>
								<Symbol>
									<xsl:choose>
										<xsl:when test ="Asset='EquityOption'">
											<xsl:value-of select="OSIOptionSymbol"/>
										</xsl:when>
										<xsl:when test="Symbol!=''">
											<xsl:value-of select="Symbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>
								<CusipSedol>
									<xsl:choose>
										<xsl:when test="SEDOL!=''">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>
										<xsl:when test="CUSIP!=''">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>
									</xsl:choose>
								</CusipSedol>
								<Qty>
									<xsl:choose>
										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="OldExecutedQuantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</Qty>
								
							<xsl:variable name="PB_NAME" select="''"/>

								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="OldCounterparty"/>
								<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
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
								<ExBroker>
									<xsl:value-of select="$Broker"/>
								</ExBroker>

								<SettleCcy>
									<xsl:value-of select="SettlCurrency"/>
								</SettleCcy>

								<Price>
									<xsl:value-of select="format-number(OldAvgPrice,'0.########')"/>
								</Price>
							
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

								<Account>
									<xsl:value-of select="$varAccountName"/>
								</Account>
								<TradeDate>
									<xsl:value-of select="concat(substring-before($OldTradeDate,'/'),'/',substring-before(substring-after($OldTradeDate,'/'),'/'),'/',substring-after(substring-after($OldTradeDate,'/'),'/'))"/>
								</TradeDate>
								
								<SettleDate>
									<xsl:value-of select="concat(substring-before($OldSettleDate,'/'),'/',substring-before(substring-after($OldSettleDate,'/'),'/'),'/',substring-after(substring-after($OldSettleDate,'/'),'/'))"/>
								</SettleDate>
								
								<Asset>
									<xsl:choose>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'STOCK'"/>
										</xsl:when>
										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="'OPTION'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Asset>

								<Interest>
									<xsl:value-of select="OldAccruedInterest"/>
								</Interest>

								<CancelCorrect>
									<xsl:value-of select ="'Delete'"/>									
								</CancelCorrect>

								<CommType>
									<xsl:value-of select="'P'"/>
								</CommType>
								<xsl:variable name="COMM">
									<xsl:value-of select="(OldCommission + OldSoftCommission) div OldExecutedQuantity"/>
								</xsl:variable>
								<Commission>
									<xsl:value-of select="$COMM"/>
								</Commission>

								<xsl:variable name="varNetAmmount">									
									<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>	
								</xsl:variable>
								<NetMoney>
									<xsl:choose>
										<xsl:when test="number($varNetAmmount)">
											<xsl:value-of select="format-number($varNetAmmount,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>									
								</NetMoney>

								<UnderlyingSymbol>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="UnderlyingSymbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>

								</UnderlyingSymbol>

								<ExpirationDate>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="concat(substring-before($OldExpirationDate,'/'),'/',substring-before(substring-after($OldExpirationDate,'/'),'/'),'/',substring-after(substring-after($OldExpirationDate,'/'),'/'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</ExpirationDate>

								<OptionType>
									<xsl:value-of select="PutOrCall"/>
								</OptionType>

								<StrikePrice>
									<xsl:choose>
										<xsl:when test="number(StrikePrice)">
											<xsl:value-of select="StrikePrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</StrikePrice>
								
								<!--This tag for show total QTY in footer-->
								<ALLOCQTY>
									<xsl:value-of select="OldExecutedQuantity"/>
								</ALLOCQTY>						

								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>

						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'true'"/>
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

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="ExpirationDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="ExpirationDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<ClientReferencenumber>
								<xsl:value-of select="PBUniqueID"/>
							</ClientReferencenumber>
							<Side>
								<xsl:choose>
									<xsl:when test="Side='Buy' ">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' ">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' ">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'BC'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Open'">
										<xsl:value-of select="'BTO'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open'">
										<xsl:value-of select="'STO'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close'">
										<xsl:value-of select="'STC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>
							<Symbol>
								<xsl:choose>
									<xsl:when test ="Asset='EquityOption'">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:when test="Symbol!=''">
										<xsl:value-of select="Symbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>
							<CusipSedol>
								<xsl:choose>
									<xsl:when test="SEDOL!=''">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
								</xsl:choose>
							</CusipSedol>
							<Qty>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="format-number(OrderQty,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Qty>

							
							<xsl:variable name="PB_NAME" select="''"/>

							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
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
							<ExBroker>
								<xsl:value-of select="$Broker"/>
							</ExBroker>
							<SettleCcy>
								<xsl:value-of select="SettlCurrency"/>
							</SettleCcy>
							<Price>
								<xsl:value-of select="format-number(AvgPrice,'0.########')"/>
							</Price>
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
							<Account>
								<xsl:value-of select="$varAccountName"/>
							</Account>
							<TradeDate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>
							<SettleDate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SettleDate>

							<Asset>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="'OPTION'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Asset>

							<Interest>
								<xsl:value-of select="AccruedInterest"/>
							</Interest>

							<CancelCorrect>
								<xsl:value-of select ="'NEW'"/>									
							</CancelCorrect>

							<CommType>
								<xsl:value-of select="'P'"/>
							</CommType>
							<xsl:variable name="COMMISS">
								<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) div OrderQty"/>
							</xsl:variable>
							<Commission>
								<xsl:value-of select="$COMMISS"/>
							</Commission>

							<NetMoney>
								<xsl:choose>
									<xsl:when test="number($NetAmount)">
										<xsl:value-of select="format-number($NetAmount,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetMoney>

							<UnderlyingSymbol>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="UnderlyingSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</UnderlyingSymbol>

							<ExpirationDate>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="concat(substring-before($ExpirationDate,'/'),'/',substring-before(substring-after($ExpirationDate,'/'),'/'),'/',substring-after(substring-after($ExpirationDate,'/'),'/'))"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExpirationDate>

							<OptionType>
								<xsl:value-of select="PutOrCall"/>
							</OptionType>

							<StrikePrice>
								<xsl:choose>
									<xsl:when test="number(StrikePrice)">
										<xsl:value-of select="StrikePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</StrikePrice>
<!--This tag for show total QTY in footer-->
							<ALLOCQTY>
								<xsl:value-of select="OrderQty"/>
							</ALLOCQTY>

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