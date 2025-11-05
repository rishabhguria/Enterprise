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

        <OFFICE>
          <xsl:value-of select="'OFFICE'"/>
        </OFFICE>

        <Account>
          <xsl:value-of select="'ACCOUNT'"/>
        </Account>

        <AccountType>
          <xsl:value-of select="'ACCT TYPE'"/>
        </AccountType>

        <CUSIP>
          <xsl:value-of select="'CUSIP/SYMBOL'"/>
        </CUSIP>

        <DESCRIPTION>
          <xsl:value-of select="'DESCRIPTION'"/>
        </DESCRIPTION>

        <BUYSELL>
          <xsl:value-of select="'BUYSELL'"/>
        </BUYSELL>

        <TradeDate>
          <xsl:value-of select="'TRADEDATE'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'SETTLE DATE'"/>
        </SettleDate>

        <QUANTITY>
          <xsl:value-of select="'QUANTITY'"/>
        </QUANTITY>

        <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

        <COMM>
          <xsl:value-of select="'COMM'"/>
        </COMM>

        <MPID>
          <xsl:value-of select="'MPID'"/>
        </MPID>

        <ExecutionAccount>
          <xsl:value-of select="'EXECUTION ACCOUNT'"/>
        </ExecutionAccount>

        <SettlementAccount>
          <xsl:value-of select="'SETTLEMENT ACCOUNT'"/>
        </SettlementAccount>


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

          <OFFICE>
            <xsl:value-of select="''"/>
          </OFFICE>

          <Account>
            <xsl:value-of select="AccountName"/>
          </Account>

          <AccountType>
            <xsl:value-of select="'1'"/>
          </AccountType>

          <CUSIP>
            <xsl:choose>
              <xsl:when test="CUSIP = ''">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CUSIP"/>
              </xsl:otherwise>
            </xsl:choose>
          </CUSIP>

          <DESCRIPTION>
            <xsl:value-of select="''"/>
          </DESCRIPTION>

          <BUYSELL>
            <xsl:value-of select="Side"/>
          </BUYSELL>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <QUANTITY>
            <xsl:value-of select="AllocatedQty"/>
          </QUANTITY>

          <PRICE>
            <xsl:value-of select="AveragePrice"/>
          </PRICE>

          <COMM>
            <xsl:value-of select="CommissionCharged"/>
          </COMM>

          <MPID>
            <xsl:value-of select="CounterParty"/>
          </MPID>

          <ExecutionAccount>
            <xsl:value-of select="'N80897775-2'"/>
          </ExecutionAccount>

          <SettlementAccount>
            <xsl:value-of select="'3DT614576-0'"/>
          </SettlementAccount>
          
          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
