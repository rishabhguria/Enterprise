<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}

	</msxsl:script>

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
	</xsl:template>


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(substring-after(normalize-space(COL12),' '),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(normalize-space(COL12),'/'),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:choose>
				<xsl:when test="string-length(substring-before(substring-after(substring-after(normalize-space(COL12),' '),' '),'/')) ='1'">
					<xsl:value-of select="concat('0',substring-before(substring-after(substring-after(normalize-space(COL12),' '),' '),'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL12),' '),' '),'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL12),'/'),'/'),' ')"/>
		</xsl:variable>

		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-before(normalize-space(COL12),' '),1,1)"/>
		</xsl:variable>

		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring-after(substring-after(substring-after(normalize-space(COL12),'/'),'/'),' '),'##.00')"/>
		</xsl:variable>
		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth"/>
				<xsl:with-param name="PutOrCall" select="$PutORCall"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL5"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="COL6"/>								
							</xsl:variable>
							
							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name = "varAsset" >
								<xsl:choose>
									<xsl:when test="COL8 ='C' or COL52='P'">
										<xsl:value-of select="'EquityOption'"/>
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
									<xsl:when test="$varAsset='EquityOption'">
										<xsl:call-template name="Option">
											<xsl:with-param name="Symbol" select="COL12"/>
											<xsl:with-param name="Suffix" select="''"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:when test="$varSEDOL = '' or $varSEDOL = '*'">
										<xsl:value-of select="COL6"/>
									</xsl:when>
									
									
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>


							<xsl:variable name="PB_FUND_NAME" select="COL3"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name='State Street']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

							<Side>
								<xsl:choose>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:choose>
										<xsl:when test="number($Quantity) &gt; 0">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="number($Quantity) &lt; 0">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($Quantity) &gt; 0">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="number($Quantity) &lt; 0">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>						
							</xsl:choose>
							</Side>
							<Quantity>
								<xsl:choose>
									<xsl:when test="number($Quantity)">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<xsl:variable name="varMarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>
							<MarketValue>
								<xsl:choose>
									<xsl:when test="$varMarketValue &gt; 0">
										<xsl:value-of select="$varMarketValue"/>
									</xsl:when>
									<xsl:when test="$varMarketValue &lt; 0">
										<xsl:value-of select="$varMarketValue * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>

							<xsl:variable name="varMarketValuebase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL21"/>
								</xsl:call-template>
							</xsl:variable>
							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="$varMarketValuebase &gt; 0">
										<xsl:value-of select="$varMarketValuebase"/>
									</xsl:when>
									<xsl:when test="$varMarketValuebase &lt; 0">
										<xsl:value-of select="$varMarketValuebase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL9"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPrice>
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
							</MarkPrice>
							<xsl:variable name="varMarkPriceB">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPriceBase>
								<xsl:choose>
									<xsl:when test="$varMarkPriceB &gt; 0">
										<xsl:value-of select="$varMarkPriceB"/>
									</xsl:when>
									<xsl:when test="$varMarkPriceB &lt; 0">
										<xsl:value-of select="$varMarkPriceB * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPriceBase>
							
				<xsl:variable name="NetNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="0"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <NetNotionalValue>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalValue &gt; 0">
						  <xsl:value-of select="$NetNotionalValue"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalValue &lt; 0">
						  <xsl:value-of select="$NetNotionalValue * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValue>

			  <xsl:variable name="NetNotionalValueBase">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="0"/>
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

							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</CompanyName>


						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
							
							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>
							
							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>
								
							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

						
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>


			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


