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


	<xsl:template name="ScientificToNumber">
		<xsl:param name="ScientificN"/>
		<xsl:variable name="vExponent" select="substring-after($ScientificN,'E')"/>
		<xsl:variable name="vMantissa" select="substring-before($ScientificN,'E')"/>
		<xsl:variable name="vFactor"
				 select="substring('100000000000000000000000000000000000000000000',
                              1, substring($vExponent,2) + 1)"/>
		<xsl:choose>
			<xsl:when test="starts-with($vExponent,'-')">
				<xsl:value-of select="$vMantissa div $vFactor"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$vMantissa * $vFactor"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
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


	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL11"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME">
							<xsl:value-of select="substring-after(COL5,' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="VarTrans">
							<xsl:choose>
								<xsl:when test="contains(COL5,'/')">
									<xsl:value-of select="translate(substring-before(COL5,' '),'/','.')"/>
								</xsl:when>
							</xsl:choose>
							
						</xsl:variable>
						
						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL5,'/')">
									<xsl:value-of select="concat($VarTrans,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>
								<xsl:when test="contains(COL5,' ')">
									<xsl:value-of select="concat(substring-before(COL5,' '),$PRANA_SUFFIX_NAME)"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


					

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test="contains(COL4,'Bond')">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL2"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>

								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>

								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>

								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>



						<xsl:variable name="SettlCurrFxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="SecFee">
							<xsl:choose>
								<xsl:when test="contains(SettlCurrFxRate,'E')">
									<xsl:call-template name="ScientificToNumber">
										<xsl:with-param name="ScientificN" select="$SettlCurrFxRate"/>
									</xsl:call-template>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						
						<SecFee>
							<xsl:choose>
								<xsl:when test="$SettlCurrFxRate &gt; 0">
									<xsl:value-of select="$SettlCurrFxRate"/>

								</xsl:when>
								<xsl:when test="$SettlCurrFxRate &lt; 0">
									<xsl:value-of select="$SettlCurrFxRate * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</SecFee>


						<xsl:variable name="Side" select="normalize-space(COL8)"/>
						<SideTagValue>

							<xsl:choose>
								<xsl:when test ="$Side='Buy'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="$Side='Sell'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$Side='Short'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:when test ="$Side='BC'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="Date">
							<xsl:value-of select="COL9"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="$Date"/>
						</PositionStartDate>

						<CurrencySymbol>
							<xsl:value-of select="COL17"/>
						</CurrencySymbol>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>