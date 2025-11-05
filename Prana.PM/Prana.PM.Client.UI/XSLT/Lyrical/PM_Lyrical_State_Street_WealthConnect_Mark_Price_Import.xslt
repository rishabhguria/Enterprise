<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position" select="COL3"/>

				<xsl:if test="number($Position) and contains(COL1,'Total')!='true'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Statestreet_WC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="MarkPrice" select="number(COL4)"/>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>

								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<Date>
							<xsl:value-of select="''"/>
						</Date>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>