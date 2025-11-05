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

          <xsl:variable name="PB_NAME" select="'NE'"/>
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <ACCOUNT>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </ACCOUNT>

          <Qty>

            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Qty>

          <B_S>
            <xsl:choose>
              <xsl:when test="contains(Side,'Buy')">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:when test="contains(Side,'Sell short')">
                <xsl:value-of select="'SS'"/>
              </xsl:when>

              <xsl:when test="contains(Side,'Sell')">
                <xsl:value-of select="'S'"/>
              </xsl:when>
            </xsl:choose>
          </B_S>

          <Symbol>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>

          </Symbol>



          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <xsl:variable name="Broker">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <Exchange_Broker>

            <xsl:value-of select="$Broker"/>
          </Exchange_Broker>

          <TradePrice>

            <xsl:value-of select="AveragePrice"/>
          </TradePrice>

          <Blank>
            <xsl:value-of select="''"/>

          </Blank>

          <Blank>
            <xsl:value-of select="''"/>

          </Blank>

          <T_D>
            <xsl:value-of select="''"/>

          </T_D>

          <S_D>
            <xsl:value-of select="''"/>
          </S_D>

          <Blank>
            <xsl:value-of select="''"/>

          </Blank>

          <C_P>
            <xsl:choose>
              <xsl:when test="contains(PutOrCall,'C')">
                <xsl:value-of select="'Call'"/>
              </xsl:when>
              <xsl:when test="contains(PutOrCall,'P')">
                <xsl:value-of select="'Put'"/>
              </xsl:when>
            </xsl:choose>

          </C_P>

          <O_C>

            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Close'">
                <xsl:value-of select="'Open'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'CLose'"/>
              </xsl:otherwise>
            </xsl:choose>


          </O_C>

          <EXPIRYMONTH>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>


          </EXPIRYMONTH>

          <Strike>

            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Strike>

          <CAPACITY>
            <xsl:value-of select="'AG'"/>


          </CAPACITY>





          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

        <xsl:if test="(AccountName='075-48566' or AccountName='075-48567' )and Asset='EquityOption'">

          <ThirdPartyFlatFileDetail>

            <RowHeader>
              <xsl:value-of select ="'true'"/>
            </RowHeader>

            <TaxlotState>
              <xsl:value-of select="TaxLotState"/>
            </TaxlotState>

            <xsl:variable name="PB_NAME" select="'NE'"/>
            <xsl:variable name = "PRANA_FUND_NAME">
              <xsl:value-of select="AccountName"/>
            </xsl:variable>

            <xsl:variable name ="THIRDPARTY_FUND_CODE">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
            </xsl:variable>

            <ACCOUNT>
              <xsl:choose>
                <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                  <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </ACCOUNT>

            <Qty>

              <xsl:choose>
                <xsl:when test="number(AllocatedQty)">
                  <xsl:value-of select="AllocatedQty"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Qty>

            <B_S>
              <xsl:choose>
                <xsl:when test="contains(Side,'Buy')">
                  <xsl:value-of select="'S'"/>
                </xsl:when>

                <xsl:when test="contains(Side,'Sell short')">
                  <xsl:value-of select="'BC'"/>
                </xsl:when>

                <xsl:when test="contains(Side,'Sell')">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
              </xsl:choose>
            </B_S>

            <Symbol>
              <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:value-of select="OSIOptionSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="Symbol"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>



            <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

            <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
              <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
            </xsl:variable>

            <xsl:variable name="Broker">
              <xsl:choose>
                <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                  <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <Exchange_Broker>

              <xsl:value-of select="$Broker"/>
            </Exchange_Broker>

            <TradePrice>

              <xsl:value-of select="AveragePrice"/>
            </TradePrice>

            <Blank>
              <xsl:value-of select="''"/>

            </Blank>

            <Blank>
              <xsl:value-of select="''"/>

            </Blank>

            <T_D>
              <xsl:value-of select="''"/>

            </T_D>

            <S_D>
              <xsl:value-of select="''"/>
            </S_D>

            <Blank>
              <xsl:value-of select="''"/>

            </Blank>

            <C_P>
              <xsl:choose>
                <xsl:when test="contains(PutOrCall,'C')">
                  <xsl:value-of select="'Call'"/>
                </xsl:when>
                <xsl:when test="contains(PutOrCall,'P')">
                  <xsl:value-of select="'Put'"/>
                </xsl:when>
              </xsl:choose>

            </C_P>

            <O_C>

              <xsl:choose>
                <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Close'">
                  <xsl:value-of select="'Open'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'CLose'"/>
                </xsl:otherwise>
              </xsl:choose>


            </O_C>

            <EXPIRYMONTH>
              <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:value-of select="ExpirationDate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>


            </EXPIRYMONTH>

            <Strike>

              <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:value-of select="StrikePrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Strike>

            <CAPACITY>
              <xsl:value-of select="'AG'"/>


            </CAPACITY>





            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

          </ThirdPartyFlatFileDetail>

        </xsl:if>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>