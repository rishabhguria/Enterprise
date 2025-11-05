<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
<xsl:output method="xml" encoding="UTF-8" indent="yes" cdata-section-elements="comments"/>
<xsl:template match="/ThirdPartyFlatFileFooter">
<ThirdPartyFlatFileFooter>
<comments>
END-OF-DATA
END-OF-FILE
</comments>
<RowHeader>
<xsl:value-of select ="'false'"/>
</RowHeader>
</ThirdPartyFlatFileFooter>
</xsl:template>
</xsl:stylesheet>
