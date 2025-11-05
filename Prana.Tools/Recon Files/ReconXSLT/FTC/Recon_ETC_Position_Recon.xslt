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


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ETC'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol" select="normalize-space(COL2)"/>



						<xsl:variable name="varAsset">
							<xsl:choose>
								<xsl:when test="COL4='OPTION' and string-length(COL2) &gt; 20">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL4='EQUITY'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$varAsset ='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>

									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$varAsset ='EquityOption'">
									<xsl:value-of select="concat(COL2,'U')"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>

									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<xsl:variable name="PB_FUND_NAME" select="COL1"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


						<xsl:variable name="Quantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>
						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($Quantity)">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>



						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>

							<xsl:choose>
								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>

								</xsl:when>
								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValue>

						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>

							<xsl:choose>
								<xsl:when test="$MarketValueBase &gt; 0">
									<xsl:value-of select="$MarketValueBase"/>

								</xsl:when>
								<xsl:when test="$MarketValueBase &lt; 0">
									<xsl:value-of select="$MarketValueBase * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValueBase>

						<xsl:variable name="varSide">
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						<Side>
							<xsl:choose>
								<xsl:when test="$varAsset ='EquityOption'">
									<xsl:choose>
										<xsl:when  test="$varSide ='L'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when  test="$varSide ='S'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>										
									</xsl:choose>	
									
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$varSide ='L'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when  test="$varSide ='S'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						
						</Side>


						<Date>
							<xsl:value-of select ="''"/>
						</Date>


						<xsl:variable name="CurrencySymbol">
							<xsl:value-of select="COL8"/>
						</xsl:variable>
						<Currency>
							<xsl:value-of select="$CurrencySymbol"/>
						</Currency>


						
						<!--<SettlementDate>
							<xsl:value-of select ="concat($Month1,'/',$Day1,'/',$Year1)"/>
						</SettlementDate>-->
						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

