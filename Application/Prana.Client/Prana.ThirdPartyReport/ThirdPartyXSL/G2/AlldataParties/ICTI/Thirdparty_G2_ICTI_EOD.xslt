<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='ICTI' and Asset = 'Equity']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

					<xsl:variable name="PB_NAME" select="'ICTI'"/>

					<Side>
						<xsl:choose>
							<xsl:when test="Side='Buy' ">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side= Side='Sell'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' ">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<Ticker>
						<xsl:value-of select="Symbol"/>
					</Ticker>

					<OrderQuantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</OrderQuantity>
					
					<PB>
						<xsl:choose>
							<xsl:when test="AccountName='G2 Investment Partners LP' or AccountName='G2 Investment Partners QP LP'">
								<xsl:value-of select="'GS'"/>
							</xsl:when>
							<xsl:when test="AccountName='Quantum Partners LP' or AccountName='MS Investment Partners LP' or AccountName='MS Investment Partners QP'">
								<xsl:value-of select="'MS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PB>					

					<ExecutionPrice>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="format-number(AveragePrice,'###.0000')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutionPrice>					

					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<TradeCommission>
						<!--<xsl:value-of select ="$varCommission"/>-->
						<xsl:value-of select ="format-number($varCommission,'###.0000')"/>
					</TradeCommission>

					<SecFees>
						<!--<xsl:value-of select="StampDuty"/>-->
						<xsl:value-of select="format-number(StampDuty,'###.0000')"/>
					</SecFees>

					<xsl:variable name = "OthFees">
						<xsl:value-of select="(TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee)"/>
					</xsl:variable>

					<OtherFees>
						<!--<xsl:value-of select="$OthFees"/>-->
						<xsl:value-of select="format-number($OthFees,'###.0000')"/>
					</OtherFees>


					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<ExecutingBrokerCode>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBrokerCode>
					
					<TradeDate>
						<xsl:value-of select ="TradeDate"/>
					</TradeDate>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>