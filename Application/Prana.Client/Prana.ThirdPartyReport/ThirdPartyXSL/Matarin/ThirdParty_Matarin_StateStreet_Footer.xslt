<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<Total>
				<xsl:value-of select="'Total'"/>
			</Total>

			<TotalQuantity>
				<xsl:value-of select ="TotalQty"/>
			</TotalQuantity>

			<Blank1>
				<xsl:value-of select="''"/>
			</Blank1>

			<Blank2>
				<xsl:value-of select="''"/>
			</Blank2>

			<Blank3>
				<xsl:value-of select="''"/>
			</Blank3>

			<Blank4>
				<xsl:value-of select="''"/>
			</Blank4>

			<Blank5>
				<xsl:value-of select="''"/>
			</Blank5>

			<Blank6>
				<xsl:value-of select="''"/>
			</Blank6>

			<Blank7>
				<xsl:value-of select="''"/>
			</Blank7>

			<TotalNetAmount>
				<xsl:value-of select="InternalNetNotional"/>
			</TotalNetAmount>

		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>