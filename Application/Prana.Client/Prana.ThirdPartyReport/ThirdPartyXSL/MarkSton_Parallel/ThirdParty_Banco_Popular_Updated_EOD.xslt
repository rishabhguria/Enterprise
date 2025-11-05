<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxlotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxlotState>

          <xsl:variable name="Pb_Name" select="'Banco Popular'"/>

          <xsl:variable name="PRANA_FUND_NAME1" select="AccountName"/>

          <xsl:variable name="THIRDPARTY_FUND_NAME1">
            <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$Pb_Name]/FundData[@PranaFund=$PRANA_FUND_NAME1]/@PBFundCode"/>
          </xsl:variable>

          <xsl:variable name="AccountId1">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_NAME1!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_NAME1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Account>
            <xsl:value-of select="concat('00061005',' ',$AccountId1)"/>
          </Account>

          <Tradedate>
            <xsl:value-of select="TradeDate"/>
          </Tradedate>

          <Settledate>
            <xsl:value-of select="SettlementDate"/>
          </Settledate>

          <Name>
            <xsl:value-of select="FullSecurityName"/>
          </Name>

          <xsl:variable name="Symbol">
            <xsl:value-of select="concat(Symbol,' Equity')"/>
          </xsl:variable>

          <Ticker>
            <xsl:value-of select="$Symbol"/>
          </Ticker>

          <Cusip>
            <xsl:value-of select="CUSIP"/>
          </Cusip>


          <Side>
            <xsl:value-of select="substring(Side,1,1)"/>
          </Side>

          <Shares>
            <xsl:value-of select="AllocatedQty"/>
          </Shares>

			<xsl:variable name="varAveragePrice">
				<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
			</xsl:variable>

			<xsl:variable name="varGrossAmount">
				<xsl:value-of select="AllocatedQty*$varAveragePrice*AssetMultiplier"/>
			</xsl:variable>

			<xsl:variable name="varOtherCharges">
				<xsl:value-of select="CommissionCharged + SoftCommissionCharged + OtherBrokerFee + ClearingBrokerFee + TaxOnCommissions + StampDuty + TransactionLevy + ClearingFee + SecFee + MiscFees + OccFee + OrfFee"/>
			</xsl:variable>

			<xsl:variable name="varSideMul">
				<xsl:choose>
					<xsl:when test="SideTag = '5' or SideTag = 'C' or SideTag = '2' ">
						<xsl:value-of select="-1"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="1"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varNetAmmount">
				<xsl:value-of select="$varGrossAmount + ($varOtherCharges * $varSideMul)"/>
			</xsl:variable>


          <xsl:variable name="AveragePrice">
            <xsl:choose>
              <xsl:when test="AveragePrice &gt; 0">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:when test="AveragePrice &lt; 0">
                <xsl:value-of select="AveragePrice * (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'-'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Tradeprice>
			  <xsl:value-of select="concat('$',format-number($AveragePrice,'0.####'))"/>
          </Tradeprice>

          <xsl:variable name="GrossAmount">
            <xsl:choose>
              <xsl:when test="$varGrossAmount &gt; 0">
                <xsl:value-of select="$varGrossAmount"/>
              </xsl:when>
              <xsl:when test="$varGrossAmount &lt; 0">
                <xsl:value-of select="$varGrossAmount * (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'-'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Principal>
            <xsl:value-of select="concat('$',format-number($GrossAmount,'0.##'))"/>
          </Principal>

          <xsl:variable name="CommissionCharged">
            <xsl:choose>
              <xsl:when test="CommissionCharged &gt; 0">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:when>
              <xsl:when test="CommissionCharged &lt; 0">
                <xsl:value-of select="CommissionCharged * (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'-'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Commission>
            <xsl:value-of select="concat('$',CommissionCharged)"/>
          </Commission>

          <xsl:variable name="SecFee">
            <xsl:choose>
              <xsl:when test="SecFee &gt; 0">
                <xsl:value-of select="SecFee"/>
              </xsl:when>
              <xsl:when test="SecFee &lt; 0">
                <xsl:value-of select="SecFee * (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'-'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <SECfee>
            <xsl:value-of select="concat('$',SecFee)"/>
          </SECfee>

          <xsl:variable name="NetAmount">
            <xsl:choose>
              <xsl:when test="$varNetAmmount &gt; 0">
                <xsl:value-of select="$varNetAmmount"/>
              </xsl:when>
              <xsl:when test="$varNetAmmount &lt; 0">
                <xsl:value-of select="$varNetAmmount * (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'-'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Netmoney>
            <xsl:value-of select="concat('$',format-number($NetAmount,'0.##'))"/>
          </Netmoney>

          <xsl:variable name="CL_Broker" select="'USB'"/>
          <xsl:variable name="PB_Name" select="'Banco Popular'"/>
          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$CL_Broker]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>
          <xsl:variable name="Broker1">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>




          <xsl:variable name="THIRDPARTY_Broker">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_Name]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
          </xsl:variable>

          <xsl:variable name="Broker">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_Broker!=''">
                <xsl:value-of select="$THIRDPARTY_Broker"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Broker>
            <xsl:value-of select="$Broker"/>
          </Broker>




          <DTC>
            <xsl:value-of select="$Broker1"/>
          </DTC>





          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>