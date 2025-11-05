<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template name="GetMonth">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month = 1" >
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month = 2" >
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month = 3" >
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month = 4" >
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month = 5" >
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month = 6" >
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month = 7" >
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month = 8" >
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month = 9" >
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month = 10" >
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month = 11" >
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month = 12" >
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Count">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="count(//ThirdPartyFlatFileDetail[Symbol=$Symbol])"/>
  </xsl:template>

  <xsl:template name="SumPrice">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="sum(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/AveragePrice)"/>
  </xsl:template>

  <xsl:template name="SumQuantity">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="sum(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/AllocatedQty)"/>
  </xsl:template>

  <xsl:template name="BLK">
    <xsl:param name="ID"/>
    <xsl:param name="Symbol"/>
    <xsl:choose>
      <xsl:when test="$ID = (//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)">
        <xsl:value-of select="(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <xsl:variable name="varCount">
            <xsl:call-template name="Count">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="SumPrice">
            <xsl:call-template name ="SumPrice">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="SumQuantity">
            <xsl:call-template name ="SumQuantity">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>


          <xsl:variable name="AvgofPrice">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>

          <xsl:variable name="BLK">
            <xsl:call-template name="BLK">
              <xsl:with-param name="ID" select="PBUniqueID"/>
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>


         
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

         
          <Side>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'BO'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'SO'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Side>

          <Complete>
            <xsl:value-of select="AllocatedQty"/>
          </Complete>

          <Symbol>
            <xsl:value-of select="substring-before(BBCode,' EQUITY')"/>
          </Symbol>
          
          <ExPrice>
            <xsl:value-of select="AveragePrice"/>
          </ExPrice>
          <xsl:variable name="SideMultiplier">
            <xsl:choose>
              <xsl:when test="SideTag = '1'  or SideTag = 'A' or SideID = 'B' or SideTag = '3' ">
                <xsl:value-of select ="1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="-1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varCommission" select="(CommissionCharged + SoftCommissionCharged)"/>

          <xsl:variable name="varOtherFees" select="(OtherBrokerFees + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions+ MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee)"/>

          <xsl:variable name="CalcNetAmt">
            <xsl:value-of select="(AllocatedQty * AveragePrice * AssetMultiplier) + (($varCommission + $varOtherFees) * $SideMultiplier)"/>
          </xsl:variable>

          <xsl:variable name="NetPrice">
            <xsl:value-of select="$CalcNetAmt div (AllocatedQty * AssetMultiplier)"/>
          </xsl:variable>

          <xsl:variable name="varVWAP">
            <xsl:value-of select="$SumPrice"/>
          </xsl:variable>
          
          <xsl:variable name="varSumQuantity">
            <xsl:value-of select="$SumQuantity"/>
          </xsl:variable>

          <xsl:variable name="varVWAP1">
            <xsl:value-of select="($varVWAP * AllocatedQty) div $varSumQuantity"/>
          </xsl:variable>
          
          <VWAP>
            <xsl:value-of select="$varVWAP1"/>
          </VWAP>
          
          
          <xsl:variable name="varLeaves">
            <xsl:value-of select="TotalQty - AllocatedQty"/>
          </xsl:variable>
          <Leaves>
            <xsl:value-of select="$varLeaves"/>
          </Leaves>

          <I>
            <xsl:value-of select="'I'"/>            
          </I>

        
          <GrossVal>
            <xsl:value-of select="GrossAmount"/>
          </GrossVal>

          <NetVal>
            <xsl:value-of select="NetAmount"/>
          </NetVal>

          <Fees>
            <xsl:value-of select="''"/>
          </Fees>

          <BrokerComm>
            <xsl:value-of select="CommissionCharged"/>
          </BrokerComm>
          
          <xsl:variable name="varBrokerrate">
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </xsl:variable>
          <Brokerrate>
            <xsl:value-of select="$varBrokerrate"/>
          </Brokerrate>

          <MerakiComm>
            <xsl:value-of select="CommissionCharged"/>
          </MerakiComm>

          <Merakirate>
            <xsl:value-of select="$varBrokerrate"/>
          </Merakirate>

          <TotalComm>
            <xsl:value-of select="$varCommission"/>
          </TotalComm>

          <FXRate>
            <xsl:value-of select="FXRate_Taxlot"/>
          </FXRate>

          <SettleCcy>
            <xsl:value-of select="SettlCurrency"/>
          </SettleCcy>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

          <PB>
            <xsl:value-of select="'GS'"/>
          </PB>

          <TD>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TD>

          <SD>
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </SD>

          <Acct>
            <xsl:value-of select="AccountName"/>
          </Acct>

         
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
