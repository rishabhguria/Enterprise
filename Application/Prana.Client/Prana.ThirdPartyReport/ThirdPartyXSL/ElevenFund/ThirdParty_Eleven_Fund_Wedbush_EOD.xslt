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


        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <Account >
          <xsl:value-of select="'Account'"/>
        </Account >
        <Type>
          <xsl:value-of select="'Type'"/>
        </Type>

        <FunctionField>
          <xsl:value-of select="'FunctionField'"/>
        </FunctionField>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <BS>
          <xsl:value-of select="'B/S'"/>
        </BS>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <SellerOption>
          <xsl:value-of select="'Seller/Option'"/>
        </SellerOption>


        <NASDEXCH>
          <xsl:value-of select="'NASD/EXCH'"/>
        </NASDEXCH>

        <Contra>
          <xsl:value-of select="'Contra'"/>
        </Contra>

        <Discount1>
          <xsl:value-of select="'Discount 1'"/>
        </Discount1>

        <Discount2>
          <xsl:value-of select="'Discount 2'"/>
        </Discount2>

        <Gross1>
          <xsl:value-of select="'Gross 1'"/>
        </Gross1>

        <Gross2>
          <xsl:value-of select="'Gross 2'"/>
        </Gross2>

        <NP>
          <xsl:value-of select="'NP'"/>
        </NP>

        <CFMNT>
          <xsl:value-of select="'CFM/NT'"/>
        </CFMNT>


        <RTP>
          <xsl:value-of select="'RTP'"/>
        </RTP>

        <Prin>
          <xsl:value-of select="'Prin'"/>
        </Prin>

        <SECFee>
          <xsl:value-of select="'SECFee'"/>
        </SECFee>

        <Comm>
          <xsl:value-of select="'Comm'"/>
        </Comm>

        <AsOf>
          <xsl:value-of select="'AsOf'"/>
        </AsOf>

        <Settle>
          <xsl:value-of select="'Settle'"/>
        </Settle>

        <SN>
          <xsl:value-of select="'SN'"/>
        </SN>

        <Reg>
          <xsl:value-of select="'Reg'"/>
        </Reg>

        <AD>
          <xsl:value-of select="''"/>
        </AD>

        <AT>
          <xsl:value-of select="'AT'"/>
        </AT>

        <OA>
          <xsl:value-of select="'OA'"/>
        </OA>

        <ET>
          <xsl:value-of select="'ET'"/>
        </ET>

        <OE>
          <xsl:value-of select="'OE'"/>
        </OE>

        <SpecTypeCode>
          <xsl:value-of select="'Spec Type Code'"/>
        </SpecTypeCode>

        <End>
          <xsl:value-of select="'End'"/>
        </End>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset!='EquityOption']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
		  
		  	<xsl:variable name="PB_NAME" select="'Wedbush'"/>


          <Account >           
            <xsl:value-of select="AccountNo"/>
          </Account >
          <Type>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Sell'">
                <xsl:value-of select="'2'"/>
              </xsl:when>
              
              <xsl:when test="Side='Buy to Close' or Side='Sell short' or  Side='Sell to Close'">
                <xsl:value-of select="'2'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Type>

          <FunctionField>
            <xsl:value-of select="'HAND'"/>
          </FunctionField>

          <Symbol>
            <xsl:choose>
			
			<xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:when test="CUSIP !=''">
                <xsl:value-of select="translate(Symbol,'.','/')"/>
              </xsl:when>
           
              <xsl:otherwise>
                <xsl:value-of select="translate(Symbol,'.','/')"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>

          <BS>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
			  <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BS>

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
                <xsl:value-of select="format-number(AveragePrice,'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Price>

          <SellerOption>
            <xsl:value-of select="''"/>
          </SellerOption>


          <NASDEXCH>
           <xsl:choose>
              <xsl:when test="Exchange='OTC'">
                <xsl:value-of select="'OTC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'OTC'"/>
              </xsl:otherwise>
            </xsl:choose>
          </NASDEXCH>
		  
		  <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterPartyID"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
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

          <Contra>
            <xsl:value-of select="$Broker"/>
          </Contra>

          
          <xsl:variable name="varCommissionFlat">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
		  
          <Discount1>
		  <xsl:choose>
              <xsl:when test="$varCommissionFlat='0'">
                <xsl:value-of select="'0.99'"/>
              </xsl:when>
              <xsl:otherwise>
    <xsl:value-of select="format-number($varCommissionFlat,'0.####')"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </Discount1>

          <Discount2>
		  	  <xsl:choose>
              <xsl:when test="$varCommissionFlat='0'">
                <xsl:value-of select="'P'"/>
              </xsl:when>
              <xsl:otherwise>
            <xsl:value-of select="'F'"/>
              </xsl:otherwise>
            </xsl:choose>
           

          </Discount2>

          <Gross1>
            <xsl:value-of select="''"/>
          </Gross1>

          <Gross2>
            <xsl:value-of select="''"/>
          </Gross2>

          <NP>
            <xsl:value-of select="''"/>
          </NP>

          <CFMNT>
            <xsl:value-of select="''"/>
          </CFMNT>


          <RTP>
            <xsl:value-of select="''"/>
          </RTP>
          <xsl:variable name="varPrincipal">
            <xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
          </xsl:variable>
          <Prin>
            <xsl:value-of select="format-number($varPrincipal,'0.##')"/>
          </Prin>

          <SECFee>
            <xsl:value-of select="''"/>
          </SECFee>

          <Comm>
            <xsl:value-of select="''"/>
          </Comm>

          <AsOf>
            <xsl:value-of select="TradeDate"/>
          </AsOf>

          <Settle>
            <xsl:value-of select="SettlementDate"/>
          </Settle>

          <SN>
            <xsl:value-of select="'N'"/>
          </SN>

          <Reg>
            <xsl:choose>
              <xsl:when test="Asset='FixedIncome'">
                <xsl:value-of select="'N'"/>
              </xsl:when>

              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'O'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'O'"/>
              </xsl:otherwise>
            </xsl:choose>
          </Reg>

          <AD>
            <xsl:value-of select="''"/>
          </AD>

          <AT>
            <xsl:value-of select="''"/>
          </AT>

          <OA>
            <xsl:value-of select="''"/>
          </OA>

          <ET>
            <xsl:value-of select="''"/>
          </ET>

          <OE>
            <xsl:value-of select="''"/>
          </OE>

          <SpecTypeCode>
            <xsl:value-of select="''"/>
          </SpecTypeCode>

          <End>
            <xsl:value-of select="'END'"/>
          </End>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>