<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:value-of select="COL14"/>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'WF'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL22"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="Symbol" select="COL8"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test="COL19!='' and COL9='0'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
							
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						
						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL19!='' and COL9='0'">
									<xsl:value-of select="COL19"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<xsl:variable name="PB_FUND_NAME" select="COL3"/>

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
						<xsl:variable name="Year" select="substring(COL5,1,4)"/>
						<xsl:variable name="Month" select="substring(COL5,5,2)"/>
						<xsl:variable name="Day" select="substring(COL5,8,2)"/>



						<Date>

							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>

						</Date>



						<xsl:variable name="CurrencyName">
							<xsl:value-of select="COL"/>
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

								<xsl:when test="COL19='' and COL7='Expense' and $Cash &gt; 0 ">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="COL19='' and COL7='Income' and $Cash &lt; 0 ">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								
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

						<xsl:variable name="Description" select="COL22"/>

						<Description>
							<xsl:value-of select="$Description"/>

						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>