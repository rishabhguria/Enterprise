<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="number(COL34)">
					<Comparision>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'MERLIN'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL" >
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundName=$PB_FUND]/@PranaFund"/>
						</xsl:variable>


						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND!=''">
									<xsl:value-of select="$PRANA_FUND"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="COL13 !=''">
											<xsl:value-of select ="normalize-space(COL13)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="$PB_SYMBOL"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CurrencySymbol>
							<xsl:value-of select ="COL2"/>
						</CurrencySymbol>

						<TradeDate>
							<xsl:value-of select ="COL8"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select ="COL9"/>
						</SettlementDate>

						<xsl:variable name="varQuantity">
							<xsl:value-of select="number(COL34)"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="$varQuantity &gt;0">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>
								<xsl:when test ="$varQuantity &lt;0">
									<xsl:value-of select ="$varQuantity *-1"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>						
						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL18)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varSide='BY'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varSide='CS'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:when test="$varSide='SL'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="$varSide='SS'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="varAvgPx">
							<xsl:value-of select="number(COL28)"/>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$varAvgPx &lt; 0">
									<xsl:value-of select="$varAvgPx *-1"/>
								</xsl:when>
								<xsl:when test="$varAvgPx &gt; 0">
									<xsl:value-of select="$varAvgPx"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<Asset>
							<xsl:value-of select ="COL29"/>
						</Asset>

						<xsl:variable name="varCommission">
							<xsl:value-of select="COL40"/>
						</xsl:variable> 
						
						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission *-1"/>
								</xsl:when>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="TotalCommissionandFees" select="number(COL40 + COL42 + COL44)"/>

						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$TotalCommissionandFees &lt; 0">
									<xsl:value-of select="$TotalCommissionandFees *-1"/>
								</xsl:when>
								<xsl:when test="$TotalCommissionandFees &gt; 0">
									<xsl:value-of select="$TotalCommissionandFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>
						
						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL42"/>
						</xsl:variable>
						
						<StampDuty>
							<xsl:choose>
								<xsl:when test="$varStampDuty &lt; 0">
									<xsl:value-of select="$varStampDuty *-1"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &gt; 0">
									<xsl:value-of select="$varStampDuty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

						<xsl:variable name="varFees">
							<xsl:value-of select="COL44"/>
						</xsl:variable>
						
						<Fees>
							<xsl:choose>
								<xsl:when test="$varFees &lt; 0">
									<xsl:value-of select="$varFees *-1"/>
								</xsl:when>
								<xsl:when test="$varFees &gt; 0">
									<xsl:value-of select="$varFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="varNetNotionalValue">
							<xsl:value-of select="COL38"/>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValue &lt; 0">
									<xsl:value-of select="$varNetNotionalValue *-1"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValue &gt; 0">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="varNetNotionalValueBase">
							<xsl:value-of select="COL48"/>
						</xsl:variable>

						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValueBase &lt; 0">
									<xsl:value-of select="$varNetNotionalValueBase *-1"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValueBase &gt; 0">
									<xsl:value-of select="$varNetNotionalValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

						<xsl:variable name="varGrossNotionalValue">
							<xsl:value-of select="COL36"/>
						</xsl:variable>

						<GrossNotionalValue>
							<xsl:choose>
								<xsl:when test="$varGrossNotionalValue &lt; 0">
									<xsl:value-of select="$varGrossNotionalValue *-1"/>
								</xsl:when>
								<xsl:when test="$varGrossNotionalValue &gt; 0">
									<xsl:value-of select="$varGrossNotionalValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</GrossNotionalValue>


					</Comparision>

				</xsl:if>
			</xsl:for-each >
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet> 
