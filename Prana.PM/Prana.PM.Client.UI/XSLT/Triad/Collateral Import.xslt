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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash) and COL19='COLLATERAL DELV TO  '">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fidelity'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL19)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>
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


						
						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL3,7,2)"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL3,5,2)"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL3,1,4)"/>
						</xsl:variable>
						
						<Date>
							<xsl:value-of select="''"/>
							<!--<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>-->
						</Date>

						<xsl:variable name="CurrencyName">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<CurrencyName>
							<xsl:value-of select="'USD'"/>
						</CurrencyName>


						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="$Cash*-1"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varAbsCash">
							<xsl:value-of select="$AbsCash div 100000"/>
						</xsl:variable>

						<xsl:variable name="varSighn">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="COL19='COLLATERAL DELV TO  '">
									<xsl:value-of select="'Collateral'"/>
								</xsl:when>
								<xsl:when test="$varSighn ='+'">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="$varSighn ='-'">
									<xsl:value-of select="'CASH_WDL'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$varSighn ='+'">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $varAbsCash, '|Cash:',$varAbsCash)"/>
								</xsl:when>
								<xsl:when test="$varSighn ='-'">
									<xsl:value-of select="concat( 'Cash:',$varAbsCash,'|', $PRANA_ACRONYM_NAME,':' , $varAbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="Description" select="COL19"/>
						<Description>
							<xsl:value-of select="$Description"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>