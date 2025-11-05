<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

        <FileHeader>
          <xsl:value-of select ="'false'"/>
        </FileHeader>

        <FileFooter>
          <xsl:value-of select ="'false'"/>
        </FileFooter>

        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <Product>
          <xsl:value-of select="'Product'"/>
        </Product>

        <RecordAction>
          <xsl:value-of select="'Record Action'"/>
        </RecordAction>

        <TradeType>
          <xsl:value-of select="'Trade Type'"/>
        </TradeType>

        <Seqno>
          <xsl:value-of select="'Seqno'"/>
        </Seqno>

        <Traderid>
          <xsl:value-of select="'Traderid'"/>
        </Traderid>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleOnValueDate>
          <xsl:value-of select="'Settle/On/Value Date'"/>
        </SettleOnValueDate>

        <OffEffectiveDate>
          <xsl:value-of select="'Off/Effective Date'"/>
        </OffEffectiveDate>

        <FutureDevelopment>
          <xsl:value-of select="'Future development'"/>
        </FutureDevelopment>

        <Clearer>
          <xsl:value-of select="'Clearer'"/>
        </Clearer>
        
        <Counterparty>
          <xsl:value-of select="'Counterparty'"/>
        </Counterparty>

       

        <InstrumentCodeType>
          <xsl:value-of select="'Instrument Code Type'"/>
        </InstrumentCodeType>


        <InstrumentCode>
          <xsl:value-of select="'Instrument Code'"/>
        </InstrumentCode>

        <InstrumentDescription>
          <xsl:value-of select="'Instrument Description'"/>
        </InstrumentDescription>


        <StrikePrice>
          <xsl:value-of select="'Strike Price'"/>
        </StrikePrice>

        <PutCallIndicator>
          <xsl:value-of select="'Put/Call Indicator'"/>
        </PutCallIndicator>

        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <TradeAllInPrice>
          <xsl:value-of select="'Trade/All-In-Price'"/>
        </TradeAllInPrice>

        <YieldRate>
          <xsl:value-of select="'Yield/Rate'"/>
        </YieldRate>

        <Principal>
          <xsl:value-of select="'Principal'"/>
        </Principal>

        <Premium>
          <xsl:value-of select="'Premium'"/>
        </Premium>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <MiscCharges>
          <xsl:value-of select="'Misc. Charges'"/>
        </MiscCharges>

        <LocalTax>
          <xsl:value-of select="'Local Tax'"/>
        </LocalTax>

        <ExchangeTax>
          <xsl:value-of select="'Exchange Tax'"/>
        </ExchangeTax>

        <SettlementAmount>
          <xsl:value-of select="'Settlement Amount'"/>
        </SettlementAmount>

        <SettlementCCYFXrate>
          <xsl:value-of select="'Settlement CCY FX rate'"/>
        </SettlementCCYFXrate>

        <SettlementCCY>
          <xsl:value-of select="'Settlement CCY'"/>
        </SettlementCCY>

        <RepoAccruedInterest>
          <xsl:value-of select="'Repo/Accrued Interest'"/>
        </RepoAccruedInterest>

        <Strategy>
          <xsl:value-of select="'Strategy'"/>
        </Strategy>

        <Sector>
          <xsl:value-of select="'Sector'"/>
        </Sector>

        <WIIndicator>
          <xsl:value-of select="'WI Indicator'"/>
        </WIIndicator>

        <Reserved1>
          <xsl:value-of select="'Reserved1'"/>
        </Reserved1>

        <CurrentFace>
          <xsl:value-of select="'Current Face'"/>
        </CurrentFace>
        <CurrentFactor>
          <xsl:value-of select="'Current Factor'"/>
        </CurrentFactor>

        <Comments>
          <xsl:value-of select="'Comments'"/>
        </Comments>

        <Reserved2>
          <xsl:value-of select="'Reserved2'"/>
        </Reserved2>

        <GroupId>
          <xsl:value-of select="'Group Id'"/>
        </GroupId>

        <Reserved3>
          <xsl:value-of select="'Reserved3'"/>
        </Reserved3>

        <TaxLotOffsetRecord>
          <xsl:value-of select="'Tax Lot Offset Record'"/>
        </TaxLotOffsetRecord>

        <AllocationRecord>
          <xsl:value-of select="'Allocation Record'"/>
        </AllocationRecord>

        <CSValueDate>
          <xsl:value-of select="'CS Value Date'"/>
        </CSValueDate>

        <CSPLMultiplier>
          <xsl:value-of select="'CS PL_Multiplier'"/>
        </CSPLMultiplier>

        <CSRootRicCode>
          <xsl:value-of select="'CS Root Ric Code'"/>
        </CSRootRicCode>

        <CSRicCode>
          <xsl:value-of select="'CS Ric Code'"/>
        </CSRicCode>

        <CSRootBloombergCode>
          <xsl:value-of select="'CS Root Bloomberg Code'"/>
        </CSRootBloombergCode>

        <CSBloombergCode>
          <xsl:value-of select="'CS Bloomberg Code'"/>
        </CSBloombergCode>

        <CustodySettleLocationName>
          <xsl:value-of select="'Custody Settle Location Name'"/>
        </CustodySettleLocationName>

        <CustodySettleLocationCode>
          <xsl:value-of select="'Custody Settle Location Code'"/>
        </CustodySettleLocationCode>


        <ClientInstrumentId>
          <xsl:value-of select="'Client Instrument Id'"/>
        </ClientInstrumentId>

        <HaircutRate>
          <xsl:value-of select="'Haircut Rate'"/>
        </HaircutRate>

        <MarginPercentage>
          <xsl:value-of select="'Margin Percentage'"/>
        </MarginPercentage>

        <MarginAmount>
          <xsl:value-of select="'Margin Amount'"/>
        </MarginAmount>

        <SafekeepingAccount>
          <xsl:value-of select="'Safekeeping Account'"/>
        </SafekeepingAccount>

        <ClearingBrokerID>
          <xsl:value-of select="'Clearing Broker ID'"/>
        </ClearingBrokerID>


        <ClearingBrokerIDType>
          <xsl:value-of select="'Clearing Broker ID Type'"/>
        </ClearingBrokerIDType>

        <ClientFilename>
          <xsl:value-of select="'Client Filename'"/>
        </ClientFilename>

        <AllocationAttribute>
          <xsl:value-of select="'Allocation Attribute'"/>
        </AllocationAttribute>

        <OptionExerciseType>
          <xsl:value-of select="'Option Exercise Type'"/>
        </OptionExerciseType>

        <OTCIdentifier>
          <xsl:value-of select="'OTC Identifier'"/>
        </OTCIdentifier>

        <OTCPlMultiplier>
          <xsl:value-of select="'OTC Pl Multiplier'"/>
        </OTCPlMultiplier>

        <OTCExpirationDate>
          <xsl:value-of select="'OTC Expiration Date'"/>
        </OTCExpirationDate>

        <AssignedTo>
          <xsl:value-of select="'Assigned To'"/>
        </AssignedTo>

        <Fees>
          <xsl:value-of select="'Fees'"/>
        </Fees>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>



      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <xsl:if test="(Asset = 'Equity' or Asset = 'EquityOption') and AccountName != 'Merger-522-9317N'">
        <ThirdPartyFlatFileDetail>

          <FileHeader>
            <xsl:value-of select ="'false'"/>
          </FileHeader>

          <FileFooter>
            <xsl:value-of select ="'false'"/>
          </FileFooter>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <Product>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="'EQ'"/>
              </xsl:when>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="'OP'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Product>

          <RecordAction>
            <xsl:choose>
              <xsl:when test="TaxLotState = 'Allocated'" >
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Amemded'" >
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Deleted'" >
                <xsl:value-of select="'D'"/>
              </xsl:when>
            </xsl:choose>
          </RecordAction>

          <TradeType>
            <xsl:value-of select="'A'"/>
          </TradeType>

          <Seqno>
            <xsl:value-of select="EntityID"/>
          </Seqno>

          <Traderid>
            <xsl:choose>
              <xsl:when test="contains(AccountName, 'naveen') != false">
                <xsl:value-of select="'IP'"/>
              </xsl:when>
              <!--<xsl:when test="contains(AccountName, 'TDF') != false">
                <xsl:value-of select="'EQD'"/>
              </xsl:when>
              <xsl:when test="contains(AccountName, 'TGR') != false">
                <xsl:value-of select="'EQG'"/>
              </xsl:when>-->
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Traderid>

          <TradeDate>
            <xsl:value-of select="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
          </TradeDate>

          <SettleOnValueDate>
            <xsl:value-of select="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
          </SettleOnValueDate>

          <OffEffectiveDate>
            <xsl:value-of select="''"/>
          </OffEffectiveDate>

          <FutureDevelopment>
            <xsl:value-of select="''"/>
          </FutureDevelopment>

          <Clearer>
            <xsl:choose>
              <!--For NewEdge Funds-->
              <xsl:when test="AccountName = 'naveen'">
                <xsl:value-of select="'CALEQ'"/>
              </xsl:when>
              <!--For GS Funds--><!--
              <xsl:when test="AccountName = 'TDF-2314706' or AccountName = 'TGR-2386845' or AccountName = 'TIP-2316651'">
                <xsl:value-of select="'GSCOEQ'"/>
              </xsl:when>-->
            </xsl:choose>
          </Clearer>

			<xsl:variable name = "Prana_CounterParty" >
				<xsl:value-of select="CounterParty"/>
			</xsl:variable>

			<xsl:variable name="PB_CounterParty">
				<xsl:value-of select="document('../ReconMappingXml/ThirdParty_Exec_BrokerMapping.xml')/BrokerMapping/PB[@Name='Touradji']/BrokerData[@PranaBrokerName=$Prana_CounterParty]/@PBBrokerName"/>
			</xsl:variable>

			<Counterparty>
				<xsl:choose>
					<xsl:when test ="$PB_CounterParty != ''">
						<xsl:value-of select ="$PB_CounterParty"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="CounterParty"/>
					</xsl:otherwise>
				</xsl:choose>
			</Counterparty>

          <InstrumentCodeType>
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="'O'"/>
              </xsl:when>
              <!--<xsl:when test="ISIN != ''">
                <xsl:value-of select="'I'"/>
              </xsl:when>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="'R'"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="'B'"/>
              </xsl:when>-->
				<xsl:otherwise>
					<xsl:value-of select ="'T'"/>
				</xsl:otherwise>
            </xsl:choose>
          </InstrumentCodeType>

          <InstrumentCode>
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <!--<xsl:when test="ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="RIC"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="BBCode"/>
              </xsl:when>-->
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
            </xsl:choose>
          </InstrumentCode>

          <InstrumentDescription>
            <xsl:value-of select="FullSecurityName"/>
          </InstrumentDescription>


          <StrikePrice>
			  <xsl:choose>
				  <xsl:when test ="number(StrikePrice)">
					  <xsl:value-of select ="StrikePrice"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </StrikePrice>

          <PutCallIndicator>
            <xsl:value-of select="substring(PutOrCall,1,1)"/>
          </PutCallIndicator>

          <Action>
			  <xsl:value-of select ="substring(Side,1,1)"/>
            <!--<xsl:choose>
              <xsl:when test="Side = 'Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
            </xsl:choose>-->
          </Action>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <TradeAllInPrice>
            <xsl:value-of select="AveragePrice"/>
          </TradeAllInPrice>

          <YieldRate>
            <xsl:value-of select="''"/>
          </YieldRate>

          <Principal>
            <xsl:value-of select="GrossAmount"/>
          </Principal>

          <Premium>
            <xsl:value-of select="''"/>
          </Premium>

          <Commission>
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </Commission>

          <MiscCharges>
			  <xsl:choose>
				  <xsl:when test ="Asset = 'EquityOption'">
					  <xsl:value-of select="TaxOnCommissions"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </MiscCharges>

          <LocalTax>
            <xsl:value-of select="''"/>
          </LocalTax>

          <ExchangeTax>
            <xsl:value-of select="''"/>
          </ExchangeTax>

          <SettlementAmount>
            <xsl:value-of select="''"/>
          </SettlementAmount>

          <SettlementCCYFXrate>
            <xsl:value-of select="''"/>
          </SettlementCCYFXrate>

          <SettlementCCY>
            <xsl:value-of select="CurrencySymbol"/>
          </SettlementCCY>

          <RepoAccruedInterest>
            <xsl:value-of select="''"/>
          </RepoAccruedInterest>

          <Strategy>
            <xsl:value-of select="'EQTY'"/>
          </Strategy>

          <Sector>
            <xsl:value-of select="''"/>
          </Sector>

          <WIIndicator>
            <xsl:value-of select="''"/>
          </WIIndicator>

          <Reserved1>
            <xsl:value-of select="''"/>
          </Reserved1>

          <CurrentFace>
            <xsl:value-of select="''"/>
          </CurrentFace>
          <CurrentFactor>
            <xsl:value-of select="''"/>
          </CurrentFactor>

          <Comments>
            <xsl:value-of select="''"/>
          </Comments>

          <Reserved2>
            <xsl:value-of select="''"/>
          </Reserved2>

          <GroupId>
            <xsl:value-of select="''"/>
          </GroupId>

          <Reserved3>
            <xsl:value-of select="''"/>
          </Reserved3>

          <TaxLotOffsetRecord>
            <xsl:value-of select="''"/>
          </TaxLotOffsetRecord>

          <AllocationRecord>
            <xsl:value-of select="''"/>
          </AllocationRecord>

          <CSValueDate>
            <xsl:value-of select="''"/>
          </CSValueDate>

          <CSPLMultiplier>
            <xsl:value-of select="''"/>
          </CSPLMultiplier>

          <CSRootRicCode>
            <xsl:value-of select="''"/>
          </CSRootRicCode>

          <CSRicCode>
            <xsl:value-of select="''"/>
          </CSRicCode>

          <CSRootBloombergCode>
            <xsl:value-of select="''"/>
          </CSRootBloombergCode>

          <CSBloombergCode>
            <xsl:value-of select="''"/>
          </CSBloombergCode>

          <CustodySettleLocationName>
            <xsl:value-of select="''"/>
          </CustodySettleLocationName>

          <CustodySettleLocationCode>
            <xsl:value-of select="''"/>
          </CustodySettleLocationCode>


          <ClientInstrumentId>
            <xsl:value-of select="''"/>
          </ClientInstrumentId>

          <HaircutRate>
            <xsl:value-of select="''"/>
          </HaircutRate>

          <MarginPercentage>
            <xsl:value-of select="''"/>
          </MarginPercentage>

          <MarginAmount>
            <xsl:value-of select="' '"/>
          </MarginAmount>

          <SafekeepingAccount>
            <xsl:value-of select="''"/>
          </SafekeepingAccount>

          <ClearingBrokerID>
            <xsl:value-of select="''"/>
          </ClearingBrokerID>

          <ClearingBrokerIDType>
            <xsl:value-of select="''"/>
          </ClearingBrokerIDType>

          <ClientFilename>
            <xsl:value-of select="''"/>
          </ClientFilename>

          <AllocationAttribute>
            <xsl:value-of select="''"/>
          </AllocationAttribute>

          <OptionExerciseType>
            <xsl:value-of select="''"/>
          </OptionExerciseType>

          <OTCIdentifier>
            <xsl:value-of select="''"/>
          </OTCIdentifier>

          <OTCPlMultiplier>
            <xsl:value-of select="''"/>
          </OTCPlMultiplier>

          <OTCExpirationDate>
            <xsl:value-of select="''"/>
          </OTCExpirationDate>

          <AssignedTo>
            <xsl:value-of select="''"/>
          </AssignedTo>

          <Fees>
            <xsl:value-of select="''"/>
          </Fees>

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
