<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			
			<Trailer>
				<xsl:value-of select ="concat('&#x0A;','Dan DeSerio')"/>
				<!--<xsl:value-of select ="'Dan Deserio'"/>-->
			</Trailer>

			<Done>
				<xsl:value-of select ="concat('&#x0A;','212-415-6611')"/>
				<!--<xsl:value-of select ="'1-212-768-3410'"/>-->
			</Done>

			<Email>
				<xsl:value-of select ="concat('&#x0A;','ddeserio@lyricalpartners.com')"/>
				<!--<xsl:value-of select ="'1-212-768-3410'"/>-->
			</Email>
		</ThirdPartyFlatFileFooter>

	</xsl:template>
</xsl:stylesheet>
