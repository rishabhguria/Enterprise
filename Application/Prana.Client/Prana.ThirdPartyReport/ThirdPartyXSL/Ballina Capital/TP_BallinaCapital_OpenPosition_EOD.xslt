<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">  
	
    <ThirdPartyFlatFileDetailCollection>

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <PositionID>
            <xsl:value-of select ="'Position_ID'"/>
          </PositionID>

          <SecurityID>
            <xsl:value-of select ="'Security_ID'"/>
          </SecurityID>

          <AccountID>
            <xsl:value-of select ="'Account_ID'"/>
          </AccountID>

          <Custodian>
            <xsl:value-of select ="'Custodian'"/>
          </Custodian>

          <LongShortIndicator>
            <xsl:value-of select ="'Long_Short_Indicator'"/>
          </LongShortIndicator>

          <CostBase>
            <xsl:value-of select ="'Cost_Base'"/>
          </CostBase>

          <Quantity>
            <xsl:value-of select ="'Quantity'"/>
          </Quantity>

          <Price>
            <xsl:value-of select ="'Price'"/>
          </Price>

          <SecurityType>
            <xsl:value-of select ="'Security_Type'"/>
          </SecurityType>

          <SecurityMultiplier>
            <xsl:value-of select ="'Security_Multiplier'"/>
          </SecurityMultiplier>

          <Currency>
            <xsl:value-of select ="'Currency'"/>
          </Currency>

          <Sedol>
            <xsl:value-of select ="'Sedol'"/>
          </Sedol>

          <Cusip>
            <xsl:value-of select ="'Cusip'"/>
          </Cusip>

          <ISIN>
            <xsl:value-of select ="'ISIN'"/>
          </ISIN>

          <SecurityDescription>
            <xsl:value-of select ="'Security_Description'"/>
          </SecurityDescription>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varPositionID">
            <xsl:choose>
              <xsl:when test="string-length(position())=1">
                <xsl:value-of select=" position()"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="position()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <PositionID>
            <xsl:value-of select ="$varPositionID"/>
          </PositionID>

          <SecurityID>
            <xsl:value-of select ="Symbol"/>
          </SecurityID>

          <AccountID>
            <xsl:value-of select ="FundID"/>
          </AccountID>

          <Custodian>
            <xsl:value-of select ="''"/>
          </Custodian>
          <!--<xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="OrderSide='Buy'">
                    <xsl:value-of select="'Long'"/>
                  </xsl:when>
                  <xsl:when test="OrderSide='Sell'">
                    <xsl:value-of select="'Short'"/>
                  </xsl:when>
                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="OrderSide='Buy'">
                    <xsl:value-of select="'Long'"/>
                  </xsl:when>
                  <xsl:when test="OrderSide='Sell'">
                    <xsl:value-of select="'Short'"/>
                  </xsl:when>
                 
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>-->
          
          
          <LongShortIndicator>
            <xsl:value-of select ="OrderSide"/>
          </LongShortIndicator>

          <CostBase>
            <xsl:value-of select ="''"/>
          </CostBase>

          <Quantity>
            <xsl:value-of select ="OpenPositions"/>
          </Quantity>

          <Price>
            <xsl:value-of select ="''"/>
          </Price>

          <SecurityType>
            <xsl:value-of select ="''"/>
          </SecurityType>

          <SecurityMultiplier>
            <xsl:value-of select ="''"/>
          </SecurityMultiplier>

          <Currency>
            <xsl:value-of select ="LocalCurrency"/>
          </Currency>

          <Sedol>
            <xsl:value-of select ="SEDOLSymbol"/>
          </Sedol>

          <Cusip>
            <xsl:value-of select ="CUSIPSymbol"/>
          </Cusip>

          <ISIN>
            <xsl:value-of select ="''"/>
          </ISIN>

          <SecurityDescription>
            <xsl:value-of select ="''"/>
          </SecurityDescription>


          <EntityID>
            <xsl:value-of select="'EntityID'"/>
          </EntityID>



        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>