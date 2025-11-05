<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		
		<ThirdPartyFlatFileDetailCollection>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>
			
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="'AgileHedge'"/>

					<Symbol>
						<xsl:value-of select="Symbol"/>
					</Symbol>

					<xsl:variable name = "PRANA_BROKER_NAME">
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>

					<xsl:variable name ="ThirdParty_BROKER_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<Broker>
						<xsl:choose>
							<xsl:when test="$ThirdParty_BROKER_CODE!=''">
								<xsl:value-of select="$ThirdParty_BROKER_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>		
							</xsl:otherwise>
						</xsl:choose>
					</Broker>

					<Action>
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
					</Action>

					<Shares>
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
					</Shares>

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

					<Commission>
						<xsl:choose>
							<xsl:when test="number(CommissionCharged)">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</Commission>					

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
