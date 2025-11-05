<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileFooter">
    <ThirdPartyFlatFileFooter>
      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

      <Trailer18111>
        <xsl:text>&#xa;&#xa;&#xa;</xsl:text>
        <xsl:value-of select="'COUNTRY'"/>
      </Trailer18111>
      
      <Done2>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'AGENT BANK'"/>
      </Done2>

      <Done3>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'CITY'"/>
      </Done3>

      <Done4>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'BIC CODE1'"/>
      </Done4>

      <Done5>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'ACCOUNT 1 NAME'"/>
      </Done5>

      <Done6>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'ACCOUNT 1'"/>
      </Done6>

      <Done7>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'BIC CODE2'"/>
      </Done7>

      <Done8>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'ACCOUNT 2 NAME'"/>
      </Done8>

      <Done9>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'ACCOUNT 2'"/>
      </Done9>

      <Done10>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'LOCAL CODES'"/>
      </Done10>

      <Done12>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'NOTES'"/>
      </Done12>

      <Done1>
        <xsl:text>&#xa;</xsl:text>
        <xsl:value-of select="'United Kingdom'"/>
      </Done1>

      <Done11111>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'Pershing Securities Ltd.'"/>
      </Done11111>

      <Done112>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'London'"/>
      </Done112>
      <Done113>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'PERLGB2LXXX'"/>
      </Done113>
      <Done114>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'Pershing LLC'"/>
      </Done114>

      <Done115>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'PNCLEAR D'"/>
      </Done115>

      <Done116>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'PRSHUS33XXX'"/>
      </Done116>

      <Done117>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="' '"/>
      </Done117>

      <Done118>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="' '"/>
      </Done118>

      <Done119>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="'Crest # 601'"/>
      </Done119>

      <Done110>
        <xsl:text>&#x9;</xsl:text>
        <xsl:value-of select="' '"/>
      </Done110>

      <Trailer18>
        <xsl:text>&#xa;&#xa;&#xa;</xsl:text>
        <xsl:value-of select="'FX INSTRUCTION'"/>
      </Trailer18>
    

      <TotalSettlementAmount>       
        <xsl:value-of select ="concat('Please Sell HKD',' ',InternalNetNotional,' ', 'vs USD with a value date of',' ',Date)"/>
      </TotalSettlementAmount>

      <Trailer1>
        <xsl:text>&#xa;&#xa;</xsl:text>
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
