<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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

	<xsl:template name="Date">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="01"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="02"/>
			</xsl:when>
			<xsl:when test="$Month='Mar'">
				<xsl:value-of select="03"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="04"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="05"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="06"/>
			</xsl:when>
			<xsl:when test="$Month='Jul'">
				<xsl:value-of select="07"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="08"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="09"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varDividend">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL16"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varDividend)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'MapleRock'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="COL7"/>

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

						<xsl:variable name="Symbol" select="COL6"/>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test ="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>



						<Amount>
							<xsl:choose>
								<xsl:when test="number($varDividend)">
									<xsl:value-of select="$varDividend"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>


						<!--<xsl:variable name="varPMonth">
							<xsl:call-template name="Date">
								<xsl:with-param name="Month" select="substring-"
							</xsl:call-template>
						</xsl:variable>-->

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

						<xsl:variable name="varPDay">
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL7,6,2)"/>
						</xsl:variable>
						<PayoutDate>
							<xsl:value-of select="$varPDay"/>
						</PayoutDate>


						<xsl:variable name="varEPDay">
							<xsl:value-of select="COL8"/>
						</xsl:variable>
						<ExDate>
							<xsl:value-of select="$varEPDay"/>
						</ExDate>

						<xsl:variable name="varRPDay">
							<xsl:value-of select="COL9"/>
						</xsl:variable>
						<RecordDate>
							<xsl:value-of select="$varRPDay"/>
						</RecordDate>


						<xsl:variable name="varCurrency">
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<Currency>
							<xsl:value-of select="'USD'"/>
						</Currency>



						<Description>
							<xsl:choose>

								<xsl:when test="$varDividend &gt; 0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>
								<xsl:when test ="$varDividend &lt; 0">
									<xsl:value-of select ="'Dividend Charged'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Description>

						<ActivityType>
							<xsl:choose>

								<xsl:when test="$varDividend &gt; 0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test ="$varDividend &lt; 0">
									<xsl:value-of select ="'DividendExpense'"/>
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