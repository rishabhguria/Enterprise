<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>



			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset!='FX' and Asset!='FXForward']">

				<ThirdPartyFlatFileDetail>				
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name ="PB_NAME">
						<xsl:value-of select="'NT'"/>
					</xsl:variable>

					<SIDE>
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
							<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SIDE>	
					
					<EXTERNALREFERENCEID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select="concat(EntityID,'A')"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="concat(EntityID,'D')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="EntityID"/>
							</xsl:otherwise>
						</xsl:choose>
					</EXTERNALREFERENCEID>

					<TRADEDATE>
						<xsl:value-of select="TradeDate"/>
					</TRADEDATE>

					<FINANCIALTYPE>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'COMMON'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="'FUTURE'"/>
							</xsl:when>
							<xsl:when test="Asset='FutureOption'">
								<xsl:value-of select="'FUTUREOPT'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'BOND'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</FINANCIALTYPE>

					<CURRENCY>
						<xsl:value-of select="CurrencySymbol"/>
					</CURRENCY>

					<INSTRUMENTIDENTIFIER>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</INSTRUMENTIDENTIFIER>

					<INSTRUMENTIDENTIFIERTYPE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="'OSI'"/>
							</xsl:when>
							<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="'SEDOL'"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="'ISIN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</INSTRUMENTIDENTIFIERTYPE>

					

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="PB_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='NT']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
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
				
				
					
					<QUANTITY>
						<xsl:value-of select="AllocatedQty"/>
					</QUANTITY>

					<PRICE>
						<xsl:value-of select="AveragePrice"/>
					</PRICE>
					
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
						<xsl:value-of select="$varAllocationStateMSG"/>
					</MSGTYPE>

					<SETTLEDATE>
						<xsl:value-of select="SettlementDate"/>
					</SETTLEDATE>

					<PREVEXTERNALREFERENCEID>
						<xsl:choose>
							<xsl:when test="TaxLotState ='Amended' or TaxLotState ='Deleted'">
								<xsl:value-of select="EntityID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PREVEXTERNALREFERENCEID>

					<FEE_OTHER_FEE>
						<xsl:value-of select="OtherBrokerFee + SecFee + StampDuty + MiscFees + OrfFee + OccFee"/>
						<!--<xsl:value-of select="OtherBrokerFee"/>-->
						
					</FEE_OTHER_FEE>

					<NOTES>
						<xsl:value-of select="''"/>
					</NOTES>

					<CASHSUBACCOUNT>
						<xsl:value-of select="'MARGIN'"/>
					</CASHSUBACCOUNT>

					<CLEARINGACCOUNT>
						<xsl:value-of select="'NTRC-CUS-IAMG'"/>
					</CLEARINGACCOUNT>


					<COMMISSIONRATE>
						<xsl:value-of select="CommissionCharged"/>
					</COMMISSIONRATE>

					<COMMISSIONTYPE>
						<xsl:value-of select="'EXPLICIT'"/>
					</COMMISSIONTYPE>

					<DESK>
						<xsl:value-of select="'IAML'"/>
					</DESK>
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

					<PB>
						<xsl:value-of select="'CASH:XXIAMGIAML0001'"/>
					</PB>

				
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
