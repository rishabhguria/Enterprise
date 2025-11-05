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

	
	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Lightspeed'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>



						<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="''"/>
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
								<xsl:with-param name="Number" select="COL9"/>
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

						<xsl:variable name ="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL3,'O:')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Side" select="normalize-space(COL7)"/>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test ="$Side='Sell to open'">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:when test ="$Side='Buy to Close'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="$Side='Sell to Close'">
											<xsl:value-of select ="'D'"/>
										</xsl:when>										
										<xsl:when test ="$Side='Buy to Open'">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$Side='Sell'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="$Side='Buy'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
										<xsl:when test ="$Side='Buy to Cover'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="$Side='Buy to Close'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="$Side='Sell short'">
											<xsl:value-of select ="'5'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>				

						</SideTagValue>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
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


	
						<xsl:variable name="Date">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after(COL1,'/'),'/'))='1'">
									<xsl:value-of select="concat(0,substring-before(substring-after(COL1,'/'),'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after(COL1,'/'),'/')"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="Month">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL1,'/'))='1'">
									<xsl:value-of select="concat(0,substring-before(COL1,'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL1,'/')"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</xsl:variable>

						<xsl:variable name="Year">
							<xsl:value-of select="substring-after(substring-after(COL1,'/'),'/')"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="concat($Month,'/',$Date,'/',$Year)"/>
						</PositionStartDate>

						<xsl:variable name="Date1">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after(COL26,'/'),'/'))='1'">
									<xsl:value-of select="concat(0,substring-before(substring-after(COL26,'/'),'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after(COL26,'/'),'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						
						</xsl:variable>

						<xsl:variable name="Month1">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL26,'/'))='1'">
									<xsl:value-of select="concat(0,substring-before(COL26,'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL26,'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Year1">
							<xsl:value-of select="substring-after(substring-after(COL26,'/'),'/')"/>
						</xsl:variable>
						<OriginalPurchaseDate>
							<xsl:value-of select="concat($Month1,'/',$Date1,'/',$Year1)"/>
						</OriginalPurchaseDate>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>