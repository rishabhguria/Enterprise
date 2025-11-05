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
				<xsl:value-of select ="concat('{1:F01IRVTUS3NXXXX',$varUniqueID,'}{2:O5981528091022MAERISLANDXX', $varUniqueID, '0910221528U}{4:')"/>
			</Header1>

      <Header2>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="concat(':20:HDR-',substring($varUniqueID,1,6),'-',substring($varUniqueID,7,5))"/>
      </Header2>

			<Header3>
				<xsl:text>&#xa;</xsl:text>
				<xsl:value-of select="':12:599'"/>
			</Header3>

      <Header4>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="':77E:/AUSERID/MAE/ACONTACT/JNARDELLI 212-635-8313'"/>
      </Header4>

      <Header5>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'/ATRFIND/1'"/>
      </Header5>

      <Header6>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'/AEOD/O'"/>
      </Header6>

      <Header7>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select="'-}'"/>
      </Header7>
      
    </ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
