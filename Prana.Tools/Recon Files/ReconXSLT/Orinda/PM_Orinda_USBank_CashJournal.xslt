<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Cash">
					<xsl:value-of select="COL34"/>
				</xsl:variable>
				<xsl:if test="number($Cash)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'WF'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="Symbol" select="COL8"/>
						<xsl:variable name="PB_FUND_NAME" select="COL1"/>
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
						<xsl:variable name="Year" select="substring(COL13,1,4)"/>
						<xsl:variable name="Month" select="substring(COL13,5,2)"/>
						<xsl:variable name="Day" select="substring(COL13,8,2)"/>
						<Date>
							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
						</Date>
						<xsl:variable name="CurrencyName">
							<xsl:value-of select="COL48"/>
						</xsl:variable>
						<CurrencyName>
							<xsl:value-of select="'USD'"/>
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
								<xsl:when test="COL179!='' and $Cash &gt; 0 ">
									<xsl:value-of select="'Interest_Income'"/>
								</xsl:when>	
								<xsl:when test="COL179!='' and $Cash &lt; 0 ">
									<xsl:value-of select="'Interest_Expense'"/>
								</xsl:when>
								<xsl:when test="COL4='Transfer' and $Cash &gt; 0 ">
									<xsl:value-of select="'CashTransfer_In'"/>
								</xsl:when>
								<xsl:when test="COL4='Transfer' and $Cash &lt; 0 ">
									<xsl:value-of select="'CashTransfer_Out'"/>
								</xsl:when>
								<xsl:when test="COL4!='Transfer' and $Cash &gt; 0 ">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="COL4!='Transfer' and $Cash &lt; 0 ">
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
					</PositionMaster>
				</xsl:if>
		</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>