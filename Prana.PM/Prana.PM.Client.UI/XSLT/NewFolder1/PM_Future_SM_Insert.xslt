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

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL24"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) ">

					<PositionMaster>


						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Demo'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol" select="normalize-space(COL27)"/>

						<TickerSymbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL27!=''">
									<xsl:value-of select="COL27"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</TickerSymbol>

					

						<Multiplier>
							<xsl:value-of select="COL24"/>
						</Multiplier>

						<UnderLyingSymbol>
							<xsl:value-of select="COL26"/>
						</UnderLyingSymbol>


						<ExpirationDate>
							<xsl:value-of select ="COL28"/>
						</ExpirationDate>
						<AUECID>
							<xsl:choose>
								<xsl:when test="contains(COL25,'CBOE')">
									<xsl:value-of select="98"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'CBT')">
									<xsl:value-of select="80"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'CME')">
									<xsl:value-of select="16"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'ICE')">
									<xsl:value-of select="98"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'Moscow')">
									<xsl:value-of select="99"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'NYME')">
									<xsl:value-of select="84"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'EUREX')">
									<xsl:value-of select="97"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'Borsa Italiana')">
									<xsl:value-of select="93"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'MEFF')">
									<xsl:value-of select="101"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'ICEL')">
									<xsl:value-of select="99"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'OSE')">
									<xsl:value-of select="26"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'Nasdaq OMX')">
									<xsl:value-of select="96"/>
								</xsl:when>
								<xsl:when test="contains(COL25,'BMF')">
									<xsl:value-of select="22"/>
								</xsl:when>	
							</xsl:choose>
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
