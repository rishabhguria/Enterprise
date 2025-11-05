<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <!--<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>-->
        <Client>

          <xsl:value-of select="'Client'"/>
        </Client>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'SettlementDate'"/>
        </SettlementDate>

        <SubAcctShortName>
          <xsl:value-of select="'SubAcctShortName'"/>
        </SubAcctShortName>

        <BackOfficeAcct>
          <xsl:value-of select="'BackOfficeAcct'"/>
        </BackOfficeAcct>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <CommType>
          <xsl:value-of select="'CommType'"/>
        </CommType>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>


      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          
          <Client>
            <xsl:value-of select="'CHIMECAP'"/>
          </Client>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <Side>
            <xsl:choose>
              <xsl:when test="Side='B'">
                <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test="Side='S'">
                <xsl:value-of select="'Sell'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Side>

          <Price>
            <xsl:value-of select="AvgPrice"/>
          </Price>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <SubAcctShortName>
            <xsl:value-of select="AccountName"/>
          </SubAcctShortName>

 

          <BackOfficeAcct>
            <xsl:value-of select="CounterParty"/>
          </BackOfficeAcct>

          <Quantity>
            <xsl:value-of select="OrderQty"/>
          </Quantity>

          <CommType>
            <xsl:value-of select="'RPS'"/>
          </CommType>

          <Commission>
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </Commission>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
