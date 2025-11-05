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




	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="MonthCodevar">
		<xsl:param name="Month"/>
		<xsl:param name="varPutCall"/>
		<xsl:if test="$varPutCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='Jan'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='Feb'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='Mar'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='Apr'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='May'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='Jun'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='Jul' ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='Aug'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='Sep'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='Oct'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='Nov'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='Dec'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$varPutCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='Jan'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='Feb'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='Mar'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='Apr'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='May'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='Jun'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='Jul'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='Aug'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='Sep'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='Oct'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='Nov'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='Dec'">
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
		<xsl:if test="contains(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),' '),1,1),'P') or contains(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),' '),1,1),'C')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-before(normalize-space(COL5),' '),'.')"/>
				<!--<xsl:value-of select="substring-before(normalize-space(COL10),'.')"/>-->
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),1,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),3,3)"/>

			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),6,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space(COL10),' '),' '),' '),' ')"/>-->
				<xsl:value-of select="format-number(substring-before(substring-after(normalize-space(COL5),' '),' '),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodevar">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
					<xsl:with-param name="varPutCall" select="$PutORCall"/>
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
			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>-->
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL9)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>-->

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL3,'Option')">
								<!--<xsl:when test="contains(substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),' '),' '),1,1),'P') or contains(substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),' '),' '),1,1),'C')">-->
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="Symbol">
							<xsl:value-of select="substring-before(normalize-space(COL10),'.')"/>
							
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
									<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name ="Option">
										<xsl:with-param name="Symbol" select="substring-before(normalize-space(COL10),'.')"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								
								<xsl:when test ="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="COL1"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						
						
						<xsl:variable name="Side" select="COL4"/>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<!--<xsl:when test="$Side='S'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>-->
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<!--<xsl:when test="$Side='S'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>-->
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>

						<xsl:variable name="Costbasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL22"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$Costbasis &gt; 0">
									<xsl:value-of select="$Costbasis"/>
								</xsl:when>
								<xsl:when test="$Costbasis &lt; 0">
									<xsl:value-of select="$Costbasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select="COL14"/>
						</PositionStartDate>
						<PBSymbol>

							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>