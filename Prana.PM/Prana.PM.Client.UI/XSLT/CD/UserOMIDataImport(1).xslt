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
						
						<Bloomberg>
							<xsl:value-of select="COL1"/>
						</Bloomberg>

					
						
						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>

						

						<xsl:variable name="varLastPrice">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL2))">
									<xsl:value-of select="COL2"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>
						
						<LastPrice>
									<xsl:value-of select="$varLastPrice"/>
						</LastPrice>
						<LastPriceUsed>
							<xsl:choose>
								<xsl:when  test="translate(COL19,$varSmall,$varCapital)  = 'FALSE' or $varLastPrice = -2147483648">
									<xsl:value-of select="'0'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose >
						</LastPriceUsed>

						
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
