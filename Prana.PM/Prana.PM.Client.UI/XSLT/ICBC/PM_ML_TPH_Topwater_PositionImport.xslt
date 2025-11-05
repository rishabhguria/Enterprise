<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL2!='Business Date' and normalize-space(COL2)!='' and number(COL77) and COL61!='Currency' and COL61!='Common Stock'">
					<PositionMaster>
						<!--   Fund -->
						<PositionStartDate>
							<xsl:value-of select="COL2"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select="COL25"/>
						</PBSymbol>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="COL25"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@CompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>


						<Symbol>
							<xsl:choose>
								<xsl:when test="COL56='USD'">
									<xsl:value-of select="COL32"/>
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
						</Symbol>


						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL80)">
									<xsl:value-of select="COL80"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="COL77 &gt; 0">
									<xsl:value-of select="COL77"/>
								</xsl:when>
								<xsl:when test="COL77 &lt; 0">
									<xsl:value-of select="COL77*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL77 &gt; 0 ">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL77 &lt; 0">
									<xsl:value-of select="2"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<Description>
							<xsl:value-of select="COL25"/>
						</Description>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>