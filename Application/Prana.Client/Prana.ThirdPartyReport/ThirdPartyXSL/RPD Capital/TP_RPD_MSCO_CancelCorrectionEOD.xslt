<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'False'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select ="TaxLotState"/>
				</TaxLotState>

				<TransactionType>
					<xsl:value-of select ="'TransactionType'"/>
				</TransactionType>

				<Account>
					<xsl:value-of select ="'Account'"/>
				</Account>

				<ClientRefNumber>
					<xsl:value-of select ="'ClientRefNumber'"/>
				</ClientRefNumber>

				<VersionNo>
					<xsl:value-of select ="'VersionNo'"/>
				</VersionNo>

				<TransactionStatus>
					<xsl:value-of select ="'TransactionStatus'"/>
				</TransactionStatus>

				<SecurityIdentifierType>
					<xsl:value-of select ="'SecurityIdentifierType'"/>
				</SecurityIdentifierType>

				<SecurityID>
					<xsl:value-of select ="'SecurityID'"/>
				</SecurityID>

				<ContractYear>
					<xsl:value-of select ="'ContractYear'"/>
				</ContractYear>

				<ContractMonth>
					<xsl:value-of select ="'ContractMonth'"/>
				</ContractMonth>

				<ContractDate>
					<xsl:value-of select ="'ContractDate'"/>
				</ContractDate>

				<SecurityDescription>
					<xsl:value-of select ="'SecurityDescription'"/>
				</SecurityDescription>

				<ExchangeCode>
					<xsl:value-of select ="'ExchangeCode'"/>
				</ExchangeCode>

				<BuySell>
					<xsl:value-of select ="'Buy/Sell'"/>
				</BuySell>

				<TradeType>
					<xsl:value-of select ="'TradeType'"/>
				</TradeType>

				<OrderToCloseIndicator>
					<xsl:value-of select ="'OrderToCloseIndicator'"/>
				</OrderToCloseIndicator>

				<AveragePriceIndicator>
					<xsl:value-of select ="'AveragePriceIndicator'"/>
				</AveragePriceIndicator>

				<SpreadTradeIndicator>
					<xsl:value-of select ="'SpreadTradeIndicator'"/>
				</SpreadTradeIndicator>

				<NightTradeIndicator>
					<xsl:value-of select ="'NightTradeIndicator'"/>
				</NightTradeIndicator>

				<ExchangeForPhysicalIndicator>
					<xsl:value-of select ="'ExchangeForPhysicalIndicator'"/>
				</ExchangeForPhysicalIndicator>

				<BlockTradeIndicator>
					<xsl:value-of select ="'BlockTradeIndicator'"/>
				</BlockTradeIndicator>

				<OffExchangeInd>
					<xsl:value-of select ="'OffExchangeInd'"/>
				</OffExchangeInd>

				<TradeDate>
					<xsl:value-of select ="'TradeDate'"/>
				</TradeDate>

				<ExecutionTime>
					<xsl:value-of select ="'ExecutionTime'"/>
				</ExecutionTime>

				<Quantity>
					<xsl:value-of select ="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select ="'Price'"/>
				</Price>

				<CallPutIndicator>
					<xsl:value-of select ="'CallPutIndicator'"/>
				</CallPutIndicator>

				<StrikePrice>
					<xsl:value-of select ="'StrikePrice'"/>
				</StrikePrice>

				<ExecutingBroker>
					<xsl:value-of select ="'Executing Broker'"/>
				</ExecutingBroker>

				<ClearingBroker>
					<xsl:value-of select ="'Clearing Broker'"/>
				</ClearingBroker>

				<GiveUpReferenceId>
					<xsl:value-of select ="'GiveUpReferenceId'"/>
				</GiveUpReferenceId>

				<HearsayInd>
					<xsl:value-of select ="'HearsayInd'"/>
				</HearsayInd>

				<ExecutionFee>
					<xsl:value-of select ="'ExecutionFee'"/>
				</ExecutionFee>

				<ExecutionFeeCcy>
					<xsl:value-of select ="'ExecutionFeeCcy'"/>
				</ExecutionFeeCcy>

				<Commission>
					<xsl:value-of select ="'Commission'"/>
				</Commission>

				<CommissionCcy>
					<xsl:value-of select ="'CommissionCcy'"/>
				</CommissionCcy>

				<ExchangeFee>
					<xsl:value-of select ="'ExchangeFee'"/>
				</ExchangeFee>

				<ExchangeFeeCcy>
					<xsl:value-of select ="'ExchangeFeeCcy'"/>
				</ExchangeFeeCcy>

				<OrderId>
					<xsl:value-of select ="'OrderId '"/>
				</OrderId>

				<DealId>
					<xsl:value-of select ="'DealId'"/>
				</DealId>

				<Message>
					<xsl:value-of select ="'Message'"/>
				</Message>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='MSCO'][AccountName ='Jones Trading - RPD' or AccountName ='Jones Trading - Fortress' or AccountName = 'MS - Prelude' or AccountName ='Topwater Equities' or AccountName ='MS - RPD Fund'][Asset ='EquityOption']">

				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<TransactionType>
						<xsl:value-of select ="'D01'"/>
					</TransactionType>

					<Account>
						<xsl:value-of select ="AccountNo"/>
					</Account>

					<ClientRefNumber>
						<xsl:value-of select="PBUniqueID"/>
					</ClientRefNumber>

					<VersionNo>
						<xsl:value-of select ="''"/>
					</VersionNo>

					<xsl:variable name="varTaxlotState">
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
					<TransactionStatus>
						<xsl:value-of select ="$varTaxlotState"/>
					</TransactionStatus>

					<SecurityIdentifierType>
						<xsl:value-of select ="''"/>
					</SecurityIdentifierType>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="Asset = 'EquityOption'">
								<xsl:value-of select="concat(substring-before(BBCode,' EQUITY'),' Equity')"/>
							</xsl:when>
							<xsl:when test="Asset = 'Equity'">
								<xsl:value-of select ="concat(substring-before(BBCode,'US'),'US Equity')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</SecurityID>

					<ContractYear>
						<xsl:value-of select ="substring(TradeDate,7,4)"/>
					</ContractYear>

					<ContractMonth>
						<xsl:value-of select ="substring(TradeDate,1,2)"/>
					</ContractMonth>

					<ContractDate>
						<xsl:value-of select ="substring(TradeDate,4,2)"/>
					</ContractDate>

					<SecurityDescription>
						<xsl:value-of select ="FullSecurityName"/>
					</SecurityDescription>

					<ExchangeCode>
						<xsl:value-of select ="''"/>
					</ExchangeCode>

					<BuySell>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Open' or Side= 'Buy to Close'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'B'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</BuySell>

					<TradeType>
						<xsl:value-of select ="''"/>
					</TradeType>

					<OrderToCloseIndicator>
						<xsl:value-of select="'Y'"/>
					</OrderToCloseIndicator>

					<AveragePriceIndicator>
						<xsl:value-of select ="''"/>
					</AveragePriceIndicator>

					<SpreadTradeIndicator>
						<xsl:value-of select ="''"/>
					</SpreadTradeIndicator>

					<NightTradeIndicator>
						<xsl:value-of select ="''"/>
					</NightTradeIndicator>

					<ExchangeForPhysicalIndicator>
						<xsl:value-of select ="''"/>
					</ExchangeForPhysicalIndicator>

					<BlockTradeIndicator>
						<xsl:value-of select ="''"/>
					</BlockTradeIndicator>

					<OffExchangeInd>
						<xsl:value-of select ="''"/>
					</OffExchangeInd>

					<xsl:variable name="Trade_Day" select="substring(TradeDate,4,2)"/>
					<xsl:variable name="Trade_Month" select="substring(TradeDate,1,2)"/>
					<xsl:variable name="Trade_Year" select="substring(TradeDate,7,4)"/>

					<TradeDate>
						<xsl:value-of select="concat($Trade_Year,$Trade_Month,$Trade_Day)"/>
					</TradeDate>

					<ExecutionTime>
						<xsl:value-of select ="''"/>
					</ExecutionTime>

					<Quantity>
						<xsl:value-of select ="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select ="AveragePrice"/>
					</Price>

					<CallPutIndicator>
						<xsl:choose>
							<xsl:when test="PutOrCall='CALL'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:when test="PutOrCall='PUT'">
								<xsl:value-of select="'P'"/>
							</xsl:when>
						</xsl:choose>
					</CallPutIndicator>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<xsl:variable name="Pb_name" select="'GS'"/>
					<xsl:variable name="varAccount" select="AccountName"/>
					
					<ExecutingBroker>
						<xsl:value-of select="'MSCO'"/>
					</ExecutingBroker>
					
					<xsl:variable name="PB_ClearingBroker">
						<xsl:value-of select="document('../ReconMappingXml/TP_ClearingBrokerMapping.xml')/BrokerMapping/PB[@Name = $Pb_name]/BrokerData[@PranaAccountName = $varAccount]/@PranaClearingBroker"/>
					</xsl:variable>
					
					<xsl:variable name="varClearingBroker">
						<xsl:choose>
							<xsl:when test="$PB_ClearingBroker != ''">
								<xsl:value-of select="$PB_ClearingBroker"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PB_ClearingBroker"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<ClearingBroker>
						<xsl:value-of select ="$varClearingBroker"/>
					</ClearingBroker>

					<GiveUpReferenceId>
						<xsl:value-of select ="''"/>
					</GiveUpReferenceId>

					<HearsayInd>
						<xsl:value-of select ="''"/>
					</HearsayInd>

					<ExecutionFee>
						<xsl:value-of select ="''"/>
					</ExecutionFee>

					<ExecutionFeeCcy>
						<xsl:value-of select ="''"/>
					</ExecutionFeeCcy>

					<xsl:variable name = "varTotalComm">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					<Commission>
						<xsl:value-of select ="$varTotalComm"/>
					</Commission>

					<CommissionCcy>
						<xsl:value-of select ="CurrencySymbol"/>
					</CommissionCcy>

					<ExchangeFee>
						<xsl:value-of select ="''"/>
					</ExchangeFee>

					<ExchangeFeeCcy>
						<xsl:value-of select ="''"/>
					</ExchangeFeeCcy>

					<OrderId>
						<xsl:value-of select ="''"/>
					</OrderId>

					<DealId>
						<xsl:value-of select ="''"/>
					</DealId>

					<Message>
						<xsl:value-of select ="''"/>
					</Message>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>