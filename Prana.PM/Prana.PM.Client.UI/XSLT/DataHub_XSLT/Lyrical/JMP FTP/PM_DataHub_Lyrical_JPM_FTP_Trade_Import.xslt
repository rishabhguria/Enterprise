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


	<xsl:template name="tempMonthCodeCALL">
		<xsl:param name="paramMonthCodeCALL"/>

		<xsl:choose>
			<xsl:when test ="$paramMonthCodeCALL='01'">
				<xsl:value-of select= "'A'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='02'">
				<xsl:value-of select= "'B'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='03'">
				<xsl:value-of select= "'C'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='04'">
				<xsl:value-of select= "'D'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='05'">
				<xsl:value-of select= "'E'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='06'">
				<xsl:value-of select= "'F'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='07'">
				<xsl:value-of select= "'G'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='08'">
				<xsl:value-of select= "'H'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='09'">
				<xsl:value-of select= "'I'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='10'">
				<xsl:value-of select= "'J'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='11'">
				<xsl:value-of select= "'K'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='12'">
				<xsl:value-of select= "'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select= "' '"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="tempMonthCodePUT">
		<xsl:param name="paramMonthCodePUT"/>

		<xsl:choose>
			<xsl:when test ="$paramMonthCodePUT='01'">
				<xsl:value-of select= "'M'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='02'">
				<xsl:value-of select= "'N'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='03'">
				<xsl:value-of select= "'O'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='04'">
				<xsl:value-of select= "'P'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='05'">
				<xsl:value-of select= "'Q'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='06'">
				<xsl:value-of select= "'R'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='07'">
				<xsl:value-of select= "'S'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='08'">
				<xsl:value-of select= "'T'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='09'">
				<xsl:value-of select= "'U'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='10'">
				<xsl:value-of select= "'V'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='11'">
				<xsl:value-of select= "'W'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='12'">
				<xsl:value-of select= "'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select= "' '"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>


			<!--<xsl:for-each select="//PositionMaster[substring(COL1,40,1) = 'O' or substring(COL1,40,1) = 'C' or substring(COL1,40,1) = 'A' or substring(COL1,40,1) = 'G']">-->
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name ="varPrice">
					<xsl:value-of select ="substring(COL1,235,16)"/>
				</xsl:variable>

				<xsl:variable name ="varPriceInt">
					<xsl:value-of select ="substring($varPrice,1,8)"/>
				</xsl:variable>

				<xsl:variable name ="varPriceFrac">
					<xsl:value-of select ="substring($varPrice,9,8)"/>
				</xsl:variable>

				<xsl:variable name ="varFormatPrice">
					<xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
				</xsl:variable>

				<xsl:variable name ="varNetPosition">

					<xsl:value-of select ="substring(COL1,94,14)"/>
				</xsl:variable>

				<xsl:variable name ="varQtyInt">
					<xsl:value-of select ="substring($varNetPosition,1,10)"/>
				</xsl:variable>


				<xsl:variable name ="varQtyFrac">
					<xsl:value-of select ="substring($varNetPosition,11,4)"/>
				</xsl:variable>

				<xsl:variable name ="varFormatQty">
					<xsl:value-of select="concat($varQtyInt,'.',$varQtyFrac)"/>
				</xsl:variable>


				<xsl:variable name ="varExpiryYear">
					<xsl:value-of select ="substring(COL1,79,2)"/>
				</xsl:variable>
				<xsl:variable name ="varStrikePriceINT">
					<xsl:value-of select ="normalize-space(substring(COL1,83,4))"/>
				</xsl:variable>
				<!--<xsl:variable name ="varStrikeDecimal">
          <xsl:choose>
            <xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 1">
              <xsl:value-of select ="concat($varStrikeDecimalPart,'00')"/>
            </xsl:when>-->
				<xsl:variable name ="varStrikePriceDEC1">
					<xsl:value-of select ="normalize-space(substring(COL1,88,2))"/>
				</xsl:variable>

				<!--<xsl:variable name="varStrikePriceDEC">
          <xsl:choose>
            <xsl:when test="$varStrikePriceDEC= ''"/>
            <xsl:vslue-of select="$varStrikePriceDEC"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:vslue-of select="$varStrikePriceDEC"/>
            </xsl:otherwise>            
          </xsl:choose>
        </xsl:variable>-->
				<xsl:variable name="varStrikePriceDEC">
					<xsl:choose>
						<xsl:when test="$varStrikePriceDEC1=''">
							<xsl:value-of select="'00'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$varStrikePriceDEC1"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				
				<xsl:variable name ="varMonthCodeOption">
					<xsl:choose>
						<xsl:when test ="substring(COL1,61,4)='CALL'">
							<xsl:call-template name="tempMonthCodeCALL">
								<xsl:with-param name="paramMonthCodeCALL" select="substring(COL1,73,2)"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test ="substring(COL1,61,3)='PUT'">
							<xsl:call-template name="tempMonthCodePUT">
								<xsl:with-param name="paramMonthCodePUT" select="substring(COL1,73,2)"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:choose>
						<xsl:when test ="substring(COL1,4,8)!= '-OF-F'">
							<xsl:value-of select="substring(COL1,4,8)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(COL1,7,5)"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JP Morgan']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<!--<xsl:if test="$PRANA_FUND_NAME!=''and ($varFormatPrice >= 0 and $varFormatQty > 0 and (substring(COL1,4,2) ='42')) ">-->
                 <xsl:if test="$PRANA_FUND_NAME!=''and number($varFormatQty)">
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JP Morgan'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="normalize-space(substring(COL1,61,30))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring(COL1,26,8))"/>
						</xsl:variable>
						
						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,61,30))"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
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


						<PositionStartDate>
							<xsl:value-of select ="''"/>
						</PositionStartDate>


						

						<xsl:variable name ="varSide">
							<xsl:value-of select="substring(COL1,109,1)"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varSide='L' and $varSide != '' ">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="$varSide='S' and $varSide != ''">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varSide='L' and number($varFormatQty)">
									<xsl:value-of select="format-number($varFormatQty, '####.00000')"/>
								</xsl:when>
								<xsl:when test="$varSide='S' and number($varFormatQty)">
									<xsl:value-of select="format-number($varFormatQty,'####.00000')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFormatQty"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						
						<xsl:variable name="CostValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select="format-number($varFormatPrice, '####.0000000')"/>
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



						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$CostValue"/>
							</xsl:call-template>
						</xsl:variable>
						<StampDuty>
							<xsl:choose>
								<xsl:when test="number($MarketValue)">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
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




						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select="format-number($varFormatPrice, '####.0000000')"/>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &lt; 0">
									<xsl:value-of select="$varMarkPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>

						<xsl:variable name="varUnRealized">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
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
