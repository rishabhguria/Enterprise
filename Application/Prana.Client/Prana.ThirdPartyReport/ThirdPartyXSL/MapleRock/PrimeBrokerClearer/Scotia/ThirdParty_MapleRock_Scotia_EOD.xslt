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
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<!--for system internal use-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select ="'true'"/>
					</FileHeader>
					<FileFooter>
						<xsl:value-of select ="'true'"/>
					</FileFooter>

					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="'MS'"/>

					<xsl:variable name="FirstSix">
						<xsl:value-of select="concat(substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'),substring(substring-after(substring-after(TradeDate,'/'),'/'),3,2))"/>
					</xsl:variable>

					<xsl:variable name="Position" select="format-number(position(),'0000')"/>

					<TickerNumber>
						<xsl:value-of select="concat($FirstSix,$Position)"/>
					</TickerNumber>

					<AllocationSequenceNumber>
						<xsl:value-of select="0"/>
					</AllocationSequenceNumber>

					<AssetType>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'Corporate Bond'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Common Equity'"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</AssetType>

					<TradeState>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'ENTER'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'CANCEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeState>

					

					<xsl:variable name = "PRANA_COUNTERPARTY_NAME" >
						<xsl:choose>
							<xsl:when test="CounterParty= 'JEFF' or CounterParty= 'ZJEFF'">
								<xsl:value-of select="'JEFF'"/>
							</xsl:when>
							<xsl:when test="CounterParty= 'CITI' or CounterParty= 'ZCITI'">
								<xsl:value-of select="'CITI'"/>
							</xsl:when>
							<xsl:when test="CounterParty= 'JPMS' or CounterParty= 'ZJPMS'">
								<xsl:value-of select="'JPMS'"/>
							</xsl:when>
							<xsl:when test="CounterParty= 'GS' or CounterParty= 'ZGS'">
								<xsl:value-of select="'GS'"/>
							</xsl:when>
							<xsl:when test="CounterParty= 'BERN' or CounterParty= 'ZBERN'">
								<xsl:value-of select="'BERN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="PB_COUNTERPARTY_NAME_A">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Cpty">
						<xsl:choose>
							<xsl:when test="$PB_COUNTERPARTY_NAME_A!=''">
								<xsl:value-of select="$PB_COUNTERPARTY_NAME_A"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Broker>
						<xsl:value-of select="$Cpty"/>						
					</Broker>

					<Account>
						<xsl:value-of select="'78200271'"/>
					</Account>

					<AccountType>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'MARGIN'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<xsl:value-of select="'SHORT'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'MARGIN'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SHORT'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountType>

					<ProductCode>
						<xsl:choose>
							<xsl:when test="Asset!='EquityOption'">
								<xsl:choose>
									<xsl:when test="SEDOL!=''">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:when test="BBCode!=''">
										<xsl:value-of select="BBCode"/>
									</xsl:when>
									<xsl:when test="RIC!=''">
										<xsl:value-of select="RIC"/>
									</xsl:when>
									<xsl:when test="ISIN!=''">
										<xsl:value-of select="ISIN"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol!=''">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:when test="RIC!=''">
										<xsl:value-of select="RIC"/>
									</xsl:when>
									<xsl:when test="BBCode!=''">
										<xsl:value-of select="BBCode"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</ProductCode>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<TransactionCode>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TransactionCode>

					<Quantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<Price>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Price>

					<AccruedInterest>
						<xsl:choose>
							<xsl:when test="AccruedInterest &gt; 0">
								<xsl:value-of select="AccruedInterest"/>
							</xsl:when>
							<xsl:when test="AccruedInterest &lt; 0">
								<xsl:value-of select="AccruedInterest * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccruedInterest>					

					<Principal>

						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="GrossAmount div 100"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="GrossAmount &gt; 0">
										<xsl:value-of select="GrossAmount"/>
									</xsl:when>
									<xsl:when test="GrossAmount &lt; 0">
										<xsl:value-of select="GrossAmount * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						
					</Principal>

					<ExchFee>
						<xsl:choose>
							<xsl:when test="StampDuty &gt; 0">
								<xsl:value-of select="StampDuty"/>
							</xsl:when>
							<xsl:when test="StampDuty &lt; 0">
								<xsl:value-of select="StampDuty * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExchFee>

					<CommissionFee>
						<xsl:choose>
							<xsl:when test="CommissionCharged &gt; 0">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:when test="CommissionCharged &lt; 0">
								<xsl:value-of select="CommissionCharged * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionFee>

					<TaxFee>
						<xsl:value-of select="0"/>
					</TaxFee>

					<TotalCurrency>

						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="NetAmount div 100"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="NetAmount &gt; 0">
										<xsl:value-of select="NetAmount"/>
									</xsl:when>
									<xsl:when test="NetAmount &lt; 0">
										<xsl:value-of select="NetAmount * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						
					</TotalCurrency>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>						
					</SettleDate>

					<ExpiryDate>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</ExpiryDate>

					<DVPInstruction>
						<xsl:value-of select="''"/>
					</DVPInstruction>

					<SecurityName>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityName>

					<PutCall>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="PutOrCall"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</PutCall>

					<StrikePriceInt>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="substring-before(StrikePrice,'.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</StrikePriceInt>

					<StrikePriceFrac>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="substring-after(StrikePrice,'.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</StrikePriceFrac>

					<ExpiryMonth>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="substring-before(ExpirationDate,'/')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</ExpiryMonth>

					<ExpiryYear>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="substring-after(substring-after(ExpirationDate,'/'),'/')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="''"/>
					</ExpiryYear>

					<ExecutionType>
						<xsl:value-of select="'ID'"/>
					</ExecutionType>

					<FillSeqNo>
						<xsl:value-of select="''"/>
					</FillSeqNo>

					<Amendment>
						<xsl:choose>
							<xsl:when test ="TaxLotState='Amended'">
								<xsl:value-of select="'Y'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Amendment>

					<PrevClientTradeID>
						<xsl:value-of select="''"/>
					</PrevClientTradeID>

					<InstRequired>
						<xsl:value-of select="''"/>
					</InstRequired>

					<ORFFee>
						<xsl:value-of select="''"/>
					</ORFFee>


					<CmsnPerShareRate>
						<xsl:value-of select="''"/>
					</CmsnPerShareRate>

					

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
