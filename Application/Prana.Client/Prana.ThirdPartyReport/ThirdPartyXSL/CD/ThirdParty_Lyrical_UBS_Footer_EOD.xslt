<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Space">
		<xsl:param name="number"/>
		<xsl:if test="$number &gt;0">
			<xsl:variable name="blank" select="''"/>
			<xsl:value-of select="concat($blank,' ')"/>
			<xsl:call-template name="Space">
				<xsl:with-param name="number" select="$number - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<xsl:variable name="varCount">
				<xsl:value-of select ="format-number(RecordCount,'0000000')"/>
			</xsl:variable>

			<xsl:variable name="totalQty">
				<xsl:value-of select ="format-number(TotalQty,'000000000000000000')"/>
			</xsl:variable>


			<xsl:variable name="Filler">
				<xsl:call-template name="Space">
					<xsl:with-param name="number" select="968"/>
				</xsl:call-template>
			</xsl:variable>

			<Trailer>
				<xsl:value-of select ="concat('TRAILER',$varCount,TotalQty,$Filler)"/>
			</Trailer>			

		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>
