<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormate">
    <xsl:param name="Date"/>

    <xsl:variable name="varMonth">
      <xsl:value-of select="substring-before($Date,'/')"/>
    </xsl:variable>

    <xsl:variable name="varDay">
      <xsl:value-of select="substring-before(substring-after($Date,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:value-of select="substring-after(substring-after($Date,'/'),'/')"/>
    </xsl:variable>

    <xsl:value-of select="concat($varDay,'-',$varMonth,'-',$varYear)"/>

  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

      <xsl:for-each select="ThirdPartyFlatFileDetail[(AccountName='Sheffield Partners LP-JPM' or AccountName='Sheffield Partners LP-MS' or AccountName='Sheffield Partners LP-MS SWAP') and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">

        <ThirdPartyFlatFileDetail>
            <RowHeader>
              <xsl:value-of select ="'true'"/>
            </RowHeader>

            <!--for system internal use-->
            <TaxLotState>
              <xsl:value-of select ="TaxLotState"/>
            </TaxLotState>

            <TradeDate>
              <xsl:value-of select ="TradeDate"/>
            </TradeDate>

            <SettleDate>
              <xsl:value-of select="SettlementDate"/>
            </SettleDate>

            <OrderSide>
              <xsl:value-of select="Side"/>
            </OrderSide>
			<Asset>
				<xsl:choose>
				<!--<xsl:value-of select="Asset"/>-->
				<xsl:when test="IsSwapped='true' and Asset = 'Equity'">
					<xsl:value-of select ="'EquitySwap'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Asset"/>
				</xsl:otherwise>
					</xsl:choose>
			</Asset>		

			<Symbol>
				<!--<xsl:value-of select="substring-before(BBCode,' EQUITY')"/>-->
				<xsl:choose>
					<xsl:when test="contains(BBCode,'EQUITY')">
						<xsl:value-of select ="BBCode"/>
					</xsl:when>
					<!--<xsl:when test="contains(BBCode,'Equity')">
						<xsl:value-of select ="substring-before(BBCode,' Equity')"/>
					</xsl:when>-->
					<xsl:otherwise>
						<xsl:value-of select ="Symbol"/>
					</xsl:otherwise>
				</xsl:choose>
			</Symbol>			
			<xsl:variable name="varFullSecurityName">
				<xsl:choose>
					<xsl:when test="contains(FullSecurityName,',')">
						<xsl:value-of select="translate(FullSecurityName,',','-')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="FullSecurityName"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<SecurityDescription>
				<!--<xsl:value-of select="FullSecurityName"/>-->
				<xsl:value-of select="$varFullSecurityName"/>
			</SecurityDescription>
			<TradeQty>
				<xsl:value-of select="AllocatedQty"/>
			</TradeQty>

			<AvgPrice>
              <xsl:value-of select="AveragePrice"/>
            </AvgPrice>		

            <GrossPrincipal>
              <xsl:value-of select="format-number(GrossAmount,'#.####')"/>
            </GrossPrincipal>

            <Commission>
              <xsl:value-of select="format-number(CommissionCharged,'0.####')"/>
            </Commission>

            <xsl:variable name="varFees">
              <xsl:value-of select="OtherBrokerFee + ClearingBrokerFee  + ClearingFee + MiscFees +  OrfFee + OccFee + SecFee + StampDuty + TransactionLevy"/>
            </xsl:variable>
            <SumOfAllFees>
              <xsl:value-of select="$varFees"/>
            </SumOfAllFees>

            <!--<xsl:variable name="varNetAmountBase">
              <xsl:value-of select="NetAmount * ForexRate_Trade "/>
            </xsl:variable>-->
            
            <NetSettlement>
              <xsl:value-of select="format-number(NetAmount,'#.####')"/>
            </NetSettlement>

            <Broker>
              <xsl:value-of select="CounterParty"/>
            </Broker>

            <Custodian>
              <xsl:choose>
                <xsl:when test="AccountName='Sheffield Partners LP-JPM'">
                  <xsl:value-of select="'JPMS'"/>
                </xsl:when>
                <xsl:when test="AccountName='Sheffield Partners LP-MS' or AccountName='Sheffield Partners LP-MS SWAP'">
                  <xsl:value-of select="'MSCO'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Custodian>

            <!-- system inetrnal use-->
            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>