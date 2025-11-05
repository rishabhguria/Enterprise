<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public int RoundOff(double Qty)
    {

    return (int)Math.Round(Qty,0);
    }
  </msxsl:script>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>


          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <TransactionType>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'Sell Short'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'Buy to Close'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>

          </TransactionType>
          
          <MessageFunction>
            <xsl:value-of select="'NEWM'"/>
          </MessageFunction>
          
         
          <xsl:variable name="varTradeDate">
            <xsl:value-of select="concat(substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'),substring-after(substring-after(TradeDate,'/'),'/'))"/>
          </xsl:variable>
          <xsl:variable name="i" select="position()" />

          <TransactionReference>

            <xsl:choose>
              <xsl:when test="$i &lt; 10">
                <xsl:value-of select="concat($varTradeDate,'0',$i)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="concat($varTradeDate,$i)"/>
              </xsl:otherwise>
            </xsl:choose>
          </TransactionReference>

          <RelatedReferenceNumber>
            <xsl:value-of select="''"/>
          </RelatedReferenceNumber>

			<xsl:variable name = "PRANA_FUND_NAME">
				<xsl:value-of select="AccountName"/>
			</xsl:variable>

			<xsl:variable name ="PB_FUND_CODE">
				<xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
			</xsl:variable>

			<FundID>
				<xsl:choose>
					<xsl:when test ="$PB_FUND_CODE!=''">
						<xsl:value-of select ="$PB_FUND_CODE"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</FundID>

          <TradeDate>
            <xsl:value-of select="$varTradeDate"/>
          </TradeDate>

          <xsl:variable name="varSettlementDate">
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </xsl:variable>

          <SettlementDate>
            <xsl:value-of select="$varSettlementDate"/>
          </SettlementDate>

      
         

          <SecurityIDType>
            <xsl:value-of select="'ISIN'"/>
          </SecurityIDType>


          <SecurityID>
            <xsl:value-of select="ISIN"/>
          </SecurityID>

          <xsl:variable name ="varQty">
            <xsl:value-of select="my:RoundOff(AllocatedQty)"/>
          </xsl:variable>

          <Quantity>
            <xsl:choose>
              <xsl:when test="number($varQty)">
                <xsl:value-of select="$varQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Quantity>

          <TradeCurrency>
            <xsl:value-of select="'USD'"/>
          </TradeCurrency>

          <DealPriceCode>
            <xsl:value-of select="'ACTU'"/>
          </DealPriceCode>

          <DealPrice>
            <xsl:value-of select="format-number(AveragePrice,'##.####')"/>
          </DealPrice>

          <PrincipalAmount>
            <xsl:value-of select="format-number(GrossAmount,'0.##')"/>
          </PrincipalAmount>



          <xsl:variable name="varCommission">
            <xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
          </xsl:variable>
          <CommissionsAmount>
            <xsl:choose>
              <xsl:when test="number($varCommission)">
                <xsl:value-of select="format-number($varCommission,'0.##')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
          </CommissionsAmount>



          <ChargesFeesAmount>
            <xsl:value-of select="format-number(StampDuty,'0.##')"/>
          </ChargesFeesAmount>



          <SettlementCurrency>
            <xsl:value-of select="'USD'"/>
          </SettlementCurrency>



          <SettlementAmount>
            <xsl:choose>
              <xsl:when test="number(NetAmount)">
                <xsl:value-of select="format-number(NetAmount,'0.##')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>

          </SettlementAmount>

          <TransactionSubType>
            <xsl:value-of select="'TRAD'"/>
          </TransactionSubType>

          <SettlementLocation>
            <xsl:value-of select="'DTCYUS33'"/>
          </SettlementLocation>


          <ExecutingBrokerIDType>
            <xsl:value-of select="'DTCYID'"/>
          </ExecutingBrokerIDType>


          <ExecutingBrokerID>
            <xsl:choose>
              <xsl:when test="CounterParty='JPM'">
                <xsl:value-of select="'352'"/>
              </xsl:when>
              <xsl:when test="CounterParty='BGCE'">
                <xsl:value-of select="'161'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMHI'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMBC'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
              <xsl:when test="CounterParty='RJET'">
                <xsl:value-of select="'725'"/>
              </xsl:when>
            </xsl:choose>
          </ExecutingBrokerID>




          <ClearingBrokerAgentIDType>
            <xsl:value-of select="'DTCYID'"/>
          </ClearingBrokerAgentIDType>

          <ClearingBrokerAgentID>
            <xsl:choose>
              <xsl:when test="CounterParty='JPM'">
                <xsl:value-of select="'352'"/>
              </xsl:when>
              <xsl:when test="CounterParty='BGCE'">
                <xsl:value-of select="'161'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMHI'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
              <xsl:when test="CounterParty='SMBC'">
                <xsl:value-of select="'443'"/>
              </xsl:when>
              <xsl:when test="CounterParty='RJET'">
                <xsl:value-of select="'725'"/>
              </xsl:when>
            </xsl:choose>
          </ClearingBrokerAgentID>

     
          <EntityID>
            <xsl:value-of select="''"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
