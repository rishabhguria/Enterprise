<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <AccountNumber>
          <xsl:value-of select="'Account Number'"/>
        </AccountNumber>

        <AccountType>
          <xsl:value-of select="'Account Type'"/>
        </AccountType>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <AvgPrice>
          <xsl:value-of select="'Avg Price'"/>
        </AvgPrice>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <Principal>
          <xsl:value-of select="'Principal'"/>
        </Principal>

        <Interest>
					<xsl:value-of select="'Interest'"/>
				</Interest>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <Solicited>
          <xsl:value-of select="'Solicited'"/>
        </Solicited>

        <Exchange>
					<xsl:value-of select="'Exchange'"/>
				</Exchange>

        <CommissionCode>
					<xsl:value-of select="'Commission Code'"/>
				</CommissionCode>

        <Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

        <GrossCode>
          <xsl:value-of select="'Gross Code'"/>
        </GrossCode>

        <SalesCredit>
          <xsl:value-of select="'Sales Credit'"/>
        </SalesCredit>

        <ContraOffsetAccount>
          <xsl:value-of select="'Contra (Offset) Account'"/>
        </ContraOffsetAccount>

        <ContraOffsetAccType>
          <xsl:value-of select="'Contra (Offset) Acc Type'"/>
        </ContraOffsetAccType>

        <ContraSub>
          <xsl:value-of select="'Contra Sub'"/>
        </ContraSub>

        <TraceMSRBReportingIndicator>
          <xsl:value-of select="'Trace &amp; MSRB Reporting Indicator'"/>
        </TraceMSRBReportingIndicator>

        <PrevailingMarketPrice>
          <xsl:value-of select="'Prevailing Market Price'"/>
        </PrevailingMarketPrice>

        <MarkUpMarkDownAmount>
         <xsl:value-of select="'Mark Up/Mark Down Amount'"/>
        </MarkUpMarkDownAmount>

        <MarkUpMarkDownPercent>
          <xsl:value-of select="'Mark Up/Mark Down Percent'"/>
        </MarkUpMarkDownPercent>

        <ExecutionTime>
          <xsl:value-of select="'Execution Time'"/>
        </ExecutionTime>

        <HandFigureCode>
          <xsl:value-of select="'Hand Figure Code'"/>
        </HandFigureCode>

        <ProcessingCode>
          <xsl:value-of select="'Processing Code'"/>
        </ProcessingCode>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='Equity']">
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <AccountNumber>
            <xsl:value-of select="AccountName"/>
          </AccountNumber>

          <AccountType>
            <xsl:value-of select="'2'"/>
          </AccountType>

          <Side>
            <xsl:value-of select="Side"/>
          </Side>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <AvgPrice>
            <xsl:value-of select="AveragePrice"/>
          </AvgPrice>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <Principal>
            <xsl:value-of select="''"/>
          </Principal>

          <Interest>
            <xsl:value-of select="''"/>
          </Interest>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

          <Solicited>
            <xsl:value-of select="''"/>
          </Solicited>

          <Exchange>
            <xsl:value-of select="substring(Exchange,1,4)"/>
          </Exchange>

          <CommissionCode>
            <xsl:value-of select="''"/>
          </CommissionCode>

          <Commission>
            <xsl:value-of select="''"/>
          </Commission>

          <GrossCode>
            <xsl:value-of select="''"/>
          </GrossCode>

          <SalesCredit>
            <xsl:value-of select="''"/>
          </SalesCredit>

          <ContraOffsetAccount>
            <xsl:value-of select="''"/>
          </ContraOffsetAccount>

          <ContraOffsetAccType>
            <xsl:value-of select="''"/>
          </ContraOffsetAccType>

          <ContraSub>
            <xsl:value-of select="''"/>
          </ContraSub>

          <TraceMSRBReportingIndicator>
            <xsl:value-of select="''"/>
          </TraceMSRBReportingIndicator>

          <PrevailingMarketPrice>
            <xsl:value-of select="''"/>
          </PrevailingMarketPrice>

          <MarkUpMarkDownAmount>
            <xsl:value-of select="''"/>
          </MarkUpMarkDownAmount>

          <MarkUpMarkDownPercent>
            <xsl:value-of select="''"/>
          </MarkUpMarkDownPercent>

          <ExecutionTime>
            <xsl:value-of select="''"/>
          </ExecutionTime>

          <HandFigureCode>
            <xsl:value-of select="''"/>
          </HandFigureCode>

          <ProcessingCode>
            <xsl:value-of select="''"/>
          </ProcessingCode>

          <EntityID>
            <xsl:value-of select="''"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
