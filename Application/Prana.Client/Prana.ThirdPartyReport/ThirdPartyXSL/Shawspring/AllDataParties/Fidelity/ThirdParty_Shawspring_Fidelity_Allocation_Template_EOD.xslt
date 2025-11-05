<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <FileHeader>
          <xsl:value-of select="'true'"/>
        </FileHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>


       

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate >


        <SettlementDate>
          <xsl:value-of select="'SettlementDate'"/>
        </SettlementDate>

        <CusipSymbol>
          <xsl:value-of select="'Cusip/Symbol'"/>
        </CusipSymbol>


        <BUYBCSLSSSE>
          <xsl:value-of select="'BUY/BC/SL/SS/SE'"/>
        </BUYBCSLSSSE>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>



        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Intrest>
          <xsl:value-of select="'Interest'"/>
        </Intrest>



       

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <SECFee>
          <xsl:value-of select="'SEC Fee'"/>
        </SECFee>

        <NetMoney>
          <xsl:value-of select="'Net Money'"/>
        </NetMoney>

        <ExecutingBroker>
          <xsl:value-of select="'Executing Broker'"/>
        </ExecutingBroker>

        <ExecutingBrokerDTC>
          <xsl:value-of select="'Executing Broker DTC#'"/>
        </ExecutingBrokerDTC>

        <ClientAccount>
          <xsl:value-of select="'ClientAccount'"/>
        </ClientAccount>

        <Type>
          <xsl:value-of select="'Type'"/>
        </Type>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty !='FIDE' and CounterParty !='Undefined']">
    
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>


              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>
              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>


              <TradeDate>
               <xsl:value-of select="TradeDate"/>
             </TradeDate >


             <SettlementDate>
               <xsl:value-of select="SettlementDate"/>
             </SettlementDate>

              <CusipSymbol>
                <xsl:choose>             
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="substring-before(BBCode,' US')"/>
                  </xsl:when>
              <xsl:when test="CurrencySymbol!='USD'">
                <xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
              </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>         
             </xsl:choose>
              </CusipSymbol>


              <BUYBCSLSSSE>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'BC'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'SS'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'SL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BUYBCSLSSSE>

              <Quantity>

                <xsl:choose>
                  <xsl:when test="number(AllocatedQty)">
                    <xsl:value-of select="AllocatedQty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>



              <Price>
                <xsl:choose>
                  <xsl:when test="number(AveragePrice)">
                    <xsl:value-of select="format-number(AveragePrice,'##.0000')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Price>

              <Intrest>
                <xsl:value-of select="format-number(AccruedInterest,'##.00')"/>
              </Intrest>



              <xsl:variable name="COMM">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:variable>

              <xsl:variable name="COMM2">
                <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </xsl:variable>

              <Commission>
                <xsl:choose>
                  <xsl:when test="number($COMM2)">
                    <xsl:value-of select="format-number($COMM2,'##.00')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Commission>

              <SECFee>
                <xsl:value-of select="format-number(SecFee,'##.00')"/>
              </SECFee>

              <NetMoney>
                <xsl:value-of select="format-number(NetAmount,'##.00')"/>
              </NetMoney>

              <ExecutingBroker>
                <xsl:value-of select="CounterParty"/>
              </ExecutingBroker>

              <ExecutingBrokerDTC>
                <xsl:choose>
                  <xsl:when test="CounterParty='CCMB'">
                    <xsl:value-of select="'0443'"/>
                  </xsl:when>
                  <xsl:when test="CounterParty='CITI'">
                    <xsl:value-of select="'0505'"/>
                  </xsl:when>
                  <xsl:when test="CounterParty='NEOV'">
                    <xsl:value-of select="'1111'"/>
                  </xsl:when>
                  <xsl:when test="CounterParty='MS'">
                    <xsl:value-of select="'0050'"/>
                  </xsl:when>
                  <xsl:when test="CounterParty='UBS'">
                    <xsl:value-of select="'0642'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>               
             </ExecutingBrokerDTC>

              <ClientAccount>
                <xsl:value-of select="'663007321'"/>
              </ClientAccount>

              <Type>
                <xsl:value-of select="''"/>
              </Type>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>


      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>