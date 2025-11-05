<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>
	
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

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="varQuantity">
					<!--<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL9"/>
					</xsl:call-template>-->
					<xsl:choose>
						<xsl:when test="contains(COL9,'(')">
							<xsl:value-of select="(-1)*substring-before(substring-after(COL9,'('),')')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL9"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<!--and not(contains(COL8,'X'))-->
				<xsl:if test ="number($varQuantity) ">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'CS'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_CODE" >
							<xsl:value-of select ="normalize-space(substring-before(substring-after(normalize-space(COL23),' '),' '))"/>
							
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>


						<xsl:variable name="FXSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL4,'FX') and contains(COL14,'EUR')">
									<xsl:value-of select="concat(COL13,'-','EUR',' ','SPOT')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<Symbol>
							<!--<xsl:value-of select ="$PB_SUFFIX_CODE"/>-->
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test ="$FXSymbol != ''">
									<xsl:value-of select ="$FXSymbol"/>
								</xsl:when>
								<xsl:otherwise>

									<xsl:choose>

										<xsl:when test ="$PRANA_SUFFIX_NAME !=''">
											<xsl:value-of select ="concat(substring-before(COL23,' '),$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="COL23"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>

						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

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

						<PositionStartDate>
							<xsl:choose>
								<xsl:when test="contains(COL4,'FX') and contains(COL14,'EUR')">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL6"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</PositionStartDate>

						<xsl:variable name="Month">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month"  select="substring-before(COL7,'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Date" select="substring-before(substring-after(COL7,'-'),'-')"/>
						<xsl:variable name="Year" select="substring-after(substring-after(COL7,'-'),'-')"/>

						<PositionSettlementDate>
							<xsl:value-of select ="concat($Month,'/',$Date,'/',$Year)"/>
						</PositionSettlementDate>

						<!--<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL9)"/>
						</xsl:variable>-->

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$varQuantity &lt;0">
									<xsl:value-of select ="$varQuantity*-1"/>
								</xsl:when>

								<xsl:when test ="$varQuantity &gt;0">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<!--<xsl:when test ="not(contains(COL8,'X'))">						
							<xsl:choose>-->

								<xsl:when test ="normalize-space(COL8)='BC'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>

								<xsl:when test ="normalize-space(COL8)='XBC'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>


								<xsl:when test ="normalize-space(COL8)='SEL'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL8)='XSL'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL8)='XSS'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>

								<xsl:when test ="normalize-space(COL8)='SS'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>

								<xsl:when test ="normalize-space(COL8)='BUY'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:when test ="normalize-space(COL8)='XBY'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>

							</xsl:choose>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise> 
							</xsl:choose>-->
						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:choose>
								<xsl:when test="number(COL9)">
									<xsl:value-of select ="COL16 div COL9"/>
								</xsl:when>
								
								<xsl:when test="contains(COL4,'FX') and contains(COL14,'EUR')">
									<xsl:value-of select ="COL16 div COL9"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

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

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<!--<xsl:variable name="varFXRate">
							<xsl:value-of select="number(COL20 div COL18)"/>
						</xsl:variable>

						<FXRate>
							<xsl:choose>

								<xsl:when test ="$varFXRate &lt;0">
									<xsl:value-of select ="$varFXRate*-1"/>
								</xsl:when>

								<xsl:when test ="$varFXRate &gt;0">
									<xsl:value-of select ="$varFXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>-->

						<xsl:variable name ="varCommision">
							<xsl:value-of select ="COL27"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>

								<xsl:when test ="$varCommision &lt;0">
									<xsl:value-of select ="$varCommision*-1"/>
								</xsl:when>

								<xsl:when test ="$varCommision &gt;0">
									<xsl:value-of select ="$varCommision"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name ="varFees">
							<xsl:value-of select ="COL228+COL30+COL31+COL32"/>
						</xsl:variable>
						<Fees>
							<xsl:choose>

								<xsl:when test ="$varFees &lt;0">
									<xsl:value-of select ="$varFees*-1"/>
								</xsl:when>

								<xsl:when test ="$varFees &gt;0">
									<xsl:value-of select ="$varFees"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Fees>


						<!--<FXConversionMethodOperator>
							<xsl:value-of select ="'M'"/>
						</FXConversionMethodOperator>-->

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
