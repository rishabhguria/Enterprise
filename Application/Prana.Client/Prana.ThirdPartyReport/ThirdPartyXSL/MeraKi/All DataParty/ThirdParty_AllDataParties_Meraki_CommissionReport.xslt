<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME" select="'MerakiCommReport'"/>

          <xsl:variable name="varPRANA_AccountName">
            <xsl:value-of select="Client"/>
          </xsl:variable>
          <xsl:variable name="varMappedAccountCode">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$varPRANA_AccountName]/@PBFundCode"/>
          </xsl:variable>


          <Client>
            <xsl:choose>
              <xsl:when test="$varMappedAccountCode != ''">
                <xsl:value-of select="$varMappedAccountCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varPRANA_AccountName"/>
              </xsl:otherwise>
            </xsl:choose>
          </Client>


          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <TransactionType>
            <xsl:value-of select="TransactionType"/>
          </TransactionType>

          <!-- <Side> -->
          <!-- <xsl:choose> -->
          <!-- <xsl:when test="Side='Buy' or Side='Buy to Open'"> -->
          <!-- <xsl:value-of select="'B'"/> -->
          <!-- </xsl:when> -->
          <!-- <xsl:when test="Side='Sell' or Side='Sell to Close'"> -->
          <!-- <xsl:value-of select="'SL'"/> -->
          <!-- </xsl:when> -->
          <!-- <xsl:when test="Side='Sell short' or Side='Sell to Open'"> -->
          <!-- <xsl:value-of select="'SS'"/> -->
          <!-- </xsl:when> -->
          <!-- <xsl:when test="Side='Buy to Close'"> -->
          <!-- <xsl:value-of select="'CS'"/> -->
          <!-- </xsl:when> -->
          <!-- <xsl:otherwise> -->
          <!-- <xsl:value-of select="Side"/> -->
          <!-- </xsl:otherwise> -->
          <!-- </xsl:choose> -->
          <!-- </Side> -->

          <Quantity>
            <xsl:value-of select="Quantity"/>
          </Quantity>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <Price>
            <xsl:value-of select="format-number(Price,'#.####')"/>
          </Price>

          <Crcy>
            <xsl:value-of select="Crcy"/>
          </Crcy>

          <GrossMoney>
            <xsl:value-of select="format-number(GrossMoney,'#.##')"/>
          </GrossMoney>

          <xsl:variable name="varPRANA_BrokerShortName">
            <xsl:value-of select="BrokerCode"/>
          </xsl:variable>
          <xsl:variable name="varMappedBrokerCode">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$varPRANA_BrokerShortName]/@PBBroker"/>
          </xsl:variable>

          <BrokerCode>
            <xsl:choose>
              <xsl:when test="$varMappedBrokerCode != ''">
                <xsl:value-of select="$varMappedBrokerCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varPRANA_BrokerShortName"/>
              </xsl:otherwise>
            </xsl:choose>
          </BrokerCode>

          <BrokerCommRate>
            <xsl:choose>
              <xsl:when test="Crcy = BaseCurrency or Crcy = 'CAD'">
                <xsl:value-of select="(format-number(BrokerCommRate,'#.####'))"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="(format-number(BrokerCommRate,'#.####'))"/>
              </xsl:otherwise>
            </xsl:choose>
          </BrokerCommRate>
		  
		    <RateType>
            <xsl:choose>
              <xsl:when test="UnderLyingName = 'US'">
                <xsl:value-of select="'CPS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'BPS'"/>
              </xsl:otherwise>
            </xsl:choose>
          </RateType>

          <MerakiCommRate>
            <xsl:choose>
              <xsl:when test="Crcy = BaseCurrency or Crcy = 'CAD'">
                <xsl:value-of select="(format-number(MerakiCommRate,'#.####'))"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="(format-number(MerakiCommRate,'#.####'))"/>
              </xsl:otherwise>
            </xsl:choose>
          </MerakiCommRate>
		  
		   <RateType1>
            <xsl:choose>
              <xsl:when test="UnderLyingName = 'US'">
                <xsl:value-of select="'CPS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'BPS'"/>
              </xsl:otherwise>
            </xsl:choose>
          </RateType1>

          <UserAssetClass>
		   <xsl:choose>
              <xsl:when test="IsSwap='true'">
                <xsl:value-of select="'SWAP'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="UserAssetClass"/>
              </xsl:otherwise>
            </xsl:choose>
          </UserAssetClass>

          <HardCommission>
            <xsl:value-of select="format-number(HardCommission_Base,'#.##')"/>
          </HardCommission>

          <SoftCommission>
            <xsl:value-of select="format-number(SoftCommission_Base,'#.##')"/>
          </SoftCommission>

          <MerakiCommission>
            <xsl:value-of select="format-number(MerakiCommission_Base,'#.##')"/>
          </MerakiCommission>

          <TotalCommission>
            <xsl:value-of select="format-number((SoftCommission_Base + HardCommission_Base + MerakiCommission_Base),'#.##')"/>
          </TotalCommission>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>