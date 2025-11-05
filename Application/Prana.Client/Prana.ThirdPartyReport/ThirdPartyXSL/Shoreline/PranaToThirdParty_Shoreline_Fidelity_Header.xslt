<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'true'"/>
			</RowHeader>
			<Header>
				<xsl:value-of select ="'HEADER'"/>
			</Header>
			<ClientName>
				<xsl:value-of select ="'SHORELINE'"/>
			</ClientName>

			<xsl:variable name = "varMth" >
				<xsl:value-of select="substring-before(Date,'/')"/>
			</xsl:variable>
			<xsl:variable name = "varDateYr" >
				<xsl:value-of select="substring-after(Date,'/')"/>
			</xsl:variable>
			<xsl:variable name = "varYR" >
				<xsl:value-of select="substring-after($varDateYr,'/')"/>
			</xsl:variable>
			<xsl:variable name = "varDt" >
				<xsl:value-of select="substring-before($varDateYr,'/')"/>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test ="string-length($varMth) &lt; 2 and string-length($varDt) &lt; 2">
					<TransmissionDate>
						<xsl:value-of select="concat($varYR,'',concat('0',$varMth),'',concat('0',$varDt))"/>
					</TransmissionDate>
				</xsl:when>
				<xsl:when test ="string-length($varMth) = 2 and string-length($varDt) &lt; 2">
					<TransmissionDate>
						<xsl:value-of select="concat($varYR,'',$varMth,'',concat('0',$varDt))"/>
					</TransmissionDate>
				</xsl:when>
				<xsl:when test ="string-length($varMth) &lt; 2 and string-length($varDt) = 2">
					<TransmissionDate>
						<xsl:value-of select="concat($varYR,'',concat('0',$varMth),'',$varDt)"/>
					</TransmissionDate>
				</xsl:when>
				<xsl:otherwise>
					<TransmissionDate>
						<xsl:value-of select="concat($varYR,'',$varMth,'',$varDt)"/>
					</TransmissionDate>
				</xsl:otherwise>
			</xsl:choose>
		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
