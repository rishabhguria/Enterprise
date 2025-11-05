<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <type>
            <xsl:value-of select="'away_trade'"/>
          </type>

          <xsl:variable name="varTDates" select="concat(substring-after(substring-after(TradeDate,'/'),'/'), substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          <xsl:variable name="varPosition" select="position()" />
          
          <client_trade_id>
            <xsl:choose>
              <xsl:when test="$varPosition &lt; 10">
                <xsl:value-of select="concat('Lyrical',$varTDates,'0',$varPosition)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="concat('Lyrical',$varTDates,$varPosition)"/>
              </xsl:otherwise>
            </xsl:choose>

          </client_trade_id>
          
          <timestamp>
            <xsl:value-of select="''"/>
          </timestamp>
          
          
          <date>
            <xsl:value-of select="$varTDates"/>
          </date>

          <xsl:variable name="PB_NAME" select="'Lyrical'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
          </xsl:variable>
          <account_id>
            <xsl:choose>
              <xsl:when test ="$THIRDPARTY_FUND_CODE != ''">
                <xsl:value-of select ="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </account_id>
          
          <quantity>
            <xsl:value-of select="AllocatedQty"/>
          </quantity>
          
          <price>
            <xsl:value-of select="format-number(AveragePrice,'0.####')"/>
          </price>
          
          <behalf_of_account_id>
            <xsl:value-of select="''"/>
          </behalf_of_account_id>
          
          <registered_rep>
            <xsl:value-of select="''"/>
          </registered_rep>
          
          <branch_office>
            <xsl:value-of select="''"/>
          </branch_office>
          
          <instrument.identifier>
            <xsl:value-of select="Symbol"/>
          </instrument.identifier>
          
          <instrument.identifier_type>
            <xsl:value-of select="'ticker'"/>
          </instrument.identifier_type>
          
          <instrument.country>
            <xsl:value-of select="'USA'"/>
          </instrument.country>
          
          <instrument.currency>
            <xsl:value-of select="'USD'"/>
          </instrument.currency>
          
          <side.direction>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'buy'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'sell'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </side.direction>
          
          <side.qualifier>
            <xsl:value-of select="''"/>
          </side.qualifier>
          
          <capacity>
            <xsl:value-of select="'agency'"/>
          </capacity>
          
          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>       
					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@MLPBroker"/>
					</xsl:variable>

          <exec_mpid>
           <xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
          </exec_mpid>
          
          <contra_mpid>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </contra_mpid>
          
          <xsl:variable name="varCounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name ="varDTCCode">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerDTCMapping.xml')/BrokerMapping/PB[@Name='Lyrical']/BrokerData[@PranaBroker=$varCounterParty]/@DTCCode"/>
          </xsl:variable>
         
          <contra_clearing_num>
            <xsl:choose>
                <xsl:when test ="$varDTCCode!=''">
								<xsl:value-of select ="$varDTCCode"/>
							</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varCounterParty"/>
                </xsl:otherwise>
              </xsl:choose>
          </contra_clearing_num>
          
          <fees.commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'0.##')"/>
          </fees.commission>
          
          <cancel_trade_id>
            <xsl:value-of select="''"/>
          </cancel_trade_id>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>