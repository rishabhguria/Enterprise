<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>
	
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<xsl:variable name="varRecordCount">
				<xsl:value-of select="RecordCount"/>
			</xsl:variable>

			<xsl:variable name ="varAccountNAVSpaces">
				<xsl:call-template name="spaces">
					<xsl:with-param name="count" select="(4) - string-length($varRecordCount)" />
				</xsl:call-template>
				
			</xsl:variable>
			<AllData>
				<xsl:value-of select="concat('09END','   ',$varAccountNAVSpaces,$varRecordCount)"/>
			</AllData>

		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>