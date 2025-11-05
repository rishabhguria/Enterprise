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

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL15"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
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
							<xsl:value-of select="substring-before(COL4,'-')"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:call-template name="Date">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL4,'-'),'-')"/>
							</xsl:call-template>							
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring-after(substring-after(COL4,'-'),'-')"/>
						</xsl:variable>

						<Date>							
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</Date>

						<xsl:variable name="CurrencyName">
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						<CurrencyName>
							<xsl:value-of select="$CurrencyName"/>
						</CurrencyName>


						<xsl:variable name="varAbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="$Cash*-1"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="'Interest_Receivable'"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="'Interest_Payable'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						
						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' ,$varAbsCash ,'|Interest_Income:',$varAbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat('|Interest_Expense:', $varAbsCash,'|',$PRANA_ACRONYM_NAME,':' , $varAbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="Description" select="COL2"/>
						<Description>
							<xsl:value-of select="$Description"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>





