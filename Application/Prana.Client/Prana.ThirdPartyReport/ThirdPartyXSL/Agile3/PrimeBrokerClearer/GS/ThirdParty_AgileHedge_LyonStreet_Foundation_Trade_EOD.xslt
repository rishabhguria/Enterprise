<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<Ordernumber>
					<xsl:value-of select="'Order number'"/>
				</Ordernumber>

				<Cancelcorrectindicator>
					<xsl:value-of select="'Cancel correct indicator'"/>
				</Cancelcorrectindicator>

				<Accountnumberoracronym>
					<xsl:value-of select="'Account number or acronym'"/>
				</Accountnumberoracronym>

				<Securityidentifier>
					<xsl:value-of select="'Security identifier'"/>
				</Securityidentifier>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<!--<OptionStyle>
					<xsl:value-of select="'Option Style'"/>
				</OptionStyle>-->

				<Custodian>
					<xsl:value-of select="'Custodian'"/>
				</Custodian>

				<Transactiontype>
					<xsl:value-of select="'Transaction type'"/>
				</Transactiontype>

				<Currencycode>
					<xsl:value-of select="'Currency code'"/>
				</Currencycode>

				<Tradedate>
					<xsl:value-of select="'Trade date'"/>
				</Tradedate>

				<Settledate>
					<xsl:value-of select="'Settle date'"/>
				</Settledate>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="'AgileHedge'"/>

					<Ordernumber>
						<xsl:value-of select="''"/>
					</Ordernumber>

					<Cancelcorrectindicator>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="'N'"/>-->
					</Cancelcorrectindicator>


					<!--<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>-->

					<Accountnumberoracronym>
						<!--<xsl:choose>
							<xsl:when test ="$PB_FUND_CODE!=''">
								<xsl:value-of select="$PB_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="'242650'"/>
					</Accountnumberoracronym>

					<Securityidentifier>
						<xsl:value-of select="Symbol"/>
					</Securityidentifier>

					<xsl:variable name = "ThirdParty_BROKER_NAME">
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>

					<xsl:variable name ="ThirdParty_BROKER_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$ThirdParty_BROKER_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<Broker>
						<xsl:choose>
							<xsl:when test="$ThirdParty_BROKER_CODE!=''">
								<xsl:value-of select="$ThirdParty_BROKER_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$ThirdParty_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>

					</Broker>

					<Custodian>
						<xsl:value-of select="'GSCO'"/>
					</Custodian>

					<Transactiontype>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Transactiontype>

					<Currencycode>
						<xsl:value-of select="'USD'"/>
					</Currencycode>

					<Tradedate>
						<xsl:value-of select="TradeDate"/>
					</Tradedate>

					<Settledate>
						<xsl:value-of select="SettlementDate"/>
					</Settledate>

					<Quantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">

								<xsl:choose>
									<xsl:when test="AllocatedQty &gt; 0">
										<xsl:value-of select="AllocatedQty"/>
									</xsl:when>
									<xsl:when test="AllocatedQty &lt; 0">
										<xsl:value-of select="AllocatedQty * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<xsl:variable name="CommissionCharged">
						<xsl:choose>
						<xsl:when test="number(AllocatedQty*AssetMultiplier)">
							<xsl:value-of select="format-number(CommissionCharged div (AllocatedQty*AssetMultiplier),'#.######')"/>
						</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Commission>
						<xsl:choose>
							<xsl:when test="number($CommissionCharged)">
								<xsl:value-of select="$CommissionCharged"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

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

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
