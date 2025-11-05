<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 != 'Side'">
					<PositionMaster>
						<OrderSideTagValue>
							<xsl:choose>
								<xsl:when test="COL1='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL1='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL1='Sell short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL1='Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderSideTagValue>

						<Symbol>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Symbol>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number(COL3)">
									<xsl:value-of select="COL3"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<AuecLocalDate>
							<xsl:value-of select ="COL8"/>
						</AuecLocalDate>
						
						<xsl:variable name ="varOrderTypeTagValue">
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<OrderTypeTagValue>

							<xsl:choose>

								<xsl:when test ="$varOrderTypeTagValue='Market'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Limit'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Stop'">
									<xsl:value-of select ="'3'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Stop Limit'">
									<xsl:value-of select ="'4'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Market on close'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='With or without'">
									<xsl:value-of select ="'6'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Limit or better'">
									<xsl:value-of select ="'7'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Limit with or without'">
									<xsl:value-of select ="'8'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='On basis'">
									<xsl:value-of select ="'9'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Pegged'">
									<xsl:value-of select ="'P'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'1'"/>
								</xsl:otherwise>
							</xsl:choose>

						</OrderTypeTagValue>

						<Price>
							<xsl:value-of select="number(COL5)"/>
						</Price>

						<Venue>
							<xsl:value-of select="'NASDAQ'"/>
						</Venue>

						<VenueID>
							<xsl:value-of select="1"/>
						</VenueID>

						<CounterPartyName>
							<xsl:value-of select="'Bernstein'"/>
						</CounterPartyName>

						<CounterPartyID>
							<xsl:value-of select="1"/>
						</CounterPartyID>

						<HandlingInstruction>
							<xsl:value-of select="3"/>
						</HandlingInstruction>

						<StopPrice>
							<xsl:value-of select="0.0"/>
						</StopPrice>

						<TIF>
							<xsl:value-of select="0"/>
						</TIF>

						<Level1ID>
							<xsl:choose>
								<xsl:when test ="COL7 &lt;0 or COL7='' or COL7='*'">
									<xsl:value-of select ="'-2147483648'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL7"/>
								</xsl:otherwise>
							</xsl:choose>
						</Level1ID>

						<Level2ID>
							<xsl:choose>
								<xsl:when test ="COL6='*'">
									<xsl:value-of select ="0"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL6"/>
								</xsl:otherwise>
							</xsl:choose>
						</Level2ID>

						<TradingAccountID>
							<xsl:value-of select="11"/>
						</TradingAccountID>

						<ExecutionInstruction>
							<xsl:value-of select="'G'"/>
						</ExecutionInstruction>

						<AUECID>
							<xsl:value-of select="1"/>
						</AUECID>

						<AssetID>
							<xsl:value-of select="1"/>
						</AssetID>

						<UnderlyingID>
							<xsl:value-of select="1"/>
						</UnderlyingID>

						<CumQty>
							<xsl:value-of select="0"/>
						</CumQty>

						<SettlCurrFxRate>
							<xsl:value-of select="1.0"/>
						</SettlCurrFxRate>

						<SettlCurrFxRateCalc>
							<xsl:value-of select="'M'"/>
						</SettlCurrFxRateCalc>

						<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>

						<ExchangeID>
							<xsl:value-of select="1"/>
						</ExchangeID>

						<CommissionRate>
							<xsl:value-of select="0.0"/>
						</CommissionRate>

						<CalcBasis>
							<xsl:value-of select="8"/>
						</CalcBasis>

						<SecurityType>
							<xsl:value-of select="'EQTY'"/>
						</SecurityType>

						<!--<DiscretionInst>
							<xsl:value-of select="0"/>
						</DiscretionInst>-->

						<DisplayQuantity>
							<xsl:value-of select="0.0"/>
						</DisplayQuantity>

						<LocateReqd>
							<xsl:value-of select="false"/>
						</LocateReqd>

						<PranaMsgType>
							<xsl:value-of select="3"/>
						</PranaMsgType>
						<UserID>
							<xsl:value-of select ="COL9"/>
						</UserID>

						<!--<Text>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Text>-->
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>