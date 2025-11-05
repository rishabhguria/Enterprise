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

				<xsl:if test="number($Cash) and (COL10='WIRE TRANSFER' or COL9='INTB' or COL9='INT' or COL10='CERTIFICATE FEE' or COL10='BOND INTEREST' or COL10='SBC' or COL10='SERVICE CHARGE' or COL10='DEL SEC/MONEY') and (COL6!='3E720012' and COL6!='3E720006' and COL6!='3E720064'and COL6!='3E720018' and COL6!='3E720075' and COL6!='3E710413' and COL6!='3E710414' and COL6!='3E720019' and COL6!='3E720025' and COL6!='3E720038' and COL6!='3E720039' and COL6!='3E720063' and COL6!='3E720069' and COL6!='3E720070' and COL6!='3E720076' and COL6!='3E720077' and COL6!='3E720078' and COL6!='3E720010' and COL6!='Account Number')">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BAML'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL10)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL6)"/>
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
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</Date>

						<xsl:variable name="CurrencyName">
							<xsl:value-of select="COL2"/>
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
								<xsl:when test="COL9='INTB'">
									<xsl:value-of select="'Interest_Income'"/>
								</xsl:when>
								<xsl:when test="COL9='INT'">
									<xsl:value-of select="'Interest_Income'"/>
								</xsl:when>
								<xsl:when test="COL10='BOND INTEREST'">
									<xsl:value-of select="'Interest_Income'"/>
								</xsl:when>
								<xsl:when test="COL10='CERTIFICATE FEE'">
									<xsl:value-of select="'Misc Fees'"/>
								</xsl:when>
								<xsl:when test="COL10='SBC'">
									<xsl:value-of select="'Misc Fees'"/>
								</xsl:when>
								<xsl:when test="COL10='SERVICE CHARGE'">
									<xsl:value-of select="'Misc Fees'"/>
								</xsl:when>
								<xsl:when test="COL10='DEL SEC/MONEY'">
									<xsl:value-of select="'Misc Fees'"/>
								</xsl:when>																							
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Cash &gt; 0">
											<xsl:value-of select="'CashTransfer'"/>
										</xsl:when>
										<xsl:when test="$Cash &lt; 0 ">
											<xsl:value-of select="'CashTransfer'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>						
								
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

						<xsl:variable name="Description" select="COL10"/>
						
						<Description>
							<xsl:choose>
								<xsl:when test="COL9='INTB'">
									<xsl:value-of select="'Margin Interest'"/>
								</xsl:when>
								<xsl:when test="COL10='BOND INTEREST'">
									<xsl:value-of select="'Margin Interest'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Description"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>
						
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>