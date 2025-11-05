<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="left-trim">
		<xsl:param name="s" />
		<xsl:choose>
			<xsl:when test="substring($s, 1, 1) = ''">
				<xsl:value-of select="$s"/>
			</xsl:when>
			<xsl:when test="normalize-space(substring($s, 1, 1)) = ''">
				<xsl:call-template name="left-trim">
					<xsl:with-param name="s" select="substring($s, 2)" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$s" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="right-trim">
		<xsl:param name="s" />
		<xsl:choose>
			<xsl:when test="substring($s, 1, 1) = ''">
				<xsl:value-of select="$s"/>
			</xsl:when>
			<xsl:when test="normalize-space(substring($s, string-length($s))) = ''">
				<xsl:call-template name="right-trim">
					<xsl:with-param name="s" select="substring($s, 1, string-length($s) - 1)" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$s" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="trim">
		<xsl:param name="s" />
		<xsl:call-template name="right-trim">
			<xsl:with-param name="s">
				<xsl:call-template name="left-trim">
					<xsl:with-param name="s" select="$s" />
				</xsl:call-template>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template name="GetSideMultiplier">
		<xsl:param name="val" />
		<xsl:choose>
			<xsl:when test="$val &lt; 0">
					<xsl:value-of select="-1"/>
			</xsl:when>
			<xsl:otherwise>
					<xsl:value-of select="1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="COL1 !='Account Number' and COL3 != '0'">
					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>

						<FundName>
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</FundName>
						<!--End fundname section-->

						<xsl:variable name = "SIDE_MULTIPLIER" >
							<xsl:call-template name="GetSideMultiplier">
								<xsl:with-param name="val" select="COL3" />
							</xsl:call-template>
						</xsl:variable>
						
						
						<!-- Net position-->
						<NetPosition>
							<xsl:value-of select="COL3 * $SIDE_MULTIPLIER"/>
						</NetPosition>
						<!--End Net position-->
						
						<!--Side-->
						<xsl:choose>
							<xsl:when test="COL2='BUY'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL2='SELL'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>
						<!--End Side-->
						
						
						<!-- Position Date -->
						<PositionStartDate>
							<xsl:value-of select="COL6"/>
						</PositionStartDate>
						<!-- End Position Date -->

						<!-- Average price -->
						<CostBasis>
							<xsl:value-of select="COL8"/>
						</CostBasis>
						<!-- End Average price -->


						<!-- Symbol section-->
						<xsl:variable name = "PB_SYMBOL" >
							<xsl:call-template name="trim">
								<xsl:with-param name="s" select="translate(COL4,'&quot;','')" />
							</xsl:call-template>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
						</Symbol>
						<PBSymbol>
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</PBSymbol>
						<!-- End Symbol section-->
						
						<!-- Commission & Fees-->
						<Commission>
							<xsl:value-of select="COL10 * -1"/>
						</Commission>
						<Fees>
							<xsl:value-of select="(COL13 + COL14) * -1"/>
						</Fees>
						<!-- End Commission & Fees-->
						
						<CounterPartyID>
							<xsl:value-of select ="1"/>
						</CounterPartyID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>