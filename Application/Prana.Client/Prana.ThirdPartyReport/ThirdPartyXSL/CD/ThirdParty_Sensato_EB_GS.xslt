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
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
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

				<!--for system internal use-->
				<FromDeleted>
					<xsl:value-of select ="'FromDeleted'"/>
				</FromDeleted>

				<Bloomberg>
					<xsl:value-of select ="'Bloomberg Ticker'"/>
				</Bloomberg>

				<Sedol>
					<xsl:value-of select ="'Sedol'"/>
				</Sedol>

				<Side>
					<xsl:value-of select ="'Side'"/>
				</Side>

				<Tradedate>
					<xsl:value-of select ="'Trade Date'"/>
				</Tradedate>

				<primeBroker>
					<xsl:value-of select ="'Prime Broker'"/>
				</primeBroker>

				<FundAccountNumber>
					<xsl:value-of select ="'Fund Account Number'"/>
				</FundAccountNumber>
				
				<ExecutingBroker>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecutingBroker>

				<QuantityTraded>
					<xsl:value-of select ="'Quantity Traded'"/>
				</QuantityTraded>

				<AverageTradePrice>
					<xsl:value-of select ="'Average Trade Price'"/>
				</AverageTradePrice>

				<EntityID>
					<xsl:value-of select ="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[(CounterParty = 'GSElec' or CounterParty = 'GSPrg') and (AccountName = 'SAPGS_CASH' or AccountName = 'SP29GS_CASH' or AccountName = 'S1GS_CASH' or AccountName = 'SP29DB_CASH' or AccountName = 'SP29CS_CASH' or AccountName = 'S1CS_CASH' or AccountName = 'SAPCS_CASH' or AccountName = 'SAPDB_CASH' or AccountName = 'S1DB_CASH' or AccountName = 'SP29MS_ALL')]">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use -->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>
					<!--for system internal use-->
					<FromDeleted>
						<xsl:value-of select ="FromDeleted"/>
					</FromDeleted>

					<!-- For Equity Option OSI Symbology-->

					<xsl:variable name="varOptionUnderlying">
						<xsl:value-of select="substring-after(substring-before(Symbol,' '),':')"/>
					</xsl:variable>

					<xsl:variable name = "BlankCount_Root" >
						<xsl:call-template name="noofBlanks">
							<xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varFormattedStrikePrice">
						<xsl:value-of select="format-number(StrikePrice,'00000.000')"/>
					</xsl:variable>

					<xsl:variable name="varOSIOptionSymbol">
						<xsl:value-of select="concat($varOptionUnderlying,$BlankCount_Root,substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2),substring(PutOrCall,1,1),translate($varFormattedStrikePrice,'.',''))"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="Asset='Equity'">
							<Bloomberg>
								<xsl:value-of select="BBCode"/>
							</Bloomberg>
						</xsl:when>
						<xsl:when test ="Asset='EquityOption'">
							<Bloomberg>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="translate($varOSIOptionSymbol,' ','')"/>
									</xsl:otherwise>
								</xsl:choose>
							</Bloomberg>
						</xsl:when>
						<xsl:otherwise>
							<Bloomberg>
								<xsl:value-of select="Symbol"/>
							</Bloomberg>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="Asset='Equity'">
							<Sedol>
								<xsl:value-of select="SEDOL"/>
							</Sedol>
						</xsl:when>
						<xsl:when test ="Asset='EquityOption'">
							<Sedol>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="translate($varOSIOptionSymbol,' ','')"/>
									</xsl:otherwise>
								</xsl:choose>
							</Sedol>
						</xsl:when>
						<xsl:otherwise>
							<Sedol>
								<xsl:value-of select="Symbol"/>
							</Sedol>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:variable name="varSide">
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select ="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side= 'Sell to Close'">
								<xsl:value-of select ="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side = 'Sell to Open'">
								<xsl:value-of select ="'SHORT'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select ="'BUY_TO_COVER'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select ="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Side>
						<xsl:value-of select="$varSide"/>
					</Side>

					<Tradedate>
						<xsl:value-of select="TradeDate"/>
					</Tradedate>

					<xsl:variable name="varFundName">
					<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name="varPB">
					<xsl:value-of select="document('../ReconMappingXml/Omgeo_BrokerMatch.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaFundName = $varFundName]/@PB"/>
					</xsl:variable>
					
					<primeBroker>
						<xsl:value-of select ="$varPB"/>
					</primeBroker>

					<FundAccountNumber>
						<xsl:value-of select="AccountNo"/>
						<!--<xsl:value-of select="AccountNo"/>-->
					</FundAccountNumber>

					<xsl:variable name="varCounterParty">
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>
					
					<xsl:variable name="varEB">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_EBBroker.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaBroker = $varCounterParty]/@PBBroker"/>
					</xsl:variable>
					
					<ExecutingBroker>
						<xsl:choose>
							<xsl:when test="$varEB != ''">
								<xsl:value-of select ="$varEB"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBroker>
					
					<!--<ExecutingBroker>
						<xsl:value-of select="CounterParty"/>
					</ExecutingBroker>-->

					<QuantityTraded>
						<xsl:value-of select ="AllocatedQty"/>
					</QuantityTraded>

					<AverageTradePrice>
						<xsl:value-of select ="AveragePrice"/>
					</AverageTradePrice>

					<!-- syster internal use only-->
					<EntityID>
						<xsl:value-of select ="EntityID"/>
					</EntityID>
					
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>