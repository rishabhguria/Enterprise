<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


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
					<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:variable name ="varQuantity">
					<xsl:value-of select ="number(COL20)"/>
				</xsl:variable>

				<xsl:if test ="number($varQuantity) and $PRANA_FUND_NAME!='' ">

					<PositionMaster>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_CODE">
							<xsl:value-of select="COL24"/>
						</xsl:variable>

						<xsl:variable name="PRANA_UNDERLYING_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@ExchangeName"/>
						</xsl:variable>

						<xsl:variable name="PRANA_PRICEMULTIPLIER_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@PriceMultiplier"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FLAG">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="varExchange">
							<xsl:choose>
								<xsl:when test="COL21='LME'">
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
							<xsl:value-of select="substring(COL27,4,1)"/>
						</xsl:variable>

						<xsl:variable name="varFutMonth">
							<xsl:value-of select="COL26"/>
						</xsl:variable>

						<xsl:variable name="varFutMonthCode">
							<xsl:call-template name="varFutMonthCode">
								<xsl:with-param name="varFutMonth" select="number($varFutMonth)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="PRANA_STRIKE_MULTIPLIER">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable name="varFutOptStrikePrice">
							<xsl:choose>
								<xsl:when test="$PRANA_STRIKE_MULTIPLIER!=''">
									<xsl:value-of select="number(COL29)*number($PRANA_STRIKE_MULTIPLIER)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(COL29)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:value-of select="COL28"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:choose>

								<xsl:when test="$PRANA_FLAG!=''">
									<xsl:choose>
										<xsl:when test="COL28='*'">
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
										<xsl:when test="COL28='*'">
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
						</xsl:variable>


						<xsl:variable name="varFutureOptSymbol">
							<xsl:choose>
								<xsl:when test="$PRANA_FLAG!=''">
									<xsl:choose>
										<xsl:when test="COL28!='*'">
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
										<xsl:when test="COL28!='*'">
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
										<xsl:when test="COL28='*'">
											<xsl:value-of select="$varFutureSymbol"/>
										</xsl:when>

										<xsl:when test="COL20!='*'">
											<xsl:value-of select="$varFutureOptSymbol"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PRANA_STRATEGY_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$PB_NAME]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
						</xsl:variable>

						<Strategy>
							<xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
						</Strategy>


						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</FundName>

						<xsl:variable name="Months">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL14,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="Day1">
							<xsl:value-of select="substring(substring-after(substring-after(substring-before(COL14,' '),'-'),'-'),1,2)"/>
						</xsl:variable>

						<xsl:variable name="Month1">
							<xsl:value-of select="substring(substring-before(substring-after(substring-before(COL14,' '),'-'),'-'),1,2)"/>
						</xsl:variable>
						<xsl:variable name="Year1">
							<xsl:value-of select="substring(substring-before(COL14,' '),1,4)"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="concat($Months,'/',$Day1,'/',$Year1)"/>
						</PositionStartDate>


						<xsl:variable name="Months1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL16,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="Day2">
							<xsl:value-of select="substring(substring-after(substring-after(substring-before(COL16,' '),'-'),'-'),1,2)"/>
						</xsl:variable>

						<xsl:variable name="Month2">
							<xsl:value-of select="substring(substring-before(substring-after(substring-before(COL16,' '),'-'),'-'),1,2)"/>
						</xsl:variable>

						<xsl:variable name="Year2">
							<xsl:value-of select="substring(substring-before(COL16,' '),1,4)"/>
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:value-of select="concat($Months1,'/',$Day2,'/',$Year2)"/>
						</PositionSettlementDate>

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
									<xsl:value-of select="number(translate(COL30,',',''))*number($PRANA_PRICEMULTIPLIER_NAME)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(translate(COL30,',',''))"/>
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

						<xsl:variable name="varSide" select="COL19"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when  test="$varSide=1">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide=2">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<xsl:variable name="Commission">
							<xsl:value-of select="translate(translate(COL9,',',''),'$','')"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ='$Commission*-1'/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="Fees">
							<xsl:value-of select="translate(translate(COL10,',',''),'$','')"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($Fees) &lt; 0'>
									<xsl:value-of select ='$Fees*-1'/>
								</xsl:when>
								<xsl:when test ='number($Fees) &gt; 0'>
									<xsl:value-of select ='$Fees'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>


						<xsl:variable name="ClearingFee">
							<xsl:value-of select="translate(translate(COL11,',',''),'$','')"/>
						</xsl:variable>

						<ClearingFee>
							<xsl:choose>
								<xsl:when test ='number($ClearingFee) &lt; 0'>
									<xsl:value-of select ='$ClearingFee*-1'/>
								</xsl:when>
								<xsl:when test ='number($ClearingFee) &gt; 0'>
									<xsl:value-of select ='$ClearingFee'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingFee>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

