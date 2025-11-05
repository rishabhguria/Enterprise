<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset='FX' or Asset='FXForward']">

				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					

					<xsl:variable name="varTransactionType">
						<xsl:choose>
							<xsl:when test ="TransactionType='SellShort'">
								<xsl:value-of select ="'Sell Short'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='BuytoClose'">
								<xsl:value-of select ="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='BuytoOpen'">
								<xsl:value-of select ="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='SelltoClose'">
								<xsl:value-of select ="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='SelltoOpen'">
								<xsl:value-of select ="'Sell to Open'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='ShortAddition'">
								<xsl:value-of select ="'Short Addition'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='ShortWithdrawal'">
								<xsl:value-of select ="'Short Withdrawal'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='ShortWithdrawalCashInLieu'">
								<xsl:value-of select ="'Short Withdrawal Cash In Lieu'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='LongWithdrawalCashInLieu'">
								<xsl:value-of select ="'Long Withdrawal Cash In Lieu'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='LongWithdrawal'">
								<xsl:value-of select ="'Long Withdrawal'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='LongCostAdj'">
								<xsl:value-of select ="'Long Cost Adj'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='LongAddition'">
								<xsl:value-of select ="'Long Addition'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='DLCostAndPNL'">
								<xsl:value-of select ="'DL Cost And PNL'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='CSClosingPx'">
								<xsl:value-of select ="'Cash Settle At Closing Date Spot PX'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='DLCostAndPNL'">
								<xsl:value-of select ="'DL Cost And PNL'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='CSCost'">
								<xsl:value-of select ="'Cash Settle At Cost'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='CSSwp'">
								<xsl:value-of select ="'Swap Expire'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='CSSwpRl'">
								<xsl:value-of select ="'Swap Expire and Rollover'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='CSZero'">
								<xsl:value-of select ="'Cash Settle At Zero Price'"/>
							</xsl:when>
							<xsl:when test ="TransactionType='DLCost'">
								<xsl:value-of select ="'Deliver FX At Cost'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="TransactionType"/>
							</xsl:otherwise>

						</xsl:choose>
					</xsl:variable>

					<EXTERNALREFERENCEID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Amended'">
								<!--<xsl:value-of select="concat('FX',substring(EntityID,string-length(EntityID)-7,string-length(EntityID)))"/>-->
								<xsl:value-of select="concat('FX',EntityID,'A')"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="concat('FX',EntityID,'D')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat('FX',EntityID)"/>
							</xsl:otherwise>
						</xsl:choose>											
					</EXTERNALREFERENCEID>

					<PREVEXTERNALREFERENCEID>
						<xsl:choose>
							<xsl:when test="TaxLotState ='Amended' or TaxLotState ='Deleted'">
								<xsl:value-of select="concat('FX',EntityID)"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PREVEXTERNALREFERENCEID>

					<EXTERNALINSTRUMENTID>
						<xsl:value-of select="concat('FX',EntityID)"/>
					</EXTERNALINSTRUMENTID>

					<DESK>
						<xsl:value-of select="'IAML'"/>
					</DESK>


					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="PB_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<EXECUTINGCOUNTERPARTY>
						<xsl:choose>
							<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</EXECUTINGCOUNTERPARTY>

					<TRADERID>
						<xsl:value-of select="'TRADING'"/>
					</TRADERID>


					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="'IAMG'"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name='NT']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<FUND>
						<xsl:choose>
							<xsl:when test="$PB_FUND_CODE!=''">
								<xsl:value-of select="$PB_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</FUND>

					<PBR>
						<xsl:value-of select="'TRADING'"/>
					</PBR>

					<TRADEDATE>
						<xsl:value-of select="TradeDate"/>
					</TRADEDATE>

					<CLEARINGACCOUNT>
						<xsl:value-of select="'NTRC-CUS-IAMG'"/>
					</CLEARINGACCOUNT>

					<CASHSUBACCOUNT>
						<xsl:value-of select="'CASH'"/>
					</CASHSUBACCOUNT>


					<STRATEGYID>
						<xsl:value-of select="'TRADING'"/>
					</STRATEGYID>
			

					<SETTLEDATE>

						<xsl:choose>
							<xsl:when test="Asset= 'FXForward'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlementDate"/>
							</xsl:otherwise>
						</xsl:choose>

					</SETTLEDATE>



					<FINANCIALTYPE>
						<xsl:choose>
							<xsl:when test="Asset= 'FXForward'">
								<xsl:value-of select="'FORWARD'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'CURRENCY'"/>
							</xsl:otherwise>
						</xsl:choose>
					</FINANCIALTYPE>


					<xsl:variable name="varTaxlotState">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'AMEND'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'CANCEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<OTCTRANSACTIONTYPE>
						<xsl:value-of select ="$varTaxlotState"/>
					</OTCTRANSACTIONTYPE>


					<OTCACTIONTYPE>
						<xsl:value-of select ="$varTaxlotState"/>
					</OTCACTIONTYPE>

					<OTCTYPE>
						<!--<xsl:value-of select="'FXFORWARD'"/>-->
						<xsl:choose>
							<xsl:when test="Asset= 'FXForward'">
								<xsl:value-of select="'FXFORWARD'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'FXCURRENCY'"/>
							</xsl:otherwise>
						</xsl:choose>
					</OTCTYPE>


					<ISNDF>
						<xsl:value-of select="''"/>
					</ISNDF>

					<FIXINGDATE>
						<xsl:value-of select="''"/>
					</FIXINGDATE>

					<xsl:variable name ="varAllocationStateMSG">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'C'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'X'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<MSGTYPE>
						<xsl:value-of select ="$varAllocationStateMSG"/>
					</MSGTYPE>

					<BUYAMOUNT>
						<xsl:choose>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
								<xsl:value-of select="ExecutedQty" />
							</xsl:when>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
								<xsl:value-of select="ExecutedQty * AveragePrice" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BUYAMOUNT>

					<SELLAMOUNT>
						<xsl:choose>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
								<xsl:value-of select ="ExecutedQty * AveragePrice"/>
							</xsl:when>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
								<xsl:value-of select="ExecutedQty" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SELLAMOUNT>

					<BUYCURRENCY>
						<xsl:choose>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
								<xsl:value-of select="LeadCurrencyName"/>
							</xsl:when>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BUYCURRENCY>

					<SELLCURRENCY>
						<xsl:choose>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
								<xsl:value-of select ="LeadCurrencyName"/>
							</xsl:when>
							<xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SELLCURRENCY>

					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
