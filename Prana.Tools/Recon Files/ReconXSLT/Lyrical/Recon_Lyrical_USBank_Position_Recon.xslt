<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<NewDataSet>

			<xsl:for-each select ="//Comparision">

				<xsl:if test ="number(COL2) and COL3 !='CASHBALANCE' and COL7!=''">

					<PositionMaster>

						<xsl:variable name = "PBNAME">
							<xsl:value-of select="'US Bank'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PBNAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>

							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="contains(COL3,'.')">

											<xsl:choose>
												<xsl:when test="contains(COL3,'.')">
													<xsl:value-of select ="substring-before(COL3,'.')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="COL3"/>
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

						

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PBNAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<TradeDate>
							<xsl:value-of select ="COL5"/>
						</TradeDate>


						<xsl:variable name ="varQuantity">
							<xsl:value-of select ="number(COL2)"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>

								<xsl:when test ="number($varQuantity)">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>					

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL4)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<Asset>
							<xsl:value-of select ="COL7"/>
						</Asset>

						<CurrencySymbol>
							<xsl:value-of select ="COL6"/>
						</CurrencySymbol>
						
						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</NewDataSet>

	</xsl:template>

</xsl:stylesheet>
