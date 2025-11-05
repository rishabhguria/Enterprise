<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>

				<!--for system internal use-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'SettleDate'"/>
				</SettleDate>

				<Side>
					<xsl:value-of select="'Side'"/>
				</Side>

				<Amount>
					<xsl:value-of select="'Amount'"/>
				</Amount>

				<Security>
					<xsl:value-of select="'Security'"/>
				</Security>

				<Done>
					<xsl:value-of select="'Done'"/>
				</Done>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<TAlpha1>
					<xsl:value-of select="'TAlpha1'"/>
				</TAlpha1>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>


				<Comm>
					<xsl:value-of select="'Comm'"/>
				</Comm>

				<TNum1>
					<xsl:value-of select="'TNum1'"/>
				</TNum1>

				<Fee>
					<xsl:value-of select="'Fee'"/>
				</Fee>

				<TNum2>
					<xsl:value-of select="'TNum2'"/>
				</TNum2>

				<Note>
					<xsl:value-of select="'Note'"/>
				</Note>

				<Manager>
					<xsl:value-of select ="'Manager'"/>
				</Manager>

				<Trader>
					<xsl:value-of select ="'Trader'"/>
				</Trader>

				<Prt>
					<xsl:value-of select ="'Prt'"/>
				</Prt>

				<Cust>
					<xsl:value-of select ="'Cust'"/>
				</Cust>

				<To_Curr>
					<xsl:value-of select="'To_Curr'"/>
				</To_Curr>

				<CurrentFace>
					<xsl:value-of select="'CurrentFace'"/>
				</CurrentFace>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<!--<xsl:if test ="FundName != 'Weisbrod Family-JPM'">-->
					<ThirdPartyFlatFileDetail>

						
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<FileHeader>
							<xsl:value-of select ="'false'"/>
						</FileHeader>

						<FileFooter>
							<xsl:value-of select ="'false'"/>
						</FileFooter>

						
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						
						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<xsl:variable name="Year" select="substring(substring-after(substring-after(TradeDate,'/'),'/'),3)"/>

						<xsl:variable name="Day" select="substring-before(substring-after(TradeDate,'/'),'/')"/>

						<xsl:variable name="Month" select="substring-before(TradeDate,'/')"/>

						<TradeDate>
							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
						</TradeDate>

						<xsl:variable name="SetYear" select="substring(substring-after(substring-after(SettlementDate,'/'),'/'),3)"/>

						<xsl:variable name="SetDay" select="substring-before(substring-after(SettlementDate,'/'),'/')"/>

						<xsl:variable name="SetMonth" select="substring-before(SettlementDate,'/')"/>

						<SettleDate>
							<xsl:value-of select="concat($SetMonth,'/',$SetDay,'/',$SetYear)"/>
						</SettleDate>

						<Side>
							<xsl:choose>
								<xsl:when test="TaxLotState = 'Allocated' or TaxLotState = 'Sent'" >
									<xsl:choose>
										<xsl:when test="Side = 'Buy' or Side = 'Sell'">
											<xsl:value-of select="Side"/>
										</xsl:when>
										<xsl:when test="Side = 'Sell short'">
											<xsl:value-of select="'Short'"/>
										</xsl:when>
										<xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption'">
											<xsl:value-of select="'Short'"/>
										</xsl:when>
										<xsl:when test="Side = 'Buy to Close'">
											<xsl:value-of select="'Cover'"/>
										</xsl:when>
										<xsl:when test="Side = 'Sell to Open' or Side = 'Sell to Close'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:when test="Side = 'Buy to Open'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<Amount>
							<xsl:value-of select="AllocatedQty"/>
						</Amount>

						<Security>
							<xsl:choose>
								<xsl:when test ="(Asset = 'Equity' and contains(Symbol,'-') != false)">
									<xsl:choose>
										<xsl:when test ="contains(BBCode,'EQUITY')!= false">
											<xsl:value-of select ="substring-before(BBCode,' EQUITY')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="BBCode"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test =" Asset = 'EquityOption' and contains(BBCode,'EQUITY')!= false and contains(BBCode,'US')!= false">
									<xsl:value-of select ="concat(substring-before(BBCode,' '), ' ', substring-before(substring-after(substring-after(BBCode,' '),' '),' EQUITY'))"/>
								</xsl:when>
								<xsl:when test =" Asset = 'EquityOption' and contains(BBCode,'US')!= false">
									<xsl:value-of select ="concat(substring-before(BBCode,' '),  substring-after(BBCode,'US'))"/>
								</xsl:when>
								<xsl:when test =" Asset = 'EquityOption'">
									<xsl:value-of select ="BBCode"/>
								</xsl:when>
								<xsl:when test="Asset='FixedIncome' and CurrencySymbol='USD'">
									<xsl:value-of select="CUSIP"/>
								</xsl:when>

								<xsl:when test="Asset='FixedIncome' and CurrencySymbol!='USD'">
									<xsl:choose>
										<xsl:when test="ISIN!=''">
											<xsl:value-of select="ISIN"/>
										</xsl:when>
										<xsl:when test="SEDOL!=''">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Security>

						<Done>
							<xsl:value-of select="AllocatedQty"/>
						</Done>

						<Broker>
							<xsl:value-of select="'CNFR'"/>
						</Broker>
						
						<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

						<xsl:variable name="THIRDPARTY_BROKER_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='US Bank']/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>
						<TAlpha1>
							<xsl:choose>
								<xsl:when test ="CounterParty = 'BTIGI'">
									<xsl:value-of select ="'BTIG'"/>
								</xsl:when>
								<xsl:when test ="CounterParty = 'BTIGALGO'">
									<xsl:value-of select ="'BTIG'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
											<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_BROKER_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</TAlpha1>

						<Price>
							<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
						</Price>


						<Comm>
							<xsl:value-of select="''"/>							
						</Comm>

						<TNum1>							
							<xsl:choose>
								<xsl:when test="CommissionCharged &gt; 0">
									<xsl:value-of select="format-number(CommissionCharged,'#.####')"/>
								</xsl:when>
								<xsl:when test="CommissionCharged &lt; 0">
									<xsl:value-of select="format-number(CommissionCharged,'#.####') * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TNum1>

						<Fee>
							<xsl:value-of select="''"/>
						</Fee>

						<TNum2>
							<xsl:choose>
								<xsl:when test="StampDuty &gt; 0">
									<xsl:value-of select="format-number(StampDuty,'#.####')"/>
								</xsl:when>
								<xsl:when test="StampDuty &lt; 0">
									<xsl:value-of select="format-number(StampDuty,'#.####') * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TNum2>

						<Note>
							<!--<xsl:choose>
								<xsl:when test="AlgorithmAcronym='BTAX'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="AlgorithmAcronym='MANUAL'">
									<xsl:value-of select="'SPECIFIED'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="AlgorithmAcronym"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="FullSecurityName"/>

						</Note>

						<Manager>
							<xsl:value-of select ="''"/>
						</Manager>

						<Trader>
							<xsl:value-of select ="''"/>
						</Trader>

						<xsl:variable name = "PRANA_FUND_NAME">
							<xsl:value-of select="FundName"/>
						</xsl:variable>

						<xsl:variable name ="THIRDPARTY_FUND_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CONIFER']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
						</xsl:variable>

						<Prt>

							<xsl:choose>
								<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
									<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Prt>

						<Cust>
							<xsl:value-of select ="'MSCO'"/>
						</Cust>

						<To_Curr>
							<xsl:value-of select="SettlCurrency"/>
						</To_Curr>

						<CurrentFace>
							<xsl:value-of select="''"/>
						</CurrentFace>

						<!-- system use only-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				<!--</xsl:if>-->
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
