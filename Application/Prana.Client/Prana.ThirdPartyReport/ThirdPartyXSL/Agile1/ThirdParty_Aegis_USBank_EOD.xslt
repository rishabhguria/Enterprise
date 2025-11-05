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
          <xsl:value-of select="'USB Acct. #'"/>
        </Account>

        <Tran>
          <xsl:value-of select="'Tran'"/>
        </Tran>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <VWAP>
          <xsl:value-of select="'VWAP'"/>
        </VWAP>

        <Gross>
          <xsl:value-of select="'Gross'"/>
        </Gross>


        <F_P>
          <xsl:value-of select ="'F/P'"/>
        </F_P>

        <Commission>
          <xsl:value-of select ="'Commission'"/>
        </Commission>

        <SecFee>
          <xsl:value-of select ="'Sec Fees'"/>
        </SecFee>

        <UKLevy>
          <xsl:value-of select ="'UK Levy'"/>
        </UKLevy>

        <NetAmount>
          <xsl:value-of select="'Net Amount'"/>
        </NetAmount>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <Settlement>
          <xsl:value-of select="'Settlement'"/>
        </Settlement>

        <FX>
          <xsl:value-of select="'FX'"/>
        </FX>

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
            <xsl:value-of select ="'false'"/>
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
			  <xsl:choose>
				  <xsl:when test ="AccountName = 'Aegis High Yield Fund:19-0021'">
					  <xsl:value-of select ="'19-0021'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'19-0022'"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </Account>

          <Tran>
            <xsl:value-of select="Side"/>
          </Tran>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <VWAP>
            <xsl:value-of select="''"/>
          </VWAP>

          <Gross>
            <xsl:value-of select="GrossAmount"/>
          </Gross>

          <F_P>
            <xsl:value-of select ="'P'"/>
          </F_P>

          <Commission>
            <xsl:value-of select ="CommissionCharged"/>
          </Commission>

          <SecFee>
            <xsl:value-of select ="SecFees"/>
          </SecFee>

          <UKLevy>
            <xsl:value-of select ="''"/>
          </UKLevy>

          <NetAmount>
            <xsl:value-of select="NetAmount"/>
          </NetAmount>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

          <Settlement>
            <xsl:value-of select="'Settlement'"/>
          </Settlement>

          <FX>
            <xsl:choose>
              <xsl:when test="CurrencySymbol = 'USD'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="ForexRate"/>
              </xsl:otherwise>
            </xsl:choose>
          </FX>
          
          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
