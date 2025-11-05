<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
    </xsl:choose>
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

  <xsl:template name="noofZeros">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofZeros">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
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
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <DealType>
          <xsl:value-of select="'DealType'"/>
        </DealType>

        <DealId>
          <xsl:value-of select="'DealId'"/>
        </DealId>

        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>

        <Client>
          <xsl:value-of select="'Client'"/>
        </Client>

        <Reserved1>
          <xsl:value-of select="'Reserved1'"/>
        </Reserved1>

        <Reserved2>
          <xsl:value-of select="'Reserved2'"/>
        </Reserved2>

        <Folder>
          <xsl:value-of select="'Folder'"/>
        </Folder>

        <Custodian>
          <xsl:value-of select="'Custodian'"/>
        </Custodian>

        <CashAccount>
          <xsl:value-of select="'CashAccount'"/>
        </CashAccount>

        <Counterparty>
          <xsl:value-of select="'Counterparty'"/>
        </Counterparty>

        <Comments>
          <xsl:value-of select="'Comments'"/>
        </Comments>

        <State>
          <xsl:value-of select="'State'"/>
        </State>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

		  <SettlementDate>
			  <xsl:value-of select="'SettlementDate'"/>
		  </SettlementDate>

        <Reserved3>
          <xsl:value-of select="'Reserved3'"/>
        </Reserved3>

        <GlobeOpSecurityIdentifier>
          <xsl:value-of select="'GlobeOp Security Identifier'"/>
        </GlobeOpSecurityIdentifier>

		  <Reserved4>
			  <xsl:value-of select="'Reserved4'"/>
		  </Reserved4>

		  <RIC>
			  <xsl:value-of select="'RIC'"/>
		  </RIC>

		  <OCCCode>
			  <xsl:value-of select ="'OCC Code'"/>
		  </OCCCode>

		  <BloombergTicker>
			  <xsl:value-of select ="'Bloomberg Ticker'"/>
		  </BloombergTicker>

		  <Symbol>
			  <xsl:value-of select="'Symbol'"/>
		  </Symbol>

		  <SecurityDescription>
			  <xsl:value-of select="'SecurityDescription'"/>
		  </SecurityDescription>

		  <TransactionIndicator>
			  <xsl:value-of select ="'TransactionIndicator'"/>
		  </TransactionIndicator>

		  <SubTransactionIndicator>
			  <xsl:value-of select ="'SubTransaction-Indicator'"/>
		  </SubTransactionIndicator>

		  <Quantity>
			  <xsl:value-of select="'Quantity'"/>
		  </Quantity>

		  <Price>
			  <xsl:value-of select="'Price'"/>
		  </Price>


		  <Commission>
			  <xsl:value-of select="'Commission'"/>
		  </Commission>

		  <Tax>
			  <xsl:value-of select ="'Tax'"/>
		  </Tax>

		  <VAT>
			  <xsl:value-of select ="'VAT'"/>
		  </VAT>

		  <Reserved5>
			  <xsl:value-of select="'Reserved5'"/>
		  </Reserved5>

		  <Reserved6>
			  <xsl:value-of select="'Reserved6'"/>
		  </Reserved6>

		  <Comments1>
			  <xsl:value-of select ="'Comments1'"/>
		  </Comments1>

		  <BrokerShortName>
			  <xsl:value-of select ="'BrokerShortName'"/>
		  </BrokerShortName>

		  <ExpirationDate>
			  <xsl:value-of select="'ExpirationDate'"/>
		  </ExpirationDate>

		  <Exchange>
			  <xsl:value-of select="'Exchange'"/>
		  </Exchange>

		  <PutCallIndicator>
			  <xsl:value-of select="'PutCallIndicator'"/>
		  </PutCallIndicator>

		  <Strike>
			  <xsl:value-of select="'Strike'"/>
		  </Strike>

		  <ClientReference>
			  <xsl:value-of select="'ClientReference'"/>
		  </ClientReference>

		  <OptionEvent>
			  <xsl:value-of select="'OptionEvent'"/>
		  </OptionEvent>

		  <BlockId>
			  <xsl:value-of select="'BlockId'"/>
		  </BlockId>

		  <BlockAmount>
			  <xsl:value-of select="'BlockAmount'"/>
		  </BlockAmount>

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

				  <DealType>
					  <xsl:value-of select="'OptionDeal'"/>
				  </DealType>

				  <DealId>
					  <xsl:value-of select="concat(PBUniqueID,AccountName)"/>
				  </DealId>

				  <Action>
					  <xsl:choose>
						  <xsl:when test="TaxLotState = 'Allocated'">
							  <xsl:value-of select="'New'"/>
						  </xsl:when>
						  <xsl:when test="TaxLotState = 'Amemded'">
							  <xsl:value-of select="'Update'"/>
						  </xsl:when>
						  <xsl:when test="TaxLotState = 'Deleted'">
							  <xsl:value-of select="'Cancel'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Action>

				  <Client>
					  <xsl:value-of select="'AMINVESTMENT'"/>
				  </Client>

            <Reserved1>
              <xsl:value-of select="''"/>
            </Reserved1>

            <Reserved2>
              <xsl:value-of select="''"/>
            </Reserved2>

			  <Folder>
				  <xsl:value-of select="'AMVI_CITI'"/>
			  </Folder>

			  <Custodian>
				  <xsl:value-of select="'SSMB'"/>
			  </Custodian>

			  <CashAccount>
				  <xsl:value-of select="'SBNAMIVIPB'"/>
			  </CashAccount>

			  <Counterparty>
				  <xsl:value-of select="CounterParty"/>
			  </Counterparty>

            <Comments>
              <xsl:value-of select="''"/>
            </Comments>

            <State>
              <xsl:value-of select="''"/>
            </State>

            <TradeDate>
              <xsl:value-of select="concat(substring(TradeDate,7,4),'-',substring(TradeDate,1,2),'-',substring(TradeDate,4,2))"/>
            </TradeDate>

				  <SettlementDate>
					  <xsl:value-of select="concat(substring(SettlementDate,7,4),'-',substring(SettlementDate,1,2),'-',substring(SettlementDate,4,2))"/>
				  </SettlementDate>

            <Reserved3>
              <xsl:value-of select="''"/>
            </Reserved3>

            <GlobeOpSecurityIdentifier>
              <xsl:value-of select="''"/>
            </GlobeOpSecurityIdentifier>

			  <Reserved4>
				  <xsl:value-of select="''"/>
			  </Reserved4>

			  <RIC>
				  <xsl:value-of select="RIC"/>
			  </RIC>

			  <OCCCode>
				  <xsl:value-of select ="OSIOptionSymbol"/>
			  </OCCCode>

			  <BloombergTicker>
				  <xsl:value-of select ="BBCode"/>
			  </BloombergTicker>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test ="BBCode = ''">
						  <xsl:value-of select ="Symbol"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <SecurityDescription>
				  <xsl:value-of select="''"/>
			  </SecurityDescription>

            <TransactionIndicator>
              <xsl:choose>
                <xsl:when test="substring(Side,1,1)= 'B'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="substring(Side,1,1)= 'S'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
              </xsl:choose>
            </TransactionIndicator>

            <SubTransactionIndicator>
				<xsl:choose>
					<xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
						<xsl:value-of select="'Buy Long'"/>
					</xsl:when>
					<xsl:when test="Side = 'Sell' or Side = 'Sell to Close'">
						<xsl:value-of select="'Sell Long'"/>
					</xsl:when>
					<xsl:when test="Side = 'Buy to Close'">
						<xsl:value-of select="'Buy Cover'"/>
					</xsl:when>
					<xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
						<xsl:value-of select="'Sell Short'"/>
					</xsl:when>
				</xsl:choose>
            </SubTransactionIndicator>

            <Quantity>
              <xsl:value-of select="AllocatedQty*AssetMultiplier"/>
            </Quantity>

            <Price>
              <xsl:value-of select="AveragePrice"/>
            </Price>

            

            <Commission>
              <xsl:value-of select="CommissionCharged"/>
            </Commission>

			  <Tax>
				  <xsl:value-of select ="TransactionLevy"/>
			  </Tax>

			  <VAT>
				  <xsl:value-of select ="''"/>
			  </VAT>

            <Reserved5>
              <xsl:value-of select="''"/>
            </Reserved5>

            <Reserved6>
              <xsl:value-of select="''"/>
            </Reserved6>

			  <Comments1>
				  <xsl:value-of select ="''"/>
			  </Comments1>

			  <BrokerShortName>
				  <xsl:value-of select ="''"/>
			  </BrokerShortName>

			  <ExpirationDate>
				  <xsl:value-of select="concat(substring(ExpirationDate,7,4),'-',substring(ExpirationDate,1,2),'-',substring(ExpirationDate,4,2))"/>
			  </ExpirationDate>

			  <Exchange>
				  <xsl:choose>
					  <xsl:when test ="BBCode = ''">
						  <xsl:value-of select="Exchange"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Exchange>

			  <PutCallIndicator>
              <xsl:value-of select="substring(PutOrCall,1,1)"/>
            </PutCallIndicator>

            <Strike>
              <xsl:value-of select="StrikePrice"/>
            </Strike>

			  <ClientReference>
				  <xsl:value-of select="''"/>
			  </ClientReference>

			  <OptionEvent>
				  <xsl:value-of select="''"/>
			  </OptionEvent>

            <BlockId>
              <xsl:value-of select="''"/>
            </BlockId>

            <BlockAmount>
              <xsl:value-of select="''"/>
            </BlockAmount>

            <!-- system use only-->
            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

          </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
