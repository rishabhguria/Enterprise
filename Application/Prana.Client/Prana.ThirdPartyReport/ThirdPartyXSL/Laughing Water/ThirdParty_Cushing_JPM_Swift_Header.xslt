<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string DateNow()
		{
		return(System.DateTime.Now.ToString("yyMMddhhmmss"));
		}
	</msxsl:script>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<xsl:variable name ="varUniqueID">
				<xsl:value-of select ="number(my:DateNow())"/>
			</xsl:variable>
      
			<Header1>
				<xsl:value-of select ="concat('H','NIRVANAXXXXX',$varUniqueID)"/>
			</Header1>

      
    </ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
