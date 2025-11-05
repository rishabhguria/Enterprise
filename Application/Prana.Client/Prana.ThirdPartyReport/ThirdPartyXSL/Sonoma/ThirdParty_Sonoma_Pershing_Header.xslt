<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileHeader">
    <ThirdPartyFlatFileHeader>

      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

      <RedordType>
        <xsl:value-of select="'HDR'"/>
      </RedordType>

      <FileCreationDate>
        <xsl:value-of select="concat(substring(DateAndTime,1,2),substring(DateAndTime,4,2),substring(DateAndTime,9,2))"/>
      </FileCreationDate>

    </ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>
