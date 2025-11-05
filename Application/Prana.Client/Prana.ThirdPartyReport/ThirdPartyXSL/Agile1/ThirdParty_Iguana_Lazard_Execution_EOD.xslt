<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <currency>
          <xsl:value-of select="'currency'"/>
        </currency>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Buysell>
          <xsl:value-of select="'Buysell'"/>
        </Buysell>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <Tradedt>
          <xsl:value-of select="'trade_dt'"/>
        </Tradedt>

        <Setttlementdt>
          <xsl:value-of select="'setttlement_dt'"/>
        </Setttlementdt>


        <LastMkt>
          <xsl:value-of select ="'LastMkt'"/>
        </LastMkt>

        <Rule80A>
          <xsl:value-of select ="'Rule80A'"/>
        </Rule80A>

        <IDSource>
          <xsl:value-of select ="'IDSource'"/>
        </IDSource>

        <securityID>
          <xsl:value-of select ="'securityID'"/>
        </securityID>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
		  <ThirdPartyFlatFileDetail>

			  <!--for system internal use-->
			  <IsCaptionChangeRequired>
				  <xsl:value-of select ="'true'"/>
			  </IsCaptionChangeRequired>

			  <FileHeader>
				  <xsl:value-of select ="'false'"/>
			  </FileHeader>
			  <FileFooter>
				  <xsl:value-of select ="'false'"/>
			  </FileFooter>

			  <!--for system internal use-->
			  <RowHeader>
				  <xsl:value-of select ="'true'"/>
			  </RowHeader>

			  <!--for system internal use-->
			  <TaxLotState>
				  <xsl:value-of select="TaxLotState"/>
			  </TaxLotState>

			  <xsl:variable name = "Prana_Exchange_Name">
				  <xsl:value-of select="Exchange"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_LastMkt">
				  <xsl:value-of select="document('../ReconMappingXml/ExchangeMapping.xml')/ExchangeMapping/PB[@Name= 'Lazard']/ExchangeData[@ExchangeName=$Prana_Exchange_Name]/@LastMktCode"/>
			  </xsl:variable>

			  <Account>
				  <xsl:value-of select="'489-999951X'"/>
			  </Account>

			  <currency>
				  <xsl:value-of select="'USD'"/>
			  </currency>

			  <Price>
				  <xsl:value-of select="AveragePrice"/>
			  </Price>

			  <Quantity>
				  <xsl:value-of select="AllocatedQty"/>
			  </Quantity>

			  <Buysell>
				  <xsl:value-of select="substring(Side,1,1)"/>
			  </Buysell>

			  <Symbol>
				  <xsl:value-of select="Symbol"/>
			  </Symbol>

			  <Tradedt>
				  <xsl:value-of select="TradeDate"/>
			  </Tradedt>

			  <Setttlementdt>
				  <xsl:value-of select="SettlementDate"/>
			  </Setttlementdt>


			  <LastMkt>
				  <xsl:value-of select ="$PRANA_LastMkt"/>
			  </LastMkt>

			  <Rule80A>
				  <xsl:value-of select ="'A'"/>
			  </Rule80A>

			  <IDSource>
				  <xsl:value-of select ="'SEDOL'"/>
			  </IDSource>

			  <securityID>
				  <xsl:value-of select ="SEDOL"/>
			  </securityID>

			  <!-- system use only-->
			  <EntityID>
				  <xsl:value-of select="EntityID"/>
			  </EntityID>

		  </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
