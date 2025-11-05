<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileHeader">
    <ThirdPartyFlatFileHeader>
      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

		<RecordType>
			<xsl:value-of select="'HDR'"/>
		</RecordType>

		<FileCreationDate>
			<xsl:choose>
				<xsl:when  test ="string-length(substring-before(substring-after(Date,'/'),'/'))=1">
					<xsl:value-of select="concat(substring-before(Date,'/'),'0',substring-before(substring-after(Date,'/'),'/'),substring(substring-after(substring-after(Date,'/'),'/'),3,2))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat(substring-before(Date,'/'),substring-before(substring-after(Date,'/'),'/'),substring(substring-after(substring-after(Date,'/'),'/'),3,2))"/>
				</xsl:otherwise>
			</xsl:choose>			
		</FileCreationDate>
		
    </ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>