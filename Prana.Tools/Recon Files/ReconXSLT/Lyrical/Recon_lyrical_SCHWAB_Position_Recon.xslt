<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				
				<xsl:variable name = "PB_NAME">
					<xsl:value-of select="'SCHWAB'"/>
				</xsl:variable>
				
				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="COL1"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
				
				<xsl:variable name="varQuantity">
					<xsl:choose>
						<xsl:when test="contains(COL1,'46370395')">
							<xsl:value-of select="COL7"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL7"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:if test ="number($varQuantity) and not(contains(COL3,'CASH')) and not(contains(COL4,'CASH01')) and $PRANA_FUND_NAME!=''">
					<PositionMaster>


						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="normalize-space(COL5)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL5)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="normalize-space(COL3)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL3)"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<Symbol>
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="$Symbol != ''">
									<xsl:value-of select ="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
									<!--<xsl:value-of select="COL4"/>-->
								</xsl:otherwise>

							</xsl:choose>
							<!--<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL3)"/>
								</xsl:otherwise>

							</xsl:choose>-->
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

						<TradeDate>
							<xsl:value-of select ="COL10"/>
						</TradeDate>

						<xsl:variable name ="NetPosition">
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL7"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Quantity>

							<xsl:choose>

								<xsl:when test ="number($NetPosition) ">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>

						<xsl:variable name ="varSide">
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="normalize-space(COL8)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL8)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Side>
							<xsl:choose>
								<xsl:when test="$varSide='L'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varSide='S'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>						

						<xsl:variable name ="MarkPrice">
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="normalize-space(COL9)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL9)"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select ="COL9"/>-->
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>


						<xsl:variable name ="GrossNotionalValue">
							<xsl:choose>
								<xsl:when test="contains(COL1,'46370395')">
									<xsl:value-of select="normalize-space(COL11)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL11)"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select ="number(COL11)"/>-->
						</xsl:variable>

						<MarketValue>

							<xsl:choose>
								<xsl:when test ="number($GrossNotionalValue) ">
									<xsl:value-of select ="$GrossNotionalValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</MarketValue>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
