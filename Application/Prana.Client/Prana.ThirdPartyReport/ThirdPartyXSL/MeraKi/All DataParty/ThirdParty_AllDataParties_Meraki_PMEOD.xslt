<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>
        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="true"/>
        </IsCaptionChangeRequired>

        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>
        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Completed>
          <xsl:value-of select="'Completed'"/>
        </Completed>

        <Description>
          <xsl:value-of select="'Description'"/>
        </Description>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <ExPrice>
          <xsl:value-of select="'Ex Price'"/>
        </ExPrice>

        <Notes>
          <xsl:value-of select="'Notes'"/>
        </Notes>

        <CurrentPercent>
          <xsl:value-of select="'Current%'"/>
        </CurrentPercent>
        <TradedPercent>
          <xsl:value-of select="'Traded%'"/>
        </TradedPercent>
        <TargetPercent>
          <xsl:value-of select="'Target%'"/>
        </TargetPercent>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'Karst Peak AMF' or AccountName = 'Karst Peak SMF' or AccountName = 'Karst Peak VMF']">

        <ThirdPartyFlatFileDetail>

          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME" select="'PMEOD_Meraki'"/>

          <xsl:variable name="varPRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <xsl:variable name="varTHIRDPARTY_FUND_CODE">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$varPRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
          
          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <Account>
            <xsl:choose>
              <xsl:when test="$varTHIRDPARTY_FUND_CODE != ''">
                <xsl:value-of select="$varTHIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varPRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </Account>

          <Side>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </Side>

          <Completed>
			<xsl:value-of select="format-number(TaxLotQty,'###,###.####')"/>
          </Completed>

          <Description>
            <xsl:value-of select="SecurityDescription"/>
          </Description>

          <Symbol>
            <xsl:value-of select="BBGSymbol"/>
          </Symbol>

          <ExPrice>
            <xsl:value-of select="format-number(AvgPrice,'###,###.0000')"/>
          </ExPrice>

          <Notes>
            <xsl:value-of select="Notes"/>
          </Notes>          

          <CurrentPercent>
            <xsl:value-of select="concat(format-number(CurrentPercent,'#0.00'), '%')"/>
          </CurrentPercent>

          <TradedPercent>
            <xsl:value-of select="concat(format-number(TradedPercent,'#0.00'), '%')"/>
          </TradedPercent>
          
          <TargetPercent>
            <xsl:value-of select="concat(format-number(TargetPercent,'#0.00'), '%')"/>
          </TargetPercent>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>