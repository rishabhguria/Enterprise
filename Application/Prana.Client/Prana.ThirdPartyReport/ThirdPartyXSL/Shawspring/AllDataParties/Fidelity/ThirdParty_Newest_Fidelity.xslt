<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>      

      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='FIDE']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <xsl:variable name="PB_NAME" select="'Meraki'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <AllocationAccount >
            <!--<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>-->
            <xsl:value-of select="AccountNo"/>
          </AllocationAccount >


          <BlockAccount>
            <xsl:value-of select="'911223735'"/>
          </BlockAccount>

          <Ticker>
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
          </Ticker>		


          <Action>            
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'CS'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                    <xsl:value-of select="'SS'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close' or Side='Sell'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>                
            </xsl:choose>
          </Action>

          <AllocationQuantity>

            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AllocationQuantity>


          	
          <Price>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Price>

          <RR>
            <xsl:value-of select="''"/>
          </RR>

          <AccountType>
            <xsl:value-of select="''"/>
          </AccountType>		

          <xsl:variable name="COMM">
            <xsl:value-of select="CommissionCharged"/>
          </xsl:variable>

          <xsl:variable name="COMM2">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <CommissionRate>
            <xsl:choose>
              <xsl:when test="number($COMM2)">
                <xsl:value-of select="format-number($COMM2 div (AllocatedQty),'0.####')"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CommissionRate>

          <CommissionCode>
            <xsl:value-of select="''"/>
          </CommissionCode>

          <BlockName>
            <xsl:value-of select="''"/>
          </BlockName>

          <OpenClose>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell to Open'  or Side='Sell short'">
                <xsl:value-of select="'Open'"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="'Close'"/>
              </xsl:otherwise>
            </xsl:choose>
          </OpenClose>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>