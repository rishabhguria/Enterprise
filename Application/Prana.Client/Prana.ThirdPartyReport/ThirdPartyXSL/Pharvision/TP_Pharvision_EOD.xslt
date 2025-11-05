<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>



      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'Boothbay Absolute Return - 82615315']">

        <ThirdPartyFlatFileDetail>
		

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
          <xsl:variable name="Trade_Day" select="substring(TradeDate,4,2)"/>
          <xsl:variable name="Trade_Month" select="substring(TradeDate,1,2)"/>
          <xsl:variable name="Trade_Year" select="substring(TradeDate,7,4)"/>
          <TradeDate>
            <xsl:value-of select="concat($Trade_Year,$Trade_Month,$Trade_Day)"/>
          </TradeDate>

          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <AccountNo>
            <xsl:choose>
			  <xsl:when test ="AccountName = 'Boothbay Absolute Return - 82615315'">
                <xsl:value-of select ="'82615'"/>
              </xsl:when>
              <xsl:when test ="$PB_FUND_NAME != ''">
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNo>
          
        
               
          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>
          
          <ProductType>
            <xsl:value-of select="Asset"/>
          </ProductType>
          
          <Cusip>
            <xsl:value-of select="CUSIP"/>
          </Cusip>
          
          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>
          
          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>
          
          <TradePrice>
            <xsl:value-of select="AveragePrice"/>
          </TradePrice>
          
          <InterestRate>
            <xsl:value-of select="''"/>
          </InterestRate>
          <xsl:variable name="STrade_Day" select="substring(SettlementDate,4,2)"/>
          <xsl:variable name="STrade_Month" select="substring(SettlementDate,1,2)"/>
          <xsl:variable name="STrade_Year" select="substring(SettlementDate,7,4)"/>
       
          <SettlementDate>
           <xsl:value-of select="concat($STrade_Year,$STrade_Month,$STrade_Day)"/>
          </SettlementDate>
          
          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>
          
		            <xsl:variable name="Side1">
          <xsl:choose>
            <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                <xsl:value-of select ="'B'"/>
             </xsl:when>            
            <xsl:when test="Side='Sell' or Side='Sell to Close' ">
             
                <xsl:value-of select ="'SL'"/>
             
            </xsl:when>
            <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
              
                <xsl:value-of select ="'SS'"/>
             
            </xsl:when>
            <xsl:when test="Side='Buy to Close' ">
              
                <xsl:value-of select ="'BC'"/>
              
            </xsl:when>
            <xsl:otherwise>
              
                <xsl:value-of select="Side"/>
             
            </xsl:otherwise>
          </xsl:choose>
		  </xsl:variable>
          
          
          <TradeDirection>
            <xsl:value-of select="$Side1"/>
          </TradeDirection>
          
          <Amount>
            <xsl:value-of select="NetAmount"/>
          </Amount>
          
          <InterestAmount>
            <xsl:value-of select="''"/>
          </InterestAmount>
          
          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.000')"/>
          </Commission>
          
          <Taxes>
            <xsl:value-of select="TaxOnCommissions"/>
          </Taxes>
          
          <ExchangeFee>
            <xsl:value-of select="TaxOnCommissions + OtherBrokerFee + ClearingBrokerFee + ClearingFee + MiscFees + SecFee + StampDuty + TransactionLevy"/>
          </ExchangeFee>
          
		  
		  <xsl:variable name ="i" select ="position()"/>
          <TradeId>
        <xsl:choose>
			 <xsl:when test ="$i  &lt; 10">
			   <xsl:value-of select ="concat(PBUniqueID,'000', $i)"/>
			 </xsl:when>
			 
			  <xsl:when test ="$i  &lt; 100">
			   <xsl:value-of select ="concat(PBUniqueID,'00', $i)"/>
			 </xsl:when>
			 <xsl:when test ="$i  &lt; 1000">
			   <xsl:value-of select ="concat(PBUniqueID,'0', $i)"/>
			 </xsl:when>
			 
			 <xsl:otherwise>
			  <xsl:value-of select ="concat(PBUniqueID, $i)"/>
			 </xsl:otherwise>
			</xsl:choose>
			
          </TradeId>
        

          <xsl:variable name="varTaxlotStateGrp">
            <xsl:choose>
              <xsl:when test="TaxLotState='N'">
                <xsl:value-of select ="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='A'">
                <xsl:value-of select ="'A'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='C'">
                <xsl:value-of select ="'C'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="'N'"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>
          
          <TradeStatus>
            <xsl:value-of select="$varTaxlotStateGrp"/>
          </TradeStatus>
          <xsl:variable name="PRANA_COUNTERPARTY">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_COUNTERPARTY">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = 'GS']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@PBBroker"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY = ''">
                <xsl:value-of select="$PRANA_COUNTERPARTY"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COUNTERPARTY"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <ExecutingBroker>
            <xsl:value-of select="$varCounterParty"/>
          </ExecutingBroker>
          
          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>
          
          <BLOOMBERG>
            <xsl:value-of select="BBCode"/>
          </BLOOMBERG>
          
          <RIC>
            <xsl:value-of select="RIC"/>
          </RIC>
          
          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>