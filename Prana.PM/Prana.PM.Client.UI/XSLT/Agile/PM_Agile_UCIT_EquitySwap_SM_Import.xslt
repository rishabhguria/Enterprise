<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:if test="COL4">

					<PositionMaster>


						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL11)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="varSymbol">
							  <xsl:value-of select="COL16"/>
						</xsl:variable>


						<TickerSymbol>
							<xsl:value-of select="$varSymbol"/>
						</TickerSymbol>


						<Multiplier>
							<xsl:value-of select="'1'"/>
						</Multiplier>


						<xsl:variable name="varUnderlyingSymbol">
							<xsl:value-of select="COL10"/>
						</xsl:variable>
						
						<UnderLyingSymbol>
							<xsl:value-of select="$varUnderlyingSymbol"/>
						</UnderLyingSymbol>

						
						<CusipSymbol>
							<xsl:choose>
								<xsl:when test="COL12='USD'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CusipSymbol>

						<SedolSymbol>
							<xsl:choose>
								<xsl:when test="COL12='USD'">
									<xsl:value-of select="COL6"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SedolSymbol>

						<ISINSymbol>
							<xsl:choose>
								<xsl:when test="COL12='USD'">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ISINSymbol>

						<BloombergSymbol>
							<xsl:value-of select="COL9"/>
						</BloombergSymbol>
						
						<AUECID>
							<xsl:value-of select="1"/>
						</AUECID>

						<LongName>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</LongName>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
