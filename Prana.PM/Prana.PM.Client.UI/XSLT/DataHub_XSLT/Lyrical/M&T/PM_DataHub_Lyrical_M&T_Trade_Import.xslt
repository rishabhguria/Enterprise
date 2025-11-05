<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month='January'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='February'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='March'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='April'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='June'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='July'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month=August">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='September'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='October'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month='November'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month='December'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>



			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="substring(COL1,127,16)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name = "PB_NAME">
					<xsl:value-of select="'MT'"/>
				</xsl:variable>

				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="normalize-space(substring(COL1,1,9))"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test ="number($Position)">

					<PositionMaster>


						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(substring(COL1,241,35))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="string-length(substring(COL1,220,11) &gt;1)">
									<xsl:value-of select ="normalize-space(substring(COL1,220,11))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>




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

						<!--<PositionStartDate>
							<xsl:value-of select ="concat(substring(COL1,663,2),'/',substring(COL1,665,2),'/',substring(COL1,659,4))"/>
						</PositionStartDate>-->
						
						<xsl:variable name="varMarketValue1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(substring(COL1,88,10))"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValue12">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(substring(COL1,103,9))"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValue2">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(substring(COL1,127,16))"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueTotal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="($varMarketValue1 + $varMarketValue12 + $varMarketValue2)"/>
							</xsl:call-template>
						</xsl:variable>

						<NetPosition>
							 <xsl:choose>
								<xsl:when test="normalize-space(substring(COL1,220,11))='WGOXX' or (normalize-space(substring(COL1,220,11))='TOIXX')">
									<xsl:choose>
										<xsl:when test="contains(normalize-space(substring(COL1,1,10)),'31048172')">
											<xsl:value-of select="$varMarketValueTotal"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="number(substring(COL1,127,16))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$Position &lt;0">
											<xsl:value-of select ="$Position*-1"/>
										</xsl:when>

										<xsl:when test ="$Position &gt;0">
											<xsl:value-of select ="$Position"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

            			<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$Position &lt;0">
									<xsl:value-of select ="'5'"/>
								</xsl:when>

								<xsl:when test ="$Position &gt;0">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>



						<xsl:variable name="CostValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(substring(COL1,144,15))"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select="$CostValue div $Position"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,644,15)"/>
							</xsl:call-template>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
							<xsl:when test="normalize-space(substring(COL1,220,11))='WGOXX' or (normalize-space(substring(COL1,220,11))='TOIXX')">
								<xsl:choose>
									<xsl:when test="contains(normalize-space(substring(COL1,1,10)),'31048172')">
											<xsl:value-of select="$varMarketValueTotal"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="number(substring(COL1,127,16))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($varMarketValue)">
											<xsl:value-of select="$varMarketValue"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</StampDuty>

						<xsl:variable name="varMarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$varMarketValueBase &gt; 0">
									<xsl:value-of select="$varMarketValueBase"/>
								</xsl:when>
								<xsl:when test="$varMarketValueBase &lt; 0">
									<xsl:value-of select="$varMarketValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name ="MArkPrice">
							<xsl:value-of select="$varMarketValue div $Position"/>
						</xsl:variable>

						<OrfFee>
							<xsl:choose>
								<xsl:when test="$MArkPrice &gt; 0">
									<xsl:value-of select="$MArkPrice"/>
								</xsl:when>
								<xsl:when test="$MArkPrice &lt; 0">
									<xsl:value-of select="$MArkPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>

						<xsl:variable name="varUnRealized">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>
						<TransactionLevy>
							<xsl:choose>
								<xsl:when test="$varUnRealized &gt; 0">
									<xsl:value-of select="$varUnRealized"/>
								</xsl:when>
								<xsl:when test="$varUnRealized &lt; 0">
									<xsl:value-of select="$varUnRealized * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>

						<xsl:variable name="varUnRealizedBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>
						<TaxOnCommissions>
							<xsl:choose>
								<xsl:when test="number($varUnRealizedBase)">
									<xsl:value-of select="$varUnRealizedBase"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TaxOnCommissions>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
