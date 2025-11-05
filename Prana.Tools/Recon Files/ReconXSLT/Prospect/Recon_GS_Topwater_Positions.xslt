<?xml version="1.0" encoding="utf-8" ?>
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
			<xsl:for-each select="//Comparision">
				<xsl:if test="normalize-space(COL2)!='Fund' and number(COL7)">
					<PositionMaster>
						<!--   Fund -->
						<PositionStartDate>
							<xsl:value-of select="COL7"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Agile']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Agile']/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="COL2"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varExDay" select ="substring(COL17,11,2)"/>
						<xsl:variable name="varExpiryDay">
							<xsl:choose>
								<xsl:when test="substring($varExDay,1,1)= '0'">
									<xsl:value-of select="substring($varExDay,2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varExDay"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varMonthNo" select="substring(COL17,9,2)"/>
						<xsl:variable name="varYearNo" select="substring(COL17,7,2)"/>
						<xsl:variable name="varSymbol" select="normalize-space(substring(COL17,1,6))"/>
						<xsl:variable name="varStrikePrice" select="format-number(concat(substring(COL17,14,5),'.',substring(COL17,19)),'#.00')"/>
						<xsl:variable name="varCallPutCode" select="substring(COL17,13,1)"/>
						<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonthNo" />
								<xsl:with-param name="varPutCall" select="$varCallPutCode"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="concat('O:',$varSymbol,' ',$varYearNo,$varMonthCode,$varStrikePrice,'D',$varExpiryDay)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="string-length(COL17) &gt; 20">
									<xsl:value-of select="$varOptionSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_Symbol_NAME!=''">
											<xsl:value-of select="$PRANA_Symbol_NAME"/>
										</xsl:when>
										<xsl:when test="COL9='USD'">
											<xsl:value-of select="COL17"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$PRANA_Symbol_NAME=''">
													<xsl:value-of select="$PB_Symbol_NAME"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_Symbol_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number(COL10)">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<Quantity>
							<xsl:choose>
								<xsl:when test="COL7 &gt; 0">
									<xsl:value-of select="COL7"/>
								</xsl:when>

								<xsl:when test="COL7 &lt; 0">
									<xsl:value-of select="COL7*(1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test="COL7 &gt;0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="COL7 &lt;0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBAssetName></PBAssetName>

						<MarketValue>
							<xsl:value-of select="COL12"/>
						</MarketValue>

						<MarketValueBase>
							<xsl:value-of select="COL13"/>
						</MarketValueBase>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>