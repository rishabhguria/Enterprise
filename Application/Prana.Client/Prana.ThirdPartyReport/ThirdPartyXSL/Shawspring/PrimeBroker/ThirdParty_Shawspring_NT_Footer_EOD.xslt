<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileFooter">
    <ThirdPartyFlatFileFooter>
      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>


      <Trailer1>
        <xsl:text>&#xa;&#xa;&#xa;</xsl:text>
        <xsl:value-of select ="'Authorized Signature:'"/>
      </Trailer1>

      <Trailer2>
        <xsl:value-of select ="''"/>
      </Trailer2>

      <Trailer21>
        <xsl:value-of select ="''"/>
      </Trailer21>

      <Trailer5>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'Name'"/>
      </Trailer5>

      <Trailer6>
        <xsl:value-of select ="''"/>
      </Trailer6>

      <Trailer61>
        <xsl:value-of select ="''"/>
      </Trailer61>

      <Trailer3>
        <xsl:value-of select ="'Paul Lashway CFO at ShawSpring Partners'"/>
      </Trailer3>

      <Trailer7>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'Date'"/>
      </Trailer7>

      <Trailer8>
        <xsl:value-of select ="''"/>
      </Trailer8>

      <Trailer81>
        <xsl:value-of select ="''"/>
      </Trailer81>
      
      <Trailer9>
        <xsl:value-of select ="Date"/>
      </Trailer9>

      <Trailer10>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'Phone'"/>
      </Trailer10>
      
      <Trailer12>
        <xsl:value-of select ="''"/>
      </Trailer12>
      <Trailer121>
        <xsl:value-of select ="''"/>
      </Trailer121>

      <Trailer11>
        <xsl:value-of select ="'617 310 4877 (office) 617 291 8389 (mobile)'"/>
      </Trailer11>

     

      <Trailer13>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="'Email'"/>
      </Trailer13>

      <Trailer14>
        <xsl:value-of select ="''"/>
      </Trailer14>


      <Trailer141>
        <xsl:value-of select ="''"/>
      </Trailer141>
      
      <Trailer15>
        <xsl:value-of select ="'paul@shawspring.com'"/>
      </Trailer15>


      <Trailer17>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="''"/>
      </Trailer17>

      <Trailer>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select ="concat('&#x0A;','FAXING TO: 312-557-9501')"/>
      </Trailer>

     
    </ThirdPartyFlatFileFooter>

  </xsl:template>
</xsl:stylesheet>
