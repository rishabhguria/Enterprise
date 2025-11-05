<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:value-of select="COL9"/>
				</xsl:variable>

				<xsl:if test="number($Cash)and COL27!='CONV BOND'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL45"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_SUFFIX_NAME">
							<xsl:value-of select="substring-after(normalize-space(COL22),' ')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name ="Symbol">
							<xsl:choose>
								
								<xsl:when test="contains(COL22,'/')">
									<xsl:value-of select="substring-before(COL22,'/')"/>
								</xsl:when>
								<xsl:when test="contains(COL22,'-')">
									<xsl:value-of select="substring-before(COL22,'-')"/>
								</xsl:when>
								<xsl:when test="contains(COL22,' ')">
									<xsl:value-of select="substring-before(COL22,' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL22)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						
						
						

						<Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

				  <xsl:when test="$Symbol!=''">
					  <xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
				  </xsl:when>
				  <!--<xsl:when test="$Symbol!=''">
					  <xsl:value-of select="$Symbol"/>
				  </xsl:when>-->


				  <xsl:otherwise>
					<xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

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

						<xsl:variable name="Date" select="COL5"/>

						<Date>
							<xsl:value-of select="$Date"/>
						</Date>

					
						<CurrencyName>
							<xsl:value-of select="COL8"/>
						</CurrencyName>

						<CurrencyID>
							
							<xsl:value-of select="1"/>
						</CurrencyID>

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
								<xsl:when test="COL9 &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="COL9 &gt; 0">
									<xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="Description" select="COL45"/>

						<Description>
							<xsl:value-of select="$Description"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>