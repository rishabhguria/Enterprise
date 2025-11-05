<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<Trailer>
				<xsl:value-of select="'FOOTER'"/>
			</Trailer>


			<RecordCount>
				<xsl:value-of select ="RecordCount"/>
			</RecordCount>

			<!--<NetNO>
				<xsl:value-of select ="InternalNetNotional"/>
			</NetNO>-->
			
			<Done>
				<xsl:value-of select="'DONE'"/>
			</Done>

		
		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>