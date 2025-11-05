<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Cash">
					<xsl:value-of select="(COL35 + COL37 + COL39 + COL41 + COL43 + COL45 + COL47 + COL51)"/>
				</xsl:variable>
				<xsl:if test="number($Cash) and COL1 = 'A'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'WF'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL18"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="normalize-space(COL12)"/>
						</xsl:variable>
						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="MonthsCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="substring(COL14,5,2)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="UnderlyingSymbol">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_ROOT_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Yeer">
							<xsl:value-of select="substring(COL14,4,1)"/>
						</xsl:variable>
						<xsl:variable name="Future">
							<xsl:value-of select="concat($MonthsCode,$Yeer)"/>
						</xsl:variable>

						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL12!=''">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',$Future)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="''"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>

						<xsl:variable name="Year" select="substring(COL8,1,4)"/>
						<xsl:variable name="Month" select="substring(COL8,5,2)"/>
						<xsl:variable name="Day" select="substring(COL8,7,2)"/>
						<Date>
							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
						</Date>
						<xsl:variable name="CurrencyName">
							<xsl:value-of select="COL"/>
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="normalize-space(COL6)"/>
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

								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="'CASH_WDL'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>
				
				      <Description>
						  <xsl:value-of select="COL18"/>
					  </Description>
					
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>