<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <AccountNumber>
          <xsl:value-of select="'Account Number'"/>
        </AccountNumber>

        <AccountType>
          <xsl:value-of select="'Account Type'"/>
        </AccountType>

        <TICKER>
          <xsl:value-of select="'TICKER'"/>
        </TICKER>

        <CODE>
          <xsl:value-of select="'CODE'"/>
        </CODE>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <SECFee>
          <xsl:value-of select="'Sec Fee'"/>
        </SECFee>

        <NetMoney>
          <xsl:value-of select="'Net Money'"/>
        </NetMoney>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <DTCSettlement>
          <xsl:value-of select="'DTC Settlement'"/>
        </DTCSettlement>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
         
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME" select="'Evolve'"/>

          
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
          </xsl:variable>
      
          <AccountNumber>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNumber>
          
          <AccountType>
            <xsl:value-of select="'Cash'"/>
          </AccountType>
          
          <TICKER>
            <xsl:value-of select="Symbol"/>
          </TICKER>
          
          <CODE>
          <xsl:choose>
            <xsl:when test="Side='Buy'">
              <xsl:value-of select="'BUY'"/>
            </xsl:when>
            <xsl:when test="Side='Sell'">
              <xsl:value-of select="'SELL'"/>
            </xsl:when>
           
            <xsl:otherwise>
              <xsl:value-of select="Side"/>
            </xsl:otherwise>
            </xsl:choose>
          </CODE>
          
          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>
          
          <Price>
            <xsl:value-of select="format-number(AveragePrice,'0.####')"/>
          </Price>
          
          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>
          
          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>
          
          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'0.##')"/>
          </Commission>
          
          <SECFee>
            <xsl:value-of select="format-number(StampDuty,'0.##')"/>
          </SECFee>
          
          <NetMoney>
            <xsl:value-of select="format-number(NetAmount,'0.##')"/>
          </NetMoney>
          
          <CUSIP>
            <xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>
          </CUSIP>

          <xsl:variable name="varCounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name ="varDTCCode">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerDTCMapping.xml')/BrokerMapping/PB[@Name='Evolve']/BrokerData[@PranaBroker=$varCounterParty]/@DTCCode"/>
          </xsl:variable>

          <DTCSettlement>
            <xsl:choose>
                <xsl:when test ="$varDTCCode!=''">
				        <xsl:value-of select ="$varDTCCode"/>
			    	</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varDTCCode"/>
                </xsl:otherwise>
              </xsl:choose>
          </DTCSettlement>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>