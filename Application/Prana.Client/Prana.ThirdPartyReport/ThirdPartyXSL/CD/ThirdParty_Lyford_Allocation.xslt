<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth = '01'">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '02'">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '03'">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '04'">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '05'">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '06'">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '07'">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '08'">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '09'">
        <xsl:value-of select="'U'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '10'">
        <xsl:value-of select="'V'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '11'">
        <xsl:value-of select="'X'"/>
      </xsl:when>
      <xsl:when test="$varMonth = '12'">
        <xsl:value-of select="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <!--<ThirdPartyFlatFileDetail>
        --><!-- for system use only--><!--
        <FileHeader>
          <xsl:value-of select ="'true'"/>
        </FileHeader>
       
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        --><!--for system use only--><!--
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        --><!--for system internal use--><!--
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <AccountReference>
          <xsl:value-of select="'Account Reference'"/>
        </AccountReference>

        <House_Client>
          <xsl:value-of select="'House/Client'"/>
        </House_Client>

        <BuySellIndicator>
          <xsl:value-of select="'Buy/ Sell Indicator'"/>
        </BuySellIndicator>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <ExchangeCode>
          <xsl:value-of select="'Exchange/ Exchange Code'"/>
        </ExchangeCode>

        <ProductInstrument>
          <xsl:value-of select="'Product/ Instrument'"/>
        </ProductInstrument>

        <ProductDescription>
          <xsl:value-of select="'Product Description'"/>
        </ProductDescription>

        <OptionStyle>
          <xsl:value-of select="'Option Style'"/>
        </OptionStyle>

        <OptionsSeries>
          <xsl:value-of select="'Options Series'"/>
        </OptionsSeries>

        <Year_Month>
          <xsl:value-of select="'Year/Month'"/>
        </Year_Month>

        <Put_Call>
          <xsl:value-of select="'Put/ Call Indicator'"/>
        </Put_Call>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <StrikePrice>
          <xsl:value-of select="'Strike Price'"/>
        </StrikePrice>

        <ExchangeOrderNo>
          <xsl:value-of select="'Exchange Order No.'"/>
        </ExchangeOrderNo>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <OrderType>
          <xsl:value-of select="'Order Type'"/>
        </OrderType>

        <OrderAllocationID>
          <xsl:value-of select="'Order Allocation ID'"/>
        </OrderAllocationID>

        <ExecutionMethod>
          <xsl:value-of select="'Execution Method'"/>
        </ExecutionMethod>

        --><!-- system use only--><!--
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>-->

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <!--<FileHeader>
            <xsl:value-of select ="'true'"/>
          </FileHeader>-->
          
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'false'"/>
          </IsCaptionChangeRequired>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <xsl:variable name="PRANA_ACCOUNT" select="AccountName"/>
          <xsl:variable name="PB_ACCOUNTID">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_FundMapping.xml')/FundMapping/PB[@Name='Knight']/FundData[@PranaFund=$PRANA_ACCOUNT]/@PBFundCode"/>
          </xsl:variable>


          <AccountReference>
            <xsl:choose>
              <xsl:when test="$PB_ACCOUNTID != ''">
                <xsl:value-of select="$PB_ACCOUNTID"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountReference>

          <House_Client>
            <xsl:value-of select="''"/>
          </House_Client>

          <xsl:variable name="Sidevar">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <BuySellIndicator>
            <xsl:value-of select="$Sidevar"/>
          </BuySellIndicator>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <ExchangeCode>
            <xsl:value-of select="Exchange"/>
          </ExchangeCode>

          <ProductInstrument>
            <xsl:choose>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="RIC"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="BBCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </ProductInstrument>

          <ProductDescription>
            <xsl:value-of select="FullSecurityName"/>
          </ProductDescription>

          <OptionStyle>
            <xsl:value-of select="''"/>
          </OptionStyle>

          <OptionsSeries>
            <xsl:value-of select="''"/>
          </OptionsSeries>

          <xsl:variable name="varMonthCode">
            <xsl:call-template name="MonthCode">
              <xsl:with-param name="varMonth" select="substring-before(ExpirationDate,'/')"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varYear">
                <xsl:value-of select="substring(ExpirationDate,10,1)"/>
          </xsl:variable>

          <Year_Month>
            <xsl:choose>
              <xsl:when test="AssetID = 3">
                <xsl:value-of select="concat($varMonthCode,$varYear)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Year_Month>

          <Put_Call>
            <xsl:value-of select="substring(PutOrCall,1,1)"/>
          </Put_Call>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <ExchangeOrderNo>
            <xsl:value-of select="''"/>
          </ExchangeOrderNo>

          <Broker>
            <xsl:value-of select="''"/>
          </Broker>

          <OrderType>
            <xsl:value-of select="''"/>
          </OrderType>

          <OrderAllocationID>
            <xsl:value-of select="TradeRefID"/>
          </OrderAllocationID>

          <ExecutionMethod>
            <xsl:value-of select="''"/>
          </ExecutionMethod>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
