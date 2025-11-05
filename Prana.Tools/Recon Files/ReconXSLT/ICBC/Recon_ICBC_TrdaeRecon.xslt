<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">
				
				<xsl:variable name = "PB_NAME">
					<xsl:value-of select="'ML'"/>
				</xsl:variable>
				
				<xsl:variable name ="PB_FUND_NAME">
					<xsl:value-of select ="concat(COL22,COL23,COL24,COL25)"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
			
				<xsl:if test ="number(COL9) and $PRANA_FUND_NAME!=''  and $PRANA_FUND_NAME!=''">

					<PositionMaster>

					

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL37"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<Symbol>

							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									
									<xsl:choose>
										<xsl:when test ="COL8!=''">
											<xsl:choose>
												<xsl:when test="string-length(COL8) &lt; 21">
													<xsl:value-of select ="COL8"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>				
										
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="string-length(COL8) &lt; 21">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(COL8,'U')"/>
								</xsl:otherwise>
							</xsl:choose>							
						</IDCOOptionSymbol>

						<SMRequest>
							<xsl:value-of select="'True'"/>
						</SMRequest>


						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<TradeDate>
							<xsl:value-of select ="COL1"/>
						</TradeDate>

						<xsl:variable name="varQuantity">
							<xsl:value-of select="number(COL9)"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="$varQuantity &lt;0">
									<xsl:value-of select ="$varQuantity*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varQuantity"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:choose>
								<xsl:when test ="number($varQuantity)">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>-->
						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>

								<xsl:when test="$varSide='B'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="$varSide='S'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:when test="$varSide='SS'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								
								<xsl:when test="$varSide='BC'">
									<xsl:value-of select="'Buy to close'"/>
								</xsl:when>


								<xsl:when test="$varSide='CB'">
									<xsl:value-of select="'Buy to close'"/>
								</xsl:when>


								<xsl:when test="$varSide='CS'">
									<xsl:value-of select="'Sell to close'"/>
								</xsl:when>


								<xsl:when test="$varSide='OB'">
									<xsl:value-of select="'Buy to open'"/>
								</xsl:when>


								<xsl:when test="$varSide='OS'">
									<xsl:value-of select="'Sell to open'"/>
								</xsl:when>
								
								
								
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<xsl:variable name="varAvgPx">
							<xsl:value-of select="COL10"/>
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

						<CurrencySymbol>
							<xsl:value-of select ="COL50"/>
						</CurrencySymbol>


						<xsl:variable name="varNetNotionalValue">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								
								<xsl:when test="number($varNetNotionalValue) ">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
								
							</xsl:choose>
						</NetNotionalValue>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
