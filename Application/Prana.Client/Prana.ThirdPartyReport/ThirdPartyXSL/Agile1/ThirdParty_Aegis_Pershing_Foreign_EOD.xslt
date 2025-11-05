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
          <xsl:value-of select="'localref#'"/>
        </Account>

        <BlockLinkage>
          <xsl:value-of select="'Blocklinkage'"/>
        </BlockLinkage>
       
        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <Cusip>
          <xsl:value-of select="'Cusip'"/>
        </Cusip>

        <Sedol>
          <xsl:value-of select="'Sedol'"/>
        </Sedol>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <Description>
          <xsl:value-of select="'Description 1'"/>
        </Description>

        <B_S>
          <xsl:value-of select="'B/S'"/>
        </B_S>

        <CancelCode>
          <xsl:value-of select="'Cancel Code'"/>
        </CancelCode>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Principal>
          <xsl:value-of select="'Principal'"/>
        </Principal>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <StampTax>
          <xsl:value-of select="'StampTax'"/>
        </StampTax>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <TradingOffset>
          <xsl:value-of select="'TradingOffset'"/>
        </TradingOffset>

        <CustomerAcctNo>
          <xsl:value-of select="'Customer Acct #'"/>
        </CustomerAcctNo>

        <Passengermktinfo>
          <xsl:value-of select="'Passenger mkt info'"/>
        </Passengermktinfo>

        <SSIshortcode>
          <xsl:value-of select="'SSI shortcode'"/>
        </SSIshortcode>

        <Setlcntrycd>
          <xsl:value-of select="'Setlcntrycd'"/>
        </Setlcntrycd>


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

          <Account>
            <xsl:value-of select="EntityID"/>
          </Account>

          <BlockLinkage>
            <xsl:value-of select="''"/>
          </BlockLinkage>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Symbol>
            <xsl:value-of select="''"/>
          </Symbol>

          <Cusip>
            <xsl:value-of select="''"/>
          </Cusip>

          <Sedol>
            <xsl:value-of select="''"/>
          </Sedol>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <Description>
            <xsl:value-of select="''"/>
          </Description>

          <B_S>
            <xsl:value-of select="substring(Side,1,1)"/>
          </B_S>

          <CancelCode>
            <xsl:value-of select="'N'"/>
          </CancelCode>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <Principal>
            <xsl:value-of select="GrossAmount"/>
          </Principal>

          <Commission>
            <xsl:value-of select="CommissionCharged"/>
          </Commission>

          <StampTax>
            <xsl:value-of select="StampDuty"/>
          </StampTax>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

          <TradingOffset>
            <xsl:value-of select="''"/>
          </TradingOffset>

          <CustomerAcctNo>
            <xsl:value-of select="''"/>
          </CustomerAcctNo>

          <Passengermktinfo>
            <xsl:value-of select="''"/>
          </Passengermktinfo>

          <SSIshortcode>
            <xsl:value-of select="''"/>
          </SSIshortcode>

          <Setlcntrycd>
            <xsl:value-of select="''"/>
          </Setlcntrycd>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
