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
				<xsl:variable name="MarkPrice">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL27"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($MarkPrice) ">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Interactive Broker'"/>
						</xsl:variable>
						<xsl:variable name = "PB_COMPANY_NAME" >
							<xsl:value-of select="COL8"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name ="Asset">
							<xsl:choose>
								<xsl:when test=" COL6='Option' and string-length(COL6 &gt; 20)">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test=" COL6='Bond'">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="Symbol">
							<xsl:value-of select="COL7"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="concat(COL7,'U')"/>
								</xsl:when>
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>
						<PBSymbol>
							<xsl:value-of select ="$PB_COMPANY_NAME"/>
						</PBSymbol>

						<Date>
							<xsl:value-of select ="concat(substring(COL32,5,2),'/',substring(COL32,7,2),'/',substring(COL32,1,4))"/>
						</Date>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>