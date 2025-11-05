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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
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

				<xsl:variable name="varCashLocal">
					<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|')='USD Cash'"/>
				</xsl:variable>


				<!--<xsl:variable name="varPosition">
					<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|')"/>
				</xsl:variable>-->

				<xsl:variable name="varMarketValue">
					<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|')"/>
				</xsl:variable>

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$varMarketValue"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash) and contains(COL1,'Cash')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Stifel'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="varSymbol">
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|')"/>
						</xsl:variable>-->


						<xsl:variable name="varSymbol">
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|')"/>
						</xsl:variable>


						<xsl:variable name="Cusip">
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|'),'|')"/>
						</xsl:variable>


						<xsl:variable name="Symbol">
							<xsl:value-of select="$varSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select="'USD'"/>

							<!--<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								--><!--<xsl:when test="$Cusip!=''">
									<xsl:value-of select="''"/>
								</xsl:when>--><!--

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>-->

						</Symbol>
						<xsl:variable name="PB_FUND_NAME" select="substring-before(COL1,'|')"/>

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
							<!--<xsl:value-of select="'FUND1'"/>-->
						</AccountName>

						<TradeDate>
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL1),'|'),'|'),'|'),'|'),'|'),'|'),'|'),'|')"/>
						</TradeDate>



						<!--<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>-->

						<EndingQuantity>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingQuantity>

						<Currency>
							<xsl:value-of select="'USD'"/>
						</Currency>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>