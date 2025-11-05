<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
<xsl:output method="xml" encoding="UTF-8" indent="yes" />
<xsl:template match="/NewDataSet">
<ThirdPartyFlatFileDetailCollection>
<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
<xsl:for-each select="ThirdPartyFlatFileDetail[BBCode!='*']">
<ThirdPartyFlatFileDetail>
<RowHeader>
<xsl:value-of select ="'false'"/>
</RowHeader>
<FileHeader>
<xsl:value-of select="'true'"/>
</FileHeader>
<FileFooter>
<xsl:value-of select="'true'"/>
</FileFooter>
<TaxlotState>
<xsl:value-of select="TaxLotState"/>
</TaxlotState>
<BBCode>
<xsl:choose>
<!--<xsl:when test ="contains(substring-after(substring-after(BBCode,' '),' '),'EQUITY')">-->
<xsl:when test ="contains(BBCode,'EQUITY')">
<xsl:value-of select="normalize-space(concat(substring-before(BBCode,'EQUITY'),'Equity'))"/>
</xsl:when>
<xsl:otherwise>
<xsl:value-of select="normalize-space(BBCode)"/>
</xsl:otherwise>
</xsl:choose>
</BBCode>
<EntityID>
<xsl:value-of select="EntityID"/>
</EntityID>
</ThirdPartyFlatFileDetail>
</xsl:for-each>
</ThirdPartyFlatFileDetailCollection>
</xsl:template>
</xsl:stylesheet>
