<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

 
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='Equity' and TradeCurrency !='USD']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>


          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side= 'Buy to Open'">
                <xsl:value-of select ="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select ="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                <xsl:value-of select ="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover' ">
                <xsl:value-of select ="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <BS>           
            <xsl:choose>
              <xsl:when test="AllocationNo='1'">
                <xsl:value-of select="$varSide"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BS>

          <SEDOL>
            <xsl:choose>
              <xsl:when test="AllocationNo='1'">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SEDOL>

          <Type>

            <xsl:choose>
              <xsl:when test="AllocationNo='1'">
                <xsl:value-of select="'SEDOL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Type>

        

          <CounterpartyID>
            <xsl:choose>
              <xsl:when test="AllocationNo='1'">
                <xsl:value-of select="CounterParty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CounterpartyID>
	
          <Block_Quantity>
            <xsl:choose>
              <xsl:when test="BlockQuantity !=0">
                <xsl:value-of select="BlockQuantity"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>            
          </Block_Quantity>

          <Alloc_Quantity>
            <xsl:choose>
              <xsl:when test="Quantity !=0">
                <xsl:value-of select="Quantity"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Alloc_Quantity>

          <Price>
            <xsl:value-of select="AvgPrice"/>
           </Price>

          <AccountID>
            <xsl:value-of select="AccountName"/>
          </AccountID>

          <AllocNumber>
            <xsl:value-of select="AllocationNo"/>
          </AllocNumber>

          <AllocAmountType>
            <xsl:value-of select="'Value'"/>
          </AllocAmountType>

          <CommissionValue>            
                <xsl:value-of select="Commission"/>              
          </CommissionValue>

          <COM_TYPE>
            <xsl:value-of select="'abs'"/>
          </COM_TYPE>


          <QTY_TYPE>
            <xsl:value-of select="'FLAT'"/>
          </QTY_TYPE>

          
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>