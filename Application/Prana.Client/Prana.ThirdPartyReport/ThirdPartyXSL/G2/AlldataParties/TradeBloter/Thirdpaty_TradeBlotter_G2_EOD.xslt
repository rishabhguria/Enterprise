<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<AccountName>
					<xsl:value-of select ="'Account Name'"/>
				</AccountName>

				<TradeDate>
					<xsl:value-of select ="'Trade Date'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select ="'Settlement Date'"/>
				</SettlementDate>

				<Side>
					<xsl:value-of select ="'Side'"/>
				</Side>

				<CUSIP>
					<xsl:value-of select ="'CUSIP'"/>
				</CUSIP>

				<Symbol>
					<xsl:value-of select ="'Symbol'"/>
				</Symbol>

				<SecurityDescription>
					<xsl:value-of select ="'Security Description'"/>
				</SecurityDescription>

				<ExecutedQty>
					<xsl:value-of select ="'Executed Qty'"/>
				</ExecutedQty>

				<AvgPrice>
					<xsl:value-of select ="'Avg Price'"/>
				</AvgPrice>

				<PrincipalAmount>
					<xsl:value-of select ="'Principal Amount'"/>
				</PrincipalAmount>

				<TotalCommission>
					<xsl:value-of select ="'Total Commission'"/>
				</TotalCommission>

				<Fees>
					<xsl:value-of select ="'Fees'"/>
				</Fees>

				<NetAmount>
					<xsl:value-of select ="'Net Amount'"/>
				</NetAmount>

				<SettleCurrency>
					<xsl:value-of select ="'Settle Currency'"/>
				</SettleCurrency>

				<Broker>
					<xsl:value-of select ="'Broker'"/>
				</Broker>

				<TRADERID>
					<xsl:value-of select="'TRADER ID'"/>
				</TRADERID>


				<!-- system inetrnal use-->
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>


			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'G2'"/>
					</xsl:variable>

					<xsl:variable name="PB_FUND_NAME" select="AccountName"/>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>


					<AccountName>
						<xsl:choose>

							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="$PB_FUND_NAME"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>

					<TradeDate>
						<xsl:value-of select ="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select ="SettlementDate"/>
					</SettlementDate>

					<Side>
						<xsl:value-of select ="Side"/>
					</Side>

					<CUSIP>
						<xsl:value-of select ="CUSIP"/>
					</CUSIP>

					<Symbol>
						<xsl:value-of select ="Symbol"/>
					</Symbol>

					<SecurityDescription>
						<xsl:value-of select ="FullSecurityName"/>
					</SecurityDescription>

					<ExecutedQty>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select ="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutedQty>

					<AvgPrice>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select ="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</AvgPrice>

					<xsl:variable name="Pricipal">
						<xsl:value-of select="(AllocatedQty * AveragePrice * AssetMultiplier)"/>
					</xsl:variable>

					<PrincipalAmount>
						<xsl:value-of select ="GrossAmount"/>
					</PrincipalAmount>

					<xsl:variable name="Commission" select="CommissionCharged + SoftCommissionCharged"/>

					<TotalCommission>
						<xsl:value-of select ="$Commission"/>
					</TotalCommission>

					<xsl:variable name="OtherFees">
						<xsl:value-of select="OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + SoftCommissionCharged"/>
					</xsl:variable>

					<Fees>
						<!--<xsl:choose>
							<xsl:when test="number($OtherFees)">
								<xsl:value-of select="$OtherFees"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="TotalCommissionAndFee"/>
					</Fees>

					<NetAmount>
						<xsl:value-of select ="NetAmount"/>
					</NetAmount>

					<SettleCurrency>
						<xsl:value-of select ="SettlCurrency"/>
					</SettleCurrency>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='19Capital']/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@MLPBroker"/>
					</xsl:variable>

					<xsl:variable name="varBROKER">
						<xsl:choose>
							<xsl:when test="$PB_BROKER_NAME != ''">
								<xsl:value-of select="$PB_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<Broker>
						<xsl:value-of select ="$varBROKER"/>
					</Broker>

					<TRADERID>
						<xsl:value-of select ="TradeAttribute3"/>
					</TRADERID>

					<!-- system inetrnal use-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
