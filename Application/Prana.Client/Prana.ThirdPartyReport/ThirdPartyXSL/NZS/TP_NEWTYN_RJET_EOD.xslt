<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>

        <Symbol>
          <xsl:value-of select ="'SYMBOL'"/>
        </Symbol>

        <Side>
          <xsl:value-of select ="'SIDE'"/>
        </Side>

        <Account>
          <xsl:value-of select ="'ACCOUNT'"/>
        </Account>

        <Quantity>
          <xsl:value-of select="'QUANTITY'"/>
        </Quantity>


        <Price>
          <xsl:value-of select="'PRICE'"/>
        </Price>

        <Commission>
          <xsl:value-of select="'Comm'"/>
        </Commission>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty = 'RJET']">

        <ThirdPartyFlatFileDetail>
					
					<Symbol>
            <xsl:value-of select ="Symbol"/>
					</Symbol>
					
					<Side>
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                <xsl:value-of select ="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                <xsl:value-of select ="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                <xsl:value-of select ="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select ="'CS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
					</Side>
					
					<Account>
            <xsl:value-of select ="AccountName"/>
					</Account>
					
				   <Quantity>
            <xsl:value-of select="AllocatedQty"/>
				   </Quantity>
				
					
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

         

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
