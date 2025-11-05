<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <AccountID>
          <xsl:value-of select ="'Account ID'"/>
        </AccountID>

        <SecurityID>
          <xsl:value-of select ="'Security ID'"/>
        </SecurityID>

        <Quantity>
          <xsl:value-of select ="'Quantity'"/>
        </Quantity>

        <IncrDcrInd>
          <xsl:value-of select ="'Incr/Dcr Ind'"/>
        </IncrDcrInd>

        <SettlementDate>
          <xsl:value-of select ="'Settlement Date'"/>
        </SettlementDate>

        <TradeDate>
          <xsl:value-of select ="'Trade Date'"/>
        </TradeDate>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail [CompanyFundID ='5' and Side='Sell']">

        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <AccountID>
            <xsl:value-of select ="AccountNo"/>
          </AccountID>
          
          <SecurityID>
            <xsl:value-of select ="SEDOL"/>
          </SecurityID>
          
          <Quantity>
            <xsl:value-of select ="AllocatedQty"/>
          </Quantity>
          
          <IncrDcrInd>
            <xsl:value-of select ="'D'"/>
          </IncrDcrInd>
          
          <SettlementDate>
            <xsl:value-of select ="SettlementDate"/>
          </SettlementDate>
          
          <TradeDate>
            <xsl:value-of select ="TradeDate"/>
          </TradeDate>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>