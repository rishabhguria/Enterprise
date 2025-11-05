<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''),'$',''))"/>
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

				<xsl:variable name="varForTestCash" select="normalize-space(COL6)"/>

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL40"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varCurrency" select ="COL23"/>

				<xsl:if test="number($varQuantity) and normalize-space(COL16)='EQ &amp; COMMON STK' and ($varCurrency!='USD')">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL11)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" >
							<xsl:choose>

								<xsl:when test="contains(normalize-space(COL8),' ')">
									<xsl:value-of select="concat(substring-before(normalize-space(COL8),' '),substring-after(normalize-space(COL8),' '))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL8)"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="normalize-space(COL6)" />
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSEDOL=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test ="$Symbol !=''">
									<xsl:value-of select ="$Symbol"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSEDOL!=''">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>
								<xsl:when test ="$Symbol !=''">
									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name="PB_FUND_NAME" select ="normalize-space(COL3)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="varMarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL43"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
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
						</MarketValueBase>

						<xsl:variable name="varMarketValueLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL44"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="$varMarketValueLocal &gt; 0">
									<xsl:value-of select="$varMarketValueLocal"/>
								</xsl:when>
								<xsl:when test="$varMarketValueLocal &lt; 0">
									<xsl:value-of select="$varMarketValueLocal * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="varMarkPriceLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL26"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPriceLocal>
							<xsl:choose>
								<xsl:when test="$varMarkPriceLocal &gt; 0">
									<xsl:value-of select="$varMarkPriceLocal"/>

								</xsl:when>
								<xsl:when test="$varMarkPriceLocal &lt; 0">
									<xsl:value-of select="$varMarkPriceLocal * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPriceLocal>

						<xsl:variable name="varMarkPriceBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL25"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPriceBase &gt; 0">
									<xsl:value-of select="$varMarkPriceBase"/>

								</xsl:when>
								<xsl:when test="$varMarkPriceBase &lt; 0">
									<xsl:value-of select="$varMarkPriceBase * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>


						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>
						
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

						<OriginalPurchaseDate>
							<xsl:value-of select="''"/>
						</OriginalPurchaseDate>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>						

						<CurrencySymbol>
							<xsl:value-of select="$varCurrency"/>
						</CurrencySymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


