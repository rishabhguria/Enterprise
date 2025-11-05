<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
	
					<PositionMaster>
						<!--  Symbol Region -->
						<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL7))">
								<Volatility>
									<xsl:value-of select="COL7"/>
								</Volatility>
						    </xsl:when >
						<xsl:otherwise>
							<Volatility>
								<xsl:value-of select="0"/>
						    </Volatility>
						</xsl:otherwise>
						</xsl:choose >
						
						<xsl:choose>
							<xsl:when  test="COL6!=''">
								<VolatilityUsed>
									<xsl:value-of select="COL6"/>
								</VolatilityUsed>
							</xsl:when >
							<xsl:otherwise>
								<VolatilityUsed>
									<xsl:value-of select="'TRUE'"/>
								</VolatilityUsed>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when  test="COL9!=''">
								<IntRateUsed>
									<xsl:value-of select="COL9"/>
								</IntRateUsed>
							</xsl:when >
							<xsl:otherwise>
								<IntRateUsed>
									<xsl:value-of select="'TRUE'"/>
								</IntRateUsed>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:choose>
							<xsl:when  test="COL12!=''">
								<DividendUsed>
									<xsl:value-of select="COL12"/>
								</DividendUsed>
							</xsl:when >
							<xsl:otherwise>
								<DividendUsed>
									<xsl:value-of select="'TRUE'"/>
								</DividendUsed>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when  test="COL15!=''">
								<DeltaUsed>
									<xsl:value-of select="COL15"/>
								</DeltaUsed>
							</xsl:when >
							<xsl:otherwise>
								<DeltaUsed>
									<xsl:value-of select="'TRUE'"/>
								</DeltaUsed>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when  test="COL18!=''">
								<LastPriceUsed>
									<xsl:value-of select="COL18"/>
								</LastPriceUsed>
							</xsl:when >
							<xsl:otherwise>
								<LastPriceUsed>
									<xsl:value-of select="'TRUE'"/>
								</LastPriceUsed>
							</xsl:otherwise>
						</xsl:choose >
					
						<xsl:choose>
							<xsl:when  test="boolean(number(COL10))">
								<IntRate>
									<xsl:value-of select="COL10"/>
								</IntRate>
							</xsl:when >
							<xsl:otherwise>
								<IntRate>
									<xsl:value-of select="0"/>
								</IntRate>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when  test="boolean(number(COL13))">
								<Dividend>
									<xsl:value-of select="COL13"/>
								</Dividend>
							</xsl:when >
							<xsl:otherwise>
								<Dividend>
									<xsl:value-of select="0"/>
								</Dividend>
							</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when  test="boolean(number(COL16))">
								<Delta>
									<xsl:value-of select="COL16"/>
								</Delta>
							</xsl:when >
							<xsl:otherwise>
								<Delta>
									<xsl:value-of select="0"/>
								</Delta>
						</xsl:otherwise>
						</xsl:choose >
						<xsl:choose>
							<xsl:when  test="boolean(number(COL19))">
								<LastPrice>
									<xsl:value-of select="COL19"/>
								</LastPrice>
							</xsl:when >
							<xsl:otherwise>
								<LastPrice>
									<xsl:value-of select="0"/>
								</LastPrice>
							</xsl:otherwise>
						</xsl:choose >
                        </PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
