<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <xsl:for-each select="ThirdPartyFlatFileDetail[CurrencySymbol !='USD']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <Sedol>
            <xsl:value-of select="SEDOL"/>
          </Sedol>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

         
          <AccountName>
            <xsl:value-of select="AccountName"/>
          </AccountName>

          <AccountNumber>
            <xsl:value-of select="AccountNo"/>
          </AccountNumber>



          <Description>
            <xsl:value-of select="FullSecurityName"/>
          </Description>


          <Qty>
            <xsl:value-of select="AllocatedQty"/>
          </Qty>


          <Tradecurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </Tradecurrency>

          <Avg>
            <xsl:value-of select="AveragePrice"/>
          </Avg>
          
          
          <FXRate>
            <xsl:value-of select="FXRate_Taxlot"/>
          </FXRate>


          <xsl:variable name="varTotalCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
          <Commission>
            <xsl:value-of select="$varTotalCommission"/>
          </Commission>




          <OtherFee>
            <xsl:value-of select="(StampDuty + OCCFee + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee + SecFee)"/>
          </OtherFee>


          <xsl:variable name = "PB_CounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>
          <xsl:variable name="varPB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <xsl:variable name="PRANA_BrokerCode">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$varPB_NAME]/BrokerData[@PranaBroker=$PB_CounterParty]/@PBBroker"/>
          </xsl:variable>
          <BrokerCode>
            <xsl:choose>
              <xsl:when test="$PRANA_BrokerCode!=''">
                <xsl:value-of select="$PRANA_BrokerCode"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="$PB_CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>
          </BrokerCode>

          <NetamountLocal>
            <xsl:value-of select="NetAmount"/>
          </NetamountLocal>
          
          <NetamountBase>
            <xsl:value-of select="(NetAmount * FXRate_Taxlot)"/>
          </NetamountBase>
          
        

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