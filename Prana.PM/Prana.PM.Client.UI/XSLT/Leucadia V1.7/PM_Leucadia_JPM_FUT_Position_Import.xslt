<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name ="varFutMonthCode">
		<xsl:param name="varFutMonth"/>

		<xsl:choose>

			<xsl:when  test ="$varFutMonth=1">
				<xsl:value-of select ="'F'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=2">
				<xsl:value-of select ="'G'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=3">
				<xsl:value-of select ="'H'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=4">
				<xsl:value-of select ="'J'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=5">
				<xsl:value-of select ="'K'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=6">
				<xsl:value-of select ="'M'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=7">
				<xsl:value-of select ="'N'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=8">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=9">
				<xsl:value-of select ="'U'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=10">
				<xsl:value-of select ="'V'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=11">
				<xsl:value-of select ="'X'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=12">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'JPM'"/>
				</xsl:variable>

				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="COL1"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:variable name ="varQuantity">
					<xsl:value-of select ="number(COL33)"/>
				</xsl:variable>

				<xsl:if test ="number($varQuantity) and $PRANA_FUND_NAME!=''">

					<PositionMaster>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_CODE">
							<xsl:value-of select="COL15"/>
						</xsl:variable>

						<xsl:variable name ="BBKey">
							<xsl:value-of select="normalize-space(COL16)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_UNDERLYING_NAME">
							<xsl:choose>
								<xsl:when test ="$BBKey!='' and $PB_CODE='SF'">
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE and @BBKey=$BBKey]/@UnderlyingCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@UnderlyingCode"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_PRICEMULTIPLIER_NAME">
							<xsl:choose>
								<xsl:when test ="$BBKey!='' and $PB_CODE='SF'">
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE and @BBKey=$BBKey]/@PriceMultiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@PriceMultiplier"/>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>

						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:choose>
								<xsl:when test ="$BBKey!='' and $PB_CODE='SF'">
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE and @BBKey=$BBKey]/@ExchangeName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@ExchangeName"/>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>

						<xsl:variable name="PRANA_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="varExchange">
							<xsl:choose>
								<xsl:when test="COL16='LME'">
									<xsl:value-of select="'-LME'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFUTUnderlying">
							<xsl:choose>
								<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
									<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_CODE"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFutYear">
							<xsl:value-of select="substring(COL43,4,1)"/>
						</xsl:variable>

						<xsl:variable name="varFutMonth">
							<xsl:value-of select="COL32"/>
						</xsl:variable>

						<xsl:variable name="varFutMonthCode">
							<xsl:call-template name="varFutMonthCode">
								<xsl:with-param name="varFutMonth" select="number($varFutMonth)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="PRANA_STRIKE_MULTIPLIER">
							<xsl:choose>
								<xsl:when test ="$BBKey!='' and $PB_CODE='SF'">
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE and @BBKey=$BBKey]/@StrikeMul"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE]/@StrikeMul"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</xsl:variable>

						<xsl:variable name="varFutOptStrikePrice">
							<xsl:choose>
								<xsl:when test="$PRANA_STRIKE_MULTIPLIER!=''">
									<xsl:value-of select="number(COL27)*number($PRANA_STRIKE_MULTIPLIER)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(COL27)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL4),'1'),' '),' '),1,1)"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:choose>
								<xsl:when test ="$PRANA_UNDERLYING_NAME!=''">
									<xsl:choose>

										<xsl:when test="$PRANA_FLAG!=''">
											<xsl:choose>
												<xsl:when test="COL29='FUT'">
													<!--<xsl:choose>
												<xsl:when test="COL16='LME'">
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutYear,$varFutMonthCode,$varExchange)"/>
												</xsl:when>
												<xsl:otherwise>-->
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutYear,$varFutMonthCode,$varExchange)"/>
													<!--</xsl:otherwise>
											</xsl:choose>-->
												</xsl:when>
											</xsl:choose>
										</xsl:when>

										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="COL29='FUT'">
													<!--<xsl:choose>
												<xsl:when test="COL16='LME'">
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutMonthCode,$varFutYear,$varExchange)"/>
												</xsl:when>
												<xsl:otherwise>-->
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutMonthCode,$varFutYear,$varExchange)"/>
													<!--</xsl:otherwise>
											</xsl:choose>-->
												</xsl:when>
											</xsl:choose>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL3"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</xsl:variable>


						<xsl:variable name="varFutureOptSymbol">
							<xsl:choose>
								<xsl:when test="$PRANA_FLAG!=''">
									<xsl:choose>
										<xsl:when test="COL29='OPT'">
											<!--<xsl:choose>
												<xsl:when test="COL16='LME'">
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutYear,$varFutMonthCode,$varPutCall,$varFutOptStrikePrice,$varExchange)"/>
												</xsl:when>
												<xsl:otherwise>-->
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutYear,$varFutMonthCode,$varPutCall,$varFutOptStrikePrice,$varExchange)"/>
												<!--</xsl:otherwise>
											</xsl:choose>-->
										</xsl:when>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL29='OPT'">
											<!--<xsl:choose>
												<xsl:when test="COL16='LME'">
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutMonthCode,$varFutYear,$varPutCall,$varFutOptStrikePrice,$varExchange)"/>
												</xsl:when>
												<xsl:otherwise>-->
													<xsl:value-of select="concat($varFUTUnderlying,' ',$varFutMonthCode,$varFutYear,$varPutCall,$varFutOptStrikePrice,$varExchange)"/>
												<!--</xsl:otherwise>
											</xsl:choose>-->
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<Symbol>

							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>
										<xsl:when test="COL29='FUT'">
											<xsl:value-of select="$varFutureSymbol"/>
										</xsl:when>

										<xsl:when test="COL29='OPT'">
											<xsl:value-of select="$varFutureOptSymbol"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

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

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varAvgPrice">
							<xsl:choose>
								<xsl:when test="$PRANA_PRICEMULTIPLIER_NAME!=''">
									<xsl:value-of select="number(translate(COL26,',',''))*$PRANA_PRICEMULTIPLIER_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(COL26,',','')"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varSide" select="COL10"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select ="normalize-space(COL12)"/>
						</PBSymbol>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="COL29='FUT'">
									<xsl:choose>
										<xsl:when test ="$PRANA_SYMBOL_NAME !=''">
											<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:otherwise>

											<xsl:choose>
												<xsl:when test="COL29='FUT'">
													<xsl:value-of select="$varFutureSymbol"/>
												</xsl:when>

												<xsl:when test="COL29='OPT'">
													<xsl:value-of select="$varFutureOptSymbol"/>
												</xsl:when>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</TradeAttribute1>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

