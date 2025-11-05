<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>


      <Trailer1>
        <xsl:text>&#xa;&#xa;</xsl:text>
        <xsl:value-of select ="'Trading Broker Number'"/>
      </Trailer1>

      <Trailer2>
        <xsl:value-of select ="''"/>
      </Trailer2>     
      
      <Trailer4>       
        <xsl:value-of select ="443"/>
      </Trailer4>

      <Trailer5>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'Clearing Broker Name'"/>
      </Trailer5>

      <Trailer6>
        <xsl:value-of select ="''"/>
      </Trailer6>

      <Trailer3>
        <xsl:value-of select ="'Sanders Morris Harris LLC'"/>
      </Trailer3>
     
      <Trailer7>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'Clearing Broker Number'"/>
      </Trailer7>

      <Trailer8>
        <xsl:value-of select ="''"/>
      </Trailer8>

      <Trailer9>
        <xsl:value-of select ="443"/>
      </Trailer9>
			
	 					
	  <Trailer16>
		  <xsl:text>&#xa;&#xa;</xsl:text>
		<xsl:value-of select ="'Authorized Signature:'"/>
	   </Trailer16>
			
	  <Trailer17>
		 <xsl:text>&#xa;</xsl:text>
		<xsl:value-of select ="''"/>
	   </Trailer17>

			<Trailer>
            <xsl:text>&#xa;</xsl:text>
				<xsl:value-of select ="concat('&#x0A;','Dan DeSerio')"/>				
			</Trailer>

			<Done>
				<xsl:value-of select ="concat('&#x0A;','212-415-6611')"/>				
			</Done>

			<Email>
				<xsl:value-of select ="concat('&#x0A;','ddeserio@lyricalpartners.com')"/>				
			</Email>
		</ThirdPartyFlatFileFooter>

	</xsl:template>
</xsl:stylesheet>
