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
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<xsl:variable name ="varUniqueID">
				<xsl:value-of select ="number(my:DateNow())+1"/>
			</xsl:variable>
			<Trailer1>
				<xsl:value-of select ="concat('{1:F01IRVTUS3NXXXX',$varUniqueID,'}{2:O5981528091022MAERISLANDXX', $varUniqueID, '0910221528U}{4:')"/>
			</Trailer1>
      
			<Trailer2>
        <xsl:text>&#xa;</xsl:text>
				<xsl:value-of select ="concat(':20:TLR-', substring($varUniqueID,1,6),'-',substring($varUniqueID,7,5))"/>
			</Trailer2>

      <Trailer3>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="':12:599'"/>
      </Trailer3>

      <Trailer4>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="concat(':77E:/ATTOT/', InternalGrossAmount)"/>
      </Trailer4>

      <Trailer5>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="concat('/ANTOT/', InternalNetNotional)"/>
      </Trailer5>

      <Trailer6>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="concat('/APTOT/', TotalQty)"/>
      </Trailer6>

      <Trailer7>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select="'-}'"/>
      </Trailer7>
      
		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>
