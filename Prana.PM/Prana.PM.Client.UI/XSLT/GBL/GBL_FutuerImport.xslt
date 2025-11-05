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

	

	<xsl:template name="MonthCodeVar">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Future">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL2,'FUTURE')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL6),' '),' '),' ')"/>
			</xsl:variable>

			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(COL6,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(COL6,' '),2,1)"/>
			</xsl:variable>

			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodeVar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:value-of select="concat($UnderlyingSymbol,' ',$MonthCode,$ExpiryYear)"/>
		</xsl:if>
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

				<xsl:if test="number($NetPosition) and contains(COL2,'FUTURE')">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>
						

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL6"/>
						</xsl:variable>



						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>




						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL2,'FUTURE')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>

								
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="Symbol">
							<xsl:value-of select="COL35"/>
						</xsl:variable>



						<xsl:variable name="Prana_ExchangeSymbol">
							<xsl:value-of select="document('../ReconMappingXML/ExchangeSymbol.xml')/ExchangeSymbol/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PB_SYMBOL_NAME]/@PBExchangeName"/>

						</xsl:variable>
						<xsl:variable name="Kuldeep">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:call-template name ="Future">
										<xsl:with-param name="Symbol" select="COL35"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

					

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

							
							
								<xsl:otherwise>
									<xsl:value-of select="concat($Kuldeep,$Prana_ExchangeSymbol)"/>
								</xsl:otherwise>


							</xsl:choose>
						</Symbol>



						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

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
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>







						<CostBasis>
							<xsl:value-of select="0"/>
						</CostBasis>


						<CurrencySymbol>
							<xsl:value-of select ="''"/>
						</CurrencySymbol>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<PositionStartDate>

							<xsl:value-of select="COL3"/>
						</PositionStartDate>

						
						<Commission>
							<xsl:value-of select="0"/>
						</Commission>



						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL6='BUY'">
									<xsl:value-of select="'1'"/>
									
								</xsl:when>
								<xsl:when test="COL6='SELL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
							</xsl:choose>
						</SideTagValue>





					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>