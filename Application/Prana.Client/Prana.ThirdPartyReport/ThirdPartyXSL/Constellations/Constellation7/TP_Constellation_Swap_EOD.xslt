<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">  
	
    <ThirdPartyFlatFileDetailCollection>

     

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'COR'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CAN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <TransStatus>
            <xsl:value-of select ="$varTaxlotState"/>
          </TransStatus>
          
          <BuySell>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Cover'">
                    <xsl:value-of select="'Buytocover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'Sellshort'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buytocover'"/>
                  </xsl:when>

                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'Sellshort'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </BuySell>
          
          <LongShort>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side= 'Buy to Open'">
                <xsl:value-of select="'Long'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side = 'Sell to Close'">
                <xsl:value-of select="'Long'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side ='Buy to Cover'">
                <xsl:value-of select="'Short'"/>
              </xsl:when>

              <xsl:when test="Side='Sell short' or Side ='Sell to Open'">
                <xsl:value-of select="'Short'"/>
              </xsl:when>
            </xsl:choose>
          </LongShort>
          
          <ClientRef>
            <xsl:value-of select ="EntityID"/>
          </ClientRef>
          
          <AccountID>
            <xsl:value-of select ="'038211165'"/>
          </AccountID>

          <xsl:variable name="PB_NAME" select="''"/>
          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
          <xsl:variable name="THIRDPARTY_BROKER">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
          </xsl:variable>


          <ExecBkr>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_BROKER!= ''">
                <xsl:value-of select="$THIRDPARTY_BROKER"/>
              </xsl:when>
              <xsl:when test="CounterParty='RBCZ' or CounterParty='RBCB'">
                <xsl:value-of select="'RBCM'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExecBkr>
          
          <SecType>
            <xsl:value-of select ="'Swap'"/>
          </SecType>
          
          <SecID>
            <xsl:value-of select ="BBCode"/>
          </SecID>
          
          <desc>
            <xsl:value-of select ="FullSecurityName"/>
          </desc>
          
          <TDate>
            <xsl:value-of select ="TradeDate"/>
          </TDate>
          
          <SDate>
            <xsl:value-of select ="SettlementDate"/>
          </SDate>
          
          <SCCY>
            <xsl:value-of select ="SettlCurrency"/>
          </SCCY>
          
          <qty>
            <xsl:value-of select ="AllocatedQty"/>
          </qty>
          
          <Price>
            <xsl:value-of select ="AveragePrice"/>
          </Price>

          

          <prin>
            <xsl:value-of select ="GrossAmount"/>
          </prin>
          
          <xsl:variable name = "varTotalComm">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
          <comm>
            <xsl:value-of select ="$varTotalComm"/>
          </comm>

          <Taxfees>
            <xsl:value-of select ="TransactionLevy"/>
          </Taxfees>
          
          <Tax2>
            <xsl:value-of select ="0"/>
          </Tax2>
          
          <interest>
            <xsl:value-of select ="''"/>
          </interest>
          
          <interestindicator>
            <xsl:value-of select ="''"/>
          </interestindicator>
          
         
          <netamount>
            <xsl:value-of select ="NetAmount"/>
          </netamount>

          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>