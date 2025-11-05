<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">D:\NirvanaCode\SourceCode\Dev\Prana_CA\Application\Prana.Client\Prana\bin\Debug\MappingFiles\PranaXSD\OptionModelInputs.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL2"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="$Position">
					<PositionMaster>
						<!--  Symbol Region -->
						<!--<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>-->

						<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>

						<Volatility>
							<xsl:choose>
								<xsl:when test="number($Position)">
									<xsl:value-of select="number($Position)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Volatility>

						<VolatilityUsed>
							<xsl:value-of select="'1'"/>
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

						

						<LastPrice>
							<xsl:value-of select="0"/>
						</LastPrice>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>

