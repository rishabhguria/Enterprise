<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template name="Option">
		<xsl:param name="varSymbol"/>


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:value-of select ="substring-before($varSymbol,' ')"/>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),1,2)"/>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),3,2)"/>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),5,2)"/>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),7,1)"/>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceInt">
			<xsl:value-of select ="number(substring(normalize-space(substring-after($varSymbol,' ')),8,5))"/>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceDec">
			<xsl:value-of select ="number(substring(normalize-space(substring-after($varSymbol,' ')),13,3))"/>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="number($varStrikePriceDec)">
					<xsl:value-of select ="format-number(concat($varStrikePriceInt,'.',$varStrikePriceDec),'#.##')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varStrikePriceInt"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<!-- <xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
 -->
		<xsl:value-of select ="concat($varUnderlyingSymbol,' ',$varMonth,'/',$varExDay,'/',$varYear,' ',$varPutCall,$varStrikePrice)"/>

	</xsl:template>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
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

				<xsl:variable name ="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
					<!--<xsl:value-of select="translate(translate(translate(COL4,'(',''),')',''),',','')"/>-->

				</xsl:variable>

				<xsl:if test ="number($Position)">
					<!--contains(COL3,'Cash')!='true' and-->

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Credit suisse'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<!--<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>-->
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>



						<FundName>
							<!--<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="'Test'"/>
						</FundName>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>


						<xsl:variable name="PRANA_SYMBOL_NAME">
							<!--<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>-->
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="VarSymbol">
							<xsl:value-of select="substring-before(normalize-space(COL7),' ')"/>
						</xsl:variable>
						<xsl:variable name="VarMonth">
							<xsl:value-of select="substring(substring-after(normalize-space(COL7),' '),3,2)"/>
						</xsl:variable>
						<xsl:variable name="VarDay">
							<xsl:value-of select="substring(substring-after(normalize-space(COL7),' '),5,2)"/>
						</xsl:variable>
						<xsl:variable name="VarYear">
							<xsl:value-of select="substring(substring-after(normalize-space(COL7),' '),1,2)"/>
						</xsl:variable>
						<xsl:variable name="VarPutCall">
							<xsl:value-of select="substring(substring-after(normalize-space(COL7),' '),7,1)"/>
						</xsl:variable>
						<xsl:variable name="VarStrikePrice">
							<!--<xsl:choose>
					<xsl:when test="contains(substring(substring-after(normalize-space(COL7),'@'),1),'.')">
						<xsl:value-of select="substring-after(normalize-space(COL7),'@')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="format-number(substring-after(normalize-space(COL7),'@'),'###')"/>
					</xsl:otherwise>
				</xsl:choose>-->
							<xsl:value-of select="format-number(substring-after(substring-after(normalize-space(COL6),'/'),' '),'###.###')"/>

						</xsl:variable>





						<!--<xsl:variable name="Symbol">
              <xsl:choose>
                <xsl:when test="string-length(COL5) &gt; 21">
                  <xsl:value-of select="concat($VarSymbol,' ',$VarMonth,'/',$VarDay,'/',$VarYear,' ',$VarPutCall,$VarStrikePrice,' ','US EQUITY')"/>
                </xsl:when>
                <xsl:when test="contains(COL8,'AUD')">
                  <xsl:value-of select="COL5"/>
                </xsl:when>
                <xsl:when test="string-length(COL5) = 9">
                  <xsl:value-of select="COL5"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(COL5,'US EQUITY')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->

						<xsl:variable name="Symbol">
							<xsl:value-of select="concat(COL7,' ','US EQUITY')"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) &gt; 20">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 7">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 9">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>


						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) &gt; 20">
									<xsl:value-of select="concat($VarSymbol,' ',$VarMonth,'/',$VarDay,'/',$VarYear,' ',$VarPutCall,$VarStrikePrice,' ','US EQUITY')"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 7">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 9">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) &gt; 20">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 7">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 9">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) &gt; 20">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 7">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 9">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>


						<Symbology>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) &gt; 20">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 7">
									<xsl:value-of select="'Sedol'"/>
								</xsl:when>
								<xsl:when test="string-length(COL7) = 9">
									<xsl:value-of select="'Cusip'"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbology>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($Position) &gt;0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="number($Position) &lt;0">
									<xsl:value-of select="$Position*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test="COL3='Listed Option'">
									<xsl:choose>
										<xsl:when test="$Position &lt; 0">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:when test="$Position &gt; 0">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Position &lt; 0">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:when test="$Position &gt; 0">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="number($CostBasis) &gt;0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:when test ="number($CostBasis) &lt;0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>
						
						<xsl:variable name="MarkPriceBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPriceBase>
							<xsl:choose>
								<xsl:when test ="number(MarkPriceBasis) &gt;0">
									<xsl:value-of select="MarkPriceBasis"/>
								</xsl:when>
								<xsl:when test ="number(MarkPriceBasis) &lt;0">
									<xsl:value-of select="MarkPriceBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPriceBase>


						<xsl:variable name="TransactionLevy">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test ="number($TransactionLevy) &gt;0">
									<xsl:value-of select="$TransactionLevy"/>
								</xsl:when>
								<xsl:when test ="number($TransactionLevy) &lt;0">
									<xsl:value-of select="$TransactionLevy"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

						<!--<xsl:variable name="VarFX" select="COL9 div  COL15"/>
						<FXRate>
							<xsl:choose>
								<xsl:when test="number($VarFX)">
									<xsl:value-of select="$VarFX"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>-->

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
