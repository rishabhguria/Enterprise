<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">D:\NirvanaCode\SourceCode\Dev\Prana_CA\Application\Prana.Client\Prana\bin\Debug\MappingFiles\PranaXSD\OptionModelInputs.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 != 'Symbol'">
					<PositionMaster>
						<!--  Symbol Region -->
						<!--<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>-->

						<CODEDISPLAY>
							<xsl:value-of select ="''"/>
						</CODEDISPLAY>
						
						<PBSymbol>
							<xsl:value-of  select="''"/>
						</PBSymbol>


						<Volatility>
							<xsl:value-of select="0"/>
						</Volatility>



						<VolatilityUsed>
							<xsl:value-of select="'0'"/>
						</VolatilityUsed>


						<IntRateUsed>
							<xsl:value-of select="'0'"/>
						</IntRateUsed>

						<DividendUsed>
							<xsl:value-of select="'0'"/>
						</DividendUsed>

						<DeltaUsed>
							<xsl:value-of select="'0'"/>
						</DeltaUsed>

						<LastPriceUsed>
							<xsl:value-of select="'0'"/>
						</LastPriceUsed>

						<IntRate>
							<xsl:value-of select="0"/>
						</IntRate>

						<Dividend>
							<xsl:value-of select="0"/>
						</Dividend>

						<Delta>
							<xsl:value-of select="0"/>
						</Delta>

						<Symbol>

							<xsl:value-of select="COL1"/>

						</Symbol>

						<LastPrice>
							<xsl:value-of select="0"/>
						</LastPrice>

						<ForwardPoints>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL6))">
									<xsl:value-of select="COL6"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForwardPoints>

						<ForwardPointsUsed>
							<xsl:value-of select="'1'"/>
						</ForwardPointsUsed>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>

