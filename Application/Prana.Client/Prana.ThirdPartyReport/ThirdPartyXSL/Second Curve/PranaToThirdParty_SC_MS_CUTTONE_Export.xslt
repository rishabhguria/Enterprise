<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<IMP_ID>
						<xsl:value-of select="position()"/>
					</IMP_ID>

					<ExecAccountID>
						<xsl:value-of select="FundMappedName"/>
					</ExecAccountID>

					<SecurityID>
						<xsl:value-of select="Symbol"/>
					</SecurityID>

					<!--   Side     -->

					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<TransCode>
								<xsl:value-of select="'BL'"/>
							</TransCode>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<TransCode>
								<xsl:value-of select="'BC'"/>
							</TransCode>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<TransCode>
								<xsl:value-of select="'SL'"/>
							</TransCode>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<TransCode>
								<xsl:value-of select="'SS'"/>
							</TransCode>
						</xsl:when>
						<xsl:otherwise>
							<TransCode>
								<xsl:value-of select="''"/>
							</TransCode>
						</xsl:otherwise>
					</xsl:choose>

					<!--   Side End    -->

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<SettlementCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettlementCurrency>
					
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<xsl:choose>
            <xsl:when test ="CounterParty='WEED'">
							<BrokerCode>
                <xsl:value-of select="'WEEE'"/>
              </BrokerCode>
            </xsl:when>
            <xsl:when test ="CounterParty='CUTTONE' or CounterParty='CUTN'">
              <BrokerCode>
                <xsl:value-of select="'CUTE'"/>
							</BrokerCode>
						</xsl:when>
						<xsl:otherwise>
							<BrokerCode>
								<xsl:value-of select="CounterParty"/>
							</BrokerCode>
						</xsl:otherwise>
					</xsl:choose>

					<Custodian>
						<xsl:value-of select ="'MSCO'"/>
					</Custodian>
					
					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<CommissionType>
						<xsl:value-of select ="'C'"/>
					</CommissionType>

					<Commission>
						<xsl:value-of select="CommissionCharged"/>
					</Commission>
					
					<AssetType>
						<xsl:value-of select="translate(Asset, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
					</AssetType>

					<PutOrCall>
						<xsl:value-of select ="PutOrCall"/>
					</PutOrCall>

					<SecurityIDType>
						<xsl:value-of select ="'TICKER'"/>
					</SecurityIDType>
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
