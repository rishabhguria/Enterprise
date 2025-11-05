<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="false"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <!--<InstrumentSubType>
					<xsl:value-of select="'Instrument Sub Type'"/>
				</InstrumentSubType>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<LifeCycle>
					<xsl:value-of select="'Life Cycle'"/>
				</LifeCycle>-->

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <TradeType>
          <xsl:value-of select="'TradeType'"/>
        </TradeType>

        <AssetClass>
          <xsl:value-of select="'AssetClass'"/>
        </AssetClass>

        <TransactionType>
          <xsl:value-of select="'TransactionType'"/>
        </TransactionType>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <FXCCYRate>
          <xsl:value-of select="'FX/XCCYRate'"/>
        </FXCCYRate>

        <FXBUYCCY>
          <xsl:value-of select="'FXBUYCCY'"/>
        </FXBUYCCY>

        <FXBuyAmount>
          <xsl:value-of select="'FXBuyAmount'"/>
        </FXBuyAmount>

        <USDAmount>
          <xsl:value-of select="'USD Amount'"/>
        </USDAmount>

        <BrokerName>
          <xsl:value-of select="'BrokerName'"/>
        </BrokerName>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <xsl:if test="(Asset = 'FXForward' or Asset = 'FX') and substring(Side,1,1) = 'S'">

          <ThirdPartyFlatFileDetail>
            <!--for system internal use-->
            <RowHeader>
              <xsl:value-of select ="false"/>
            </RowHeader>

            <!--for system use only-->
            <IsCaptionChangeRequired>
              <xsl:value-of select ="true"/>
            </IsCaptionChangeRequired>

            <!--for system internal use-->
            <TaxLotState>
              <xsl:value-of select ="TaxLotState"/>
            </TaxLotState>

            <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
            <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>


            <!--<InstrumentSubType>
						<xsl:value-of select="Asset"/>
					</InstrumentSubType>

					<Comments>
						<xsl:value-of select="''"/>
					</Comments>-->


            <Account>
              <xsl:value-of select="'883656'"/>
            </Account>

            <TradeType>
              <xsl:value-of select="'FX'"/>
            </TradeType>

            <AssetClass>
              <xsl:value-of select="'FXCU'"/>
            </AssetClass>

            <TransactionType>
              <xsl:choose>
                <xsl:when test="Asset = 'FXForward'">
                  <xsl:value-of select="'FORWARD'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'SPOT'"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionType>

            <TradeDate>
              <xsl:value-of select="TradeDate"/>
            </TradeDate>

            <SettleDate>
              <xsl:value-of select="SettlementDate"/>
            </SettleDate>

            <FXCCYRate>
              <xsl:value-of select="ForexRate"/>
            </FXCCYRate>

            <FXBUYCCY>
              <xsl:value-of select="VsCurrencyName"/>
            </FXBUYCCY>

            <FXBuyAmount>
              <xsl:value-of select="GrossAmount"/>
            </FXBuyAmount>

            <USDAmount>
              <xsl:value-of select="format-number(GrossAmount div ForexRate,'#.00')"/>
            </USDAmount>

            <BrokerName>
              <xsl:value-of select="CounterParty"/>
            </BrokerName>

            <!-- system use only-->
            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

          </ThirdPartyFlatFileDetail>
        </xsl:if>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>
</xsl:stylesheet>
