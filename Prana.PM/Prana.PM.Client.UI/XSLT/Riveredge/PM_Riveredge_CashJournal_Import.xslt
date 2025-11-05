<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$varMonth='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$varMonth='Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
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

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'PFR'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL45"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="varDay" select="COL3"/>					

						<Date>
							<xsl:value-of select="$varDay"/>
						</Date>


						<xsl:variable name="CurrencyName">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<CurrencyName>
							<xsl:value-of select="$CurrencyName"/>
						</CurrencyName>


						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Cash * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="COL71='GLOBAL - CLEARANCE CHARGES'">
									<xsl:value-of select="'Misc_Fees'"/>
								</xsl:when>
								
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="'CASH_WDL'"/>
								</xsl:when>
								
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat('Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>


						<xsl:variable name="varDescription" select="COL71"/>

						<Description>
							<xsl:value-of select="$varDescription"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>