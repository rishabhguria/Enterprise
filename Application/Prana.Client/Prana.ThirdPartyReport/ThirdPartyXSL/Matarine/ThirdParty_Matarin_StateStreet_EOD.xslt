<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
		
			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--<FileHeader>
					<xsl:value-of select="'true'"/>
				</FileHeader>-->

				<FileFooter>
					<xsl:value-of select="'true'"/>
				</FileFooter>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>
				
				<BuySell>
					<xsl:value-of select="'Buy/Sell'"/>
				</BuySell>

				<ALLOCQTY>
					<xsl:value-of select="'ALLOCQTY'"/>
				</ALLOCQTY>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Description>
					<xsl:value-of select="'Description'"/>
				</Description>

				<Symbol>
					<xsl:value-of select="'Symbol'"/>
				</Symbol>

				<AvgPx>
					<xsl:value-of select="'AvgPx'"/>
				</AvgPx>

				<CUSIPID>
					<xsl:value-of select="'CUSIP ID'"/>
				</CUSIPID>

				<RefID>
					<xsl:value-of select="'Ref. ID'"/>
				</RefID>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<SECFee>
					<xsl:value-of select="'SEC Fee'"/>
				</SECFee>

				<InternalNetNotional>
					<xsl:value-of select="'InternalNetNotional'"/>
				</InternalNetNotional>
				
				<PostCommissionNetMoney>
					<xsl:value-of select="'Post Commission Net Money'"/>
				</PostCommissionNetMoney>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<PrincipalAmt>
					<xsl:value-of select="'Principal Amt'"/>
				</PrincipalAmt>

				<ExecutingBroker>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecutingBroker>


				<AccountName>
					<xsl:value-of select="'Account Name'"/>
				</AccountName>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--<FileHeader>
							<xsl:value-of select="'true'"/>
						</FileHeader>-->

						<FileFooter>
							<xsl:value-of select="'true'"/>
						</FileFooter>

						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<!--for system internal use-->
						<BuySell>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<xsl:value-of select="'Bought'"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close'">
									<xsl:value-of select="'Sold'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</BuySell>

						<ALLOCQTY>
							<xsl:value-of select="AllocatedQty"/>
						</ALLOCQTY>

						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<Description>
							<xsl:value-of select="FullSecurityName"/>
						</Description>

						<Symbol>
							<xsl:value-of select="Symbol"/>
						</Symbol>

						<AvgPx>
							<xsl:choose>
								<xsl:when test="number(AveragePrice)">
									<xsl:value-of select="AveragePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPx>

						<CUSIPID>
							<xsl:value-of select="CUSIP"/>
						</CUSIPID>

						<RefID>
							<xsl:value-of select="EntityID"/>
						</RefID>

						<Commission>
							<xsl:choose>
								<xsl:when test="number(CommissionCharged)">
									<xsl:value-of select="CommissionCharged"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<SECFee>
							<xsl:choose>
								<xsl:when test="number(StampDuty)">
									<xsl:value-of select="StampDuty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SECFee>

						<InternalNetNotional>
							<xsl:value-of select="NetAmount"/>
						</InternalNetNotional>

						<PostCommissionNetMoney>
							<xsl:choose>
								<xsl:when test="number(NetAmount)">
									<xsl:value-of select="NetAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</PostCommissionNetMoney>

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="SettlementDate"/>
						</SettlementDate>

						<PrincipalAmt>
							<xsl:value-of select="GrossAmount"/>
						</PrincipalAmt>

						<xsl:variable name="PB_NAME" select="'StateStreet'"/>

						<xsl:variable name = "PRANA_Broker" >
							<xsl:value-of select="CounterParty"/>
						</xsl:variable>

						<xsl:variable name="THIRDPARTY_Broker">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker = $PRANA_Broker]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<ExecutingBroker>
							<xsl:choose>
								<xsl:when test="$THIRDPARTY_Broker!=''">
									<xsl:value-of select="$THIRDPARTY_Broker"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_Broker"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExecutingBroker>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="FundName"/>
						</xsl:variable>


						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				
			</xsl:for-each>
			
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>