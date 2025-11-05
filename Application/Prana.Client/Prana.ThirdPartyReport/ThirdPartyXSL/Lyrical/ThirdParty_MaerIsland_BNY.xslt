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
          <xsl:value-of select ="'true'"/>
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

        <FunctionMessage>
          <xsl:value-of select="'Function of the Message'"/>
        </FunctionMessage>

        <TradeType>
          <xsl:value-of select="'Trade Type'"/>
        </TradeType>

        <AccountNo>
          <xsl:value-of select="'Account No'"/>
        </AccountNo>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <BrokerID>
          <xsl:value-of select="'Broker ID'"/>
        </BrokerID>

        <ClearerID>
          <xsl:value-of select="'Clearer ID'"/>
        </ClearerID>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <AssetType>
          <xsl:value-of select="'Asset Type'"/>
        </AssetType>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <LocalCost>
          <xsl:value-of select="'Local Cost'"/>
        </LocalCost>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <BrokerAccount>
          <xsl:value-of select="'Broker Account'"/>
        </BrokerAccount>

        <SECFee>
          <xsl:value-of select="'SEC Fee '"/>
        </SECFee>

        <OtherCharges>
          <xsl:value-of select="'Other Charges '"/>
        </OtherCharges>

        <Principal>
          <xsl:value-of select="'Principal'"/>
        </Principal>

        <Interest>
          <xsl:value-of select="'Interest'"/>
        </Interest>

        <FinalMoney>
          <xsl:value-of select="'Final Money'"/>
        </FinalMoney>

        <SecurityId>
          <xsl:value-of select="'Security Id'"/>
        </SecurityId>

        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>

        <MaturityDate>
          <xsl:value-of select="'Maturity Date'"/>
        </MaturityDate>

        <IssueDate>
          <xsl:value-of select="'Issue Date'"/>
        </IssueDate>

        <CurrentRate>
          <xsl:value-of select="'Current Rate'"/>
        </CurrentRate>

        <SafekeepingPlace>
          <xsl:value-of select="'Safekeeping place'"/>
        </SafekeepingPlace>

        <SettlementPlace>
          <xsl:value-of select="'Settlement Place'"/>
        </SettlementPlace>

        <Reference>
          <xsl:value-of select="'Reference'"/>
        </Reference>

        <StampDuty>
          <xsl:value-of select="'Stamp Duty'"/>
        </StampDuty>

        <OriginalFace>
          <xsl:value-of select="'Original Face'"/>
        </OriginalFace>

        <FXExecute>
          <xsl:value-of select="'FX Execute'"/>
        </FXExecute>

        <BuySellCurrency>
          <xsl:value-of select="'Buy/Sell Currency'"/>
        </BuySellCurrency>

        <FXSpecial>
          <xsl:value-of select="'FX Special'"/>
        </FXSpecial>

        <Market>
          <xsl:value-of select="'Market'"/>
        </Market>

        <SpecialInstruction>
          <xsl:value-of select="'Special Instruction'"/>
        </SpecialInstruction>

        <BlockDetailCounter>
          <xsl:value-of select="'Block Detail Counter'"/>
        </BlockDetailCounter>

        <RelatedReference>
          <xsl:value-of select="'Related Reference'"/>
        </RelatedReference>

        <DeliverToAccount>
          <xsl:value-of select="'Deliver To Account'"/>
        </DeliverToAccount>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <PoolNumber>
          <xsl:value-of select="'Pool Number'"/>
        </PoolNumber>

        <Factor>
          <xsl:value-of select="'Factor'"/>
        </Factor>

        <ADELDate>
          <xsl:value-of select="'ADEL Date'"/>
        </ADELDate>

        <Taxes>
          <xsl:value-of select="'Taxes'"/>
        </Taxes>

        <SettlementMethod>
          <xsl:value-of select="'Settlement Method'"/>
        </SettlementMethod>

        <SecurityIdType>
          <xsl:value-of select="'Security Id Type'"/>
        </SecurityIdType>

        <Investor>
          <xsl:value-of select="'Investor'"/>
        </Investor>

        <BrokerIdType>
          <xsl:value-of select="'Broker ID Type'"/>
        </BrokerIdType>

        <BrokerDescription>
          <xsl:value-of select="'Broker Description'"/>
        </BrokerDescription>

        <ClearerIDType>
          <xsl:value-of select="'Clearer ID Type'"/>
        </ClearerIDType>

        <ClearerDescription>
          <xsl:value-of select="'Clearer Description'"/>
        </ClearerDescription>

        <ClearerAccount>
          <xsl:value-of select="'Clearer Account'"/>
        </ClearerAccount>

        <CustodianIDType>
          <xsl:value-of select="'Custodian ID Type'"/>
        </CustodianIDType>

        <CustodianID>
          <xsl:value-of select="'Custodian ID'"/>
        </CustodianID>

        <CustodianDescription>
          <xsl:value-of select="'Custodian Description'"/>
        </CustodianDescription>

        <CustodianAccount>
          <xsl:value-of select="'Custodian Account'"/>
        </CustodianAccount>

        <BaseCost>
          <xsl:value-of select="'Base Cost'"/>
        </BaseCost>

        <BaseCurrency>
          <xsl:value-of select="'Base Currency'"/>
        </BaseCurrency>

        <LocalCurrency>
          <xsl:value-of select="'Local Currency'"/>
        </LocalCurrency>

        <ChangeOwner_Reg>
          <xsl:value-of select="'Change Owner/Reg'"/>
        </ChangeOwner_Reg>

        <SettlementIndicator>
          <xsl:value-of select="'Settlement Indicator'"/>
        </SettlementIndicator>

        <ItalianTaxId>
          <xsl:value-of select="'Italian Tax Id'"/>
        </ItalianTaxId>

        <SettlementTransactionIndicator>
          <xsl:value-of select="'Settlement Transaction Indicator'"/>
        </SettlementTransactionIndicator>

        <SubCustodianSpecialInst>
          <xsl:value-of select="'Sub-Custodian Special Inst'"/>
        </SubCustodianSpecialInst>

        <InventoryBook>
          <xsl:value-of select="'Inventory Book'"/>
        </InventoryBook>

        <ManualBrokerDescFlagBook>
          <xsl:value-of select="'Manual Broker Description Flag Book'"/>
        </ManualBrokerDescFlagBook>

        <ManualBrokerDesc>
          <xsl:value-of select="'Manual Broker Description'"/>
        </ManualBrokerDesc>

        <StampConsideration>
          <xsl:value-of select="'Stamp consideration'"/>
        </StampConsideration>


        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="true"/>
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

          <!--Exercise / Assign Need To Ask-->
          <xsl:variable name="varLifeCycle">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select="'Replace'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'Delete'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Sent'">
                <xsl:value-of select="'Expire'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="TaxLotState"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <!--<LifeCycle>
						<xsl:value-of select="$varLifeCycle"/>
					</LifeCycle>-->

          <!--Exercise / Assign Need To Ask-->

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated' or TaxLotState='Sent'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select="'R'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'D'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <FunctionMessage>
            <xsl:value-of select="'NEW'"/>
          </FunctionMessage>

          <TradeType>
            <xsl:choose>
              <xsl:when test="Side = 'Buy' or Side = 'Buy to Close' or Side = 'Buy to Open'">
                <xsl:value-of select="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell' or Side = 'Sell short' or Side = 'Sell to Open' or Side = 'Sell to Close'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
            </xsl:choose>
            <xsl:value-of select="''"/>
          </TradeType>

          <AccountNo>
            <xsl:value-of select="'TESFZ999992'"/>
          </AccountNo>

          <TradeDate>
            <xsl:value-of select="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
          </SettlementDate>

          <xsl:variable name="CounterParty_Name">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_BrokerID">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@PranaBroker = $CounterParty_Name]/@PBBroker"/>
          </xsl:variable>

          <BrokerID>
            <xsl:value-of select="''"/>
          </BrokerID>

          <ClearerID>
            <xsl:value-of select="''"/>
          </ClearerID>

          <Price>
            <xsl:value-of select="format-number(AveragePrice,'0000000.0000000')"/>
          </Price>

          <AssetType>
            <xsl:value-of select="''"/>
          </AssetType>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

          <LocalCost>
            <xsl:value-of select="''"/>
          </LocalCost>

          <Commission>
            <xsl:value-of select="''"/>
          </Commission>

          <BrokerAccount>
            <xsl:value-of select="''"/>
          </BrokerAccount>

          <SECFee>
            <xsl:value-of select="''"/>
          </SECFee>

          <OtherCharges>
            <xsl:value-of select="''"/>
          </OtherCharges>

          <Principal>
            <xsl:choose>
              <xsl:when test="CurrencySymbol = 'JPY' or CurrencySymbol = 'ADP'">
                <xsl:value-of select="format-number(GrossAmount,'0000000000000')"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol = 'BHD' or CurrencySymbol = 'KWD' or CurrencySymbol = 'LTL' or CurrencySymbol = 'MUR' or CurrencySymbol = 'OMR' or CurrencySymbol = 'SZL' or CurrencySymbol = 'UAH'">
                <xsl:value-of select="format-number(GrossAmount,'0000000000000.000')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number(GrossAmount,'0000000000000.00')"/>
              </xsl:otherwise>
            </xsl:choose>
          </Principal>

          <Interest>
            <xsl:value-of select="''"/>
          </Interest>

          <FinalMoney>
            <xsl:choose>
              <xsl:when test="CurrencySymbol = 'JPY' or CurrencySymbol = 'ADP'">
                <xsl:value-of select="format-number(GrossAmount,'0000000000000')"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol = 'BHD' or CurrencySymbol = 'KWD' or CurrencySymbol = 'LTL' or CurrencySymbol = 'MUR' or CurrencySymbol = 'OMR' or CurrencySymbol = 'SZL' or CurrencySymbol = 'UAH'">
                <xsl:value-of select="format-number(GrossAmount,'0000000000000.000')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number(GrossAmount,'0000000000000.00')"/>
              </xsl:otherwise>
            </xsl:choose>
          </FinalMoney>

          <SecurityId>
            <xsl:choose>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityId>

          <SecurityDescription>
            <xsl:value-of select="''"/>
          </SecurityDescription>

          <MaturityDate>
            <xsl:value-of select="''"/>
          </MaturityDate>

          <IssueDate>
            <xsl:value-of select="''"/>
          </IssueDate>

          <CurrentRate>
            <xsl:value-of select="''"/>
          </CurrentRate>

          <SafekeepingPlace>
            <xsl:value-of select="''"/>
          </SafekeepingPlace>

          <SettlementPlace>
            <xsl:value-of select="concat(substring(CurrencySymbol,1,2),'-BIC')"/>
          </SettlementPlace>

          <Reference>
            <xsl:value-of select="PBUniqueID"/>
          </Reference>

          <StampDuty>
            <xsl:value-of select="''"/>
          </StampDuty>

          <OriginalFace>
            <xsl:value-of select="''"/>
          </OriginalFace>

          <FXExecute>
            <xsl:value-of select="''"/>
          </FXExecute>

          <BuySellCurrency>
            <xsl:value-of select="''"/>
          </BuySellCurrency>

          <FXSpecial>
            <xsl:value-of select="''"/>
          </FXSpecial>

          <xsl:variable name="Prana_Country_Name">
            <xsl:value-of select="Country"/>
          </xsl:variable>

          <xsl:variable name="PB_Country_Code">
            <xsl:value-of select="document('../ReconMappingXml/CountryMapping.xml')/CountryMapping/PB[@Name='BNY']/CurrencyData[@PranaCountry = $Prana_Country_Name]/@PBCountry"/>
          </xsl:variable>

          <Market>
            <xsl:value-of select="substring(CurrencySymbol,1,2)"/>
          </Market>

          <SpecialInstruction>
            <xsl:value-of select="''"/>
          </SpecialInstruction>

          <BlockDetailCounter>
            <xsl:value-of select="''"/>
          </BlockDetailCounter>

          <RelatedReference>
            <xsl:value-of select="''"/>
          </RelatedReference>

          <DeliverToAccount>
            <xsl:value-of select="''"/>
          </DeliverToAccount>

          <Quantity>
            <xsl:value-of select="format-number(AllocatedQty,'00000000000000.0000')"/>
          </Quantity>

          <PoolNumber>
            <xsl:value-of select="''"/>
          </PoolNumber>

          <Factor>
            <xsl:value-of select="''"/>
          </Factor>

          <ADELDate>
            <xsl:value-of select="''"/>
          </ADELDate>

          <Taxes>
            <xsl:value-of select="''"/>
          </Taxes>

          <SettlementMethod>
            <xsl:choose>
              <xsl:when test="contains(Symbol,'-') = false and AssetID = 1">
                <xsl:value-of select="'NORMAL'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol = 'EUR'">
                <xsl:value-of select="'EUROCLEAR'"/>
              </xsl:when>
            </xsl:choose>
          </SettlementMethod>

          <SecurityIdType>
            <xsl:choose>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="'ISIN'"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="'CUSIP'"/>
              </xsl:when>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="'SEDOL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'COMMON'"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityIdType>

          <Investor>
            <xsl:value-of select="''"/>
          </Investor>

          <BrokerIdType>
            <xsl:value-of select="'BIC'"/>
          </BrokerIdType>

          <BrokerDescription>
            <xsl:value-of select="''"/>
          </BrokerDescription>

          <ClearerIDType>
            <xsl:value-of select="'BIC'"/>
          </ClearerIDType>

          <ClearerDescription>
            <xsl:value-of select="''"/>
          </ClearerDescription>

          <ClearerAccount>
            <xsl:value-of select="''"/>
          </ClearerAccount>

          <CustodianIDType>
            <xsl:value-of select="''"/>
          </CustodianIDType>

          <CustodianID>
            <xsl:value-of select="''"/>
          </CustodianID>

          <CustodianDescription>
            <xsl:value-of select="''"/>
          </CustodianDescription>

          <CustodianAccount>
            <xsl:value-of select="''"/>
          </CustodianAccount>

          <BaseCost>
            <xsl:value-of select="''"/>
          </BaseCost>

          <BaseCurrency>
            <xsl:value-of select="''"/>
          </BaseCurrency>

          <LocalCurrency>
            <xsl:value-of select="''"/>
          </LocalCurrency>

          <ChangeOwner_Reg>
            <xsl:value-of select="''"/>
          </ChangeOwner_Reg>

          <SettlementIndicator>
            <xsl:value-of select="''"/>
          </SettlementIndicator>

          <ItalianTaxId>
            <xsl:value-of select="''"/>
          </ItalianTaxId>

          <SettlementTransactionIndicator>
            <xsl:value-of select="''"/>
          </SettlementTransactionIndicator>

          <SubCustodianSpecialInst>
            <xsl:value-of select="''"/>
          </SubCustodianSpecialInst>

          <InventoryBook>
            <xsl:value-of select="''"/>
          </InventoryBook>

          <ManualBrokerDescFlagBook>
            <xsl:value-of select="''"/>
          </ManualBrokerDescFlagBook>

          <ManualBrokerDesc>
            <xsl:value-of select="''"/>
          </ManualBrokerDesc>

          <StampConsideration>
            <xsl:value-of select="''"/>
          </StampConsideration>
          
          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  
</xsl:stylesheet>
