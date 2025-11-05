<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'False'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select ="TaxLotState"/>
				</TaxLotState>

				<TRADEDATE>
					<xsl:value-of select="'TRADEDATE'"/>
				</TRADEDATE>

				<ACCOUNT>
					<xsl:value-of select="'ACCOUNT'"/>
				</ACCOUNT>

				<ACTION>
					<xsl:value-of select="'ACTION'"/>
				</ACTION>

				<SHARES>
					<xsl:value-of select="'SHARES'"/>
				</SHARES>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<ASSET_TYPE>
					<xsl:value-of select="'ASSET_TYPE'"/>
				</ASSET_TYPE>

				<PRICE>
					<xsl:value-of select="'PRICE'"/>
				</PRICE>

				<COMMISSION_METHOD>
					<xsl:value-of select ="'COMMISSION_METHOD'"/>
				</COMMISSION_METHOD>

				<ENTRY_COMMISSION>
					<xsl:value-of select ="'ENTRY_COMMISSION'"/>
				</ENTRY_COMMISSION>

				<SD_ENTRY_COMMISSION>
					<xsl:value-of select ="'SD_ENTRY_COMMISSION'"/>
				</SD_ENTRY_COMMISSION>

				<BROKER>
					<xsl:value-of select ="'BROKER'"/>
				</BROKER>

				<BuyCurrency>
					<xsl:value-of select="'Buy Currency'"/>
				</BuyCurrency>

				<SellCurrency>
					<xsl:value-of select="'Sell Currency'"/>
				</SellCurrency>

				<FXRate>
					<xsl:value-of select="'FX Rate'"/>
				</FXRate>

				<SellAmount>
					<xsl:value-of select="'Sell Amount'"/>
				</SellAmount>

				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<BuyAmount>
					<xsl:value-of select="'Buy Amount'"/>
				</BuyAmount>

				<AccountType>
					<xsl:value-of select="'Account Type'"/>
				</AccountType>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty ='WCHV' or (contains(AccountName, 'Booth Bay SMA'))]">

				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

					<xsl:variable name="PB_NAME" select="'WellsFargo'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<TRADEDATE>
						<xsl:value-of select="TradeDate"/>
					</TRADEDATE>
					
					

					<ACCOUNT>
						
						<xsl:choose>
							<xsl:when test="AccountName='Chimera Capital'">
								<xsl:value-of select="'2XO10376'"/>								
							</xsl:when>
							<xsl:when test="AccountName='Booth Bay SMA'">
								<xsl:value-of select="'2MA01033'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountName"/>
							</xsl:otherwise>
						</xsl:choose>
					</ACCOUNT>
					
					

					<ACTION>
						<xsl:choose>
							<xsl:when test="Asset!='FX'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'CS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close' or Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</ACTION>

					<SHARES>
						<xsl:choose>
							<xsl:when test="Asset!='FX'">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</SHARES>

					<TICKER>
						<xsl:choose>
							<xsl:when test="Asset!='FX'">

								<xsl:choose>
									<xsl:when test="contains(Asset,'Future')">
										<xsl:choose>
											<xsl:when test="BBCode!=''">
												<xsl:value-of select="substring-before(BBCode,' ')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="Symbol"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test="contains(Asset,'Option')">										
										 <xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:when test="contains(Asset,'FixedIncome')">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:when test="SettlCurrency!='USD'">
										<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</TICKER>

					<ASSET_TYPE>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'Equity Option'"/>
							</xsl:when>
							<xsl:when test="Asset!='FX'">
								<xsl:value-of select="Asset"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</ASSET_TYPE>

					<PRICE>
						<xsl:choose>
							<xsl:when test="Asset!='FX'">
								<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</PRICE>

					<COMMISSION_METHOD>
						<xsl:choose>
							<xsl:when test="Asset!='FX'">
								<xsl:value-of select ="'PER SHARE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</COMMISSION_METHOD>
					
					<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="COMM2">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					
					<ENTRY_COMMISSION>
						<xsl:choose>
							<xsl:when test="Asset!='FX' and CounterParty ='WCHV'">
								<xsl:value-of select="format-number($COMM div (AllocatedQty),'0.####')"/>
							</xsl:when>

							<xsl:when test="Asset!='FX' and CounterParty!='WCHV'">
								<xsl:value-of select="format-number($COMM2 div (AllocatedQty),'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</ENTRY_COMMISSION>


					<xsl:variable name="COMM1">
						<xsl:value-of select="SoftCommissionCharged"/>
					</xsl:variable>
					<SD_ENTRY_COMMISSION>
						<xsl:choose>
							<xsl:when test="Asset!='FX' and CounterParty='WCHV'">
								<xsl:value-of select="format-number($COMM1 div (AllocatedQty),'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'0'"/>
							</xsl:otherwise>
						</xsl:choose>
					</SD_ENTRY_COMMISSION>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
					</xsl:variable>      

					<BROKER>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</BROKER>

					
					<BuyCurrency>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="LeadCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</BuyCurrency>

					<SellCurrency>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</SellCurrency>

					<FXRate>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:choose>
									<xsl:when test="Asset='FX'">
										<xsl:value-of select="AveragePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="number(FXRate_Taxlot)">
												<xsl:value-of select="FXRate_Taxlot"/>
											</xsl:when>
											<xsl:when test="number(ForexRate)">
												<xsl:value-of select="ForexRate"/>
											</xsl:when>

											<xsl:otherwise>
												<xsl:value-of select="1"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>

								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>					
						
					</FXRate>

					<SellAmount>
						<xsl:value-of select="''"/>
					</SellAmount>

					<SettlementDate>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="SettlementDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</SettlementDate>

					<BuyAmount>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="ExecutedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</BuyAmount>

					<AccountType>
						<xsl:choose>
							<xsl:when test="Asset='FX'">
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'Long'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>									
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</AccountType>
					

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>