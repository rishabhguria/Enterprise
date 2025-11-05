<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name ="varSide">
					<xsl:value-of select ="translate(COL5,'&quot;','')"/>
				</xsl:variable>
				
				<xsl:variable name ="varQty">
					<xsl:value-of select ="translate(COL6,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="number($varQty)">
					<PositionMaster>
						<OrderSideTagValue>
							<xsl:choose>
								<xsl:when test="$varSide='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy to Open'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell to Close'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell to Open'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderSideTagValue>

						<Symbol>
							<xsl:value-of select="translate(normalize-space(COL1),'&quot;','')"/>
						</Symbol>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQty)">
									<xsl:value-of select="number($varQty)"/>
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
	
						<OrderTypeTagValue>
							<xsl:value-of select ="'2'"/>
						</OrderTypeTagValue>

						<xsl:variable name ="varPrice">
							<xsl:value-of select ="translate(COL2,'&quot;','')"/>
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

						<xsl:variable name ="varAccountID">
					      <xsl:value-of select ="translate(COL3,'&quot;','')"/>
				        </xsl:variable>
				
						<Level1ID>
							 <xsl:value-of select="$varAccountID"/>
						</Level1ID>
						
						<CounterPartyName>
							<xsl:choose>
								<xsl:when test="$varAccountID='9'">
									<xsl:value-of select="'MSCO'"/>
								</xsl:when>
								<xsl:when test="$varAccountID='10' or $varAccountID='11'">
									<xsl:value-of select="'BTIG'"/>
								</xsl:when>
								<xsl:when test="$varAccountID='14'">
									<xsl:value-of select="'VCGO'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'MS'"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyName>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$varAccountID='9'">
									<xsl:value-of select="47"/>
								</xsl:when>
								<xsl:when test="$varAccountID='10' or $varAccountID='11'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="$varAccountID='14'">
									<xsl:value-of select="'126'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<HandlingInstruction>
							<xsl:value-of select="3"/>
						</HandlingInstruction>


						<Level2ID>
								<xsl:value-of select ="0"/>
						</Level2ID>

						<TradingAccountID>
							<xsl:value-of select="11"/>
						</TradingAccountID>

						<ExecutionInstruction>
							<xsl:value-of select="'E'"/>
						</ExecutionInstruction>
						
						<!--<FXRate>
							<xsl:value-of select="COL7"/>
						</FXRate>-->
						
						<!--<FXConversionMethodOperator>
							<xsl:value-of select="'M'"/>
						</FXConversionMethodOperator>-->

						<!--<AUECID>
							<xsl:value-of select="1"/>
						</AUECID>

						<AssetID>
							<xsl:value-of select="1"/>
						</AssetID>-->

						

						<CumQty>
							<xsl:value-of select="0"/>
						</CumQty>

						<!-- <SettlCurrFxRate> -->
							<!-- <xsl:value-of select="COL7"/> -->
						<!-- </SettlCurrFxRate> -->

						<!-- <SettlCurrFxRateCalc> -->
							<!-- <xsl:value-of select="'M'"/> -->
						<!-- </SettlCurrFxRateCalc> -->

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
									<xsl:value-of select="'77'"/>
								<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL9"/>
								</xsl:otherwise>
									</xsl:choose>-->
						</UserID>
						
						<!-- <TradeAttribute1>                   -->
                          <!-- <xsl:value-of select="'DMA_E'"/>                     -->
              <!-- </TradeAttribute1> -->
			  
			  <!-- <TradeAttribute2>                   -->
                          <!-- <xsl:value-of select="'DMA_O'"/>                     -->
              <!-- </TradeAttribute2> -->

						<!--<Text>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Text>-->
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>