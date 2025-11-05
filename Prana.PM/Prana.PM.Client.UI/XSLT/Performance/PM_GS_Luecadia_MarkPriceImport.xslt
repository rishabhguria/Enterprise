<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:param name="varPutCall"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth='01' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='01' and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12' and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="normalize-space(COL2)!='Fund' and number(COL7) and not(contains(COL4,'RTS/'))">
					<PositionMaster>

						<xsl:variable name = "PB_SymbolTicker_NAME" >
							<xsl:value-of select="(COL4)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SymbolTicker_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_SymbolTicker_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name = "PB_SymbolCurrency_NAME" >
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name = "PB_Currency_NAME" >
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SymbolCurrency_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varMonthNo" select="substring(COL18,9,2)"/>
						<xsl:variable name="varYearNo" select="substring(COL18,7,2)"/>
						<xsl:variable name="varSymbol" select="normalize-space(substring(COL18,1,6))"/>
						<xsl:variable name="varStrikePrice" select="format-number(concat(substring(COL18,14,5),'.',substring(COL18,19)),'#.00')"/>
						<xsl:variable name="varCallPutCode" select="substring(COL18,13,1)"/>
						<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonthNo" />
								<xsl:with-param name="varPutCall" select="$varCallPutCode"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="concat('O:',$varSymbol,' ',$varYearNo,$varMonthCode,$varStrikePrice)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="string-length(COL5) &gt; 20">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL9='USD'">
											<xsl:choose>
												<xsl:when test="$PRANA_SymbolTicker_NAME!=''">
													<xsl:value-of select="$PRANA_SymbolTicker_NAME"/>
												</xsl:when>
												<xsl:when test="COL5!='*'">
													<xsl:value-of select="COL5"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PB_SymbolTicker_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$PRANA_SymbolCurrency_NAME=''">
													<xsl:value-of select="$PB_SymbolCurrency_NAME"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="string-length(COL5) &gt; 20">
									<xsl:value-of select="concat(COL5, 'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="COL10 &gt; 0">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:when test="COL10 &lt; 0">
									<xsl:value-of select="COL10*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<Date>
							<xsl:value-of select="''"/>
						</Date>


					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
