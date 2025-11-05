<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month, int date)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
		}

		public string AddDay(int year, int month, int date)
		{
		DateTime weekEnd = new DateTime(year, month, date);
		return weekEnd.AddDays(1).ToString();
		}

	</msxsl:script>

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

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition)">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Bank of America Merrill Lynch'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>-->


						<!--<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>-->

						<!--<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL20,'Equities')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->


						<xsl:variable name="Symbol">
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name="ISIN">
							<xsl:value-of select ="COL19"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$ISIN!='*'">
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

						<ISIN>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$ISIN!='*'">
									<xsl:value-of select="$ISIN"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ISIN>


						<Symbology>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>
								<xsl:when test="$ISIN!='*'">
									<xsl:value-of select="'ISIN'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbology>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<!--<xsl:variable name ="PRANA_FUND_NAME">
                            <xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
                       </xsl:variable>-->
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

						<xsl:variable name="Month1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL14,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="Day1">
							<xsl:value-of select="substring(substring-after(substring-after(substring-before(COL14,' '),'-'),'-'),1,2)"/>
						</xsl:variable>

						
						<xsl:variable name="Year1">
							<xsl:value-of select="substring(substring-before(COL14,' '),1,4)"/>
						</xsl:variable>

						
						<PositionStartDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL14),'-'),'-'),'20')">
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL14),'-'),'/',substring-after(substring-after(COL14,'-'),'-'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL14),'-'),'/',20,substring-after(substring-after(COL14,'-'),'-'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionStartDate>

						<xsl:variable name="Month2">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL17,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="Day2">
							<xsl:value-of select="substring(substring-after(substring-after(substring-before(COL17,' '),'-'),'-'),1,2)"/>
						</xsl:variable>

						
						<xsl:variable name="Year2">
							<xsl:value-of select="substring(substring-before(COL17,' '),1,4)"/>
						</xsl:variable>

					
						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL17),'-'),'-'),'20')">
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL17),'-'),'/',substring-after(substring-after(COL17,'-'),'-'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL17),'-'),'/',20,substring-after(substring-after(COL17,'-'),'-'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionSettlementDate>



						<NetPosition>
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>
								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						<xsl:variable name="Side">
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$Side='Buy'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="$Side='Sell'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$Side='Cover Short'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>
								<xsl:when test ="$Side='Sell Short'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>



						<xsl:variable name="PB_Contract" select="COL21"/>

						<xsl:variable name="PRANA_Multiplier">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMapping.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
						</xsl:variable>

						<xsl:variable name="PRANA_MultiplierToExclude">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMappingToExclude.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
						</xsl:variable>

						<xsl:variable name="Price">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="$PRANA_MultiplierToExclude!=''">
									<xsl:value-of select="$Price"/>
								</xsl:when>
								<xsl:when test="$PRANA_Multiplier!=''">
									<xsl:value-of select="format-number($Price * $PRANA_Multiplier,'#.######')"/>
									<!--<xsl:value-of select="$Price * $PRANA_Multiplier"/>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number($Price)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$CostBasis &lt;0">
									<xsl:value-of select ="$CostBasis*-1"/>
								</xsl:when>
								<xsl:when test ="$CostBasis &gt;0">
									<xsl:value-of select ="$CostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<xsl:variable name ="varCommision">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
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


						<xsl:variable name="Multiplier" select="COL8"/>

						<Multiplier>

							<xsl:choose>
								<xsl:when test="$PRANA_MultiplierToExclude!=''">
									<xsl:value-of select="$PRANA_MultiplierToExclude"/>
								</xsl:when>
								<xsl:when test="$PRANA_Multiplier!=''">
									<xsl:value-of select="$Multiplier div $PRANA_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Multiplier"/>
								</xsl:otherwise>
							</xsl:choose>

						</Multiplier>
						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>
						<IsPreferPrimaryExchange>
							<xsl:value-of select ="'true'"/>
						</IsPreferPrimaryExchange>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
