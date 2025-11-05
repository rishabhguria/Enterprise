<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template name="tempMetalSymbolCode">
		<xsl:param name="paramMetalSymbol"/>
		<!-- 1 characters for metal code -->
		<!--  e.g. A represents A = aluminium-->
		<xsl:choose>
			<xsl:when test ="$paramMetalSymbol='U S DOLLAR'">
				<xsl:value-of select ="'USD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='UK POUND STERLING'">
				<xsl:value-of select ="'GBP'"/>
			</xsl:when>

			<xsl:when test ="$paramMetalSymbol='EURO'">
				<xsl:value-of select ="'EUR'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='AUSTRALIAN DOLLAR'">
				<xsl:value-of select ="'AUD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='HONG KONG DOLLAR'">
				<xsl:value-of select ="'HKD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='INDONESIAN RUPIAH'">
				<xsl:value-of select ="'IDR'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='JAPANESE YEN'">
				<xsl:value-of select ="'JPY'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='MALAYSIAN RINGGIT'">
				<xsl:value-of select ="'MYR'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='NEW ZEALAND DOLLAR'">
				<xsl:value-of select ="'NZD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='PHILIPPINO PESO'">
				<xsl:value-of select ="'PBP'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='SINGAPORE DOLLAR'">
				<xsl:value-of select ="'SGD'"/>
			</xsl:when>

			<xsl:when test ="$paramMetalSymbol='THAI BAHT'">
				<xsl:value-of select ="'THB'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="COL3"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test="number(COL58) and normalize-space(COL26)='Journal' and normalize-space(COL14) !='Rebalancing'">
				
					<PositionMaster>

						<!--Start Of mandatory columns-->


						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>

								<xsl:when test="COL33='D' and (COL58 &lt; 0)">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>

								<xsl:when test="COL33='C' and (COL58 &gt; 0)">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								 <!-- <xsl:when  test="contains(COL15,'PURCHASE') and COL58 &gt; 0">
										<xsl:value-of select="'MoneyMarket'"/>
									</xsl:when>
									 <xsl:when  test="contains(COL15,'PURCHASE') and COL58 &lt; 0">
										<xsl:value-of select="'MoneyMarket'"/>
									</xsl:when>
									<xsl:when  test="contains(COL15,'REDEMPTION') and COL58 &gt; 0">
										<xsl:value-of select="'MoneyMarket'"/>
									</xsl:when>
									 <xsl:when  test="contains(COL15,'REDEMPTION') and COL58 &lt; 0">
										<xsl:value-of select="'MoneyMarket'"/>
									</xsl:when> -->

								<!--<xsl:when test="((normalize-space(COL4) = 'FEDWT' ) or (normalize-space(COL4) = 'JRL' or normalize-space(COL5) = 'CONTRIBUTIONS')) and (COL6 &gt; 0) ">
									<xsl:value-of select="'CASH-DEP'"/>
								</xsl:when>-->
							</xsl:choose>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
							</xsl:choose>
						</AccountName>

						<xsl:variable name = "amount" >
							<xsl:choose>
								<xsl:when test="number(COL58) &gt; 0">
									<xsl:value-of select="number(COL82)"/>
								</xsl:when>
								<xsl:when test="number(COL58) &lt; 0">
									<xsl:value-of select="number(COL82)*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Date>
							<xsl:value-of select="COL36"/>
						</Date>

						<Description>
							<xsl:value-of select ="normalize-space(COL15)"/>
						</Description>

						<CurrencyName>
							<xsl:value-of select ="COL55"/>
						</CurrencyName>

						<CurrencyID>
							<xsl:value-of select ="'1'"/>
						</CurrencyID>

						<JournalEntries>							
							<xsl:choose>
							    <xsl:when  test="contains(normalize-space(COL14),'Money Market')">
								   <xsl:value-of select="concat('MoneyMarket',':' , $amount , '|MoneyMarket:' , $amount)"/>
								</xsl:when>	
                                <xsl:when test="not(contains(COL15,'REDEMPTION')) and COL58 &gt; 0">
									<xsl:value-of select="concat('Cash:', $amount , '|', $PRANA_ACRONYM_NAME, ':' , $amount)"/>
								</xsl:when>								
								<xsl:when  test="not(contains(COL15,'REDEMPTION')) and COL58 &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $amount , '|Cash:' , $amount)"/>
								</xsl:when>	  																						
							</xsl:choose>
						</JournalEntries>
						
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
