<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name ="varSide">
					<xsl:value-of select ="translate(COL1,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="number(COL3) and $varSide != 'Side' and translate(normalize-space(COL2),'&quot;','')!='USD' and translate(normalize-space(COL2),'&quot;','')!='CASH'">
					<PositionMaster>
						<OrderSideTagValue>
							<xsl:choose>
								<xsl:when test="$varSide='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy To Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderSideTagValue>

						<Symbol>
							<xsl:value-of select="translate(normalize-space(COL2),'&quot;','')"/>
						</Symbol>

						<xsl:variable name ="varQuantity">
							<xsl:value-of select ="translate(COL3,'&quot;','')"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="number($varQuantity)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<!--<AuecLocalDate>
						<xsl:choose>
								<xsl:when test="COL8='*' or COL8=''">
									<xsl:value-of select="'2014-03-06 08:43:29.000'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL8"/>
								</xsl:otherwise>
							</xsl:choose>
						</AuecLocalDate>-->
						
						<xsl:variable name ="varOrderTypeTagValue">
							<xsl:value-of select ="translate(COL4,'&quot;','')"/>
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

						<xsl:variable name ="varPrice">
							<xsl:value-of select ="translate(COL5,'&quot;','')"/>
						</xsl:variable>
						
						
						<xsl:variable name ="varBroker">
							<xsl:value-of select ="translate(COL10,'&quot;','')"/>
						</xsl:variable>

						<Price>
							<xsl:choose>
								<xsl:when test ="$varPrice='' or $varPrice='*'">
							<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</Price>

						<Venue>
							<xsl:value-of select="'Drops'"/>
						</Venue>

						<VenueID>
							<xsl:value-of select="1"/>
						</VenueID>

						<CounterPartyName>
							<xsl:choose>
								<xsl:when test="$varBroker='MSCOS'">
									<xsl:value-of select="'MSCOS'"/>
								</xsl:when>
								<xsl:when test="$varBroker='CCMB'">
									<xsl:value-of select="'CCMB'"/>
								</xsl:when>
								
								<xsl:when test="$varBroker='MSCOU'">
									<xsl:value-of select="'MSCOU'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="'MSCOS'"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyName>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$varBroker='MSCOS'">
									<xsl:value-of select="21"/>
								</xsl:when>
								<xsl:when test="$varBroker='CCMB'">
									<xsl:value-of select="53"/>
								</xsl:when>
								
								<xsl:when test="$varBroker='MSCOU'">
									<xsl:value-of select="59"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="21"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<HandlingInstruction>
							<xsl:value-of select="3"/>
						</HandlingInstruction>

						<!--<StopPrice>
							<xsl:value-of select="0.0"/>
						</StopPrice>-->

						<!--<TIF>
							<xsl:value-of select="0"/>
						</TIF>-->


						<xsl:variable name ="varFund">
							<xsl:value-of select ="translate(COL9,'&quot;','')"/>
						</xsl:variable>


						<Level1ID>
							<xsl:choose>
							
								<xsl:when test ="$varFund = 'KH1 - BTIG'">
									<xsl:value-of select="1"/>
								</xsl:when>
								
								<xsl:when test ="$varFund = 'KH1 - GS'">
									<xsl:value-of select="2"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'KH2 - BTIG'">
									<xsl:value-of select="3"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'KH2 - GS'">
									<xsl:value-of select="4"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'SEI - Longs - US Bank Main'">
									<xsl:value-of select="5"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'SEI - Longs - US Bank Pledge'">
									<xsl:value-of select="6"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'SEI - Shorts - Morgan Stanley'">
									<xsl:value-of select="7"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'LoCorr - Longs - US Bank Main'">
									<xsl:value-of select="8"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'LoCorr - Longs - US Bank Pledge'">
									<xsl:value-of select="9"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'LoCorr - Shorts - Morgan Stanley'">
									<xsl:value-of select="10"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'KH1 - Pershing'">
									<xsl:value-of select="11"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'KH2 - Pershing'">
									<xsl:value-of select="12"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'KH Tech'">
									<xsl:value-of select="13"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'PACE - Longs - State Street'">
									<xsl:value-of select="14"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'PACE - Shorts - Morgan Stanley'">
									<xsl:value-of select="15"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'Heptagon - Longs - BBH'">
									<xsl:value-of select="16"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'Heptagon - Shorts - Morgan Stanley'">
									<xsl:value-of select="17"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'Catenary - JPM'">
									<xsl:value-of select="18"/>
								</xsl:when>

								<xsl:when test ="$varFund = 'Walleye - GS'">
									<xsl:value-of select="19"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose>
						
						</Level1ID>

						<Level2ID>
									<xsl:value-of select ="0"/>
						</Level2ID>

						<TradingAccountID>
							<xsl:value-of select="11"/>
						</TradingAccountID>

						<!--<ExecutionInstruction>
							<xsl:value-of select="'G'"/>
						</ExecutionInstruction>-->

						<!--<AUECID>
							<xsl:value-of select="1"/>
						</AUECID>

						<AssetID>
							<xsl:value-of select="1"/>
						</AssetID>-->

						

						<CumQty>
							<xsl:value-of select="0"/>
						</CumQty>

						<!--<SettlCurrFxRate>
							<xsl:value-of select="1.0"/>
						</SettlCurrFxRate>

						<SettlCurrFxRateCalc>
							<xsl:value-of select="'M'"/>
						</SettlCurrFxRateCalc>-->

						<!--<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>

						<ExchangeID>
							<xsl:value-of select="1"/>
						</ExchangeID>

						<CommissionRate>
							<xsl:value-of select="0.0"/>
						</CommissionRate>-->

						<!--<CalcBasis>
							<xsl:value-of select="8"/>
						</CalcBasis>

						<SecurityType>
							<xsl:value-of select="'EQTY'"/>
						</SecurityType>-->

						<!--<DiscretionInst>
							<xsl:value-of select="0"/>
						</DiscretionInst>-->

						<!--<DisplayQuantity>
							<xsl:value-of select="0.0"/>
						</DisplayQuantity>

						<LocateReqd>
							<xsl:value-of select="false"/>
						</LocateReqd>-->

						<PranaMsgType>
							<xsl:value-of select="3"/>
						</PranaMsgType>
						<UserID>
								<!--<xsl:choose>
								<xsl:when test="COL9='*' or COL9=''">-->
									<xsl:value-of select="'44'"/>
								<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL9"/>
								</xsl:otherwise>
									</xsl:choose>-->
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