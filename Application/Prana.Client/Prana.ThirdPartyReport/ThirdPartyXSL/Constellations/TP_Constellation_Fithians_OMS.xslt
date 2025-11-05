<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <SubAccount>
          <xsl:value-of select ="'Sub Account #'"/>
        </SubAccount>

        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <SecuritySymbol>
          <xsl:value-of select="'Security Symbol'"/>
        </SecuritySymbol>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

   <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
		
          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
         

          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <!--<xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>-->

          <SubAccount>
            <xsl:value-of select ="AccountNo"/>
          </SubAccount>

          <Action>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'SS'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'SL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
          </Action>
          
          <Quantity>
            <xsl:value-of select="format-number(AllocatedQty, '##.0000')"/>
          </Quantity>

          <SecuritySymbol>
            <xsl:value-of select="Symbol"/>
          </SecuritySymbol>    

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>