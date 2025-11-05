<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="GetDTCode">
		<xsl:param name="CounterParty"/>
		<xsl:choose>
			<xsl:when test="$CounterParty = 'AABA'">
				<xsl:value-of select="'0158'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'BMOC'">
				<xsl:value-of select="'0045'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'BTIG'">
				<xsl:value-of select="'0501'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'CNFR'">
				<xsl:value-of select="'0443'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'COWN'">
				<xsl:value-of select="'0226'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'CSTI'">
				<xsl:value-of select="'0352'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'DBPC'">
				<xsl:value-of select="'0443'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'FBPC'">
				<xsl:value-of select="'0443'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'GSCO'">
				<xsl:value-of select="'0005'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'INCA'">
				<xsl:value-of select="'0067'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'ISIG'">
				<xsl:value-of select="'0352'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'JEFF'">
				<xsl:value-of select="'0019'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'JPMS'">
				<xsl:value-of select="'0352'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'JSSF'">
				<xsl:value-of select="'0352'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'LAZA'">
				<xsl:value-of select="'0443'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'LEHM'">
				<xsl:value-of select="'0229'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'MLCO'">
				<xsl:value-of select="'161'"/>
			</xsl:when>
		<xsl:when test="$CounterParty = 'MSCO'">
				<xsl:value-of select="'50'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'PIPR'">
				<xsl:value-of select="'0311'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'STFL'">
				<xsl:value-of select="'0793'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'SBSH'">
				<xsl:value-of select="'0418'"/>
			</xsl:when>
			<xsl:when test="$CounterParty = 'WCHV'">
				<xsl:value-of select="'0250'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
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

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <currency>
          <xsl:value-of select="'currency'"/>
        </currency>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Buysell>
          <xsl:value-of select="'Buysell'"/>
        </Buysell>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <Tradedt>
          <xsl:value-of select="'trade_dt'"/>
        </Tradedt>

        <Setttlementdt>
          <xsl:value-of select="'setttlement_dt'"/>
        </Setttlementdt>


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

		  <ExecutionTime>
			  <xsl:value-of select ="'ExecutionTime'"/>
		  </ExecutionTime>

		  <Exec_Broker>
			  <xsl:value-of select ="'Exec_Broker'"/>
		  </Exec_Broker>

		  <BrokerOS>
			  <xsl:value-of select ="'BrokerOS'"/>
		  </BrokerOS>

		  <ShortSaleLoc>
			  <xsl:value-of select ="'ShortSaleLoc'"/>
		  </ShortSaleLoc>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
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

			  <Account>
				  <xsl:value-of select="'QLP101005'"/>
			  </Account>

			  <currency>
				  <xsl:value-of select="'USD'"/>
			  </currency>

			  <Price>
				  <xsl:value-of select="AveragePrice"/>
			  </Price>

			  <Quantity>
				  <xsl:value-of select="AllocatedQty"/>
			  </Quantity>

			  <Buysell>
				  <xsl:value-of select="substring(Side,1,1)"/>
			  </Buysell>

			  <Symbol>
				  <xsl:value-of select="Symbol"/>
			  </Symbol>

			  <Tradedt>
				  <xsl:value-of select="TradeDate"/>
			  </Tradedt>

			  <Setttlementdt>
				  <xsl:value-of select="SettlementDate"/>
			  </Setttlementdt>


			  <LastMkt>
				  <xsl:value-of select ="$PRANA_LastMkt"/>
			  </LastMkt>

			  <Rule80A>
				  <xsl:value-of select ="'A'"/>
			  </Rule80A>

			  <IDSource>
				  <xsl:value-of select ="'SEDOL'"/>
			  </IDSource>

			  <securityID>
				  <xsl:value-of select ="SEDOL"/>
			  </securityID>

			  <ExecutionTime>
				  <xsl:value-of select ="''"/>
			  </ExecutionTime>

			  <Exec_Broker>
				  <xsl:choose>
					  <xsl:when test ="substring(CounterParty, 5, 1) = 'O'">
						  <xsl:value-of select ="substring(CounterParty, 1, 4)"/>
					  </xsl:when>
				  </xsl:choose>
			  </Exec_Broker>

			  <BrokerOS>
				  <xsl:call-template name ="GetDTCode">
					  <xsl:with-param name ="CounterParty" select ="CounterParty"/>
				  </xsl:call-template>
			  </BrokerOS>

			  <ShortSaleLoc>
				  <xsl:value-of select ="''"/>
			  </ShortSaleLoc>

			  <!-- system use only-->
			  <EntityID>
				  <xsl:value-of select="EntityID"/>
			  </EntityID>

		  </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
