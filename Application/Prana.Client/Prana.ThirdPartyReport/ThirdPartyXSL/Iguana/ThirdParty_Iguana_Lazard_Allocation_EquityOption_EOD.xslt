<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <ExecAccount>
          <xsl:value-of select="'ExecAccount'"/>
        </ExecAccount>

        <AllocAccount>
          <xsl:value-of select="'AllocAccount'"/>
        </AllocAccount>

        <AllocShares>
          <xsl:value-of select="'AllocShares'"/>
        </AllocShares>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Buysell>
          <xsl:value-of select="'Buysell'"/>
        </Buysell>

        <commission>
          <xsl:value-of select="'commission'"/>
        </commission>

        <CommType>
          <xsl:value-of select="'CommType'"/>
        </CommType>

        <currency>
          <xsl:value-of select="'currency'"/>
        </currency>


        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <Setttlementdt>
          <xsl:value-of select="'setttlement_dt'"/>
        </Setttlementdt>

        <Tradedt>
          <xsl:value-of select="'trade_dt'"/>
        </Tradedt>

        <LastMkt>
          <xsl:value-of select ="'LastMkt'"/>
        </LastMkt>

        <Rule80A>
          <xsl:value-of select ="'Rule80A'"/>
        </Rule80A>

        <IDSource>
          <xsl:value-of select ="'IDSource'"/>
        </IDSource>

        <securityID>
          <xsl:value-of select ="'securityID'"/>
        </securityID>

        <ROOT_SYMBOl>
          <xsl:value-of select="'ROOT_SYMBOl'"/>
        </ROOT_SYMBOl>

        <BROKER>
          <xsl:value-of select="'BROKER'"/>
        </BROKER>

        <OPEN_CLOSE>
          <xsl:value-of select="'OPEN/CLOSE'"/>
        </OPEN_CLOSE>

        <YEAR>
          <xsl:value-of select="'YEAR'"/>
        </YEAR>

        <MONTH>
          <xsl:value-of select="'MONTH'"/>
        </MONTH>

        <DAY>
          <xsl:value-of select="'DAY'"/>
        </DAY>

        <PUT_CALL>
          <xsl:value-of select="'PUT/CALL'"/>
        </PUT_CALL>

        <STRIKE>
          <xsl:value-of select="'STRIKE'"/>
        </STRIKE>
        

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <xsl:if test="Asset = 'FutureOption' or Asset = 'EquityOption'">
			<ThirdPartyFlatFileDetail>

				<!--for system internal use-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<FileHeader>
					<xsl:value-of select ="'false'"/>
				</FileHeader>
				<FileFooter>
					<xsl:value-of select ="'false'"/>
				</FileFooter>

				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>

				<xsl:variable name = "Prana_Exchange_Name">
					<xsl:value-of select="Exchange"/>
				</xsl:variable>

				<xsl:variable name="PRANA_LastMkt">
					<xsl:value-of select="document('../ReconMappingXml/ExchangeMapping.xml')/ExchangeMapping/PB[@Name= 'Lazard']/ExchangeData[@ExchangeName=$Prana_Exchange_Name]/@LastMktCode"/>
				</xsl:variable>

				<ExecAccount>
					<xsl:value-of select="'489-999951X'"/>
				</ExecAccount>

				<AllocAccount>
					<xsl:value-of select="AccountMappedName"/>
				</AllocAccount>

				<AllocShares>
					<xsl:value-of select="AllocatedQty"/>
				</AllocShares>

				<Price>
					<xsl:value-of select="AveragePrice"/>
				</Price>

				<Buysell>
					<xsl:value-of select="substring(Side,1,1)"/>
				</Buysell>

				<commission>
					<xsl:value-of select="CommissionCharged"/>
				</commission>

				<CommType>
					<xsl:value-of select="'A'"/>
				</CommType>

				<currency>
					<xsl:value-of select="'USD'"/>
				</currency>


				<Symbol>
					<xsl:value-of select="BBCode"/>
				</Symbol>

				<Setttlementdt>
					<xsl:value-of select="SettlementDate"/>
				</Setttlementdt>

				<Tradedt>
					<xsl:value-of select="TradeDate"/>
				</Tradedt>

				<LastMkt>
					<xsl:value-of select ="$PRANA_LastMkt"/>
				</LastMkt>

				<Rule80A>
					<xsl:value-of select ="'A'"/>
				</Rule80A>

				<IDSource>
					<xsl:value-of select ="'OSI'"/>
				</IDSource>

				<securityID>
					<xsl:value-of select ="OSIOptionSymbol"/>
				</securityID>

				<ROOT_SYMBOl>
					<xsl:value-of select="UnderlyingSymbol"/>
				</ROOT_SYMBOl>

				<BROKER>
					<xsl:value-of select="CounterParty"/>
				</BROKER>

				<OPEN_CLOSE>
					<xsl:value-of select="'O'"/>
				</OPEN_CLOSE>

				<YEAR>
					<xsl:value-of select="substring-after(substring-after(ExpirationDate,'/'),'/')"/>
				</YEAR>

				<MONTH>
					<xsl:value-of select="substring-before(ExpirationDate,'/')"/>
				</MONTH>

				<DAY>
					<xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>
				</DAY>

				<PUT_CALL>
					<xsl:value-of select="substring(PutOrCall,1,1)"/>
				</PUT_CALL>

				<STRIKE>
					<xsl:value-of select="StrikePrice"/>
				</STRIKE>

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
