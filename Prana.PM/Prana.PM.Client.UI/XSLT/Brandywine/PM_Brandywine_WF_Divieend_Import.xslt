<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month, int date)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
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
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varDividend">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL11"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="($varDividend &gt; 0.1 or $varDividend &lt; -0.1) ">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'Brandywine'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="COL2"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
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

						<xsl:variable name="Symbol" select="COL2"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>



						<Amount>
							<xsl:choose>
								<xsl:when test="number($varDividend)">
									<xsl:value-of select="-1 * $varDividend"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>


						<xsl:variable name="varEMonth">
							<xsl:choose>
								<xsl:when test="contains(substring(COL6,1,1),'0')">
									<xsl:value-of select="substring(COL6,1,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('0',substring(COL6,1,1))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varEDay">
							<xsl:value-of select="substring(COL6,2,2)"/>
						</xsl:variable>

						<xsl:variable name="varEYear">
							<xsl:value-of select="substring(COL6,6,2)"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:choose>
								<xsl:when test="contains(substring(COL7,1,1),'0')">
									<xsl:value-of select="substring(COL7,1,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('0',substring(COL7,1,1))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL7,2,2)"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL7,6,2)"/>
						</xsl:variable>
						
						
						<PayoutDate>
							<xsl:value-of select="COL6"/>
						</PayoutDate>

						
						<ExDate>
							<xsl:value-of select="COL6"/>
						</ExDate>

						<RecordDate>
							<xsl:value-of select="''"/>
						</RecordDate>


						<CurrencyID>
							<xsl:choose>
								<xsl:when test="COL8='USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL8='HKD'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL8='GBP'">
									<xsl:value-of select="'4'"/>
								</xsl:when>
								<xsl:when test="COL8='JPY'">
									<xsl:value-of select="'3'"/>
								</xsl:when>
								<xsl:when test="COL8='CAD'">
									<xsl:value-of select="'7'"/>
								</xsl:when>
								<xsl:when test="COL8='EUR'">
									<xsl:value-of select="'8'"/>
								</xsl:when>
								<xsl:when test="COL8='SGD'">
									<xsl:value-of select="'10'"/>
								</xsl:when>
								<xsl:when test="COL8='SEK'">
									<xsl:value-of select="'13'"/>
								</xsl:when>
								<xsl:when test="COL8='AUD'">
									<xsl:value-of select="'14'"/>
								</xsl:when>
								<xsl:when test="COL8='KRW'">
									<xsl:value-of select="'16'"/>
								</xsl:when>
								<xsl:when test="COL8='CHF'">
									<xsl:value-of select="'23'"/>
								</xsl:when>
								<xsl:when test="COL8='DKK'">
									<xsl:value-of select="'27'"/>
								</xsl:when>
								<xsl:when test="COL8='NZD'">
									<xsl:value-of select="'36'"/>
								</xsl:when>
								<xsl:when test="COL8='BRL'">
									<xsl:value-of select="'6'"/>
								</xsl:when>
								<xsl:when test="COL8='KRW'">
									<xsl:value-of select="'16'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</CurrencyID>



						<Description>
						
								
						
											<xsl:value-of select="'Dividend Received Adjustment on Pay Date'"/>
								

						
						</Description>

						<ActivityType>
							<xsl:choose>

								<xsl:when test="$varDividend &gt; 0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test ="$varDividend &lt; 0">
									<xsl:value-of select ="'DividendIncome'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ActivityType>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>