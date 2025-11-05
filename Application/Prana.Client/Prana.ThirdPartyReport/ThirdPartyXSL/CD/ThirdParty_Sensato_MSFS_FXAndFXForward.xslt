<?xml version="1.0" encoding="UTF-8"?>
								<!-- Description: MSFS_Sensato EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

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

				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>

				<TransStatus>
					<xsl:value-of select="'Transaction Status (Type)'"/>
				</TransStatus>

				<TransactionLevel>
					<xsl:value-of select="'Transaction Level (Entity)'"/>
				</TransactionLevel>

				<!-- COL D-->
				<Product_Intrumenttype>
					<xsl:value-of select="'Product/Intrument type'"/>
				</Product_Intrumenttype>

				
				<ClientRef>
					<xsl:value-of select="'Client Traqde Ref No. (RequestId)'"/>
				</ClientRef>

				<Associated>
					<xsl:value-of select="'Associated Trade/Request Id'"/>
				</Associated>

				<ExecAccount>
					<xsl:value-of select="'Execution Account'"/>
				</ExecAccount>

				<Account_Id_Fund>
					<xsl:value-of select="'Account Id/Fund'"/>
				</Account_Id_Fund>

				<ExecBkr>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecBkr>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<!-- COL K-->
				<ValueDate>
					<xsl:value-of select="'Value Date'"/>
				</ValueDate>

				<BuyQuantity>
					<xsl:value-of select="'Buy Quantity'"/>
				</BuyQuantity>

				<SellQuantity>
					<xsl:value-of select="'Sell Quantity'"/>
				</SellQuantity>

				<BuyCCY>
					<xsl:value-of select="'Buy CCY'"/>
				</BuyCCY>

				<SellCCY>
					<xsl:value-of select="'Sell CCY'"/>
				</SellCCY>

				<DealtCurrency>
					<xsl:value-of select="'Dealt Currency'"/>
				</DealtCurrency>

				<Rate>
					<xsl:value-of select="'Rate'"/>
				</Rate>

				<NdfFlag>
					<xsl:value-of select="'Ndf Flag'"/>
				</NdfFlag>

				<!-- COL S-->
				<NdfFixingDate>
					<xsl:value-of select="'NdfFixingDate'"/>
				</NdfFixingDate>

				<NdfLinkedTrade>
					<xsl:value-of select="'NdfLinkedTrade'"/>
				</NdfLinkedTrade>

				<PB>
					<xsl:value-of select="'PB'"/>
				</PB>

				<FarValueDate>
					<xsl:value-of select="'FarValueDate'"/>
				</FarValueDate>

				<FarValueRate>
					<xsl:value-of select="'FarValueRate'"/>
				</FarValueRate>

				<ClientBaseEquivalent>
					<xsl:value-of select="'Client Base Equivalent'"/>
				</ClientBaseEquivalent>

				<HedgeorSpeculative>
					<xsl:value-of select="'Hedge or Speculative'"/>
				</HedgeorSpeculative>

				<TaxIndicator>
					<xsl:value-of select="'Tax Indicator'"/>
				</TaxIndicator>

				<HearsayInd>
					<xsl:value-of select="'Hearsay Ind'"/>
				</HearsayInd>

				<Custodian>
					<xsl:value-of select="'Custodian'"/>
				</Custodian>

				<MoneyManager>
					<xsl:value-of select="'Money Manager'"/>
				</MoneyManager>

				<DealId>
					<xsl:value-of select="'Deal Id'"/>
				</DealId>

				<AcquisitionDate>
					<xsl:value-of select="'Acquisition Date'"/>
				</AcquisitionDate>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<TradeType>
					<xsl:value-of select="'TradeType'"/>
				</TradeType>


				<Reporter>
					<xsl:value-of select="'Reporter'"/>
				</Reporter>

		
				
				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			
		</ThirdPartyFlatFileDetail>

			<!--<xsl:for-each select="ThirdPartyFlatFileDetail[(Asset ='FX' or Asset ='FXForward') and (FundName = '73Y1F0' or FundName = 'YVPRP0' or FundName = 'PMFG' or FundName = '10607884' or FundName = '038CDFGT3' or FundName = '002-200640' or FundName = '013-216619' or FundName = '4901411' or FundName = '465568 ' or FundName = '465569' or FundName = '465572 ' or FundName = '465573' or FundName = '465574' or FundName = '465575' or FundName = '465576' or FundName = '465577' or FundName = '471824' or FundName = 'S1DB_433311' or FundName = 'SAPDB_416596')]">-->
			<xsl:for-each select="ThirdPartyFlatFileDetail[(Asset ='FX' or Asset ='FXForward')]">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>
					
					<!--for system use only-->					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>
					
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

					<TransactionType>
						<xsl:value-of select="'FX002'"/>
					</TransactionType>


					<xsl:variable name="varTransStatus">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'CAN'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select="'COR'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'NEW'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<TransStatus>
						<xsl:value-of select="$varTransStatus"/>
					</TransStatus>

					<TransactionLevel>
						<xsl:value-of select="'S'"/>
					</TransactionLevel>

					<!-- COL D-->
					<xsl:choose>
						<xsl:when test ="Asset='FX'">
							<Product_Intrumenttype>
								<xsl:value-of select="'SPOT'"/>
							</Product_Intrumenttype>
						</xsl:when>
						<xsl:when test ="Asset='FXForward'">
							<Product_Intrumenttype>
								<xsl:value-of select="'FORWARD'"/>
							</Product_Intrumenttype>
						</xsl:when>
						<xsl:otherwise>
							<Product_Intrumenttype>
								<xsl:value-of select="Asset"/>
							</Product_Intrumenttype>
						</xsl:otherwise>
					</xsl:choose>

					<ClientRef>
						<xsl:value-of select="TradeRefID"/>
					</ClientRef>

					<Associated>
						<xsl:value-of select="''"/>
					</Associated>

					<ExecAccount>
						<xsl:value-of select="''"/>
					</ExecAccount>

					<Account_Id_Fund>
						<xsl:value-of select="'038Q5053'"/>
					</Account_Id_Fund>

					<xsl:variable name="varCounterParty">
						<xsl:if test="CounterParty != ''">
						<xsl:value-of select="CounterParty"/>
						</xsl:if>
					</xsl:variable>

					<xsl:variable name="varExecutionBroker">
						<xsl:if test="$varCounterParty != ''">
						<xsl:value-of select="document('../ReconMappingXml/ThirdPExecBrokerMapping.xml')/BrokerMapping/PB[@Name='MSFS']/BrokerData[@PranaBroker = $varCounterParty]/@PBBroker"/>
						</xsl:if>
					</xsl:variable>

					<!--<ExecBkr>
						<xsl:choose>
							<xsl:when test="$varExecutionBroker != ''">
								<xsl:value-of select ="DBPR"/>
							</xsl:when>
							--><!--<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>--><!--
						</xsl:choose>

					</ExecBkr>-->

					<ExecBkr>
						<xsl:value-of select="'DBPR'"/>
					</ExecBkr>


					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<!-- COL K-->
					<ValueDate>
						<xsl:value-of select="SettlementDate"/>
					</ValueDate>

					<xsl:choose>
						<xsl:when test ="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
							<BuyQuantity>
								<xsl:value-of select ="AllocatedQty"/>
							</BuyQuantity>
							<SellQuantity>
								<xsl:value-of select="(AllocatedQty * AveragePrice)" />
								<!--<xsl:value-of select ="AllocatedQty * AveragePrice"/>-->
							</SellQuantity>
							<BuyCCY>
								<xsl:value-of select ="LeadCurrencyName"/>
							</BuyCCY>
							<SellCCY>
								<xsl:value-of select ="VsCurrencyName"/>
							</SellCCY>							
						</xsl:when>
						<xsl:when test ="Side='Sell' or Side='Sell to Open' or Side='Sell short' or Side='Sell to Close'">
							<BuyQuantity>
								<!--<xsl:value-of select ="AllocatedQty * AveragePrice"/>-->
								<xsl:value-of select="AllocatedQty * AveragePrice" />
							</BuyQuantity>
							<SellQuantity>
								<xsl:value-of select ="AllocatedQty"/>
							</SellQuantity>
							<BuyCCY>
								<xsl:value-of select ="VsCurrencyName"/>
							</BuyCCY>
							<SellCCY>
								<xsl:value-of select ="LeadCurrencyName"/>
							</SellCCY>							
						</xsl:when>
						<xsl:otherwise>
							<BuyQuantity>
								<xsl:value-of select ="0"/>
							</BuyQuantity>
							<SellQuantity>
								<xsl:value-of select ="0"/>
							</SellQuantity>
							<BuyCCY>
								<xsl:value-of select ="''"/>
							</BuyCCY>							
							<SellCCY>
								<xsl:value-of select ="''"/>
							</SellCCY>							
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
							<DealtCurrency>
								<xsl:value-of select="LeadCurrencyName"/>
							</DealtCurrency>
						</xsl:when>
						<xsl:when test ="Side='Sell' or Side='Sell to Open' or Side='Sell short' or Side='Sell to Close'">
							<DealtCurrency>
								<xsl:value-of select="VsCurrencyName"/>
							</DealtCurrency>
						</xsl:when>
						<xsl:otherwise>
							<DealtCurrency>
								<xsl:value-of select="''"/>
							</DealtCurrency>
						</xsl:otherwise>
					</xsl:choose>

					<Rate>
						<xsl:value-of select="AveragePrice"/>
					</Rate>

					<NdfFlag>
						<xsl:value-of select="'FALSE'"/>
					</NdfFlag>

					<!-- COL S-->
					<NdfFixingDate>
						<xsl:value-of select="''"/>
					</NdfFixingDate>

					<NdfLinkedTrade>
						<xsl:value-of select="''"/>
					</NdfLinkedTrade>

					<PB>
						<xsl:value-of select="MSCO"/>
					</PB>

					<FarValueDate>
						<xsl:value-of select="''"/>
					</FarValueDate>

					<FarValueRate>
						<xsl:value-of select="''"/>
					</FarValueRate>

					<ClientBaseEquivalent>
						<xsl:value-of select="''"/>
					</ClientBaseEquivalent>

					<HedgeorSpeculative>
						<xsl:value-of select="''"/>
					</HedgeorSpeculative>

					<TaxIndicator>
						<xsl:value-of select="''"/>
					</TaxIndicator>

					<HearsayInd>
						<xsl:value-of select="'Y'"/>
					</HearsayInd>

					<Custodian>
						<xsl:value-of select="'DBAB'"/>
					</Custodian>

					<MoneyManager>
						<xsl:value-of select="''"/>
					</MoneyManager>

					<DealId>
						<xsl:value-of select="''"/>
					</DealId>

					<AcquisitionDate>
						<xsl:value-of select="''"/>
					</AcquisitionDate>

					<Comments>
						<xsl:value-of select="''"/>
					</Comments>

					<TradeType>
						<xsl:value-of select="''"/>
					</TradeType>


					<Reporter>
						<xsl:value-of select="''"/>
					</Reporter>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
