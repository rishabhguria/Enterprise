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

					<UBSAccount>
						<xsl:value-of select ="FundMappedName"/>
					</UBSAccount>
				
					<xsl:variable name = "varTradeMth" >
						<xsl:value-of select="substring(TradeDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeDay" >
						<xsl:value-of select="substring(TradeDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeYR" >
						<xsl:value-of select="substring(TradeDate,7,4)"/>
					</xsl:variable>
					<TradeDate>
						<xsl:value-of select="concat($varTradeYR,'',$varTradeMth,'',$varTradeDay)"/>
					</TradeDate>

					<xsl:variable name = "varSettleMth" >
						<xsl:value-of select="substring(SettlementDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleDay" >
						<xsl:value-of select="substring(SettlementDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleYR" >
						<xsl:value-of select="substring(SettlementDate,7,4)"/>
					</xsl:variable>
					<SettleDate>
						<xsl:value-of select="concat($varSettleYR,'',$varSettleMth,'',$varSettleDay)"/>
					</SettleDate>

					<!--   Side     -->

					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<ActionCode>
								<xsl:value-of select="'B'"/>
							</ActionCode>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<ActionCode>
								<xsl:value-of select="'BC'"/>
							</ActionCode>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<ActionCode>
								<xsl:value-of select="'S'"/>
							</ActionCode>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<ActionCode>
								<xsl:value-of select="'SS'"/>
							</ActionCode>
						</xsl:when>
						<xsl:otherwise>
							<ActionCode>
								<xsl:value-of select="''"/>
							</ActionCode>
						</xsl:otherwise>
					</xsl:choose>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

          <xsl:choose>
            <xsl:when test="Asset = 'Equity'">
					<SecurityID>
						<xsl:value-of select ="Symbol"/>
					</SecurityID>
            </xsl:when>
            <xsl:when test ="Asset = 'EquityOption'">
              <xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
              <xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
              <SecurityID>
                <xsl:value-of select ="concat($varSymbolBef,'+',$varSymbolAft)"/>
              </SecurityID>
            </xsl:when>
            <xsl:otherwise>
              <SecurityID>
                <xsl:value-of select ="Symbol"/>
              </SecurityID>
            </xsl:otherwise>
          </xsl:choose>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>
					<Commission>
						<xsl:value-of select="CommissionCharged + TaxOnCommissions"/>
					</Commission>
					
					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
							<NetAmmount>
								<xsl:value-of select="NetAmount - (StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee)"/>
							</NetAmmount>
						</xsl:when>					
						<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
							<NetAmmount>
								<xsl:value-of select="NetAmount + (StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee)"/>
							</NetAmmount>
						</xsl:when>						
						<xsl:otherwise>
							<NetAmmount>
								<xsl:value-of select="NetAmount"/>
							</NetAmmount>
						</xsl:otherwise>
					</xsl:choose>					

					<SettleCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettleCurrency>					

					<!--   Side End    -->
					<xsl:choose>
						<xsl:when test ="CounterParty='CUTTONE' or CounterParty='CUTN'">
							<ExecBrokerCode>
								<xsl:value-of select="'CUTE'"/>
							</ExecBrokerCode>
						</xsl:when>
						<xsl:otherwise>
							<ExecBrokerCode>
								<xsl:value-of select="CounterParty"/>
							</ExecBrokerCode>
						</xsl:otherwise>
					</xsl:choose>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
