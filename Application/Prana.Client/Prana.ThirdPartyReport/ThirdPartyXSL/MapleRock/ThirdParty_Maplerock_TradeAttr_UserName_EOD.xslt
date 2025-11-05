<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
	
      <xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail[Side='Sell short']">
      <ThirdPartyFlatFileDetail>
	  
        <!--  system inetrnal use -->
        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>      


        <TradeDate>
          <xsl:value-of select="TradeDate"/>
        </TradeDate>

        <Ticker>
          <xsl:value-of select="Symbol"/>
        </Ticker>

        <SecurityName>
          <xsl:value-of select="CompanyName"/>
        </SecurityName>
		
		<BorrowBroker>
          <xsl:value-of select="translate(TradeAttribute1,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
        </BorrowBroker>

        <LocatedQty>
          <xsl:value-of select="TradeAttribute3"/>
        </LocatedQty>

        <LocatedID>
          <xsl:value-of select="TradeAttribute2"/>
        </LocatedID>

        <Ratebps>
          <xsl:value-of select="TradeAttribute4"/>
        </Ratebps>
		
		<UserName>
          <xsl:value-of select="UserName"/>
        </UserName>

		<EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
		
      </ThirdPartyFlatFileDetail>
	   </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>