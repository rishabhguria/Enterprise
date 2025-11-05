<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<NewDataSet>

		<xsl:for-each select ="//Comparision">
			<xsl:if test ="number(COL13)">
				<Comparision>

					<xsl:variable name ="varAvgPx">
						<xsl:value-of select ="COL26"/>
					</xsl:variable>


					<AccountName>

						<xsl:value-of select ="'ECOR'"/>

					</AccountName>

					<PrimeBroker>

						<xsl:value-of select ="'BTIG'"/>

					</PrimeBroker>

					<MarkPrice>
						<xsl:choose>
							<xsl:when test ="$varAvgPx &lt;0">
								<xsl:value-of select ="$varAvgPx*-1"/>
							</xsl:when>

							<xsl:when test ="$varAvgPx &gt;0">
								<xsl:value-of select ="$varAvgPx"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MarkPrice>

					<xsl:variable name ="varMarketValueLocal">
						<xsl:value-of select ="COL17"/>
					</xsl:variable>


					<MarketValue>
						<xsl:choose>
							<xsl:when test ="$varMarketValueLocal &lt;0">
								<xsl:value-of select ="$varMarketValueLocal*-1"/>
							</xsl:when>

							<xsl:when test ="$varMarketValueLocal &gt;0">
								<xsl:value-of select ="$varMarketValueLocal"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MarketValue>

					<TradeDate>
						<xsl:value-of select ="substring(COL1,1,9)"/>
					</TradeDate>

					<PBSymbol>
						<xsl:value-of select ="COL12"/>
					</PBSymbol>

					<xsl:variable name = "PB_SYMBOL_NAME" >
						<xsl:value-of select ="COL12"/>
					</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Newland_GS']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
					</xsl:variable>


					<xsl:choose>

						<xsl:when test ="$PRANA_SYMBOL_NAME !=''">

							<Symbol>
								<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
							</Symbol>

							<IDCOOptionSymbol>
								<xsl:value-of select ="''"/>
							</IDCOOptionSymbol>

						</xsl:when>

						<xsl:otherwise>

							<xsl:choose>
								<xsl:when test ="COL5!=''">
									<xsl:choose>
										<xsl:when test ="string-length(COL5) &gt; 15">

											<Symbol>
												<xsl:value-of select ="''"/>
											</Symbol>

											<IDCOOptionSymbol>
												<xsl:value-of select ="concat(COL5,'U')"/>
											</IDCOOptionSymbol>

										</xsl:when>


										<xsl:otherwise>

											<Symbol>
												<xsl:value-of select ="COL5"/>
											</Symbol>

											<IDCOOptionSymbol>
												<xsl:value-of select ="''"/>
											</IDCOOptionSymbol>

										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>

									<Symbol>
										<xsl:value-of select ="COL12"/>
									</Symbol>

									<IDCOOptionSymbol>
										<xsl:value-of select ="''"/>
									</IDCOOptionSymbol>

								</xsl:otherwise>
							</xsl:choose>

						</xsl:otherwise>

					</xsl:choose>


					<xsl:variable name="varSide">
						<xsl:value-of select="COL14"/>
					</xsl:variable>

					<Quantity>
						<xsl:choose>
							<xsl:when test="$varSide='L' and number(COL13)">
								<xsl:value-of select="COL13"/>
							</xsl:when>
							<xsl:when test="$varSide='S' and number(COL13)">
								<xsl:value-of select="COL13*-1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>



					<Side>
						<xsl:choose>
							<xsl:when test="string-length(COL5) &gt;15">
								<xsl:choose>
									<xsl:when test="$varSide='L'">
										<xsl:value-of select="'Buy to Open'"/>
									</xsl:when>
									<xsl:when test="$varSide='S'">
										<xsl:value-of select="'Sell to Open'"/>
									</xsl:when>
								</xsl:choose>								
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$varSide='L'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$varSide='S'">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<SMRequest>
						<xsl:value-of select ="'TRUE'"/>
					</SMRequest>


				</Comparision>
			</xsl:if>
		</xsl:for-each>
	</NewDataSet>
</xsl:template>

</xsl:stylesheet> 
