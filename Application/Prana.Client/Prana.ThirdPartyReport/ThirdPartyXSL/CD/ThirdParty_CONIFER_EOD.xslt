<?xml version="1.0" encoding="UTF-8"?>

<!--Auther: Ashish
											   Date:25-10-2011
											   
											-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

				<Tradedate>
					<xsl:value-of select ="'Tradedate'"/>
				</Tradedate>
				<Settledate>
					<xsl:value-of select ="'Settledate'"/>
				</Settledate>

				<Side>
					<xsl:value-of select ="'Side'"/>
				</Side>

				<Amount>
					<xsl:value-of select ="'Amount'"/>
				</Amount>

				<Security>
					<xsl:value-of select ="'Security'"/>
				</Security>

				<Done>
					<xsl:value-of select ="'Done'"/>
				</Done>

				<Broker>
					<xsl:value-of select ="'Broker'"/>
				</Broker>

				<TAlpha1>
					<xsl:value-of select="'TAlpha1'"/>
				</TAlpha1>

				<Price>
					<xsl:value-of select ="'Price'"/>
				</Price>

				<Comm>
					<xsl:value-of select ="'Comm'"/>
				</Comm>

				<Tnum1>
					<xsl:value-of select ="'Tnum1'"/>
				</Tnum1>

				<Fee>
					<xsl:value-of select ="'Fee'"/>
				</Fee>

				<Tnum2>
					<xsl:value-of select="'Tnum2'"/>
				</Tnum2>


				<Note>
					<xsl:value-of select ="'Note'"/>
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
					<xsl:value-of select ="'To_Curr'"/>
				</To_Curr>



				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test ="AccountNo='441885' or AccountNo='SAPMFLP' or AccountNo='013425764' or AccountNo='002276293' or AccountNo='9530857'">
					<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<xsl:variable name ="varCurrency">
						<xsl:value-of select ="CurrencySymbol"/>
					</xsl:variable>

						<xsl:variable name="varSecFees">
							<xsl:value-of select="StampDuty + TransactionLevy + OtherBrokerFee + ClearingFee + MiscFees + TaxOnCommissions"/>
						</xsl:variable>

						<xsl:variable name="SideMultiplier">
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="-1"/>
								</xsl:when>
								<xsl:otherwise>
										<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>						
						
						
					<!--<xsl:variable name="SwapFxRate">
						<xsl:value-of select="document('../ReconMappingXml/SwapFxRates.xml')/FxRate/Rate[@Name='SENSATO']/FxData[@CurrSymbol=$varCurrency]/@FxPrice"/>
					</xsl:variable>-->

						<!-- Non Swap Accounts does not need to be converted in to USD so normalized rate should be 1 -->
					<xsl:variable name="NormalizedFxRate">
						<xsl:choose>
							<xsl:when test="ForexRate_Trade='' or ForexRate_Trade='0' or AccountNo = 'SAPMFLP' or AccountNo = '002276293'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="ForexRate_Trade"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
						
						<!-- For Swap Accounts we need to send net price for non swap  gross is fine-->
					<xsl:variable name="NormalizedAveragePrice">
						<xsl:choose>
							<xsl:when test="AccountNo = 'SAPMFLP' or AccountNo = '002276293'">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AveragePrice + $SideMultiplier * ((CommissionCharged+$varSecFees)div ExecutedQty)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
						
						<!-- For Swap Accounts we don't need to send tnum1-->
					<xsl:variable name="NormalizedTnum1">
						<xsl:choose>
							<xsl:when test="AccountNo = 'SAPMFLP' or AccountNo = '002276293'">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
						<!-- For Swap Accounts we don't need to send tnum2-->
						<xsl:variable name="NormalizedTnum2">
						<xsl:choose>
							<xsl:when test="AccountNo = 'SAPMFLP' or AccountNo = '002276293'">
								<xsl:value-of select="$varSecFees"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
						
					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>
					<!--for system internal use-->

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<Tradedate>
						<xsl:value-of select ="TradeDate"/>
					</Tradedate>

					<Settledate>
						<xsl:value-of select ="SettlementDate"/>
					</Settledate>

					<!--<Side>
				  <xsl:value-of select ="Side"/>
			  </Side>-->
					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<Side>
								<xsl:value-of select="'buy'"/>
							</Side>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<Side>
								<xsl:value-of select="'cover'"/>
							</Side>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<Side>
								<xsl:value-of select="'sell'"/>
							</Side>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<Side>
								<xsl:value-of select="'short'"/>
							</Side>
						</xsl:when>
						<xsl:otherwise>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
						</xsl:otherwise>
					</xsl:choose>

					<Amount>
						<xsl:value-of select ="TotalQty"/>
					</Amount>



					<Done>
						<xsl:value-of select ="ExecutedQty"/>
					</Done>

					<!--Need To Ask-->
					<Broker>
						<xsl:value-of select ="'CNFR'"/>
					</Broker>
					<!--Need To Ask-->
					<xsl:choose>
						<xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
							<TAlpha1>
								<xsl:value-of select="'GSCO'"/>
							</TAlpha1>
						</xsl:when>
						<xsl:when test ="CounterParty='UBS Program' or CounterParty='UBS Electronic'">
							<TAlpha1>
								<xsl:value-of select="'UBSW'"/>
							</TAlpha1>
						</xsl:when>
						<xsl:when test ="CounterParty='DBPrg' or CounterParty='DBElec'">
							<TAlpha1>
								<xsl:value-of select="'DBAB'"/>
							</TAlpha1>
						</xsl:when>
						<xsl:when test ="CounterParty='INSTElec' or CounterParty='DBElec'">
							<TAlpha1>
								<xsl:value-of select="'INST'"/>
							</TAlpha1>
						</xsl:when>
						<xsl:otherwise >
							<TAlpha1>
								<xsl:value-of select="CounterParty"/>
							</TAlpha1>
						</xsl:otherwise>
					</xsl:choose >

					<Price>
						<xsl:value-of select ="$NormalizedAveragePrice*$NormalizedFxRate"/>
					</Price>

					<!--<xsl:variable name="varCommision">
						<xsl:choose>
							<xsl:when test="CommissionCharged =0">
								<xsl:value-of select="'0'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>-->

					<!--<xsl:choose>
				  <xsl:when test="$varCommision = ''">
					  <Comm>
						  <xsl:value-of select ="NONE"/>
					  </Comm>
				  </xsl:when>
				  <xsl:otherwise>
					  <Comm>
						  <xsl:value-of select ="$varCommision"/>
					  </Comm>
				  </xsl:otherwise>
			  </xsl:choose>-->

						<xsl:choose>
							<xsl:when test="$NormalizedTnum1 ='' or $NormalizedTnum1 =0">
								<Comm>
									<xsl:value-of select ="'NONE'"/>
								</Comm>
							</xsl:when>
							<xsl:otherwise>
								<Comm>
									<xsl:value-of select ="'PLUG'"/>
								</Comm>
							</xsl:otherwise>
						</xsl:choose>

					<xsl:choose>
						<xsl:when test="$NormalizedTnum1=0 or $NormalizedTnum1=''">
							<Tnum1>
								<xsl:value-of select ="''"/>
							</Tnum1>
						</xsl:when>
						<xsl:otherwise>
							<Tnum1>
								<xsl:value-of select ="$NormalizedTnum1*$NormalizedFxRate"/>
							</Tnum1>
						</xsl:otherwise>
					</xsl:choose>

						<xsl:choose>
							<xsl:when test="$NormalizedTnum2 ='' or $NormalizedTnum2 =0">
								<Fee>
									<xsl:value-of select ="'NONE'"/>
								</Fee>
							</xsl:when>
							<xsl:otherwise>
								<Fee>
									<xsl:value-of select ="'PLUG'"/>
								</Fee>
							</xsl:otherwise>
						</xsl:choose>

					<xsl:choose>
						<xsl:when test="$NormalizedTnum2 ='' or $NormalizedTnum2 =0">
							<Tnum2>
								<xsl:value-of select="''"/>
							</Tnum2>
						</xsl:when>
						<xsl:otherwise>
							<Tnum2>
								<xsl:value-of select="$NormalizedTnum2*$NormalizedFxRate"/>
							</Tnum2>
						</xsl:otherwise>
					</xsl:choose>




					<Note>
						<xsl:value-of select ="concat('&quot;',FullSecurityName,'&quot;')"/>
					</Note>

					<Manager>
						<xsl:value-of select ="''"/>
					</Manager>

					<Trader>
						<xsl:value-of select ="''"/>
					</Trader>

					<xsl:choose>
						<xsl:when test="AccountNo='441885' or AccountNo='32577' or AccountNo='013425764' or AccountNo='013102025' or AccountNo='9530857' or AccountNo='5870004'">
							<Security>
								<xsl:value-of select="concat(BBCode,' S')"/>
							</Security>
						</xsl:when>
						<xsl:otherwise>
							<Security>
								<xsl:value-of select="BBCode"/>
							</Security>
						</xsl:otherwise>
					</xsl:choose>

					<!--<Prt>
						<xsl:value-of select="AccountNo"/>
					</Prt>-->


					<xsl:choose>
						<xsl:when test="AccountNo='441885' or AccountNo='SAPMFLP'">
							<Prt>
								<xsl:value-of select="'sendb'"/>
							</Prt>
						</xsl:when>
						<xsl:when test="AccountNo='002276293' or AccountNo='013425764'">
							<Prt>
								<xsl:value-of select="'sengs'"/>
							</Prt>
						</xsl:when>
						<xsl:when test="AccountNo='9530857'">
							<Prt>
								<xsl:value-of select="'senubs'"/>
							</Prt>
						</xsl:when>
						<xsl:otherwise>
							<Prt>
								<xsl:value-of select="AccountNo"/>
							</Prt>
						</xsl:otherwise>
					</xsl:choose>

					<Cust>
						<xsl:value-of select ="AccountNo"/>
					</Cust>

					
					<xsl:choose>
						<xsl:when test="AccountNo='441885' or AccountNo='32577' or AccountNo='013425764' or AccountNo='013102025' or AccountNo='9530857' or AccountNo='5870004'">
							<To_Curr>
								<xsl:value-of select="'USD'"/>
							</To_Curr>
						</xsl:when>
						<xsl:otherwise>
							<To_Curr>
								<xsl:value-of select="CurrencySymbol"/>
							</To_Curr>
						</xsl:otherwise>
					</xsl:choose>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
