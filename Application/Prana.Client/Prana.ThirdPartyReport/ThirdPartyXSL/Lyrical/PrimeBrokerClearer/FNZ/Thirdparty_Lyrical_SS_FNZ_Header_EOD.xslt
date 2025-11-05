<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<Header>
				<!--<xsl:value-of select ="concat('&#x0A;&#x0A;','Lyrical Asset Management')"/>-->
				<xsl:value-of select ="'Lyrical Asset Management'"/>
			</Header>

			<Done>
				<xsl:value-of select="'&#x0A;'"/>
			</Done>
			<Done1>
				<xsl:value-of select="'&#x0A;'"/>
			</Done1>

			<ClientCode>
				<xsl:value-of select ="'Transmit via Secure Email - contact ops@lyricalpartners.com with any issues'"/>
			</ClientCode>

		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
