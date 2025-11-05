<?xml version="1.0" encoding="UTF-8"?>

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

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<TradeType>
					<xsl:value-of select="'Trade Type'"/>
				</TradeType>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'Settle Date'"/>
				</SettleDate>

				<TradePrice>
					<xsl:value-of select="'Trade Price'"/>
				</TradePrice>

				<SecurityIdentifier>
					<xsl:value-of select="'Security Identifier'"/>
				</SecurityIdentifier>

				<SharesPar>
					<xsl:value-of select="'Shares / Par'"/>
				</SharesPar>

				<QuantityCurrentFace>
					<xsl:value-of select="'Quantity/Current Face'"/>
				</QuantityCurrentFace>

				<QuantityOriginalFace>
					<xsl:value-of select="'Quantity/Original Face'"/>
				</QuantityOriginalFace>

				<AccountNumber>
					<xsl:value-of select="'Account Number'"/>
				</AccountNumber>

				<ExecutingBrokerNumber>
					<xsl:value-of select="'Executing Broker Number'"/>
				</ExecutingBrokerNumber>

				<ClearingBrokerNumber>
					<xsl:value-of select="'Clearing Broker Number'"/>
				</ClearingBrokerNumber>

				<PrincipalAmount>
					<xsl:value-of select="'Principal Amount'"/>
				</PrincipalAmount>

				<BrokerCommission>
					<xsl:value-of select="'Broker Commission'"/>
				</BrokerCommission>

				<AccruedInterest>
					<xsl:value-of select="'Accrued Interest'"/>
				</AccruedInterest>

				<MiscellaneousFee>
					<xsl:value-of select="'Miscellaneous Fee'"/>
				</MiscellaneousFee>

				<PairoffAmount>
					<xsl:value-of select="'Pairoff Amount'"/>
				</PairoffAmount>

				<SettlementAmount>
					<xsl:value-of select="'Settlement Amount'"/>
				</SettlementAmount>

				<DeliveryInstructions>
					<xsl:value-of select="'Delivery Instructions'"/>
				</DeliveryInstructions>

				<SpecialInstructions>
					<xsl:value-of select="'Special Instructions'"/>
				</SpecialInstructions>




				<!-- system use only-->
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

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>				

					<TradeType>
						<xsl:choose>
							<xsl:when test="contains(Side,' ')">
								<xsl:value-of select="substring-before(Side,' ')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeType>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>


					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<TradePrice>
						<xsl:value-of select="AveragePrice"/>
					</TradePrice>

					<SecurityIdentifier>
						<xsl:value-of select="CUSIP"/>
					</SecurityIdentifier>

					<SharesPar>
						<xsl:value-of select="AllocatedQty"/>
					</SharesPar>

					<QuantityCurrentFace>
						<xsl:value-of select="AllocatedQty*AveragePrice"/>
					</QuantityCurrentFace>

					<QuantityOriginalFace>
						<xsl:value-of select="''"/>
					</QuantityOriginalFace>

					<AccountNumber>

						<xsl:choose>
							<xsl:when test ="AccountNo='Okabena U.S. Satellite Equity Fund LLC'">
								<xsl:value-of select="'12570901'"/>
							</xsl:when>
							<xsl:when test ="AccountNo='The Purpleville Foundation: 14462208'">
								<xsl:value-of select="'14462208'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountNo"/>
							</xsl:otherwise>
						</xsl:choose>
						
						
						
						
						<!--<xsl:value-of select="AccountNo"/>-->
					</AccountNumber>

					<xsl:variable name ="varBroker">
						<xsl:choose>
							<xsl:when test ="CounterParty='JPM'">
								<xsl:value-of select="'0187'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<ExecutingBrokerNumber>
						<xsl:value-of select="$varBroker"/>
					</ExecutingBrokerNumber>

					<ClearingBrokerNumber>
						<xsl:value-of select="''"/>
					</ClearingBrokerNumber>

					<PrincipalAmount>
						<xsl:value-of select="AllocatedQty*AveragePrice"/>
					</PrincipalAmount>

					<BrokerCommission>
						<xsl:value-of select="''"/>
					</BrokerCommission>

					<AccruedInterest>
						<xsl:value-of select="''"/>
					</AccruedInterest>

					<MiscellaneousFee>
						<xsl:value-of select="''"/>
					</MiscellaneousFee>

					<PairoffAmount>
						<xsl:value-of select="''"/>
					</PairoffAmount>

					<SettlementAmount>
						<xsl:value-of select="AllocatedQty*AveragePrice"/>
					</SettlementAmount>

					<DeliveryInstructions>
						<xsl:value-of select="''"/>
					</DeliveryInstructions>

					<SpecialInstructions>
						<xsl:value-of select="''"/>
					</SpecialInstructions>


					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
