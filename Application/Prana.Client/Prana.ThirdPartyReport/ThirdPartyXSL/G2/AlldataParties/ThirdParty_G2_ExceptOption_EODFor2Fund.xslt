<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'/'),'/'),'/',substring-before($Date,'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>
        <!--  system inetrnal use -->
        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>
        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>
        <OriginalPurchaseDate>
          <xsl:value-of select="'OriginalPurchaseDate'"/>
        </OriginalPurchaseDate>
        <AccountNumber>
          <xsl:value-of select="'AccountNumber'"/>
        </AccountNumber>
        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>
        <TransactionType>
          <xsl:value-of select="'TransactionType'"/>
        </TransactionType>
        <Identifier>
          <xsl:value-of select="'Identifier'"/>
        </Identifier>
        <IdentifierType>
          <xsl:value-of select="'IdentifierType'"/>
        </IdentifierType>
        <Qunatity>
          <xsl:value-of select="'Quantity'"/>
        </Qunatity>
        <TradeCCY>
          <xsl:value-of select="'TradeCCY'"/>
        </TradeCCY>
        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>
        <SettleCCY>
          <xsl:value-of select="'SettleCCY'"/>
        </SettleCCY>

        <TradeBookFXRate>
          <xsl:value-of select="'TradeBookFXRate'"/>
        </TradeBookFXRate>

        <TradeSettleFXRate>
          <xsl:value-of select="'TradeSettleFXRate'"/>
        </TradeSettleFXRate>

        <OtherPayments1>
          <xsl:value-of select="'OtherPayments1'"/>
        </OtherPayments1>

        <OtherPayments1Type>
          <xsl:value-of select="'OtherPayments1Type'"/>
        </OtherPayments1Type>

        <OtherPayments1Implied>
              <xsl:value-of select="'OtherPayments1Implied'"/>
        </OtherPayments1Implied>

        <OtherPayments2>
          <xsl:value-of select="'OtherPayments2'"/>
        </OtherPayments2>

        <OtherPayments2Type>
          <xsl:value-of select="'OtherPayments2Type'"/>
        </OtherPayments2Type>

        <OtherPayments2Implied>
              <xsl:value-of select="'OtherPayments2Implied'"/>
        </OtherPayments2Implied>

        <OtherPayments3>
          <xsl:value-of select="'OtherPayments3'"/>
        </OtherPayments3>

        <OtherPayments3Type>
          <xsl:value-of select="'OtherPayments3Type'"/>
        </OtherPayments3Type>

        <OtherPayments3Implied>
          <xsl:value-of select="'OtherPayments3Implied'"/>
        </OtherPayments3Implied>

        <OtherPayments4>
          <xsl:value-of select="'OtherPayments4'"/>
        </OtherPayments4>

        <OtherPayments4Type>
          <xsl:value-of select="'OtherPayments4Type'"/>
        </OtherPayments4Type>

        <OtherPayments4Implied>
          <xsl:value-of select="'OtherPayments4Implied'"/>
        </OtherPayments4Implied>

        <TraderName>
          <xsl:value-of select="'TraderName'"/>
        </TraderName>

        <TraderType>
          <xsl:value-of select="'TraderType'"/>
        </TraderType>

        <TRSId>
          <xsl:value-of select="'TRSId'"/>
        </TRSId>

        <BookId>
          <xsl:value-of select="'BookId'"/>
        </BookId>

        <BookPath>
          <xsl:value-of select="'BookPath'"/>
        </BookPath>

        <ExternalReference>
          <xsl:value-of select="'ExternalReference'"/>
        </ExternalReference>

        <InternalReference>
          <xsl:value-of select="'InternalReference'"/>
        </InternalReference>

        <Notes>
          <xsl:value-of select="'Notes'"/>
        </Notes>

        <BookingStatus>
          <xsl:value-of select="'BookingStatus'"/>
        </BookingStatus>

        <DealId>
          <xsl:value-of select="'DealId'"/>
        </DealId>

        <TradeStatus>
          <xsl:value-of select="'TradeStatus'"/>
        </TradeStatus>

        <CancelCorrectIDType>
          <xsl:value-of select="'CancelCorrectIDType'"/>
        </CancelCorrectIDType>

        <CancelCorrectID>
          <xsl:value-of select="'CancelCorrectID'"/>
        </CancelCorrectID>

        <SettlementInstructionId>
          <xsl:value-of select="'SettlementInstructionId'"/>
        </SettlementInstructionId>

        <BorrowId>
          <xsl:value-of select="'BorrowId'"/>
        </BorrowId>

        <TradeKeywordName1>
          <xsl:value-of select="'TradeKeywordName1'"/>
        </TradeKeywordName1>

        <TradeKeywordValue1>
          <xsl:value-of select="'TradeKeywordValue1'"/>
        </TradeKeywordValue1>

        <TradeKeywordName2>
          <xsl:value-of select="'TradeKeywordName2'"/>
        </TradeKeywordName2>

        <TradeKeywordValue2>
          <xsl:value-of select="'TradeKeywordValue2'"/>
        </TradeKeywordValue2>

        <TradeVenue>
          <xsl:value-of select="'TradeVenue'"/>
        </TradeVenue>

        <AllocationTemplateId>
          <xsl:value-of select="'AllocationTemplateId'"/>
        </AllocationTemplateId>

        <AllocationAccountId>
          <xsl:value-of select="'AllocationAccountId'"/>
        </AllocationAccountId>

        <AllocationHierarchies>
          <xsl:value-of select="'AllocationHierarchies'"/>
        </AllocationHierarchies>

        <AllocationBookPaths>
          <xsl:value-of select="'AllocationBookPaths'"/>
        </AllocationBookPaths>

        <AllocationAmounts>
          <xsl:value-of select="'AllocationAmounts'"/>
        </AllocationAmounts>

        <AllocationNumberType>
          <xsl:value-of select="'AllocationNumberType'"/>
        </AllocationNumberType>

      </ThirdPartyFlatFileDetail>
		<xsl:for-each select="ThirdPartyFlatFileDetail[Asset!='EquityOption' and (AccountName='G2 Investment Partners LP' or AccountName='G2 Investment Partners QP LP')]">
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          <!--  system inetrnal use -->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
          <xsl:variable name="varTradeDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="TradeDate"/>
            </xsl:call-template>
          </xsl:variable>
          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>
          <xsl:variable name="varSettleDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate"/>
            </xsl:call-template>
          </xsl:variable>
          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>
          <xsl:variable name="varOriginalPurchaseDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="OriginalPurchaseDate"/>
            </xsl:call-template>
          </xsl:variable>
          <OriginalPurchaseDate>
            <xsl:value-of select="OriginalPurchaseDate"/>
          </OriginalPurchaseDate>
          <xsl:variable name="varAccountNumber">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
          <AccountNumber>
            <xsl:value-of select="$varAccountNumber"/>
          </AccountNumber>
          <xsl:variable name="varBroker">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>
          <Broker>
            <xsl:value-of select="$varBroker"/>
          </Broker>
          <xsl:variable name="varTransactionType">
            <xsl:value-of select="TransactionType"/>
          </xsl:variable>
          <TransactionType>
            <xsl:value-of select="$varTransactionType"/>
          </TransactionType>
          <xsl:variable name="varSedol">
            <xsl:value-of select="SEDOL"/>
          </xsl:variable>
          <Identifier>
            <xsl:value-of select="$varSedol"/>
          </Identifier>
          <IdentifierType>
            <xsl:value-of select="'SEDOL'"/>
          </IdentifierType>
          <xsl:variable name="varQuantity">
            <xsl:value-of select="AllocatedQty"/>
          </xsl:variable>
          <Qunatity>
            <xsl:value-of select="$varQuantity"/>
          </Qunatity>
          <xsl:variable name="varCurrency">
            <xsl:value-of select="CurrencySymbol"/>
          </xsl:variable>
          <TradeCCY>
            <xsl:value-of select="$varCurrency"/>
          </TradeCCY>
          <xsl:variable name="varPrice">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>
          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>
          <xsl:variable name="varSettleCurrency">
            <xsl:value-of select="SettlCurrency"/>
          </xsl:variable>
          <SettleCCY>
            <xsl:value-of select="$varSettleCurrency"/>
          </SettleCCY>
          
          <TradeBookFXRate>
            <xsl:value-of select="''"/>
          </TradeBookFXRate>
          
          <TradeSettleFXRate>
            <xsl:value-of select="''"/>
          </TradeSettleFXRate>
          
          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged "/>
          </xsl:variable>
          
          <OtherPayments1>
            <xsl:value-of select="$varCommission"/>
          </OtherPayments1>
          
          <OtherPayments1Type>
            <xsl:value-of select="'Flat Commission'"/>
          </OtherPayments1Type>
          
          <OtherPayments1Implied>
            <xsl:choose>
              <xsl:when test="number($varCommission)">
                <xsl:value-of select="'TRUE'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'FALSE'"/>
              </xsl:otherwise>
            </xsl:choose>
          </OtherPayments1Implied>

          <xsl:variable name="varFees">
            <xsl:value-of select="SecFees + OtherBrokerFee + ClearingBrokerFee + MiscFees + SecFee + OccFee + OrfFee + ClearingFee + TaxOnCommissions + StampDuty + TransactionLevy"/>
          </xsl:variable>

          <OtherPayments2>
            <xsl:value-of select="$varFees"/>
          </OtherPayments2>

          <OtherPayments2Type>
            <xsl:value-of select="'Other Fees'"/>
          </OtherPayments2Type>

          <OtherPayments2Implied>
            <xsl:choose>
              <xsl:when test="number($varFees)">
                <xsl:value-of select="'TRUE'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'FALSE'"/>
              </xsl:otherwise>
            </xsl:choose>
          </OtherPayments2Implied>

          <OtherPayments3>
            <xsl:value-of select="''"/>
          </OtherPayments3>

          <OtherPayments3Type>
            <xsl:value-of select="''"/>
          </OtherPayments3Type>

          <OtherPayments3Implied>
            <xsl:value-of select="''"/>
          </OtherPayments3Implied>

          <OtherPayments4>
            <xsl:value-of select="''"/>
          </OtherPayments4>

          <OtherPayments4Type>
            <xsl:value-of select="''"/>
          </OtherPayments4Type>

          <OtherPayments4Implied>
            <xsl:value-of select="''"/>
          </OtherPayments4Implied>

          <TraderName>
            <xsl:value-of select="''"/>
          </TraderName>

          <TraderType>
            <xsl:value-of select="''"/>
          </TraderType>

          <TRSId>
            <xsl:value-of select="''"/>
          </TRSId>

          <BookId>
            <xsl:value-of select="''"/>
          </BookId>

          <BookPath>
            <xsl:value-of select="''"/>
          </BookPath>

          <ExternalReference>
            <xsl:value-of select="''"/>
          </ExternalReference>

          <InternalReference>
            <xsl:value-of select="''"/>
          </InternalReference>

          <Notes>
            <xsl:value-of select="''"/>
          </Notes>

          <BookingStatus>
            <xsl:value-of select="''"/>
          </BookingStatus>

          <DealId>
            <xsl:value-of select="''"/>
          </DealId>

          <TradeStatus>
            <xsl:choose>
              <xsl:when test="TaxLotStateID=0">
                <xsl:value-of select="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotStateID=1">
                <xsl:value-of select="'Send'"/>
              </xsl:when>
              <xsl:when test="TaxLotStateID=2">
                <xsl:value-of select="'Correction'"/>
              </xsl:when>
              <xsl:when test="TaxLotStateID=3">
                <xsl:value-of select="'Cancel'"/>
              </xsl:when>
              <xsl:when test="TaxLotStateID=4">
                <xsl:value-of select="'Ignore'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </TradeStatus>

          <CancelCorrectIDType>
            <xsl:value-of select="''"/>
          </CancelCorrectIDType>

          <CancelCorrectID>
            <xsl:value-of select="''"/>
          </CancelCorrectID>

          <SettlementInstructionId>
            <xsl:value-of select="''"/>
          </SettlementInstructionId>

          <BorrowId>
            <xsl:value-of select="''"/>
          </BorrowId>

          <TradeKeywordName1>
            <xsl:value-of select="''"/>
          </TradeKeywordName1>

            <TradeKeywordValue1>
              <xsl:value-of select="''"/>
            </TradeKeywordValue1>

          <TradeKeywordName2>
            <xsl:value-of select="''"/>
          </TradeKeywordName2>

          <TradeKeywordValue2>
            <xsl:value-of select="''"/>
            </TradeKeywordValue2>

            <TradeVenue>
               <xsl:value-of select="''"/>
             </TradeVenue>

            <AllocationTemplateId>
              <xsl:value-of select="''"/>
            </AllocationTemplateId>

          <AllocationAccountId>
              <xsl:value-of select="''"/>
            </AllocationAccountId>

            <AllocationHierarchies>
              <xsl:value-of select="''"/>
            </AllocationHierarchies>

            <AllocationBookPaths>
              <xsl:value-of select="''"/>
            </AllocationBookPaths>

            <AllocationAmounts>
              <xsl:value-of select="''"/>
            </AllocationAmounts>

          <AllocationNumberType>
            <xsl:value-of select="''"/>
          </AllocationNumberType>
          
        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>