<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'JPY'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CHF'">
				<xsl:value-of select="'-SWX'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'EUR'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CAD'">
				<xsl:value-of select="'-TC'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'TT'">
				<xsl:value-of select="'-TAI'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
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
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="varEntryCondition">
					<xsl:choose>
						<xsl:when test="COL2='Cash and Equivalents'">
							<xsl:value-of select="0"/>
						</xsl:when>
						<xsl:when test="COL2='Currency'">
							<xsl:value-of select="0"/>
						</xsl:when>
						<xsl:when test="COL2='FX Forward'">
							<xsl:choose>
								<xsl:when test="COL10!=0">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="1"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>

							<xsl:variable name ="varPBName">
								<xsl:value-of select ="'Jefferies'"/>
							</xsl:variable>
							<xsl:variable name ="PB_FUND_NAME">
								<xsl:value-of select ="'Shelter Haven-Jeff Swap'"/>
							</xsl:variable>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>
							<FundName>
								<xsl:value-of select="'Shelter Haven-Jeff Swap'"/>
							</FundName>

							<xsl:variable name ="varCurrency">
								<xsl:value-of select ="COL7"/>
							</xsl:variable>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Jeff'"/>
							</xsl:variable>

							<xsl:variable name = "PB_SYMBOL_NAME" >
								<xsl:value-of select ="COL3"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name ="Asset">
								<xsl:choose>
									<xsl:when test="string-length(COL3) = 21">
										<xsl:value-of select="'Option'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'Equity Swap'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="varSuffix1">
								<xsl:call-template name="GetSuffix">
									<xsl:with-param name="Suffix" select="substring-after(COL3,'/')"/>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="varsymbol">
								<xsl:value-of select="substring-before(COL3,'/')"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="COL12 = 'Swap'">
										<xsl:value-of select="concat($varsymbol,$varSuffix1,'/SWAP')"/>
									</xsl:when>


									<xsl:otherwise>
										<xsl:value-of select="COL3"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<PBSymbol>
								<xsl:value-of select="COL3"/>
							</PBSymbol>

							<CurrencySymbol>
								<xsl:value-of select="COL4"/>
							</CurrencySymbol>

							<xsl:choose>
								<xsl:when test ="boolean(number(COL5))">
									<MarkPrice>
										<xsl:value-of select="COL5"/>
									</MarkPrice>
								</xsl:when>
								<xsl:otherwise>
									<MarkPrice>
										<xsl:value-of select="0"/>
									</MarkPrice>
								</xsl:otherwise>
							</xsl:choose>
							<Quantity>
								<xsl:choose>
									<xsl:when test="COL2 = 'SHORT'">
										<xsl:value-of select="$Quantity * -1"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$Quantity"/>
									</xsl:otherwise>
								</xsl:choose>

							</Quantity>

							<Side>
								<xsl:choose>
									<xsl:when test="COL2 = 'SHORT'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="COL2 = 'LONG'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>


						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>


							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>
							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>
