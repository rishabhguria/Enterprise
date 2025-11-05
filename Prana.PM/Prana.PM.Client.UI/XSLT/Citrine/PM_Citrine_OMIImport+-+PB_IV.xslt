<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">D:\NirvanaCode\SourceCode\Dev\Prana_CA\Application\Prana.Client\Prana\bin\Debug\MappingFiles\PranaXSD\OptionModelInputs.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 != 'Symbol'">
					<PositionMaster>
						<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>						
						<Volatility>
							<xsl:value-of select="COL2"/>
						</Volatility>
						
						<LastPrice>
							<xsl:value-of select="'0'"/>
						</LastPrice>

						<IntRate>
							<xsl:value-of select="'0'"/>
						</IntRate>
						
						<Dividend>
							<xsl:value-of select="'0'"/>
						</Dividend>

						<Delta>
							<xsl:value-of select="'0'"/>
						</Delta>						
						
						<SharesOutstanding>
							<xsl:value-of select="'0'"/>
						</SharesOutstanding>

						<ForwardPoints>
							<xsl:value-of select="'0'"/>
						</ForwardPoints>
												
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>

